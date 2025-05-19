using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Api.Extensions;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities.Identity;
using PrimeCare.Shared.Dtos.User;
using PrimeCare.Shared.Errors;
using StackExchange.Redis;

namespace PrimeCare.Api.Controllers
{
    // Controller responsible for authentication-related operations
    public class AuthController : BaseApiController
    {
        // Dependencies used for user management, authentication, token generation, etc.
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IDatabase _database;

        // Constructor with dependency injection
        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            IMapper mapper,
            IEmailService emailService,
            IConfiguration configuration,
            IConnectionMultiplexer redis)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
            _database = redis.GetDatabase(); // Redis DB for temporary storage (e.g., codes)
        }

        // Get currently logged-in user info (requires authentication)
        [Authorize]
        [HttpGet("current-user")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindEmailFromPrincipal(HttpContext.User);

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName
            };
        }

        // Check if email already exists in the database
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        // Get the authenticated user's address
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FinddUserByClaimsPrincipallWithAddressAsync(HttpContext.User);
            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        // Update the authenticated user's address
        [Authorize]
        [HttpPut("update-address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FinddUserByClaimsPrincipallWithAddressAsync(HttpContext.User);
            user.Address = _mapper.Map<AddressDto, Address>(address);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(_mapper.Map<Address, AddressDto>(user.Address));
            return BadRequest("Problem updating the user address");
        }

        // Login endpoint
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName
            };
        }

        // Register new user
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // Ensure passwords match
            if (registerDto.Password != registerDto.Repassword)
                return BadRequest(new ApiResponse(400, "Passwords do not match"));

            // Create a new ApplicationUser object
            var user = new ApplicationUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                FullName = $"{registerDto.Fname} {registerDto.Lname}",
                Address = new Address
                {
                    FirstName = registerDto.Fname,
                    LastName = registerDto.Lname,
                    Street = "N/A",
                    City = "N/A",
                    State = "N/A",
                    ZipCode = "00000"
                }
            };

            // Attempt to create the user
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new ApiResponse(400, $"Registration failed: {errors}"));
            }

            // Generate authentication token
            var token = _tokenService.CreateToken(user);

            return Ok(new UserDto
            {
                UserName = user.UserName,
                Token = token,
                Email = user.Email,
            });
        }

        // Return page or payload for password reset
        [HttpGet("reset-password")]
        public IActionResult ResetPasswordPage(string email, string token)
        {
            return Ok(new { Email = email, Token = token, Message = "Ready for password reset" });
        }

        // Send password reset verification code to email
        [HttpPost("forget-password")]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new ApiResponse(400, "Email not found"));

            // Generate a 6-digit verification code
            var verificationCode = new Random().Next(100000, 999999).ToString();

            // Save code in Redis for 15 minutes
            await _database.StringSetAsync($"reset-code-{model.Email}", verificationCode, TimeSpan.FromMinutes(15));

            var subject = "Password Reset Verification Code";
            var body = $@"
                <h2>Password Reset</h2>
                <p>Your verification code is: <strong>{verificationCode}</strong></p>
                <p>This code is valid for 15 minutes.</p>";

            // Send code via email
            await _emailService.SendEmailAsync(model.Email, subject, body);

            return Ok(new { message = "Verification code sent to email" });
        }

        // Verify the entered code
        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto verifyCodeDto)
        {
            var user = await _userManager.FindByEmailAsync(verifyCodeDto.Email);
            if (user == null)
                return BadRequest(new ApiResponse(400, "Email not found"));

            // Get saved code from Redis
            var savedCode = await _database.StringGetAsync($"reset-code-{verifyCodeDto.Email}");
            if (savedCode != verifyCodeDto.Code)
                return BadRequest(new ApiResponse(400, "Invalid verification code"));

            // Mark the email as verified and delete the code
            await _database.StringSetAsync($"verified-reset-{verifyCodeDto.Email}", "true", TimeSpan.FromMinutes(15));
            await _database.KeyDeleteAsync($"reset-code-{verifyCodeDto.Email}");

            return Ok(new { Message = "Code verified" });
        }

        // Reset the user's password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            // Ensure new password and confirmation match
            if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
                return BadRequest(new ApiResponse(400, "Passwords do not match"));

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest(new ApiResponse(400, "Email not found"));

            // Check if the reset was verified
            var verified = await _database.StringGetAsync($"verified-reset-{resetPasswordDto.Email}");
            if (verified != "true")
                return BadRequest(new ApiResponse(400, "Verification required"));

            // Generate and apply password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new ApiResponse(400, $"Reset password failed: {errors}"));
            }

            // Remove verification flag from Redis
            await _database.KeyDeleteAsync($"verified-reset-{resetPasswordDto.Email}");

            return Ok(new { Message = "Password reset successful" });
        }
    }
}

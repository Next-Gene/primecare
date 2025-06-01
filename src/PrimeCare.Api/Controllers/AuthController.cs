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
    public class AuthController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IDatabase _database;
        private readonly IRoleManagementService _roleManagementService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            IMapper mapper,
            IEmailService emailService,
            IConfiguration configuration,
            IConnectionMultiplexer redis,
            IRoleManagementService roleManagementService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
            _database = redis.GetDatabase();
            _roleManagementService = roleManagementService;
        }

        [Authorize]
        [HttpGet("current-user")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindEmailFromPrincipal(HttpContext.User);
            if (user == null)
                return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FinddUserByClaimsPrincipallWithAddressAsync(HttpContext.User);
            if (user == null)
                return Unauthorized(new ApiResponse(401));

            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("update-address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FinddUserByClaimsPrincipallWithAddressAsync(HttpContext.User);
            if (user == null)
                return Unauthorized(new ApiResponse(401));

            user.Address = _mapper.Map<AddressDto, Address>(address);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Ok(_mapper.Map<Address, AddressDto>(user.Address));

            return BadRequest(new ApiResponse(400, "Problem updating the user address"));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
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

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.Repassword)
                return BadRequest(new ApiResponse(400, "Passwords do not match"));

            if ((await CheckEmailExists(registerDto.Email)).Value)
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "Email is already in use" }
                });

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

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new ApiResponse(400, $"Registration failed: {errors}"));
            }

            // Automatically assign the "User" role to newly registered users
            try
            {
                await _roleManagementService.AssignDefaultRoleToUserAsync(user.Email);
            }
            catch (Exception ex)
            {
                // Log the error but don't fail the registration
                // You might want to use a logger here
                Console.WriteLine($"Warning: Failed to assign default role to user {user.Email}: {ex.Message}");
            }

            return Ok(new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            });
        }

        [HttpGet("reset-password")]
        public IActionResult ResetPasswordPage(string email, string token)
        {
            return Ok(new { Email = email, Token = token, Message = "Ready for password reset" });
        }

        [HttpPost("forget-password")]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new ApiResponse(400, "Email not found"));

            var verificationCode = new Random().Next(100000, 999999).ToString();
            await _database.StringSetAsync($"reset-code-{model.Email}", verificationCode, TimeSpan.FromMinutes(15));

            var subject = "🔐 Password Reset Verification Code";
            var body = CreateCreativeEmailTemplate(verificationCode, user.FullName ?? "User");

            await _emailService.SendEmailAsync(model.Email, subject, body);
            return Ok(new { message = "Verification code sent to email" });
        }

        private string CreateCreativeEmailTemplate(string verificationCode, string userName)
        {
            return $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Password Reset</title>
        <style>
            * {{
                margin: 0;
                padding: 0;
                box-sizing: border-box;
            }}
            
            body {{
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                line-height: 1.6;
                color: #333;
                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                padding: 20px;
            }}
            
            .email-container {{
                max-width: 600px;
                margin: 0 auto;
                background: #ffffff;
                border-radius: 20px;
                box-shadow: 0 20px 40px rgba(0,0,0,0.1);
                overflow: hidden;
                position: relative;
            }}
            
            .header {{
                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                padding: 40px 30px;
                text-align: center;
                position: relative;
                overflow: hidden;
            }}
            
            .header::before {{
                content: '';
                position: absolute;
                top: -50%;
                left: -50%;
                width: 200%;
                height: 200%;
                background: radial-gradient(circle, rgba(255,255,255,0.1) 0%, transparent 70%);
                animation: pulse 4s ease-in-out infinite;
            }}
            
            @keyframes pulse {{
                0%, 100% {{ transform: scale(1); opacity: 0.5; }}
                50% {{ transform: scale(1.1); opacity: 0.8; }}
            }}
            
            .header h1 {{
                color: #ffffff;
                font-size: 28px;
                font-weight: 700;
                margin-bottom: 10px;
                position: relative;
                z-index: 1;
            }}
            
            .lock-icon {{
                font-size: 48px;
                margin-bottom: 20px;
                display: inline-block;
                animation: bounce 2s ease-in-out infinite;
                position: relative;
                z-index: 1;
            }}
            
            @keyframes bounce {{
                0%, 20%, 50%, 80%, 100% {{ transform: translateY(0); }}
                40% {{ transform: translateY(-10px); }}
                60% {{ transform: translateY(-5px); }}
            }}
            
            .content {{
                padding: 40px 30px;
                text-align: center;
            }}
            
            .greeting {{
                font-size: 20px;
                color: #2c3e50;
                margin-bottom: 25px;
                font-weight: 600;
            }}
            
            .message {{
                font-size: 16px;
                color: #5a6c7d;
                margin-bottom: 35px;
                line-height: 1.8;
            }}
            
            .code-container {{
                background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
                border-radius: 15px;
                padding: 30px;
                margin: 30px 0;
                position: relative;
                overflow: hidden;
            }}
            
            .code-container::before {{
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                background: url('data:image/svg+xml,<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 100 100""><defs><pattern id=""grain"" width=""100"" height=""100"" patternUnits=""userSpaceOnUse""><circle cx=""50"" cy=""50"" r=""1"" fill=""white"" opacity=""0.1""/></pattern></defs><rect width=""100"" height=""100"" fill=""url(%23grain)""/></svg>');
                opacity: 0.3;
            }}
            
            .code-label {{
                color: #ffffff;
                font-size: 14px;
                font-weight: 600;
                margin-bottom: 15px;
                text-transform: uppercase;
                letter-spacing: 1px;
                position: relative;
                z-index: 1;
            }}
            
            .verification-code {{
                font-size: 36px;
                font-weight: 800;
                color: #ffffff;
                letter-spacing: 8px;
                margin: 10px 0;
                text-shadow: 2px 2px 4px rgba(0,0,0,0.3);
                font-family: 'Courier New', monospace;
                position: relative;
                z-index: 1;
            }}
            
            .timer-info {{
                background: #fff3cd;
                border: 2px solid #ffeaa7;
                border-radius: 10px;
                padding: 20px;
                margin: 25px 0;
                display: flex;
                align-items: center;
                justify-content: center;
                gap: 10px;
            }}
            
            .timer-icon {{
                font-size: 20px;
                color: #e17055;
            }}
            
            .timer-text {{
                color: #6c5700;
                font-weight: 600;
                font-size: 14px;
            }}
            
            .security-note {{
                background: #e8f4fd;
                border-left: 4px solid #3498db;
                padding: 20px;
                margin: 25px 0;
                border-radius: 0 10px 10px 0;
            }}
            
            .security-note h3 {{
                color: #2980b9;
                font-size: 16px;
                margin-bottom: 10px;
                display: flex;
                align-items: center;
                gap: 8px;
            }}
            
            .security-note p {{
                color: #34495e;
                font-size: 14px;
                line-height: 1.6;
            }}
            
            .footer {{
                background: #f8f9fa;
                padding: 30px;
                text-align: center;
                border-top: 1px solid #e9ecef;
            }}
            
            .footer p {{
                color: #6c757d;
                font-size: 13px;
                margin-bottom: 10px;
            }}
            
            .company-name {{
                color: #495057;
                font-weight: 600;
                font-size: 16px;
            }}
            
            .decorative-line {{
                height: 3px;
                background: linear-gradient(90deg, #667eea, #764ba2, #f093fb, #f5576c);
                margin: 20px 0;
                border-radius: 2px;
            }}
            
            @media (max-width: 600px) {{
                .email-container {{
                    margin: 10px;
                    border-radius: 15px;
                }}
                
                .header, .content, .footer {{
                    padding: 25px 20px;
                }}
                
                .verification-code {{
                    font-size: 28px;
                    letter-spacing: 4px;
                }}
                
                .header h1 {{
                    font-size: 24px;
                }}
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='header'>
                <div class='lock-icon'>🔐</div>
                <h1>Password Reset Request</h1>
            </div>
            
            <div class='content'>
                <div class='greeting'>Hello {userName}! 👋</div>
                
                <p class='message'>
                    We received a request to reset your password. Don't worry, we've got you covered! 
                    Use the verification code below to proceed with resetting your password.
                </p>
                
                <div class='code-container'>
                    <div class='code-label'>Your Verification Code</div>
                    <div class='verification-code'>{verificationCode}</div>
                </div>
                
                <div class='timer-info'>
                    <span class='timer-icon'>⏰</span>
                    <span class='timer-text'>This code expires in 15 minutes</span>
                </div>
                
                <div class='security-note'>
                    <h3>🛡️ Security Notice</h3>
                    <p>
                        If you didn't request this password reset, please ignore this email and your password will remain unchanged. 
                        For your security, never share this code with anyone.
                    </p>
                </div>
                
                <div class='decorative-line'></div>
                
                <p style='color: #7f8c8d; font-size: 14px; margin-top: 25px;'>
                    Need help? Contact our support team - we're here to assist you! 💬
                </p>
            </div>
            
            <div class='footer'>
                <p class='company-name'>Your Company Name</p>
                <p>This is an automated message, please do not reply to this email.</p>
                <p>&copy; 2024 Your Company. All rights reserved.</p>
            </div>
        </div>
    </body>
    </html>";
        }




        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto verifyCodeDto)
        {
            var user = await _userManager.FindByEmailAsync(verifyCodeDto.Email);
            if (user == null)
                return BadRequest(new ApiResponse(400, "Email not found"));

            var savedCode = await _database.StringGetAsync($"reset-code-{verifyCodeDto.Email}");
            if (savedCode != verifyCodeDto.Code)
                return BadRequest(new ApiResponse(400, "Invalid verification code"));

            await _database.StringSetAsync($"verified-reset-{verifyCodeDto.Email}", "true", TimeSpan.FromMinutes(15));
            await _database.KeyDeleteAsync($"reset-code-{verifyCodeDto.Email}");

            return Ok(new { Message = "Code verified" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
                return BadRequest(new ApiResponse(400, "Passwords do not match"));

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest(new ApiResponse(400, "Email not found"));

            var verified = await _database.StringGetAsync($"verified-reset-{resetPasswordDto.Email}");
            if (verified != "true")
                return BadRequest(new ApiResponse(400, "Verification required"));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new ApiResponse(400, $"Reset password failed: {errors}"));
            }

            await _database.KeyDeleteAsync($"verified-reset-{resetPasswordDto.Email}");

            return Ok(new { Message = "Password reset successful" });
        }

        // Role management endpoints

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("change-user-role")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleDto changeRoleDto)
        {
            try
            {
                await _roleManagementService.ChangeUserRoleAsync(changeRoleDto.Email, changeRoleDto.NewRole);
                return Ok(new { Message = $"User role changed to {changeRoleDto.NewRole} successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("user-roles/{email}")]
        public async Task<ActionResult<UserRoleDto>> GetUserRoles(string email)
        {
            try
            {
                var userRoles = await _roleManagementService.GetUserRolesAsync(email);
                return Ok(userRoles);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("available-roles")]
        public async Task<ActionResult<IEnumerable<string>>> GetAvailableRoles()
        {
            try
            {
                var roles = await _roleManagementService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }
        }
    }
}
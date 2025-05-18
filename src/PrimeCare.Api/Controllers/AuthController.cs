using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities.Identity;
using PrimeCare.Shared.Dtos.User;
using PrimeCare.Shared.Errors;

namespace PrimeCare.Api.Controllers
{
    public class AuthController : BaseApiController
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)

        {

            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;

        }

        [HttpPost("login")]

        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return new UserDto
            {

                Email = loginDto.Email,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName


            };



        }



        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.Repassword)
                return BadRequest(new ApiResponse(400, "Passwords do not match"));

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

            var token = _tokenService.CreateToken(user);

            return Ok(new UserDto
            {
                UserName = user.UserName,
                Token = token,
                Email = user.Email,
            });
        }




    }
}
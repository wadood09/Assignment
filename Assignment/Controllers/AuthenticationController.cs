using Assignment.Core.Application.Interfaces.Services;
using Assignment.Models.UserModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;
        private readonly IConfiguration _config;

        public AuthenticationController(IUserService userService, IIdentityService identityService, IConfiguration config)
        {
            _userService = userService;
            _identityService = identityService;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequestModel model)
        {
            var user = await _userService.Login(model);

            if (user.IsSuccessful == true)
            {
                var token = _identityService.GenerateToken(_config["Jwt:Key"], _config["Jwt:Issuer"], user.Value);
                //bool isValid = _identityService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), user.Value.Token);
                //if (!isValid)
                //{
                //    return BadRequest();
                //}
                return Ok(token);
            }
            else
            {
                return StatusCode(400, user.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserRequest request)
        {
            var user = await _userService.CreateUser(request);

            if (user.IsSuccessful == true)
            {
                return Ok(user.Message);
            }
            else
            {
                return StatusCode(400, user.Message);
            }
        }
    }
}

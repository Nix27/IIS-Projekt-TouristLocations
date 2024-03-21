using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Service.Abstraction;
using ServiceLayer.ServiceModel;

namespace TouristLocationsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var response = await _userService.Register(registerRequest);
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var response = await _userService.Login(loginRequest);
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshExpiredToken([FromBody] RefreshRequest refreshRequest)
        {
            var response = await _userService.RefreshToken(refreshRequest);
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string userEmail)
        {
            var response = await _userService.Logout(userEmail);
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }
    }
}

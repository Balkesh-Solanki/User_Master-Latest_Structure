using DTO.ReqDTO;
using DTO.ResDTO;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interfaces;

namespace User_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReqDTO req)
        {
            var response = await _loginService.Login(req);
            if (response == null)
                return Unauthorized("Invalid credentials");

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenReqDTO req)
        {
            var accessTokenresponse = await _loginService.RefreshToken(req.AccessToken);
            if (accessTokenresponse == null)
                return Unauthorized("AccessToken is still valid. No need to refresh.");

            var response = await _loginService.RefreshToken(req.RefreshToken);
            if (response == null)
                return Unauthorized("Invalid or expired refresh token");

            return Ok(response);
        }
    }
}

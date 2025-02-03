using BussinessLayer;
using DTO.ReqDTO;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interfaces;

namespace User_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var language = Request.Headers["Accept-Language"].ToString();
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound("User not found.");
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserReqDTO req)
        {
            var result = await _userService.AddUser(req);
            if (!result)
                return BadRequest("Failed to add user.");
            return Ok("User added successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserReqDTO req)
        {
            var result = await _userService.UpdateUser(id, req);
            if (!result)
                return NotFound("User not found.");
            return Ok("User updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (!result)
                return NotFound("User not found.");
            return Ok("User deleted successfully.");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rental_Application.EntityLayer.UserModel;
using Rental_Application.EntityLayer.Utility;
using Rental_Application.IBusinessAccessLayer.IRoleMasterService;
using Rental_Application.IBusinessAccessLayer.IUserService;

namespace Rental_Application.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleMasterService _roleMasterService;
        public AuthController(IUserService userService, IRoleMasterService roleMasterService)
        {
            this._userService = userService;
            _roleMasterService = roleMasterService;
        }

        [HttpPost("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateRequest request)
        {

            var response = await _userService.ValidateUser(request);
            return Ok(response);

        }

        [Authorize]
        [HttpPost("GetUser")]
        public async Task<IActionResult> GetUser([FromBody] UserRequest request)
        {

            var response = await _userService.GetUserDetailsById(request.Username);
            return Ok(response);

        }

        [HttpPost("ValidatePassword")]
        public IActionResult ValidatePassword([FromBody] string password)
        {
            if (PasswordValidator.ValidatePassword(password))
            {
                return Ok(new { Message = "Password is valid." });
            }
            else
            {
                return BadRequest(new { Message = "Password is invalid. Ensure it meets all required criteria." });
            }
        }


        [HttpGet("GetRole")]
        public async Task<IActionResult> GetRole()
        {
            var response = await _roleMasterService.GetRoles();
            return Ok(response);
        }

        [HttpPost("LogoutUser")]
        public async Task<IActionResult> LogoutUser([FromBody] LogoutRequest request)
        {
            await _userService.LogoutUser(request.LOGIN_ID);
            return Ok();
        }
    }
}

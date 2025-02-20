using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rental_Application.EntityLayer.UserModel;
using Rental_Application.IBusinessAccessLayer.IUserService;

namespace Rental_Application.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateRequest request)
        {

            var response = await _userService.ValidateUser(request.Username, request.Password);
            return Ok(response);

        }
    }
}

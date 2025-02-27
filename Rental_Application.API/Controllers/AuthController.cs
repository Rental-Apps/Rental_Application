﻿using Microsoft.AspNetCore.Authorization;
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

        //[Authorize]
        //[HttpPost("GetUser")]
        //public async Task<IActionResult> GetUser(string loginId)
        //{

        //    var response = await _userService.GetUserDetailsById(loginId);
        //    return Ok(response);

        //}

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
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return Ok(new { status = "FAIL", message = "Roles not found" });
            }
        }

        [HttpPost("LogoutUser")]
        public async Task<IActionResult> LogoutUser([FromBody] LogoutRequest request)
        {
            await _userService.LogoutUser(request.LOGIN_ID);
            return Ok();
        }
        [Authorize]
        [HttpPost("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP(OTPRequestModel oTPRequestModel)
        {
            var response = await _userService.VerifyOTP(oTPRequestModel.loginId, oTPRequestModel.otp_code);
            return Ok(response);
        }


        [Authorize]
        [HttpGet("WelcomeMessage")]
        public async Task<IActionResult> WelcomeMessage()
        {

            var response = "Welcome to the rental application";
            return Ok(response);

        }
    }
}

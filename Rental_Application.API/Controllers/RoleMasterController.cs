using Microsoft.AspNetCore.Mvc;
using Rental_Application.IBusinessAccessLayer.IRoleMasterService;

namespace Rental_Application.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleMasterController : Controller
    {
        private readonly IRoleMasterService _roleMasterService;
        public RoleMasterController(IRoleMasterService roleMasterService)
        {
            _roleMasterService = roleMasterService;
        }

        [HttpGet("GetRole")]
        public async Task<IActionResult> GetRole()
        {
            var response = await _roleMasterService.GetRoles();
            return Ok(response);
        }
    }
}

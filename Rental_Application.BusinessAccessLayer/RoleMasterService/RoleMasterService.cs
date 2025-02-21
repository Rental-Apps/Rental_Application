using System.Net;
using Microsoft.Extensions.Logging;
using Rental_Application.DataAccessLayer.RoleMasterRepository;
using Rental_Application.EntityLayer.Response;
using Rental_Application.EntityLayer.Utility;
using Rental_Application.IBusinessAccessLayer.IRoleMasterService;

namespace Rental_Application.BusinessAccessLayer.RoleMasterService
{
    public class RoleMasterService : IRoleMasterService
    {
        private readonly IRoleMasterRepository _roleMasterRepository;
        private readonly ILogger<RoleMasterService> _logger;

        public RoleMasterService(IRoleMasterRepository roleMasterRepository, ILogger<RoleMasterService> logger)
        {
            _roleMasterRepository = roleMasterRepository;
            _logger = logger;
        }

        public async Task<Response> GetRoles()
        {
            var response = new Response();
            try
            {
                var role = await _roleMasterRepository.GetRolesAsync();
                if (role != null) 
                {
                    response = GenericResponse.CreateSingleResponse(role, "Roles Found!", MessageConstrains.SUCCESS, (int)HttpStatusCode.OK);
                }
                else
                {
                    response = GenericResponse.CreateResponse(new List<Response>(), MessageConstrains.MSG_NOTFOUND, MessageConstrains.FAIL, (int)HttpStatusCode.NotFound);
                }
                return response;
            }
            catch (Exception ex) 
            {
                _logger.LogInformation("Something Wrong!", ex.Message);
                response = GenericResponse.CreateResponse(new List<Response>(), ex.Message, MessageConstrains.FAIL, (int)HttpStatusCode.InternalServerError);
                return response;
            }
        }
    }
}

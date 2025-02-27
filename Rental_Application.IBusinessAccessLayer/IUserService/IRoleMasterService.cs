using Rental_Application.EntityLayer.Response;

namespace Rental_Application.IBusinessAccessLayer.IRoleMasterService
{
    public interface IRoleMasterService
    {
        Task<Response> GetRoles();
    }
}

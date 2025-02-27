using Rental_Application.EntityLayer.RoleMasterModel;

namespace Rental_Application.DataAccessLayer.RoleMasterRepository
{
    public interface IRoleMasterRepository
    {
        Task<List<RoleMasterModel>> GetRolesAsync();
    }
}

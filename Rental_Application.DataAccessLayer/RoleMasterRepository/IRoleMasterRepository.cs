using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rental_Application.EntityLayer.RoleMasterModel;

namespace Rental_Application.DataAccessLayer.RoleMasterRepository
{
    public interface IRoleMasterRepository
    {
        Task<List<RoleMasterModel>> GetRolesAsync();
    }
}

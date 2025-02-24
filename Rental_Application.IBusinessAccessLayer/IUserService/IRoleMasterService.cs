using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rental_Application.EntityLayer.Response;

namespace Rental_Application.IBusinessAccessLayer.IRoleMasterService
{
    public interface IRoleMasterService
    {
        Task<Response> GetRoles();
    }
}

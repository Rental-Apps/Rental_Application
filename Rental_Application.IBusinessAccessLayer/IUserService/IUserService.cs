using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rental_Application.EntityLayer.Response;
using Rental_Application.EntityLayer.UserModel;

namespace Rental_Application.IBusinessAccessLayer.IUserService
{
    public interface IUserService
    {
        Task<Response> ValidateUser(AuthenticateRequest request);

        Task<Response> GetUserDetailsById(string username);

        Task LogoutUser(string login_id);

    }
}

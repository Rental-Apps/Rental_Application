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

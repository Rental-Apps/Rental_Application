using Rental_Application.EntityLayer.UserModel;

namespace Rental_Application.DataAccessLayer.UserRepository
{
    public interface IUserDataRepository
    {
        Task<UserModel> GetUserByUsernameAndPasswordAsync(AuthenticateRequest request);
        //Task<UserModel> AuthenticateUser(string username, string password);

        Task<UserModel> GetUserById(string username);
    }
}

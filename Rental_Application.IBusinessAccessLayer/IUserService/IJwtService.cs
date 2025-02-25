using Rental_Application.EntityLayer.UserModel;

namespace Rental_Application.IBusinessAccessLayer.IUserService
{
    public interface IJwtService
    {

        string GenerateToken(UserModel user);

        string GenerateRefreshToken();
    }
}

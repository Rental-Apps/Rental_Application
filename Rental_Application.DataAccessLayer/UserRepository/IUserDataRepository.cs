using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rental_Application.EntityLayer.UserModel;

namespace Rental_Application.DataAccessLayer.UserRepository
{
    public interface IUserDataRepository
    {
        Task<UserModel> GetUserByUsernameAndPasswordAsync(string username, string password);
        //Task<UserModel> AuthenticateUser(string username, string password);

        Task<UserModel> GetUserById(string username);
    }
}

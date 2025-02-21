using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental_Application.EntityLayer.UserModel
{
    public class AuthenticateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LogoutRequest
    {
        public string LOGIN_ID { get; set; }
    }
}

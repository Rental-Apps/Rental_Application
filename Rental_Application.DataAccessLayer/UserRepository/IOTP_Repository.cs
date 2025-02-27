using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental_Application.DataAccessLayer.UserRepository
{
    public interface IOTP_Repository
    {
        Task<string> saveAuthOTP(string otp, string emailId);

        Task<string> verifyOTP(string emailId, string otp);
    }
}

namespace Rental_Application.EntityLayer.UserModel
{
    public class AuthenticateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }

        // public string Remote_Address { get; set; }



    }
    public class LogoutRequest
    {
        public string LOGIN_ID { get; set; }
    }


    public class OTPRequestModel
    {
        public string loginId { get; set; }

        public string otp_code { get; set; }
    }
}

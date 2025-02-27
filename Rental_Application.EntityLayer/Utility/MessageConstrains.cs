namespace Rental_Application.EntityLayer.Utility
{
    public class MessageConstrains
    {
        //status
        public const string SUCCESS = "Success";
        public const string FAIL = "Fail";
        //message
        public const string MSG_SUCCESS = "Records get Succssfully";
        public const string MSG_NOTFOUND = "Records not found";
        public const string MSG_ERROR = "Internal Server Error";
        public const string MSG_UPDATE = "Upsert records Successfully";

        public const string MSG_INVALID_OTP = "Invalid OTP";
        public const string MSG_WRONG_OTP = "Worng OTP";
    }
}

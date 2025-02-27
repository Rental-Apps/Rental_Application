namespace Rental_Application.EntityLayer.LogInLog
{
    public class LogInLogModel
    {
        public string LOGIN_ID { get; set; }
        public string IP { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string SESSION_ID { get; set; }
    }
}

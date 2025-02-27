namespace Rental_Application.EntityLayer.Utility
{
    public class TransactionLog
    {
        public string UserId { get; set; }

        public string LogMessage { get; set; }

        public string LogDescription { get; set; }

        public string LogStatus { get; set; }

        public DateTime LogInTime { get; set; }

        public string IPAddress { get; set; }

        public DateTime? LogOutTime { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }


    }
}

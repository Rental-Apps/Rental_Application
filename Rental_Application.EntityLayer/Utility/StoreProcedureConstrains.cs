namespace Rental_Application.EntityLayer.Utility
{
    public static class StoreProcedureConstrains
    {
        public const string ValidateUser = "Rentalbasic.PROC_LOGIN_USER";
        public const string GetUser = "RENTALBASIC.GET_EMAIL_BY_LOGIN_ID";
        public const string Save_TransactionLog = "InsertTransactionLog";
        public const string SP_RoleMaster = "Rentalbasic.PL_MASTER_USER_ROLE";
        public const string SP_LogInLogInsert = "Rentalbasic.PI_LOGIN_ACTIVITY";
        public const string sp_GetLatestLoginLog = "Rentalbasic.SP_GetLatestLoginLog";
        public const string sp_UpdateLoginLog = "Rentalbasic.PU_LOGIN_ACTIVITY";

        public const string SaveOTP = "Rentalbasic.SaveOTP";
        public const string VerifyOTP = "Rentalbasic.VERIFYOTP";
        public const string sp_GetSessionId = "RentalBasic.SP_GetSessionId";
    }
}

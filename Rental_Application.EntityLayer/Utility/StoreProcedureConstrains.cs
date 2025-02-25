namespace Rental_Application.EntityLayer.Utility
{
    public static class StoreProcedureConstrains
    {
        public const string ValidateUser = "Rentalbasic.PROC_LOGIN_USER";
        public const string GetUser = "Rentalbasic.TEMP_GET_USER";
        public const string Save_TransactionLog = "InsertTransactionLog";
        public const string SP_RoleMaster = "Rentalbasic.PL_MASTER_USER_ROLE";
        public const string SP_LogInLogInsert = "Rentalbasic.PI_LOGIN_ACTIVITY";
        public const string sp_GetLatestLoginLog = "Rentalbasic.SP_GetLatestLoginLog";
        public const string sp_UpdateLoginLog = "Rentalbasic.PU_LOGIN_ACTIVITY";
    }
}

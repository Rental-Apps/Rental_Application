using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental_Application.EntityLayer.Utility
{
    public static class StoreProcedureConstrains
    {
        public const string ValidateUser = "Rentalbasic.TEMP_VALIDATE_USER";
        public const string Save_TransactionLog = "InsertTransactionLog";
        public const string SP_LogInLogInsert = "Rentalbasic.PI_LOGIN_ACTIVITY";
        public const string sp_GetLatestLoginLog = "Rentalbasic.SP_GetLatestLoginLog";
        public const string sp_UpdateLoginLog = "Rentalbasic.PU_LOGIN_ACTIVITY";
    }
}

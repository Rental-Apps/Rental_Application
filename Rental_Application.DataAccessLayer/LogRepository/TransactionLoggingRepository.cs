using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Rental_Application.DataAccessLayer.DataRepository;
using Rental_Application.EntityLayer.Utility;

namespace Rental_Application.DataAccessLayer.LogRepository
{
    public class TransactionLoggingRepository : ITransactionLoggingRepository
    {
        private readonly IDapper _dapper;

        public TransactionLoggingRepository(IDapper dapper)
        {
            _dapper = dapper;
        }
        public async Task<int> CreateLogAsync(TransactionLog transactionlog)
        {
            using (var connection = _dapper.CreateConnection())
            {
                var parameter = new DynamicParameters();
                parameter.Add("@UserId", transactionlog.UserId);
                parameter.Add("@LogMessage", transactionlog.LogMessage);
                parameter.Add("@LogDescription", transactionlog.LogDescription);
                parameter.Add("@LogStatus", transactionlog.LogStatus);
                parameter.Add("@LoginTime", transactionlog.LogInTime);
                parameter.Add("@IPAddress", transactionlog.IPAddress);
                parameter.Add("@LogoutTime", transactionlog.LogOutTime);
                parameter.Add("@CreatedBy", transactionlog.CreatedBy);
                parameter.Add("@ModifiedBy", transactionlog.ModifiedBy);
                //return await _dapperDbConnection.InsertDeleteUpdate(connection, "sp_AddUser", parameters, CommandType.StoredProcedure);
                return await _dapper.InsertDeleteUpdate(connection,StoreProcedureConstrains.Save_TransactionLog, parameter, CommandType.StoredProcedure);
            }
        }
    }
}

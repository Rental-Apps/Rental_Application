using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Oracle;
using Rental_Application.DataAccessLayer.DataRepository;
using Rental_Application.EntityLayer.LogInLog;
using Rental_Application.EntityLayer.Utility;

namespace Rental_Application.DataAccessLayer.LoginLogRepository
{
    public class LoginLogRepository : ILoginLogRepository
    {
        private readonly IDapper _dapper;

        public LoginLogRepository(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task AddLoginLogAsync(LogInLogModel loginLog)
        {
            using (var connection = _dapper.CreateConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_LOGIN_ID", loginLog.LOGIN_ID, OracleMappingType.Varchar2, ParameterDirection.Input);
                parameters.Add("p_IP", loginLog.IP, OracleMappingType.Varchar2, ParameterDirection.Input);
                parameters.Add("p_TIMEIN", loginLog.LoginTime, OracleMappingType.TimeStamp, ParameterDirection.Input);
                
                await connection.ExecuteReaderAsync(StoreProcedureConstrains.SP_LogInLogInsert, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<LogInLogModel> GetLatestLoginLogAsync(string login_id)
        {
            using (var connection = _dapper.CreateConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_LOGIN_ID", login_id, OracleMappingType.Varchar2, ParameterDirection.Input);
                parameters.Add("p_cursor", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                var result = await connection.QueryAsync<LogInLogModel>(StoreProcedureConstrains.sp_GetLatestLoginLog, parameters, commandType: CommandType.StoredProcedure);

                return result.FirstOrDefault();
            }
        }

        public async Task UpdateLoginLogAsync(LogInLogModel loginLog)
        {
            using (var connection = _dapper.CreateConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("p_LOGIN_ID", loginLog.LOGIN_ID, OracleMappingType.Varchar2, ParameterDirection.Input);
                parameters.Add("p_LogoutTime", loginLog.LogoutTime, OracleMappingType.TimeStamp, ParameterDirection.Input);

                await connection.ExecuteReaderAsync(StoreProcedureConstrains.sp_UpdateLoginLog, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}

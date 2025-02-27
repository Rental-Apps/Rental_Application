using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Oracle;
using Rental_Application.DataAccessLayer.DataRepository;
using Rental_Application.EntityLayer.Utility;

namespace Rental_Application.DataAccessLayer.UserRepository
{
    public class OTP_Repository : IOTP_Repository
    {
        private readonly IDapper _dapper;

        public OTP_Repository(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<string> saveAuthOTP(string otp, string emailId)
        {
            var parameters = new OracleDynamicParameters();
            DateTime timeWithTwoMinAdded = DateTime.Now.AddMinutes(10);

            var status = "Active";
            parameters.Add("p_Email_Id", emailId, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("p_OTP_Code", otp, OracleMappingType.Varchar2, ParameterDirection.Input);
            parameters.Add("p_Expiry_Time", timeWithTwoMinAdded, OracleMappingType.Date, ParameterDirection.Input);
            parameters.Add("p_Status", status, OracleMappingType.Varchar2, ParameterDirection.Input);
            return await _dapper.ExecuteAsync<string>(StoreProcedureConstrains.SaveOTP, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<string> verifyOTP(string emailId,string otp)
        {
            using (var connection = _dapper.CreateConnection())
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("INPUT_OTP", otp, OracleMappingType.Varchar2, ParameterDirection.Input);
                parameters.Add("LOGIN_ID", emailId, OracleMappingType.Varchar2, ParameterDirection.Input);
                parameters.Add("OUT_RESULT", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
                return await _dapper.ExecuteAsync<string>(StoreProcedureConstrains.VerifyOTP, parameters, commandType: CommandType.StoredProcedure);
            }
        }

    }
}

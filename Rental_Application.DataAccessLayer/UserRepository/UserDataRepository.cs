using System.Data;
using Dapper;
using Dapper.Oracle;
using Rental_Application.DataAccessLayer.DataRepository;
using Rental_Application.EntityLayer.UserModel;
using Rental_Application.EntityLayer.Utility;

namespace Rental_Application.DataAccessLayer.UserRepository
{
    public class UserDataRepository : IUserDataRepository
    {
        private readonly IDapper _dapper;

        public UserDataRepository(IDapper dapper)
        {
            _dapper = dapper;
        }



        public async Task<UserModel> GetUserByUsernameAndPasswordAsync(AuthenticateRequest request)
        {
            using (var connection = _dapper.CreateConnection())
            {
                var parameters = new OracleDynamicParameters();

                parameters.Add("PRM_LOGIN_ID", request.Username, OracleMappingType.Varchar2, ParameterDirection.Input);
                parameters.Add("PRM_PASSWORD", request.Password, OracleMappingType.Varchar2, ParameterDirection.Input);
                parameters.Add("PRM_ROLE", request.Role, OracleMappingType.Int32, ParameterDirection.Input);
                parameters.Add("PRM_IP", "1000", OracleMappingType.Varchar2, ParameterDirection.Input);
                parameters.Add("OUT_LOG_ID", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                // Execute stored procedure and get the cursor result
                using (var reader = await connection.ExecuteReaderAsync(StoreProcedureConstrains.ValidateUser, parameters, commandType: CommandType.StoredProcedure))
                {
                    if (reader.Read()) // Ensure there is a result
                    {
                        return new UserModel
                        {
                            Login_Id = reader["USER_ID"].ToString(),
                            Email_Id = reader["EMAIL_ID"].ToString(),
                            First_Name = reader["FNAME"].ToString(),
                            Password = reader["PASSWORD"].ToString(),
                            Role_Id = Convert.ToInt32(reader["ROLE_ID"].ToString())
                        };

                    }

                }
            }

            return null; // Return null if no user is found
        }

        public async Task<UserModel> GetUserById(string username)
        {
            using (var connection = _dapper.CreateConnection())
            {
                var parameters = new OracleDynamicParameters();

                parameters.Add("USERNAME", username, OracleMappingType.Varchar2, ParameterDirection.Input);
                parameters.Add("OUT_LOG_ID", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                // Execute stored procedure and get the cursor result
                using (var reader = await connection.ExecuteReaderAsync(StoreProcedureConstrains.GetUser, parameters, commandType: CommandType.StoredProcedure))
                {
                    if (reader.Read()) // Ensure there is a result
                    {
                        return new UserModel
                        {
                            //LOGIN_ID = reader["LOGIN_ID"].ToString(),
                            //EMAIL_ID = reader["EMAIL_ID"].ToString()
                        };
                    }
                }
            }

            return null; // Return null if no user is found
        }

        #region
        //public async Task<UserModel> GetUserByUsernameAndPasswordAsync(string username, string password)
        //{
        //    using (var connection = _dapper.CreateConnection())
        //    {

        //        var parameters = new DynamicParameters();
        //        parameters.Add("USERNAME", username, DbType.String, ParameterDirection.Input);
        //        parameters.Add("PASSWORD", password, DbType.String, ParameterDirection.Input);
        //        parameters.Add("OUT_LOG_ID", dbType: DbType.Object, direction: ParameterDirection.Output);


        //        await _dapper.ReadQuery<UserModel>(connection, StoreProcedureConstrains.ValidateUser, parameters, CommandType.StoredProcedure);

        //        using (var reader = ((OracleRefCursor)parameters.Get<OracleRefCursor>("OUT_LOG_ID")).GetDataReader())
        //        {

        //            var user = new UserModel
        //            {
        //                LOGIN_ID = reader.GetString(reader.GetOrdinal("LOGIN_ID")),
        //                EMAIL_ID = reader.GetString(reader.GetOrdinal("EMAIL_ID"))
        //            };

        //            return user;

        //        }
        //    }
        //}
        #endregion



    }
}

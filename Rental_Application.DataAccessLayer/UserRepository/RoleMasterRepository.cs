using System.Data;
using Dapper;
using Dapper.Oracle;
using Rental_Application.DataAccessLayer.DataRepository;
using Rental_Application.EntityLayer.RoleMasterModel;
using Rental_Application.EntityLayer.Utility;

namespace Rental_Application.DataAccessLayer.RoleMasterRepository
{
    public class RoleMasterRepository : IRoleMasterRepository
    {
        private readonly IDapper _dapper;

        public RoleMasterRepository(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<RoleMasterModel>> GetRolesAsync()
        {
            var roleMasterList = new List<RoleMasterModel>();
            using (var connection = _dapper.CreateConnection())
            {
                var parameters = new OracleDynamicParameters();

                parameters.Add("outCursor", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

                using (var reader = await connection.ExecuteReaderAsync(StoreProcedureConstrains.SP_RoleMaster, parameters, commandType: CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var roleMaster = new RoleMasterModel
                        {
                            Role_Id = Convert.ToInt32(reader["ROLE_ID"]),
                            RoleName = reader["ROLE"].ToString()
                            //STATUS = Convert.ToBoolean(reader["STATUS"])
                        };
                        roleMasterList.Add(roleMaster);
                    }
                    return roleMasterList;
                }
            }
            return null;
        }
    }
}

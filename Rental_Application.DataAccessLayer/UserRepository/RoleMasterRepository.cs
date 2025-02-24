using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Oracle;
using Dapper;
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
                            ROLE_ID = Convert.ToInt32(reader["ROLE_ID"]),
                            ROLE = reader["ROLE"].ToString(),
                            STATUS = Convert.ToBoolean(reader["STATUS"])
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

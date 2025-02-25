using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Rental_Application.DataAccessLayer.DataRepository
{
    public class DapperContext : IDapper
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection");
        }

        public IDbConnection CreateConnection()
        {

            return new OracleConnection(_connectionString);
        }

        public async Task<int> InsertDeleteUpdate(IDbConnection connection, string storedProcedure, object parameters, CommandType commandType)
        {
            return await connection.ExecuteAsync(storedProcedure, parameters, commandType: commandType);
        }

        public async Task<T> ReadQuery<T>(IDbConnection connection, string storedProcedure, object parameters, CommandType commandType)
        {
            return await connection.QuerySingleOrDefaultAsync<T>(storedProcedure, parameters, commandType: commandType);
        }

        public async Task<T> CheckQuery<T>(IDbConnection connection, string storedProcedure, object parameters, CommandType commandType)
        {
            return await connection.ExecuteScalarAsync<T>(storedProcedure, parameters, commandType: commandType);
        }

        public async Task<IEnumerable<T>> ReadMultipleQuery<T>(IDbConnection connection, string storedProcedure, object parameters, CommandType commandType)
        {
            return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: commandType);
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>)> ReadMultipleQuery<T1, T2, T3>(IDbConnection connection, string storedProcedure, object parameters, CommandType commandType)
        {
            using (var multi = await connection.QueryMultipleAsync(storedProcedure, parameters, commandType: commandType))
            {
                var result1 = await multi.ReadAsync<T1>();
                var result2 = await multi.ReadAsync<T2>();
                var result3 = await multi.ReadAsync<T3>();

                return (result1, result2, result3);
            }
        }


    }
}

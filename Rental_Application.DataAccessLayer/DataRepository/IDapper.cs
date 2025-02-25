using System.Data;

namespace Rental_Application.DataAccessLayer.DataRepository
{
    public interface IDapper
    {
        IDbConnection CreateConnection();
        Task<int> InsertDeleteUpdate(IDbConnection connection, string storedProcedure, object parameters, CommandType commandType);
        Task<T> ReadQuery<T>(IDbConnection connection, string storedProcedure, object parameters, CommandType commandType);
        Task<T> CheckQuery<T>(IDbConnection connection, string storedProcedure, object parameters, CommandType commandType);
        Task<IEnumerable<T>> ReadMultipleQuery<T>(IDbConnection connection, string storedProcedure, object parameters, CommandType commandType);
        Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>)> ReadMultipleQuery<T1, T2, T3>(IDbConnection connection, string storedProcedure, object parameters, CommandType commandType);

    }
}

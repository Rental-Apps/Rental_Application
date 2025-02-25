using Rental_Application.EntityLayer.Utility;

namespace Rental_Application.DataAccessLayer.LogRepository
{
    public interface ITransactionLoggingRepository
    {
        Task<int> CreateLogAsync(TransactionLog transactionlog);
    }
}

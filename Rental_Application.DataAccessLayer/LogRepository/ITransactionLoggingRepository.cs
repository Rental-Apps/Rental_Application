using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rental_Application.EntityLayer.Utility;

namespace Rental_Application.DataAccessLayer.LogRepository
{
    public interface ITransactionLoggingRepository
    {
       Task<int> CreateLogAsync(TransactionLog transactionlog);
    }
}

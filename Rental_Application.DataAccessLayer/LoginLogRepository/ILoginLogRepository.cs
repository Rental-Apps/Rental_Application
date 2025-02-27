using Rental_Application.EntityLayer.LogInLog;

namespace Rental_Application.DataAccessLayer.LoginLogRepository
{
    public interface ILoginLogRepository
    {
        Task AddLoginLogAsync(LogInLogModel loginLog);
        Task<LogInLogModel> GetLatestLoginLogAsync(string login_id);
        Task UpdateLoginLogAsync(LogInLogModel loginLog);
    }
}

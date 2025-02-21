using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rental_Application.DataAccessLayer.LoginLogRepository;
using Rental_Application.DataAccessLayer.LogRepository;
using Rental_Application.DataAccessLayer.UserRepository;
using Rental_Application.EntityLayer.LogInLog;
using Rental_Application.EntityLayer.Response;
using Rental_Application.EntityLayer.Utility;
using Rental_Application.IBusinessAccessLayer.IUserService;


namespace Rental_Appication.BusinessAccessLayer.UserService
{
    public class UserService : IUserService
    {

        private readonly IUserDataRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly ITransactionLoggingRepository _loggingRepository;
        private readonly ILoginLogRepository _loginLogRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUserDataRepository userRepository, ILogger<UserService> logger, ITransactionLoggingRepository loggingRepository, 
            ILoginLogRepository loginLogRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _logger = logger;
            _loggingRepository = loggingRepository;
            _loginLogRepository = loginLogRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> ValidateUser(string username, string password)
        {
            var response = new Response();
            try
            {
                //_loggingRepository.CreateLogAsync(new TransactionLog { UserId = username, LogMessage = "In User Login", LogInTime = DateTime.Now });

                var user = await _userRepository.GetUserByUsernameAndPasswordAsync(username, password);
                if (user != null)
                {
                    response = GenericResponse.CreateSingleResponse(user, "Login successful", "SUCCESS", (int)HttpStatusCode.OK);
                    
                    // Log login details
                    var loginLog = new LogInLogModel
                    {
                        LOGIN_ID = username,
                        IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                        LoginTime = DateTime.Now
                    };
                    await _loginLogRepository.AddLoginLogAsync(loginLog);
                }
                else
                {
                    response = GenericResponse.CreateResponse(new List<Response>(), MessageConstrains.MSG_NOTFOUND, MessageConstrains.FAIL, (int)HttpStatusCode.NotFound);
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error in Login", ex.Message);
                response = GenericResponse.CreateResponse(new List<Response>(), ex.Message, MessageConstrains.FAIL, (int)HttpStatusCode.InternalServerError);
                return response;
            }
        }

        public async Task LogoutUser(string login_id)
        {
            var loginLog = await _loginLogRepository.GetLatestLoginLogAsync(login_id);
            if (loginLog != null)
            {
                loginLog.LogoutTime = DateTime.Now;
                await _loginLogRepository.UpdateLoginLogAsync(loginLog);
            }
        }
    }
}

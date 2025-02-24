using System.Configuration;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rental_Application.DataAccessLayer.LoginLogRepository;
using Rental_Application.DataAccessLayer.LogRepository;
using Rental_Application.DataAccessLayer.UserRepository;
using Rental_Application.EntityLayer.LogInLog;
using Rental_Application.EntityLayer.Response;
using Rental_Application.EntityLayer.UserModel;
using Rental_Application.EntityLayer.Utility;
using Rental_Application.IBusinessAccessLayer.IUserService;


namespace Rental_Appication.BusinessAccessLayer.UserService
{
    public class UserService : IUserService
    {
        private readonly AESHelper _aesHelper;
        private readonly IUserDataRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly ITransactionLoggingRepository _loggingRepository;
        private readonly IJwtService _jwtService;
        private readonly ILoginLogRepository _loginLogRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserDataRepository userRepository, ILogger<UserService> logger, ITransactionLoggingRepository loggingRepository, IJwtService jwtService, IConfiguration configuration, ILoginLogRepository loginLogRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _logger = logger;
            _loggingRepository = loggingRepository;
            _jwtService = jwtService;
            _aesHelper = new AESHelper(configuration["Encryption:Passphrase"] ?? throw new ArgumentNullException("Passphrase not found in appsettings"));
            _loginLogRepository = loginLogRepository;
            _httpContextAccessor = httpContextAccessor;
        }

       

        public async Task<Response> ValidateUser(AuthenticateRequest request)
        {
            var response = new Response();
            try
            {
                //_loggingRepository.CreateLogAsync(new TransactionLog { UserId = username, LogMessage = "In User Login", LogInTime = DateTime.Now });
                //request.Password=_aesHelper.Encrypt(request.Password);
                //_aesHelper.Decrypt(encryptedString);
                var user = await _userRepository.GetUserByUsernameAndPasswordAsync(request);

                if (user != null)
                {
                    //if (user.Password == request.Password)
                    //{
                        var token = _jwtService.GenerateToken(user);
                        var refreshToken = _jwtService.GenerateRefreshToken();

                        // Create a response object with user details and token
                        var result = new
                        {
                            // User = user,  // User details
                            Token = token,
                            RefreshToken = refreshToken
                        };
                        response = GenericResponse.CreateSingleResponse(result, "Login successful", "SUCCESS", (int)HttpStatusCode.OK);
                    //}
                        var loginLog = new LogInLogModel
                        {
                            LOGIN_ID = request.Username,
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


        public async Task<Response> GetUserDetailsById(string username)
        {
            var response = new Response();
            try
            {
                //_loggingRepository.CreateLogAsync(new TransactionLog { UserId = username, LogMessage = "In User Login", LogInTime = DateTime.Now });

                var user = await _userRepository.GetUserById(username);
                if (user != null)
                {
                    response = GenericResponse.CreateSingleResponse(user, "Records found", "SUCCESS", (int)HttpStatusCode.OK);
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

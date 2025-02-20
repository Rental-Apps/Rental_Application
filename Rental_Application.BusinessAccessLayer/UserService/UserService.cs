using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rental_Application.DataAccessLayer.LogRepository;
using Rental_Application.DataAccessLayer.UserRepository;
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
        public UserService(IUserDataRepository userRepository, ILogger<UserService> logger, ITransactionLoggingRepository loggingRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
            _loggingRepository = loggingRepository;
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
    }
}

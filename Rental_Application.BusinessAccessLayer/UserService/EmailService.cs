using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rental_Application.EntityLayer.Response;
using Rental_Application.EntityLayer.Utility;
using Rental_Application.IBusinessAccessLayer.IUserService;
using Rental_Application.EntityLayer.EmailSettingsModel;
using Rental_Application.EntityLayer.UserModel;
using Rental_Application.DataAccessLayer.UserRepository;

namespace Rental_Application.BusinessAccessLayer.UserService
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettingsModel _smtpSettings;
        private readonly IOTP_Repository _otpRepository;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOTP_Repository otp_Repository, IOptions<EntityLayer.EmailSettingsModel.SmtpSettingsModel> smtpSettings)
        {
            _otpRepository = otp_Repository;
            _smtpSettings = smtpSettings.Value;
        }
        //public void SendEmail(string recipientEmail) //, string subject, string body
        public async Task<Response> SendEmail(string recipientEmail)
        {
            var response = new Response();
            try
            {
                //var otpData = new OTPModel();
                var data = await SendEmailAsync(recipientEmail);
                if (data != null)
                {
                    var otpData = await _otpRepository.saveAuthOTP(data.OTP_Code, data.Email);
                    response = GenericResponse.CreateSingleResponse(otpData, "Login successful", "SUCCESS", (int)HttpStatusCode.OK);
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

        public async Task<OTPModel> SendEmailAsync(string recipientEmail)
        {
            var otpModel = new OTPModel();
            try
            {
                var smtpClient = new SmtpClient(_smtpSettings.Server)
                {
                    Port = _smtpSettings.Port,
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = _smtpSettings.EnableSsl,
                    Timeout = _smtpSettings.Timeout, // Set the timeout value
                };
                var otpCode = new Random().Next(100000, 999999).ToString();
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = "Your OTP Mail", //subject
                    Body = $"<h1>{otpCode}</h1>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(recipientEmail);
                smtpClient.Send(mailMessage);
                var otp = new OTPModel
                {
                    Email = recipientEmail,
                    OTP_Code = otpCode,
                    //CreatedAt = DateTime.Now,
                    //ExpiredAt = DateTime.Now.AddMinutes(5)
                };
                return otp;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error in Login", ex.Message);
                return otpModel;
            }
        }
    }
}

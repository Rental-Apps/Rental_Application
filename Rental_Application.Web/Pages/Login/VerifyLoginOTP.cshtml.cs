using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rental_Application.EntityLayer.UserModel;

namespace Rental_Application.Web.Pages.Login
{
    public class VerifyLoginOTPModel : PageModel
    {

        private readonly HttpClient _httpClient;

        public VerifyLoginOTPModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();


        }
        public string Message { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Otp is required")]
        public string Otp { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {


            var token = GetTokenFromCookie();
            if (string.IsNullOrEmpty(token))
            {
                // Handle missing token case
                return RedirectToPage("/Login/Login");
            }
            var otpData1 = decodeToekn(token);
            var otpData = new OTPRequest
            {
                loginId = otpData1.loginId,
                otp_code = Otp

            };

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7036/api/Auth/VerifyOTP", otpData);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiLoginResponse>();
            if (apiResponse.Status == "SUCCESS")
            {
                Message = await response.Content.ReadAsStringAsync();
                return RedirectToPage("/Login/Welcome");
                // Process the result as needed
            }
            else
            {
                // ErrorMessage = "Failed to make authenticated request";
            }

            return Page();
        }

        private string GetTokenFromCookie()
        {
            HttpContext.Request.Cookies.TryGetValue("AuthToken", out var token);
            return token;
        }

        public class OTPRequest
        {
            public string loginId { get; set; }
            public string otp_code { get; set; }
        
        }

        public class ApiLoginResponse
        {
            public List<LoginDataVerifyOTP> Data { get; set; }
            public string Status { get; set; }
            public int StatusCode { get; set; }
            public string Message { get; set; }
        }

        public class LoginDataVerifyOTP
        {
            public string Result { get; set; }
        }

        public OTPRequest decodeToekn(string userToken)
        {
            var tokenData = new OTPRequest();
            string token = userToken; // Replace with your JWT token

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken != null)
            {
                var jwtToken = jsonToken;

                // Get Claims
                var claims = jwtToken.Claims;

                // Extract login ID and password claims (if present)
                var emailId = claims.FirstOrDefault(c => c.Type == "Email_Id")?.Value;
                var password = claims.FirstOrDefault(c => c.Type == "password")?.Value;

                // Print the extracted claims
                Console.WriteLine($"Login ID: {emailId}");
                Console.WriteLine($"Password: {password}");
                tokenData.loginId = emailId;
                return tokenData;
            }
            else
            {
                Console.WriteLine("Invalid JWT token.");
            }
            return tokenData;   
        }
    }
}

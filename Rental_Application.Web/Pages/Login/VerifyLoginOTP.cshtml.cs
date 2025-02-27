using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

            var otpData = new OTPRequest
            {
                loginId = "GIL000000897",
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
            //public List<LoginData> Data { get; set; }
            public string Status { get; set; }
            public int StatusCode { get; set; }
            public string Message { get; set; }
        }

    }
}

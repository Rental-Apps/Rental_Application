using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Rental_Application.Web.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public List<Role> UserRoles { get; set; }


        [BindProperty]
        [Required(ErrorMessage = "Login ID is required")]
        public string LoginId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select a user role")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid user role")]
        public int SelectedRoleId { get; set; }

        public string ErrorMessage { get; set; }
        //public async Task OnGetAsync()
        //{
        //    var response = await _httpClient.GetAsync("https://localhost:7036/api/Auth/GetRole");
        //    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

        //    if (apiResponse.Status == "Success")
        //    {
        //        UserRoles = apiResponse?.Data?[0] ?? new List<Role>();

        //    }
        //    else
        //    {
        //        UserRoles = new List<Role>();

        //    }
        //}
        public async Task OnGetAsync()
        {
            UserRoles = await FetchUserRolesAsync();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            UserRoles = await FetchUserRolesAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var loginData = new AuthenticateRequest
            {
                username = LoginId,
                password = Password,
                role = SelectedRoleId
            };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7036/api/Auth/AuthenticateUser", loginData);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiLoginResponse>();
            if (apiResponse.Status == "SUCCESS")
            {
                //TempData["Token"] = apiResponse.Data[0].Token;
                var token = apiResponse.Data[0].Token;
                var refreshToken = apiResponse.Data[0].RefreshToken; // Assuming refreshToken is included in the response

                // Set the token in an HttpOnly cookie
                HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Ensure the cookie is sent over HTTPS
                    SameSite = SameSiteMode.Strict, // Adjust according to your needs
                    Expires = DateTimeOffset.UtcNow.AddHours(1) // Set cookie expiration time
                });

                // Set the refresh token in an HttpOnly cookie
                HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Ensure the cookie is sent over HTTPS
                    SameSite = SameSiteMode.Strict, // Adjust according to your needs
                    Expires = DateTimeOffset.UtcNow.AddDays(30) // Set refresh token expiration time
                });
                return RedirectToPage("/Login/VerifyLoginOTP");
            }
            else
            {
                ErrorMessage = "Login Failed";
                //ModelState.AddModelError(string.Empty, "Login failed: " + apiResponse.Message);
            }
            return Page();
        }

        private async Task<List<Role>> FetchUserRolesAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7036/api/Auth/GetRole");
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

            if (apiResponse.Status == "Success")
            {
                return apiResponse?.Data?[0] ?? new List<Role>();
            }

            return new List<Role>();
        }


    }
}
public class Role
{
    public int Role_Id { get; set; }
    public string RoleName { get; set; }
}

public class ApiResponse
{
    public List<List<Role>> Data { get; set; }

    public string Status { get; set; }
}

public class AuthenticateRequest
{
    public string username { get; set; }
    public string password { get; set; }
    public int role { get; set; }
}

public class ApiLoginResponse
{
    public List<LoginData> Data { get; set; }
    public string Status { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
}

public class LoginData
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}




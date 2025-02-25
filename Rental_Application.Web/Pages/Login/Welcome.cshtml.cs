using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Rental_Application.Web.Pages
{
    public class WelcomeModel : PageModel
    {

        private readonly HttpClient _httpClient;

        public WelcomeModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();


        }
        public string Message { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {

            var token = GetTokenFromCookie();
            if (string.IsNullOrEmpty(token))
            {
                // Handle missing token case
                return RedirectToPage("/Login/Login");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("https://localhost:7036/api/Auth/WelcomeMessage");
            if (response.IsSuccessStatusCode)
            {
                Message = await response.Content.ReadAsStringAsync();
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

    }

}

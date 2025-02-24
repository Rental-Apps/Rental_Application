using System.Text.RegularExpressions;

namespace Rental_Application.EntityLayer.Utility
{
    public static class PasswordValidator
    {
        public static bool ValidatePassword(string password)
        {
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$");
            return regex.IsMatch(password);
        }
    }
}

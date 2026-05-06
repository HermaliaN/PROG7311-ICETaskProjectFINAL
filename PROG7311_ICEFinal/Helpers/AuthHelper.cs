using Microsoft.AspNetCore.Http;

// Hermalia Naidoo

namespace PROG7311_ICEFinal.Helpers
{
    public static class AuthHelper
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private static HttpContext? HttpContext => _httpContextAccessor?.HttpContext;

        public static bool IsLoggedIn()
        {
            return HttpContext?.Session.GetString("UserRole") != null;
        }

        public static string GetUserRole()
        {
            return HttpContext?.Session.GetString("UserRole") ?? "";
        }

        public static string GetUserName()
        {
            return HttpContext?.Session.GetString("UserName") ?? "";
        }

        public static string GetUserId()
        {
            return HttpContext?.Session.GetString("UserId") ?? "";
        }

        public static void Login(string userId, string role, string name)
        {
            HttpContext?.Session.SetString("UserId", userId);
            HttpContext?.Session.SetString("UserRole", role);
            HttpContext?.Session.SetString("UserName", name);
        }

        public static void Logout()
        {
            HttpContext?.Session.Clear();
        }
    }
}
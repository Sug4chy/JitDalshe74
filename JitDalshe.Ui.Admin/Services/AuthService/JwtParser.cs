using System.Text.Json;

namespace JitDalshe.Ui.Admin.Services.AuthService;

public static class JwtParser
{
    public static string? ParseRoleFromToken(string token)
    {
        try
        {
            var parts = token.Split('.');
            if (parts.Length < 2) return null;
            var payload = parts[1];
            
            payload = payload.Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }
            
            var bytes = Convert.FromBase64String(payload);
            using var jsonDoc = JsonDocument.Parse(bytes);
            
            if (jsonDoc.RootElement.TryGetProperty("role", out var roleProp))
            {
                return roleProp.GetString();
            }
            if (jsonDoc.RootElement.TryGetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var fullRoleProp))
            {
                return fullRoleProp.GetString();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}
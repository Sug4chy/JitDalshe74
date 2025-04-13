using System.Text;

namespace JitDalshe.Infrastructure.Persistence.Extensions;

public static class StringExtensions
{
    public static string ToSnakeCase(this string @string)
    {
        ArgumentNullException.ThrowIfNull(@string, nameof(@string));
        if (@string.Length < 2)
        {
            return @string.ToLowerInvariant();
        }

        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(@string[0]));
        for (int i = 1; i < @string.Length; i++)
        {
            char c = @string[i];
            if (char.IsUpper(c))
            {
                sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}
using System.Text.RegularExpressions;

namespace MRCD.Domain.Common;

internal static partial class ContactRules
{
    [GeneratedRegex(@"^[\p{L}]+$")]
    private static partial Regex Letters();

    [GeneratedRegex("^[0-9]+$")]
    private static partial Regex Digits();

    public static bool IsName(string value) => !string.IsNullOrWhiteSpace(value) && Letters().IsMatch(value);
    public static bool IsPhone(string? value) => string.IsNullOrWhiteSpace(value) || Digits().IsMatch(value.Trim());
}

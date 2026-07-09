using System.Text;
using System.Text.RegularExpressions;

namespace MRCD.Application.Services.CommonService;

internal sealed partial class CommonService : ICommonService
{
    [GeneratedRegex(@"^[\p{L}]+$")]
    private static partial Regex OnlyLettersRegex();
    [GeneratedRegex(@"^[\p{L} ]+$")]
    private static partial Regex OnlyLettersWithSpacesRegex();
    [GeneratedRegex("^[0-9]+$")]
    private static partial Regex OnlyNumbersRegex();
    [GeneratedRegex("^[A-Za-z]+\\.[A-Za-z]+$")]
    private static partial Regex PermissionRegex();

    public bool HasOnlyLetters(
        string text
    ) => OnlyLettersRegex()
        .IsMatch(text);

    public bool HasOnlyLettersWithSpaces(
        string text
    ) => OnlyLettersWithSpacesRegex()
        .IsMatch(text);

    public bool HasOnlyNumbers(
        string text
    ) => OnlyNumbersRegex()
        .IsMatch(text);

    public bool IsValidPermission(
        string permission
    ) => !string.IsNullOrWhiteSpace(permission)
        && PermissionRegex().IsMatch(permission);

    public string NormalizeString(
        string text
    )
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;
        var normalized = text.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var result = new StringBuilder();

        foreach (var character in normalized)
        {
            var category = char.GetUnicodeCategory(character);
            if (
                category != System.Globalization.UnicodeCategory.NonSpacingMark
                && !char.IsWhiteSpace(character)
            ) result.Append(character);
        }
        return result.ToString().Normalize(NormalizationForm.FormC);
    }
}
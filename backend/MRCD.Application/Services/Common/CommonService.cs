using System.Text;
using System.Text.RegularExpressions;

namespace MRCD.Application.Services.Common;

internal sealed partial class CommonService : ICommonService
{
    [GeneratedRegex(@"^[\p{L}]+$")]
    private static partial Regex OnlyLettersRegex();
    [GeneratedRegex(@"^[\p{L} ]+$")]
    private static partial Regex OnlyLettersWithSpacesRegex();

    public bool HasOnlyLetters(
        string text
    ) => OnlyLettersRegex()
        .IsMatch(text);

    public bool HasOnlyLettersWithSpaces(
        string text
    ) => OnlyLettersWithSpacesRegex()
        .IsMatch(text);

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
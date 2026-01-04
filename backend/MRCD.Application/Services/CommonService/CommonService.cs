using System.Text;
using System.Text.RegularExpressions;

namespace MRCD.Application.Services.CommonService;

internal sealed partial class CommonService : ICommonService
{
    [GeneratedRegex("^[A-Za-z]+$")]
    private static partial Regex OnlyLettersRegex();
    [GeneratedRegex("^[0-9]+$")]
    private static partial Regex OnlyNumbersRegex();

    public bool HasOnlyLetters(
        string text
    ) => OnlyLettersRegex()
        .IsMatch(text);

    public bool HasOnlyNumbers(
        string text
    ) => OnlyNumbersRegex()
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
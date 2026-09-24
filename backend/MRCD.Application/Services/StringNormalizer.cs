using System.Text;

namespace MRCD.Application.Services;

internal static class StringNormalizer
{
    public static string NormalizeString(
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
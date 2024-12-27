using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace api.Common;

public interface ICommonFunctions
{
    string GetNormalizedText(string text);
}

public partial class Functions : ICommonFunctions
{
    [GeneratedRegex("[^a-z]")]
    private static partial Regex LettersOnlyRegex();

    public string GetNormalizedText(
        string text
    )
    {
        string decomposed = text.ToLowerInvariant().Normalize(NormalizationForm.FormD);

        StringBuilder sb = new();
        foreach (char c in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        string recomposed = sb.ToString().Normalize(NormalizationForm.FormC);
        return LettersOnlyRegex().Replace(recomposed, "");
    }
}
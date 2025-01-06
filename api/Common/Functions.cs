using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace api.Common;

public interface ICommonService
{
    string GetNormalizedText(string text);
    string GetHashedString(string text);
}

public partial class CommonService : ICommonService
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

    /// <summary>
    /// Devuelve el hash de una cadena para almacenarlo en campos comparativos
    /// </summary>
    /// <param name="text">El texto no debe estar normalizado, se hace internamente :3</param>
    /// <returns></returns>
    public string GetHashedString(
        string text
    )
    {
        var normalized = GetNormalizedText(text);
        byte[] bytes = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    }
}
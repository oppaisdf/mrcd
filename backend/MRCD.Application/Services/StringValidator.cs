using System.Text.RegularExpressions;

namespace MRCD.Application.Services;

internal static partial class StringValidator
{
    [GeneratedRegex(@"^[\p{L}]+$")]
    private static partial Regex OnlyLettersRegex();
    [GeneratedRegex(@"^[\p{L} ]+$")]
    private static partial Regex OnlyLettersWithSpacesRegex();

    public static bool HasOnlyLetters(
        string text
    ) => OnlyLettersRegex()
        .IsMatch(text);

    public static bool HasOnlyLettersWithSpaces(
        string text
    ) => OnlyLettersWithSpacesRegex()
        .IsMatch(text);
}
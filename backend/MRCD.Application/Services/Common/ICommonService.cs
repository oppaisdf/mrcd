namespace MRCD.Application.Services.Common;

public interface ICommonService
{
    /// <summary>
    ///  Devuelve cadena normalizada sin espacios, en minúscula y sin números
    /// </summary>
    /// <param name="text"></param>
    /// <returns>string</returns>
    string NormalizeString(string text);
    bool HasOnlyLetters(string text);
    bool HasOnlyLettersWithSpaces(string text);
    bool HasOnlyNumbers(string text);
    bool IsValidPermission(string permission);
}
namespace MRCD.Application.Services.CommonService;

public interface ICommonService
{
    /// <summary>
    ///  Devuelve cadena normalizada sin espacios, en minúscula y sin números
    /// </summary>
    /// <param name="text"></param>
    /// <returns>string</returns>
    string NormalizeString(string text);
    bool HasOnlyLetters(string text);
}
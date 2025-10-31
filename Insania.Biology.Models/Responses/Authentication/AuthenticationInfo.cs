using Insania.Shared.Models.Responses.Base;

namespace Insania.Biology.Models.Responses.Authentication;

/// <summary>
/// Модель информации об аутентификации
/// </summary>
public class AuthenticationInfo(bool success, string? token) : BaseResponse(success)
{
    #region Конструкторы
    /// <summary>
    /// Конструктор для десериализации
    /// </summary>
    public AuthenticationInfo() : this(false, null) { }
    #endregion

    #region Поля
    /// <summary>
    /// Токен доступа
    /// </summary>
    public string? Token { get; set; } = token;
    #endregion
}
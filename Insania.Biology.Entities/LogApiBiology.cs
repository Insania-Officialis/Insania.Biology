using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

namespace Insania.Biology.Entities;

/// <summary>
/// Модель сущности лога сервиса биологии
/// </summary>
[Table("r_logs_api_biology")]
[Comment("Логи сервиса биологии")]
public class LogApiBiology : Log
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности лога сервиса биологии
    /// </summary>
    public LogApiBiology() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности лога сервиса биологии без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string" name="method">Наименование вызываемого метода</param>
    /// <param cref="string" name="type">Тип вызываемого метода</param>
    /// <param cref="string" name="dataIn">Данные на вход</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public LogApiBiology(string username, bool isSystem, string method, string type, string? dataIn = null, DateTime? dateDeleted = null) : base(username, isSystem, method, type, dataIn, dateDeleted)
    {

    }

    /// <summary>
    /// Конструктор модели сущности лога сервиса биологии с идентификатором
    /// </summary>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="string" name="method">Наименование вызываемого метода</param>
    /// <param cref="string" name="type">Тип вызываемого метода</param>
    /// <param cref="string" name="dataIn">Данные на вход</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public LogApiBiology(long id, string username, bool isSystem, string method, string type, string? dataIn = null, DateTime? dateDeleted = null) : base(id, username, isSystem, method, type, dataIn, dateDeleted)
    {

    }
    #endregion
}
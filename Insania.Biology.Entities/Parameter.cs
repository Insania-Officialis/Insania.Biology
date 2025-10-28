using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Biology.Entities;

/// <summary>
/// Модель сущности параметра биологии
/// </summary>
[Table("c_parameters")]
[Comment("Параметры биологии")]
public class ParameterBiology: Parameter
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности параметра биологии
    /// </summary>
    public ParameterBiology() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности параметра биологии без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="name">Значение параметра</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public ParameterBiology(ITransliterationSL transliteration, string username, string name, string? value = null, DateTime? dateDeleted = null) : base(transliteration, username, name, value, dateDeleted)
    {

    }

    /// <summary>
    /// Конструктор модели сущности параметра биологии с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string?" name="value">Значение параметра</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public ParameterBiology(ITransliterationSL transliteration, long id, string username, string name, string? value = null, DateTime? dateDeleted = null) : base(transliteration, id, username, name, value, dateDeleted)
    {

    }
    #endregion
}
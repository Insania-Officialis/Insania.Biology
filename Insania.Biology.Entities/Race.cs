using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Biology.Entities;

/// <summary>
/// Модель сущности расы
/// </summary>
[Table("d_races")]
[Comment("Расы")]
public class Race : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности расы
    /// </summary>
    public Race() : base()
    {
        Description = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности расы без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="description">Описание</param>
    /// <param cref="string" name="maxAge">Максимальный возраст в циклах</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Race(ITransliterationSL transliteration, string username, string name, string description, int? maxAge = null, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {
        Description = description;
        MaxAge = maxAge;
    }

    /// <summary>
    /// Конструктор модели сущности расы с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="description">Описание</param>
    /// <param cref="string" name="maxAge">Максимальный возраст в циклах</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Race(ITransliterationSL transliteration, long id, string username,string name, string description, int? maxAge = null, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {
        Description = description;
        MaxAge = maxAge;
    }
    #endregion

    #region Поля
    /// <summary>
    ///	Описание
    /// </summary>
    [Column("description")]
    [Comment("Описание")]
    public string Description { get; private set; }

    /// <summary>
    ///	Максимальный возраст в циклах
    /// </summary>
    [Column("max_age")]
    [Comment("Максимальный возраст в циклах")]
    public int? MaxAge { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи описания
    /// </summary>
    /// <param cref="string" name="description">Описание</param>
    public void SetDescription(string description)
    {
        Description = description;
    }

    /// <summary>
    /// Метод записи максимального возраста в циклах
    /// </summary>
    /// <param cref="string" name="maxAge">Максимальный возраст в циклах</param>
    public void SetMaxAge(int? maxAge)
    {
        MaxAge = maxAge;
    }
    #endregion
}
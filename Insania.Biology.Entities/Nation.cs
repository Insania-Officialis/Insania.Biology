using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Biology.Entities;

/// <summary>
/// Модель сущности нации
/// </summary>
[Table("r_nations")]
[Comment("Нации")]
public class Nation : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности нации
    /// </summary>
    public Nation() : base()
    {
        Description = string.Empty;
        LanguageForPersonalNames = string.Empty;
        RaceEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности нации без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="description">Описание</param>
    /// <param cref="string" name="languageForPersonalNames">Язык для имён</param>
    /// <param cref="Race" name="race">Раса</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Nation(ITransliterationSL transliteration, string username, string name, string description, string languageForPersonalNames, Race race, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {
        Description = description;
        LanguageForPersonalNames = languageForPersonalNames;
        RaceId = race.Id;
        RaceEntity = race;
    }

    /// <summary>
    /// Конструктор модели сущности нации с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="description">Описание</param>
    /// <param cref="string" name="languageForPersonalNames">Язык для имён</param>
    /// <param cref="Race" name="race">Раса</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Nation(ITransliterationSL transliteration, long id, string username, string name, string description, string languageForPersonalNames, Race race, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {
        Description = description;
        LanguageForPersonalNames = languageForPersonalNames;
        RaceId = race.Id;
        RaceEntity = race;
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
    ///	Язык для имён
    /// </summary>
    [Column("language_for_personal_names")]
    [Comment("Язык для имён")]
    public string LanguageForPersonalNames { get; private set; }

    /// <summary>
    ///	Идентификатор расы
    /// </summary>
    [Column("race_id")]
    [Comment("Идентификатор расы")]
    public long RaceId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство расы
    /// </summary>
    public Race RaceEntity { get; private set; }
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
    /// Метод записи языка для имён
    /// </summary>
    /// <param cref="string" name="languageForPersonalNames">Язык для имён</param>
    public void SetLanguageForPersonalNames(string languageForPersonalNames)
    {
        LanguageForPersonalNames = languageForPersonalNames;
    }

    /// <summary>
    /// Метод записи расы
    /// </summary>
    /// <param cref="Race" name="race">Раса</param>
    public void SetRace(Race race)
    {
        RaceId = race.Id;
        RaceEntity = race;
    }
    #endregion
}
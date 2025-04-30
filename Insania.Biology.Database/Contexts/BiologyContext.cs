using Microsoft.EntityFrameworkCore;

using Insania.Biology.Entities;

namespace Insania.Biology.Database.Contexts;

/// <summary>
/// Контекст бд биологии
/// </summary>
public class BiologyContext : DbContext
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор контекста бд биологии
    /// </summary>
    public BiologyContext() : base()
    {

    }

    /// <summary>
    /// Конструктор контекста бд биологии с опциями
    /// </summary>
    /// <param cref="DbContextOptions{BiologyContext}" name="options">Параметры</param>
    public BiologyContext(DbContextOptions<BiologyContext> options) : base(options)
    {

    }
    #endregion

    #region Поля
    /// <summary>
    /// Расы
    /// </summary>
    public virtual DbSet<Race> Races { get; set; }

    /// <summary>
    /// Нации
    /// </summary>
    public virtual DbSet<Nation> Nations { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод при создании моделей
    /// </summary>
    /// <param cref="ModelBuilder" name="modelBuilder">Конструктор моделей</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Установка схемы бд
        modelBuilder.HasDefaultSchema("insania_biology");

        //Создание ограничения уникальности на псевдоним расы
        modelBuilder.Entity<Race>().HasAlternateKey(x => x.Alias);

        //Создание ограничения уникальности на псевдоним нации
        modelBuilder.Entity<Nation>().HasAlternateKey(x => x.Alias);
    }
    #endregion
}
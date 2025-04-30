using Microsoft.EntityFrameworkCore;

using Insania.Biology.Entities;

namespace Insania.Biology.Database.Contexts;

/// <summary>
/// Контекст бд логов сервиса биологии
/// </summary>
public class LogsApiBiologyContext : DbContext
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор контекста бд логов сервиса биологии
    /// </summary>
    public LogsApiBiologyContext() : base()
    {

    }

    /// <summary>
    /// Конструктор контекста бд логов сервиса биологии с опциями
    /// </summary>
    /// <param cref="DbContextOptions{LogsApiBiologyContext}" name="options">Параметры</param>
    public LogsApiBiologyContext(DbContextOptions<LogsApiBiologyContext> options) : base(options)
    {

    }
    #endregion

    #region Поля
    /// <summary>
    /// Пользователи
    /// </summary>
    public virtual DbSet<LogApiBiology> Logs { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод при создании моделей
    /// </summary>
    /// <param cref="ModelBuilder" name="modelBuilder">Конструктор моделей</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Установка схемы бд
        modelBuilder.HasDefaultSchema("insania_logs_api_biology");
        
        //Добавление gin-индекса на поле с входными данными логов
        modelBuilder.Entity<LogApiBiology>().HasIndex(x => x.DataIn).HasMethod("gin");

        //Добавление gin-индекса на поле с выходными данными логов
        modelBuilder.Entity<LogApiBiology>().HasIndex(x => x.DataOut).HasMethod("gin");
    }
    #endregion
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using Insania.Shared.Contracts.DataAccess;
using Insania.Shared.Contracts.Services;
using Insania.Shared.Services;

using Insania.Biology.BusinessLogic;
using Insania.Biology.DataAccess;
using Insania.Biology.Database.Contexts;
using Insania.Biology.Models.Settings;
using Insania.Biology.Models.Mapper;

namespace Insania.Biology.Tests.Base;

/// <summary>
/// Базовый класс тестирования
/// </summary>
public abstract class BaseTest
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор базового класса тестирования
    /// </summary>
    public BaseTest()
    {
        //Создание коллекции сервисов
        IServiceCollection services = new ServiceCollection();

        //Создание коллекции ключей конфигурации
        Dictionary<string, string> configurationKeys = new()
        {
           {"LoggingOptions:FilePath", DetermineLogPath()},
           {"InitializationDataSettings:ScriptsPath", DetermineScriptsPath()},
           {"InitializationDataSettings:InitStructure", "false"},
           {"InitializationDataSettings:Tables:Races", "true"},
           {"InitializationDataSettings:Tables:Nations", "true"}
        };

        //Создание экземпляра конфигурации в памяти
        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(configurationKeys!).Build();

        //Установка игнорирования типов даты и времени
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        //Внедрение зависимостей сервисов
        services.AddSingleton(_ => configuration); //конфигурация
        services.AddScoped<ITransliterationSL, TransliterationSL>(); //сервис транслитерации
        services.AddScoped<IInitializationDAO, InitializationDAO>(); //сервис инициализации данных в бд биологии
        services.AddBiologyBL(); //сервисы работы с бизнес-логикой в зоне биологии

        //Добавление контекстов бд в коллекцию сервисов
        services.AddDbContext<BiologyContext>(options => options.UseInMemoryDatabase(databaseName: "insania_biology").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))); //бд пользователей
        services.AddDbContext<LogsApiBiologyContext>(options => options.UseInMemoryDatabase(databaseName: "insania_logs_api_biology").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))); //бд логов сервиса пользователей

        //Добавление параметров логирования
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File(path: configuration["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
            .WriteTo.Debug()
            .CreateLogger();
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

        //Добавление параметров преобразования моделей
        services.AddAutoMapper(cfg => { cfg.AddProfile<BiologyMappingProfile>(); });

        //Добавление параметров инициализации данных
        IConfigurationSection? initializationDataSettings = configuration.GetSection("InitializationDataSettings");
        services.Configure<InitializationDataSettings>(initializationDataSettings);

        //Создание поставщика сервисов
        ServiceProvider = services.BuildServiceProvider();

        //Выполнение инициализации данных
        IInitializationDAO initialization = ServiceProvider.GetRequiredService<IInitializationDAO>();
        initialization.Initialize().Wait();
    }
    #endregion

    #region Поля
    /// <summary>
    /// Поставщик сервисов
    /// </summary>
    protected IServiceProvider ServiceProvider { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод определения пути для логов
    /// </summary>
    /// <returns cref="string">Путь для сохранения логов</returns>
    private static string DetermineLogPath()
    {
        //Проверка запуска в докере
        bool isRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true" || File.Exists("/.dockerenv");

        //Возврат нужного пути
        if (isRunningInDocker) return "/logs/log.txt";
        else return "G:\\Program\\Insania\\Logs\\Biology.Tests\\log.txt";
    }

    /// <summary>
    /// Метод определения пути для скриптов
    /// </summary>
    /// <returns cref="string">Путь к скриптам</returns>
    private static string DetermineScriptsPath()
    {
        //Проверка запуска в докере
        bool isRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true" || File.Exists("/.dockerenv");

        if (isRunningInDocker) return "/src/Insania.Biology.Database/Scripts";
        else return "G:\\Program\\Insania\\Insania.Biology\\Insania.Biology.Database\\Scripts";
    }
    #endregion
}
}
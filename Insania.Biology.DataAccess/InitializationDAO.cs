using System.Text.RegularExpressions;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Npgsql;

using Insania.Shared.Contracts.DataAccess;
using Insania.Shared.Contracts.Services;

using Insania.Biology.Database.Contexts;
using Insania.Biology.Entities;
using Insania.Biology.Models.Settings;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;
using InformationMessages = Insania.Shared.Messages.InformationMessages;

using ErrorMessagesBiology = Insania.Biology.Messages.ErrorMessages;


namespace Insania.Biology.DataAccess;

/// <summary>
/// Сервис инициализации данных в бд биологии
/// </summary>
/// <param cref="ILogger{InitializationDAO}" name="logger">Сервис логгирования</param>
/// <param cref="BiologyContext" name="biologyContext">Контекст базы данных пользователей</param>
/// <param cref="LogsApiBiologyContext" name="logsApiBiologyContext">Контекст базы данных логов сервиса пользователей</param>
/// <param cref="IOptions{InitializationDataSettings}" name="settings">Параметры инициализации данных</param>
/// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
/// <param cref="IConfiguration" name="configuration">Конфигурация приложения</param>
public class InitializationDAO(ILogger<InitializationDAO> logger, BiologyContext biologyContext, LogsApiBiologyContext logsApiBiologyContext, IOptions<InitializationDataSettings> settings, ITransliterationSL transliteration, IConfiguration configuration) : IInitializationDAO
{
    #region Поля
    private readonly string _username = "initializer";
    #endregion

    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<InitializationDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных пользователей
    /// </summary>
    private readonly BiologyContext _biologyContext = biologyContext;

    /// <summary>
    /// Контекст базы данных логов сервиса пользователей
    /// </summary>
    private readonly LogsApiBiologyContext _logsApiBiologyContext = logsApiBiologyContext;

    /// <summary>
    /// Параметры инициализации данных
    /// </summary>
    private readonly IOptions<InitializationDataSettings> _settings = settings;

    /// <summary>
    /// Сервис транслитерации
    /// </summary>
    private readonly ITransliterationSL _transliteration = transliteration;

    /// <summary>
    /// Конфигурация приложения
    /// </summary>
    private readonly IConfiguration _configuration = configuration;
    #endregion

    #region Методы
    /// <summary>
    /// Метод инициализации данных
    /// </summary>
    /// <exception cref="Exception">Исключение</exception>
    public async Task Initialize()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredInitializeMethod);

            //Инициализация структуры
            if (_settings.Value.InitStructure == true)
            {
                //Логгирование
                _logger.LogInformation("{text}", InformationMessages.InitializationStructure);

                //Инициализация баз данных в зависимости от параметров
                if (_settings.Value.Databases?.Biology == true)
                {
                    //Формирование параметров
                    string connectionServer = _configuration.GetConnectionString("BiologySever") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternDatabases = @"^databases_biology_\d+\.sql$";
                    string connectionDatabase = _configuration.GetConnectionString("BiologyEmpty") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternSchemes = @"^schemes_biology_\d+\.sql$";

                    //Создание базы данных
                    await CreateDatabase(connectionServer, patternDatabases, connectionDatabase, patternSchemes);
                }
                if (_settings.Value.Databases?.LogsApiBiology == true)
                {
                    //Формирование параметров
                    string connectionServer = _configuration.GetConnectionString("LogsApiBiologyServer") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternDatabases = @"^databases_logs_api_biology_\d+\.sql$";
                    string connectionDatabase = _configuration.GetConnectionString("LogsApiBiologyEmpty") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternSchemes = @"^schemes_logs_api_biology_\d+\.sql$";

                    //Создание базы данных
                    await CreateDatabase(connectionServer, patternDatabases, connectionDatabase, patternSchemes);
                }

                //Выход
                return;
            }

            //Накат миграций
            if (_biologyContext.Database.IsRelational()) await _biologyContext.Database.MigrateAsync();
            if (_logsApiBiologyContext.Database.IsRelational()) await _logsApiBiologyContext.Database.MigrateAsync();

            //Проверки
            if (string.IsNullOrWhiteSpace(_settings.Value.ScriptsPath)) throw new Exception(ErrorMessagesShared.EmptyScriptsPath);

            //Инициализация данных в зависимости от параметров
            if (_settings.Value.Tables?.Races == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _biologyContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции сущностей
                    List<Race> entities =
                    [
                        new(_transliteration, 1, _username, "Ихтид", "Ихтиды - "),
                        new(_transliteration, 2, _username, "Древний", "Древние - ", null, DateTime.UtcNow),
                        new(_transliteration, 3, _username, "Наг", "Наги - ", null, DateTime.UtcNow),
                        new(_transliteration, 4, _username, "Мраат", "Мрааты - "),
                        new(_transliteration, 5, _username, "Человек", "Люди - "),
                        new(_transliteration, 6, _username, "Вампир", "Вампиры - "),
                        new(_transliteration, 7, _username, "Эльф", "Эльфы - "),
                        new(_transliteration, 8, _username, "Метаморф", "Метаморфы - "),
                        new(_transliteration, 9, _username, "Орк", "Орки - "),
                        new(_transliteration, 10, _username, "Дварф", "Дварфы - "),
                        new(_transliteration, 11, _username, "Тролль", "Тролли - "),
                        new(_transliteration, 12, _username, "Гоблин", "Гоблины - "),
                        new(_transliteration, 13, _username, "Огр", "Огры - "),
                        new(_transliteration, 14, _username, "Альв", "Альвы - "),
                        new(_transliteration, 15, _username, "Антропозавр", "Антропозавры - "),
                        new(_transliteration, 16, _username, "Элвин", "Элвины - "),
                        new(_transliteration, 17, _username, "Дану", "Дану - "),
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_biologyContext.Races.Any(x => x.Id == entity.Id)) await _biologyContext.Races.AddAsync(entity);
                    }

                    //Сохранение изменений в бд
                    await _biologyContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.Nations == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _biologyContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "Истинный ихтид", "Истинные ихтиды - ", "Хинди", "1", DateTime.UtcNow.ToString()],
                        ["2", "Отвергнутый ихтид", "Отвергнутые ихтиды - ", "Хинди", "1", ""],
                        ["3", "Древний", "Древние - ", "Латынь", "2", ""],
                        ["4", "Наг", "Наги - ", "Латынь", "3", ""],
                        ["5", "Дикий мраат", "Дикие мрааты - ", "Исландский", "4", ""],
                        ["6", "Цивилизованный мраат", "Цивилизованные мрааты - ", "Исландский", "4", ""],
                        ["7", "Лисциец", "Лисцийцы - ", "Итальянский", "5", ""],
                        ["8", "Рифут", "Рифуты - ", "Итальянский", "5", ""],
                        ["9", "Ластат", "Ластаты - ", "Итальянский", "5", ""],
                        ["10", "Дестинец", "Дестинцы - ", "Итальянский", "5", ""],
                        ["11", "Илмариец", "Илмарийцы - ", "Итальянский", "5", ""],
                        ["12", "Асуд", "Асуды - ", "Итальянский", "5", ""],
                        ["13", "Вальтирец", "Вальтирцы - ", "Итальянский", "5", ""],
                        ["14", "Саорсин", "Саорсины - ", "Ирландский", "5", ""],
                        ["15", "Теоранец", "Теоранцы - ", "Ирландский", "5", ""],
                        ["16", "Анкостец", "Анкостцы - ", "Ирландский", "5", ""],
                        ["17", "Тавалинец", "Тавалинцы - ", "Эстонский", "5", ""],
                        ["18", "Иглессиец", "Иглессийцы - ", "Литовский", "5", ""],
                        ["19", "Плекиец", "Плекийцы - ", "Литовский", "5", ""],
                        ["20", "Сиервин", "Сиервины - ", "Литовский", "5", ""],
                        ["21", "Виегиец", "Виегийцы - ", "Литовский", "5", ""],
                        ["22", "Западный вампир", "Западные вампиры - ", "Шотландский", "6", ""],
                        ["23", "Восточный вампир", "Восточные вампиры - ", "Шотландский", "6", ""],
                        ["24", "Высший эльф", "Высшие эльфы - ", "Французский", "7", ""],
                        ["25", "Ночной эльф", "Ночные эльфы - ", "Французский", "7", ""],
                        ["26", "Кровавый эльф", "Кровавые эльфы - ", "Французский", "7", ""],
                        ["27", "Лесной эльф", "Лесные эльфы - ", "Французский", "7", ""],
                        ["28", "Горный эльф", "Горные эльфы - ", "Французский", "7", ""],
                        ["29", "Речной эльф", "Речные эльфы - ", "Французский", "7", ""],
                        ["30", "Солнечный эльф", "Солнечные эльфы - ", "Французский", "7", ""],
                        ["31", "Морской эльф", "Морские эльфы - ", "Французский", "7", ""],
                        ["32", "Волчий метаморф", "Волчьи метаморфы - ", "Эсперанто", "8", ""],
                        ["33", "Медвежий метаморф", "Медвежьи метаморфы - ", "Эсперанто", "8", ""],
                        ["34", "Кошачий метаморф", "Кошачьи метаморфы - ", "Эсперанто", "8", ""],
                        ["35", "Серый орк", "Серые орки - ", "Норвежский", "9", ""],
                        ["36", "Чёрный орк", "Чёрные орки - ", "Норвежский", "9", ""],
                        ["37", "Зелёный орк", "Зелёные орки - ", "Норвежский", "9", ""],
                        ["38", "Белый орк", "Белые орки - ", "Норвежский", "9", ""],
                        ["39", "Южный орк", "Южные орки - ", "Норвежский", "9", ""],
                        ["40", "Баккер", "Баккеры - ", "Немецкий", "10", ""],
                        ["41", "Нордерец", "Нордерцы - ", "Немецкий", "10", ""],
                        ["42", "Вервирунгец", "Вервирунгцы - ", "Немецкий", "10", ""],
                        ["43", "Шмид", "Шмиды - ", "Немецкий", "10", ""],
                        ["44", "Кригер", "Кригеры - ", "Немецкий", "10", ""],
                        ["45", "Куфман", "Куфманы - ", "Немецкий", "10", ""],
                        ["46", "Горный тролль", "Горные тролли - ", "Шведский", "11", ""],
                        ["47", "Снежный тролль", "Снежные тролли - ", "Шведский", "11", ""],
                        ["48", "Болотный тролль", "Болотные тролли - ", "Шведский", "11", ""],
                        ["49", "Лесной тролль", "Лесные тролли - ", "Шведский", "11", ""],
                        ["50", "Удстирец", "Удстирцы - ", "Датский", "12", ""],
                        ["51", "Фискирец", "Фискирйцы - ", "Датский", "12", ""],
                        ["52", "Монт", "Монты - ", "Датский", "12", ""],
                        ["53", "Огр", "Огры - ", "Датский", "13", ""],
                        ["54", "Альв", "Альвы - ", "Исландский", "14", ""],
                        ["55", "Антропозавр", "Антропозавры - ", "Латынь", "15", ""],
                        ["56", "Элвин", "Элвины - ", "Эсперанто", "16", ""],
                        ["57", "Дану", "Дану - ", "Французский", "17", ""]
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_biologyContext.Nations.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            Race race = await _biologyContext.Races.FirstOrDefaultAsync(x => x.Id == long.Parse(key[4])) ?? throw new Exception(ErrorMessagesBiology.NotFoundRace);

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[5])) dateDeleted = DateTime.Parse(key[5]);
                            Nation entity = new(_transliteration, long.Parse(key[0]), _username, key[1], key[2], key[3], race, dateDeleted);

                            //Добавление сущности в бд
                            await _biologyContext.Nations.AddAsync(entity);
                        }
                    }

                    //Сохранение изменений в бд
                    await _biologyContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }

    /// <summary>
    /// Метод создание базы данных
    /// </summary>
    /// <param name="connectionServer">Строка подключения к серверу</param>
    /// <param name="patternDatabases">Шаблон файлов создания базы данных</param>
    /// <param name="connectionDatabase">Строка подключения к базе данных</param>
    /// <param name="patternSchemes">Шаблон файлов создания схемы</param>
    /// <returns></returns>
    private async Task CreateDatabase(string connectionServer, string patternDatabases, string connectionDatabase, string patternSchemes)
    {
        //Проход по всем скриптам в директории и создание баз данных
        foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), patternDatabases)))
        {
            //Выполнение скрипта
            await ExecuteScript(file, connectionServer);
        }

        //Проход по всем скриптам в директории и создание схем
        foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), patternSchemes)))
        {
            //Выполнение скрипта
            await ExecuteScript(file, connectionDatabase);
        }
    }

    /// <summary>
    /// Метод выполнения скрипта со строкой подключения
    /// </summary>
    /// <param cref="string" name="filePath">Путь к скрипту</param>
    /// <param cref="string" name="connectionString">Строка подключения</param>
    private async Task ExecuteScript(string filePath, string connectionString)
    {
        //Логгирование
        _logger.LogInformation("{text} {params}", InformationMessages.ExecuteScript, filePath);

        try
        {
            //Создание соединения к бд
            using NpgsqlConnection connection = new(connectionString);

            //Открытие соединения
            connection.Open();

            //Считывание запроса
            string sql = File.ReadAllText(filePath);

            //Создание sql-запроса
            using NpgsqlCommand command = new(sql, connection);

            //Выполнение команды
            await command.ExecuteNonQueryAsync();

            //Логгирование
            _logger.LogInformation("{text} {params}", InformationMessages.ExecutedScript, filePath);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {params} из-за ошибки {ex}", ErrorMessagesShared.NotExecutedScript, filePath, ex);
        }
    }
    #endregion
}
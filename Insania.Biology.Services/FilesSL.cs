using Insania.Biology.Contracts.DataAccess;
using Insania.Biology.Contracts.Services;
using Insania.Biology.Entities;
using Insania.Biology.Messages;
using Insania.Biology.Models.Responses.Authentication;
using Insania.Shared.Models.Responses.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ErrorMessagesBiology = Insania.Biology.Messages.ErrorMessages;
using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Biology.Services;

/// <summary>
/// Сервис работы с файлами
/// </summary>
/// <param cref="ILogger{RacesBL}" name="logger">Сервис логгирования</param>
/// <param cref="IMemoryCache" name="cache">Сервис кэширования</param>
/// <param cref="IHttpClientFactory" name="httpClientFactory">Фабрика создания http-запросов</param>
/// <param cref="IParametersDAO" name="parametersDAO">Сервис работы с данными параметров</param>
public class FilesSL(ILogger<FilesSL> logger, IMemoryCache cache, IHttpClientFactory httpClientFactory, IParametersDAO parametersDAO) : IFilesSL
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<FilesSL> _logger = logger;

    /// <summary>
    /// Сервис кэширования
    /// </summary>
    private readonly IMemoryCache _cache = cache;

    /// <summary>
    /// Фабрика создания http-запросов
    /// </summary>
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    /// <summary>
    /// Сервис работы с данными параметров
    /// </summary>
    private readonly IParametersDAO _parametersDAO = parametersDAO;
    #endregion

    #region Поля
    /// <summary>
    /// Ключ кэша файлов рас
    /// </summary>
    private const string _racesFilesKey = "RacesFiles";

    /// <summary>
    /// Ключ кэша файлов наций
    /// </summary>
    private const string _nationsFilesKey = "NationsFiles";

    /// <summary>
    /// Токен
    /// </summary>
    private string? Token { get; set; }

    /// <summary>
    /// Параметры файлового сервиса
    /// </summary>
    private ParameterBiology?[]? FilesServiceParameters { get; set; }

    /// <summary>
    /// Параметры сервиса пользователей
    /// </summary>
    private ParameterBiology?[]? UsersServiceParameters { get; set; }

    /// <summary>
    /// Типы файлов
    /// </summary>
    private BaseResponseList? FilesTypes { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод инициализации
    /// </summary>
    public async Task Initialize(CancellationToken cancellationToken)
    {
        //Получение параметров сервиса пользователей
        if (UsersServiceParameters == null || UsersServiceParameters.Length < 1) await GetUsersServiceParameters();

        //Аутентификация
        if (string.IsNullOrWhiteSpace(Token)) await Authentication(cancellationToken);

        //Получение параметров файлового сервиса
        if (FilesServiceParameters == null || FilesServiceParameters.Length < 1) await GetFileServiceParameters();

        //Получение типов файлов
        if (FilesTypes == null || FilesTypes.Items == null || FilesTypes.Items.Count < 1) await GetFilesTypes(cancellationToken);
    }

    /// <summary>
    /// Метод получения списка файлов расы
    /// </summary>
    /// <param cref="long" name="raceId">Идентификатор расы</param>
    /// <param cref="CancellationToken" name="cancellationToken">Токен отмены</param>
    /// <returns cref="BaseResponseList?">Список файлов рас</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<BaseResponseList?> GetRaceFilesList(long raceId, CancellationToken cancellationToken = default)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListRaceFilesMethod);

            //Получение файлов
            BaseResponseList? files = await GetFilesList(raceId, "Расы", _racesFilesKey, cancellationToken);

            //Возврат результата
            return files;
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
    /// Метод получения списка файлов нации
    /// </summary>
    /// <param cref="long" name="nationId">Идентификатор нации</param>
    /// <param cref="CancellationToken" name="cancellationToken">Токен отмены</param>
    /// <returns cref="BaseResponseList?">Список файлов нации</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<BaseResponseList?> GetNationFilesList(long nationId, CancellationToken cancellationToken = default)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListNationFilesMethod);

            //Получение файлов
            BaseResponseList? files = await GetFilesList(nationId, "Нации", _nationsFilesKey, cancellationToken);

            //Возврат результата
            return files;
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion

    #region Внутренние методы
    /// <summary>
    /// Метод получения списка файлов
    /// </summary>
    /// <param cref="long" name="id">Идентификатор сущности</param>
    /// <param cref="string" name="type">Тип сущности</param>
    /// <param cref="string" name="key">Ключ кэша</param>
    /// <param cref="CancellationToken" name="cancellationToken">Токен отмены</param>
    /// <returns cref="BaseResponseList?">Список файлов рас</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<BaseResponseList?> GetFilesList(long id, string type, string key, CancellationToken cancellationToken = default)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListFilesMethod);

            //Проверка входящих параметров
            if (string.IsNullOrWhiteSpace(type)) throw new Exception(ErrorMessagesBiology.EmptyFilesType);
            if (string.IsNullOrWhiteSpace(key)) throw new Exception(ErrorMessagesBiology.EmptyKeyCache);

            //Формирование ключа кэша
            string keyCache = key + "_" + id;

            //Возврат данных при их наличии в кэше
            if (_cache.TryGetValue(keyCache, out BaseResponseList? cachedResponse) && cachedResponse != null) return cachedResponse;

            //Проверка наличия параметров
            for (int i = 0; i < FilesServiceParameters?.Length; i++)
            {
                if (FilesServiceParameters[i]?.Value == null) throw new Exception($"{ErrorMessagesShared.EmptyParameter} {FilesServiceParameters[i]?.Value}");
            }
            if (FilesServiceParameters == null || FilesServiceParameters.Length < 3) throw new Exception(ErrorMessagesShared.EmptyParameter);

            //Проверка наличия типов файлов
            if (FilesTypes == null || FilesTypes.Items == null || FilesTypes.Items.Count < 1) throw new Exception(ErrorMessagesBiology.EmptyFilesTypes);

            //Получение типа
            BaseResponseListItem fileType = FilesTypes.Items.FirstOrDefault(x => x.Name == type) ?? throw new Exception($"{ErrorMessagesBiology.EmptyFilesType} {type}");

            //Формирование ссылки
            UriBuilder uriBuilder = new($"{FilesServiceParameters[0]?.Value}{FilesServiceParameters[2]?.Value}")
            {
                Query = $"entity_id={Uri.EscapeDataString(id.ToString())}&type_id={Uri.EscapeDataString(fileType.Id.ToString() ?? "")}"
            };
            //Получение файлов
            BaseResponseList? response = await GetHttpResponse<BaseResponseList>(uriBuilder.ToString(), cancellationToken);

            //Формирование результата
            BaseResponseList result = GetFilesURLs(response);

            //Запись в кэш
            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
            _cache.Set(keyCache, result, cacheOptions);

            //Возврат результата
            return result;
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
    /// Метод получения параметров файлового сервиса
    /// </summary>
    /// <exception cref="Exception">Исключение</exception>
    private async Task GetFileServiceParameters()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetFilesServiceParametersMethod);

            //Получение параметров файлового сервиса
            ParameterBiology?[] result = [
                await _parametersDAO.GetByAlias("Ssylka_na_faiylovyiy_syervis"),
                await _parametersDAO.GetByAlias("Myetod_poluchyeniya_spiska_tipov_faiylov"),
                await _parametersDAO.GetByAlias("Myetod_poluchyeniya_spiska_faiylov_po_idyentifikatoru_sushchnosti_i_idyentifikatoru_tipa"),
                await _parametersDAO.GetByAlias("Myetod_poluchyeniya_faiyla_po_idyentifikatoru"),
            ];

            //Запись параметров
            FilesServiceParameters = result;

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
    /// Метод получения параметров сервиса пользователей
    /// </summary>
    /// <exception cref="Exception">Исключение</exception>
    private async Task GetUsersServiceParameters()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetUsersServiceParametersMethod);

            //Получение параметров сервиса пользователей
            ParameterBiology?[] result = [
                await _parametersDAO.GetByAlias("Ssylka_na_syervis_pol'zovatyelyeiy"),
                await _parametersDAO.GetByAlias("Myetod_autyentifikatsii"),
                await _parametersDAO.GetByAlias("Login_administratora"),
                await _parametersDAO.GetByAlias("Parol'_administratora"),
            ];

            //Запись параметров
            UsersServiceParameters = result;

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
    /// Метод получения типов файлов
    /// </summary>
    /// <param cref="CancellationToken" name="cancellationToken">Токен отмены</param>
    /// <exception cref="Exception">Исключение</exception>
    private async Task GetFilesTypes(CancellationToken cancellationToken = default)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListFilesTypesMethod);

            //Проверка наличия параметров
            if (FilesServiceParameters == null || FilesServiceParameters.Length < 2) throw new Exception(ErrorMessagesShared.EmptyParameter);

            //Получение типов
            BaseResponseList? response = await GetHttpResponse<BaseResponseList>($"{FilesServiceParameters[0]?.Value}{FilesServiceParameters[1]?.Value}", cancellationToken);

            //Запись ответа
            FilesTypes = response;
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
    /// Метод аутентификации
    /// </summary>
    /// <param cref="CancellationToken" name="cancellationToken">Токен отмены</param>
    /// <exception cref="Exception">Исключение</exception>
    private async Task Authentication(CancellationToken cancellationToken = default)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListFilesTypesMethod);

            //Проверка наличия параметров
            if (UsersServiceParameters == null || UsersServiceParameters.Length < 4) throw new Exception(ErrorMessagesShared.EmptyParameter);

            //Получение типов
            AuthenticationInfo? response = await GetHttpResponse<AuthenticationInfo>($"{UsersServiceParameters[0]?.Value}{UsersServiceParameters[1]?.Value}?login={UsersServiceParameters[2]?.Value}&password={UsersServiceParameters[3]?.Value}", cancellationToken);

            //Запись ответа
            Token = response?.Token;
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
    /// Метод получения ответа на http-запрос
    /// </summary>
    /// <typeparam name="T">Тип ответа</typeparam>
    /// <param cref="string" name="requestUrl">Ссылка для запроса</param>
    /// <param cref="CancellationToken" name="cancellationToken">Токен отмены</param>
    /// <exception cref="Exception">Исключение</exception>
    private async Task<T?> GetHttpResponse<T>(string requestUrl, CancellationToken cancellationToken = default)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetHttpResponseMethod);

            //Создание клиента http-запроса
            using HttpClient httpClient = _httpClientFactory.CreateClient(requestUrl);

            //Установка таймаута запроса
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            //Добавление токена авторизации, если он передан
            if (!string.IsNullOrWhiteSpace(Token)) httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

            //Проверка ссылки
            if (!Uri.TryCreate(requestUrl, UriKind.Absolute, out Uri? uri)) return default;

            //Выполнение http-запроса
            HttpResponseMessage response = await httpClient.GetAsync(uri, cancellationToken);

            //Считывание ответа
            string content = await response.Content.ReadAsStringAsync(cancellationToken);

            //Создание опций
            JsonSerializerSettings settings = new()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };

            //Десериализация ответа
            T? result = JsonConvert.DeserializeObject<T>(content, settings);

            //При неуспешном ответа
            if (!response.IsSuccessStatusCode)
            {
                //Получение ошибки
                BaseResponseError? error = JsonConvert.DeserializeObject<BaseResponseError>(content, settings);

                //Проброс ошибки
                throw new Exception(error?.Message ?? "Необработанная ошибка");
            }

            //Возврат результата
            return result;
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
    /// Метод получения ссылок файлов
    /// </summary>
    /// <param cref="BaseResponseList?" name="files">Список файлов</param>
    /// <returns cref="BaseResponseList">Список ссылок на файлы</returns>
    /// <exception cref="Exception">Исключение</exception>
    private BaseResponseList GetFilesURLs(BaseResponseList? files)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetFilesURLsMethod);

            //Формирование переменной результата
            List<BaseResponseListItem> fileURLs = [];
            BaseResponseList result = new(true, fileURLs);

            //Проверка наличия параметров
            if (FilesServiceParameters == null || FilesServiceParameters.Length < 4) throw new Exception(ErrorMessagesShared.EmptyParameter);

            //Выход при отсутствии файлов
            if (files?.Items == null) return result;

            //Запись ссылок на файлы
            foreach (var file in files.Items)
            {
                //Пропуск при отсутствии идентификатора файла
                if (file.Id == null) continue;

                //Формирование ссылок
                fileURLs.Add(new(file.Id ?? 0, $"{FilesServiceParameters[0]?.Value}{FilesServiceParameters[3]?.Value}?id={file.Id}"));
            }

            //Возврат результата
            return result;
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion
}
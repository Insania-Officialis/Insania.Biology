using System.Net;

namespace Insania.Biology.Tests.Mock.HttpClientFactory;

/// <summary>
/// Тестовая реализация фабрики создания http-запросов
/// </summary>
public class MockHttpClientFactory : IHttpClientFactory
{
    #region Конструкторы
    /// <summary>
    /// Конструктор тестовой реализации фабрики создания http-запросов
    /// </summary>
    public MockHttpClientFactory()
    {
        //Инициализация полей
        _messageHandler = new MockHttpMessageHandler();
        _clients = [];

    }
    #endregion

    #region Поля
    /// <summary>
    /// Коллекция http-клиентов
    /// </summary>
    private readonly Dictionary<string, HttpClient> _clients;

    /// <summary>
    /// Обработчик http-сообщений
    /// </summary>
    private readonly HttpMessageHandler _messageHandler;
    #endregion

    #region Методы
    /// <summary>
    /// Метод создания http-клиента
    /// </summary>
    /// <param cref="string" name="name">Наименование клиента</param>
    /// <returns cref="HttpClient">http-клиент</returns>
    public HttpClient CreateClient(string name)
    {
        //Возврат клиента при его наличии в коллекции
        if (_clients.TryGetValue(name, out HttpClient? value)) return value;

        //Создание клиента
        HttpClient client = new(_messageHandler)
        {
            BaseAddress = new Uri("https://mock-api.com")
        };

        //Добавление заголовков
        client.DefaultRequestHeaders.Add("User-Agent", "Insania-Biology-Tests");
        client.DefaultRequestHeaders.Add("X-Test-Mode", "true");

        //Запись клиента
        _clients[name] = client;

        //Возврат клиента
        return _clients[name];
    }

    /// <summary>
    /// Метод записи ответа для конкретной ссылки
    /// </summary>
    /// <param cref="string" name="url">Ссылка</param>
    /// <param cref="HttpStatusCode" name="statusCode">Код статуса</param>
    /// <param cref="string" name="content">Содержимое</param>
    /// <param cref="string" name="contentType">Тип контента</param>
    public void SetupResponse(string url, HttpStatusCode statusCode, string content = "", string contentType = "application/json")
    {
        //Запись ответа, если обработчик сообщений тестовый
        if (_messageHandler is MockHttpMessageHandler mockHandler) mockHandler.SetupResponse(url, statusCode, content, contentType);
    }

    /// <summary>
    /// Метод записи ответа для конкретного клиента
    /// </summary>
    /// <param cref="string" name="clientName">Наименование клиента</param>
    /// <param cref="string" name="url">Ссылка</param>
    /// <param cref="HttpStatusCode" name="statusCode">Код статуса</param>
    /// <param cref="string" name="content">Содержимое</param>
    /// <param cref="string" name="contentType">Тип контента</param>
    public void SetupResponseForClient(string clientName, string url, HttpStatusCode statusCode, string content = "", string contentType = "application/json")
    {
        //Если обработчик сообщений тестовый
        if (_messageHandler is MockHttpMessageHandler mockHandler)
        {
            //Формирование уникального ключа
            string fullUrl = $"{clientName}_{url}";

            //Запись ответа
            mockHandler.SetupResponse(fullUrl, statusCode, content, contentType);
        }
    }

    /// <summary>
    /// Очистка всех настроенных ответов
    /// </summary>
    public void ClearResponses()
    {
        //Очистка ответов, если обработчик сообщений тестовый
        if (_messageHandler is MockHttpMessageHandler mockHandler) mockHandler.ClearResponses();
    }
    #endregion
}
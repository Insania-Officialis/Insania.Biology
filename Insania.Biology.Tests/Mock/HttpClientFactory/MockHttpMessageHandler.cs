using System.Net;

using Insania.Shared.Messages;

namespace Insania.Biology.Tests.Mock.HttpClientFactory;

/// <summary>
/// Тестовый обработчик http-сообщений
/// </summary>
public class MockHttpMessageHandler : HttpMessageHandler
{
    #region Поля
    /// <summary>
    /// Коллекция ответов
    /// </summary>
    private readonly Dictionary<string, HttpResponseMessage> _responses = [];
    #endregion

    #region Методы
    /// <summary>
    /// Метод настройки ответа для URL
    /// </summary>
    public void SetupResponse(string url, HttpStatusCode statusCode, string content = "", string contentType = "application/json")
    {
        //Создание ответа
        HttpResponseMessage response = new(statusCode)
        {
            Content = new StringContent(content, System.Text.Encoding.UTF8, contentType)
        };

        //Возврат ответа по ссылке
        _responses[url] = response;
    }

    /// <summary>
    /// Метод очистки всех настроенных ответов
    /// </summary>
    public void ClearResponses()
    {
        //Проход по всем ответам
        foreach (var response in _responses.Values)
        {
            response.Dispose();
        }

        //Очистка коллекции ответов
        _responses.Clear();
    }
    #endregion

    #region Внуренние методы
    /// <summary>
    /// Метод отправки запроса
    /// </summary>
    /// <param cref="HttpRequestMessage" name="request">Запрос</param>
    /// <param cref="CancellationToken" name="cancellationToken">Токен отмены</param>
    /// <returns cref="HttpResponseMessage">Ответ</returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //Имитация сетевой задержки
        await Task.Delay(10, cancellationToken);

        //Получение ссылки
        string url = request.RequestUri?.ToString() ?? string.Empty;

        //Проход по списку ответов
        foreach (var kvp in _responses)
        {
            //Если ссылка содержит ключ или равна ключу
            if (url.Contains(kvp.Key) || url == kvp.Key)
            {
                //Получение ответа
                HttpResponseMessage response = kvp.Value;

                //Возврат ответа
                return new HttpResponseMessage
                {
                    StatusCode = response.StatusCode,
                    Content = response.Content,
                    Version = response.Version,
                    RequestMessage = request
                };
            }
        }

        //Возврат ответа
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"success\": true, \"items\": []}", System.Text.Encoding.UTF8, "application/json"),
            RequestMessage = request
        };
    }
    #endregion
}
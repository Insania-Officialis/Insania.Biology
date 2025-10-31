using System.Net;

using Microsoft.Extensions.DependencyInjection;

using Insania.Shared.Models.Responses.Base;

using Insania.Biology.Contracts.Services;
using Insania.Biology.Tests.Base;
using Insania.Biology.Tests.Mock.HttpClientFactory;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesBiology = Insania.Biology.Messages.ErrorMessages;

namespace Insania.Biology.Tests.Services;

/// <summary>
/// Тесты сервиса работы с файлами
/// </summary>
[TestFixture]
public class FilesSLTests : BaseTest
{
    #region Зависимости
    /// <summary>
    /// Сервис работы с файлами
    /// </summary>
    private IFilesSL FilesSL { get; set; }

    /// <summary>
    /// Фабрика создания http-запросов
    /// </summary>
    private IHttpClientFactory HttpClientFactory { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        FilesSL = ServiceProvider.GetRequiredService<IFilesSL>();
        HttpClientFactory = ServiceProvider.GetRequiredService<IHttpClientFactory>();

        //Выход, если фабрика создания http-клиентов не тестовая
        if (HttpClientFactory is not MockHttpClientFactory httpClientFactory) return;

        //Формирование тестовых ответов
        Dictionary<string, string> responses = new()
        {
            { "authentication/login", "{\"success\": true, \"token\": \"token\"}" },
            { "files_types/list", "{\"success\": true, \"items\": [{\"id\": 1, \"name\": \"Расы\"},{\"id\": 2, \"name\": \"Нации\"}]}" },
            { "files/list?entity_id=-1&type_id=1", "{\"success\": true, \"items\": []}" },
            { "files/list?entity_id=1&type_id=1", "{\"success\": true, \"items\": [{\"id\": 1, \"name\": \"race.png\"}]}" },
            { "files/list?entity_id=-1&type_id=2", "{\"success\": true, \"items\": []}" },
            { "files/list?entity_id=1&type_id=2", "{\"success\": true, \"items\": [{\"id\": 2, \"name\": \"nation.png\"}]}" },
        };

        //Запись тестовых ответов
        foreach (var response in responses)
        {
            httpClientFactory.SetupResponse(response.Key, HttpStatusCode.OK, response.Value);
        }
    }

    /// <summary>
    /// Метод, вызываемый после тестов
    /// </summary>
    [TearDown]
    public void TearDown()
    {

    }
    #endregion

    #region Методы тестирования
    /// <summary>
    /// Тест метода инициализации
    /// </summary>
    [Test]
    public async Task InitializeTest()
    {
        try
        {
            //Выполнение метода
            await FilesSL.Initialize();
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }

    /// <summary>
    /// Тест метода получения файла расы
    /// </summary>
    [TestCase(-1)]
    [TestCase(1)]
    public async Task GetRaceFilesListTest(long raceId)
    {
        try
        {
            //Инициализация
            await FilesSL.Initialize();

            //Получение результата
            BaseResponseList? result = await FilesSL.GetRaceFilesList(raceId);

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result?.Success, Is.True);
                Assert.That(result?.Items, Is.Not.Null);
            }
            switch (raceId)
            {
                case -1:
                    Assert.That(result?.Items, Is.Empty);
                    break;
                case 1: 
                    Assert.That(result?.Items, Is.Not.Empty);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }

    /// <summary>
    /// Тест метода получения файла нации
    /// </summary>
    [TestCase(-1)]
    [TestCase(1)]
    public async Task GetNationFilesListTest(long nationId)
    {
        try
        {
            //Инициализация
            await FilesSL.Initialize();

            //Получение результата
            BaseResponseList? result = await FilesSL.GetNationFilesList(nationId);

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result?.Success, Is.True);
                Assert.That(result?.Items, Is.Not.Null);
            }
            switch (nationId)
            {
                case -1:
                    Assert.That(result?.Items, Is.Empty);
                    break;
                case 1: 
                    Assert.That(result?.Items, Is.Not.Empty);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }
    #endregion
}
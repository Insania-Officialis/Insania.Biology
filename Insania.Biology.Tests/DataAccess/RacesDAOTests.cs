using Microsoft.Extensions.DependencyInjection;

using Insania.Biology.Contracts.DataAccess;
using Insania.Biology.Entities;
using Insania.Biology.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesBiology = Insania.Biology.Messages.ErrorMessages;

namespace Insania.Biology.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными рас
/// </summary>
[TestFixture]
public class RacesDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными рас
    /// </summary>
    private IRacesDAO RacesDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        RacesDAO = ServiceProvider.GetRequiredService<IRacesDAO>();
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
    /// Тест метода получения списка рас
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<Race>? result = await RacesDAO.GetList();

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }

    /// <summary>
    /// Тест метода получения расы
    /// </summary>
    /// <param cref="long?" name="raceId">Идентификатор расы</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(1)]
    public async Task GetItemTest(long? raceId)
    {
        try
        {
            //Получение результата
            Race? result = await RacesDAO.GetItem(raceId);

            //Проверка результата
            switch (raceId)
            {
                case -1: Assert.That(result, Is.Null); break;
                case 1: Assert.That(result, Is.Not.Null); break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (raceId)
            {
                case null: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesBiology.EmptyRace)); break;
                default: throw;
            }
        }
    }
    #endregion
}
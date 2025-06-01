using Microsoft.Extensions.DependencyInjection;

using Insania.Biology.Contracts.DataAccess;
using Insania.Biology.Entities;
using Insania.Biology.Messages;
using Insania.Biology.Tests.Base;

namespace Insania.Biology.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными наций
/// </summary>
[TestFixture]
public class NationsDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными наций
    /// </summary>
    private INationsDAO NationsDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        NationsDAO = ServiceProvider.GetRequiredService<INationsDAO>();
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
    /// Тест метода получения списка наций
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<Nation>? result = await NationsDAO.GetList();

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
    /// Тест метода получения списка наций
    /// </summary>
    /// <param cref="long?" name="raceId">Идентификатор расы</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(1)]
    public async Task GetListTest(long? raceId)
    {
        try
        {
            //Получение результата
            List<Nation>? result = await NationsDAO.GetList(raceId);

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            switch (raceId)
            {
                case -1: Assert.That(result, Is.Empty); break;
                case 1: Assert.That(result, Is.Not.Empty); break;
                default: throw new Exception(ErrorMessages.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (raceId)
            {
                case null: Assert.That(ex.Message, Is.EqualTo(ErrorMessages.EmptyRace)); break;
                default: throw;
            }
        }
    }
    #endregion
}
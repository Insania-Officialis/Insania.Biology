using Microsoft.Extensions.DependencyInjection;

using Insania.Biology.Contracts.DataAccess;
using Insania.Biology.Entities;
using Insania.Biology.Tests.Base;

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
    #endregion
}
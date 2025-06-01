using Microsoft.Extensions.DependencyInjection;

using Insania.Biology.Contracts.BusinessLogic;
using Insania.Biology.Tests.Base;
using Insania.Shared.Models.Responses.Base;

namespace Insania.Biology.Tests.BusinessLogic;

/// <summary>
/// Тесты сервиса работы с бизнес-логикой рас
/// </summary>
[TestFixture]
public class RacesBLTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с бизнес-логикой рас
    /// </summary>
    private IRacesBL RacesBL { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        RacesBL = ServiceProvider.GetRequiredService<IRacesBL>();
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
            BaseResponseList? result = await RacesBL.GetList();

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Items, Is.Not.Null);
                Assert.That(result.Items, Is.Not.Empty);
            });
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }
    #endregion
}
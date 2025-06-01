using Microsoft.Extensions.DependencyInjection;

using Insania.Biology.Contracts.BusinessLogic;
using Insania.Biology.Messages;
using Insania.Biology.Tests.Base;
using Insania.Shared.Models.Responses.Base;

namespace Insania.Biology.Tests.BusinessLogic;

/// <summary>
/// Тесты сервиса работы с бизнес-логикой наций
/// </summary>
[TestFixture]
public class NationsBLTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с бизнес-логикой наций
    /// </summary>
    private INationsBL NationsBL { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        NationsBL = ServiceProvider.GetRequiredService<INationsBL>();
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
    /// <param cref="long?" name="raceId">Идентификатор расы</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(2)]
    [TestCase(1)]
    public async Task GetListTest(long? raceId)
    {
        try
        {
            //Получение результата
            BaseResponseList? result = await NationsBL.GetList(raceId);

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Items, Is.Not.Null);
            });
            switch (raceId)
            {
                case 1: Assert.That(result.Items, Is.Not.Empty); break;
                default: throw new Exception(ErrorMessages.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (raceId)
            {
                case null: Assert.That(ex.Message, Is.EqualTo(ErrorMessages.EmptyRace)); break;
                case -1: Assert.That(ex.Message, Is.EqualTo(ErrorMessages.NotFoundRace)); break;
                case 2: Assert.That(ex.Message, Is.EqualTo(ErrorMessages.DeletedRace)); break;
                default: throw;
            }
        }
    }
    #endregion
}
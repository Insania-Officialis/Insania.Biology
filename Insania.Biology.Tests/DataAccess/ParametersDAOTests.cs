using Microsoft.Extensions.DependencyInjection;

using Insania.Biology.Contracts.DataAccess;
using Insania.Biology.Entities;
using Insania.Biology.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesBiology = Insania.Biology.Messages.ErrorMessages;

namespace Insania.Biology.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными параметров
/// </summary>
[TestFixture]
public class ParametersDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными параметров
    /// </summary>
    private IParametersDAO ParametersDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        ParametersDAO = ServiceProvider.GetRequiredService<IParametersDAO>();
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
    /// Тест метода получения списка параметров
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<ParameterBiology>? result = await ParametersDAO.GetList();

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
    /// Тест метода получения параметра по псевдониму
    /// </summary>
    /// <param cref="string" name="alias">Псевдоним параметра</param>
    [TestCase("")]
    [TestCase("incorrect")]
    [TestCase("Ssylka_na_faiylovyiy_syervis")]
    public async Task GetByAliasTest(string alias)
    {
        try
        {
            //Получение результата
            ParameterBiology? result = await ParametersDAO.GetByAlias(alias);

            //Проверка результата
            switch (alias)
            {
                case "incorrect": Assert.That(result, Is.Null); break;
                case "Ssylka_na_faiylovyiy_syervis": Assert.That(result, Is.Not.Null); Assert.That(result!.Alias, Is.EqualTo(alias)); break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (alias)
            {
                case "": Assert.That(ex.Message, Is.EqualTo(ErrorMessagesShared.EmptyAlias)); break;
                default: throw;
            }
        }
    }
    #endregion
}
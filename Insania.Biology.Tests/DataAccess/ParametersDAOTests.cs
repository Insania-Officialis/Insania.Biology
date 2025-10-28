using Microsoft.Extensions.DependencyInjection;

using Insania.Biology.Contracts.DataAccess;
using Insania.Biology.Entities;
using Insania.Biology.Tests.Base;

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
    #endregion
}
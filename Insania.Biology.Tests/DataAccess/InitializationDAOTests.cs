using Microsoft.Extensions.DependencyInjection;

using Insania.Shared.Contracts.DataAccess;

using Insania.Biology.Contracts.DataAccess;
using Insania.Biology.Entities;
using Insania.Biology.Tests.Base;

namespace Insania.Biology.Tests.DataAccess;

/// <summary>
/// Тесты сервиса инициализации данных в бд биологии
/// </summary>
[TestFixture]
public class InitializationDAOTests : BaseTest
{
    #region Зависимости
    /// <summary>
    /// Сервис инициализации данных в бд биологии
    /// </summary>
    private IInitializationDAO InitializationDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными рас
    /// </summary>
    private IRacesDAO RacesDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными наций
    /// </summary>
    private INationsDAO NationsDAO { get; set; }

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
        InitializationDAO = ServiceProvider.GetRequiredService<IInitializationDAO>();
        RacesDAO = ServiceProvider.GetRequiredService<IRacesDAO>();
        NationsDAO = ServiceProvider.GetRequiredService<INationsDAO>();
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
    /// Тест метода инициализации данных
    /// </summary>
    [Test]
    public async Task InitializeTest()
    {
        try
        {
            //Выполнение метода
            await InitializationDAO.Initialize();

            //Получение сущностей
            List<Race> races = await RacesDAO.GetList();
            List<Nation> nations = await NationsDAO.GetList();
            List<ParameterBiology> parameters = await ParametersDAO.GetList();

            //Проверка результата
            using (Assert.EnterMultipleScope())
            {
                Assert.That(races, Is.Not.Empty);
                Assert.That(nations, Is.Not.Empty);
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
using Microsoft.Extensions.DependencyInjection;

using Insania.Biology.Contracts.DataAccess;

namespace Insania.Biology.DataAccess;

/// <summary>
/// Расширение для внедрения зависимостей сервисов работы с данными в зоне биологии
/// </summary>
public static class Extension
{
    /// <summary>
    /// Метод внедрения зависимостей сервисов работы с данными в зоне биологии
    /// </summary>
    /// <param cref="IServiceCollection" name="services">Исходная коллекция сервисов</param>
    /// <returns cref="IServiceCollection">Модифицированная коллекция сервисов</returns>
    public static IServiceCollection AddBiologyDAO(this IServiceCollection services) =>
        services
            .AddScoped<IRacesDAO, RacesDAO>() //сервис работы с данными рас
            .AddScoped<INationsDAO, NationsDAO>() //сервис работы с данными наций
            .AddScoped<IParametersDAO, ParametersDAO>() //сервис работы с данными параметров
        ;
}
using Microsoft.Extensions.DependencyInjection;

using Insania.Biology.DataAccess;

namespace Insania.Biology.BusinessLogic;

/// <summary>
/// Расширение для внедрения зависимостей сервисов работы с бизнес-логикой в зоне биологии
/// </summary>
public static class Extension
{
    /// <summary>
    /// Метод внедрения зависимостей сервисов работы с бизнес-логикой в зоне биологии
    /// </summary>
    /// <param cref="IServiceCollection" name="services">Исходная коллекция сервисов</param>
    /// <returns cref="IServiceCollection">Модифицированная коллекция сервисов</returns>
    public static IServiceCollection AddBiologyBL(this IServiceCollection services) =>
        services
            .AddBiologyDAO() //сервисы работы с данными в зоне биологии
        ;
}
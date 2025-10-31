using Insania.Biology.Entities;

namespace Insania.Biology.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными параметров
/// </summary>
public interface IParametersDAO
{
    /// <summary>
    /// Метод получения списка параметров
    /// </summary>
    /// <returns cref="List{ParameterBiology}">Список параметров</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<ParameterBiology>> GetList();

    /// <summary>
    /// Метод получения параметра по псевдониму
    /// </summary>
    /// <param cref="string" name="alias">Псевдоним параметра</param>
    /// <returns cref="ParameterBiology?">Параметр</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<ParameterBiology?> GetByAlias(string alias);
}
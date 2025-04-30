using Insania.Biology.Entities;

namespace Insania.Biology.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным рас
/// </summary>
public interface IRacesDAO
{
    /// <summary>
    /// Метод получения списка рас
    /// </summary>
    /// <returns cref="List{Race}">Список рас</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Race>> GetList();
}
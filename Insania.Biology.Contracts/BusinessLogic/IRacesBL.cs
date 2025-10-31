using Insania.Shared.Models.Responses.Base;

using Insania.Biology.Models.Responses.Races;

namespace Insania.Biology.Contracts.BusinessLogic;

/// <summary>
/// Интерфейс работы с бизнес-логикой рас
/// </summary>
public interface IRacesBL
{
    /// <summary>
    /// Метод получения списка рас
    /// </summary>
    /// <returns cref="BaseResponseList">Стандартный ответ</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<BaseResponseList> GetList();

    /// <summary>
    /// Метод получения списка рас с нациями
    /// </summary>
    /// <returns cref="RacesWithNationsResponseList">Список рас с нациями</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<RacesWithNationsResponseList> GetListWithNations();
}
using Insania.Biology.Entities;
using Insania.Shared.Models.Responses.Base;

namespace Insania.Biology.Contracts.BusinessLogic;

/// <summary>
/// Интерфейс работы с бизнес-логикой наций
/// </summary>
public interface INationsBL
{
    /// <summary>
    /// Метод получения списка наций
    /// </summary>
    /// <param cref="long?" name="raceId">Идентификатор расы</param>
    /// <returns cref="BaseResponseList">Стандартный ответ</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<BaseResponseList> GetList(long? raceId);
}
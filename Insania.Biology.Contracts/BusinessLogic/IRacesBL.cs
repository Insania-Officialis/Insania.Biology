using Insania.Shared.Models.Responses.Base;

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
}
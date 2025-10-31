using Insania.Shared.Models.Responses.Base;

namespace Insania.Biology.Contracts.Services;

/// <summary>
/// Интерфейс сервиса работы с файлами
/// </summary>
public interface IFilesSL
{
    /// <summary>
    /// Метод инициализации
    /// </summary>
    /// <param cref="CancellationToken" name="cancellationToken">Токен отмены</param>
    Task Initialize(CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод получения списка файлов расы
    /// </summary>
    /// <param cref="long" name="raceId">Идентификатор расы</param>
    /// <param cref="CancellationToken" name="cancellationToken">Токен отмены</param>
    /// <returns cref="BaseResponseList?">Список файлов рас</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<BaseResponseList?> GetRaceFilesList(long raceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод получения списка файлов нации
    /// </summary>
    /// <param cref="long" name="nationId">Идентификатор нации</param>
    /// <param cref="CancellationToken" name="cancellationToken">Токен отмены</param>
    /// <returns cref="BaseResponseList?">Список файлов нации</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<BaseResponseList?> GetNationFilesList(long nationId, CancellationToken cancellationToken = default);
}
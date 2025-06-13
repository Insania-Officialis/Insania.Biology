using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Biology.Contracts.DataAccess;
using Insania.Biology.Database.Contexts;
using Insania.Biology.Entities;
using Insania.Biology.Messages;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesBiology = Insania.Biology.Messages.ErrorMessages;

namespace Insania.Biology.DataAccess;

/// <summary>
/// Сервис работы с данными наций
/// </summary>
/// <param cref="ILogger{NationsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="BiologyContext" name="context">Контекст базы данных биологии</param>
public class NationsDAO(ILogger<NationsDAO> logger, BiologyContext context) : INationsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<NationsDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных биологии
    /// </summary>
    private readonly BiologyContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка наций
    /// </summary>
    /// <returns cref="List{Nation}">Список наций</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<Nation>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListNationsMethod);

            //Получение данных из бд
            List<Nation> data = await _context.Nations.Where(x => x.DateDeleted == null).ToListAsync();

            //Возврат результата
            return data;
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }

    }

    /// <summary>
    /// Метод получения списка наций
    /// </summary>
    /// <param cref="long?" name="raceId">Идентификатор расы</param>
    /// <returns cref="List{Nation}">Список наций</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<Nation>> GetList(long? raceId)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListNationsMethod);

            //Проверки
            if (raceId == null) throw new Exception(ErrorMessagesBiology.EmptyRace);

            //Получение данных из бд
            List<Nation> data = await _context.Nations.Where(x => x.DateDeleted == null && x.RaceId == raceId).ToListAsync();

            //Возврат результата
            return data;
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion
}
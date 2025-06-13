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
/// Сервис работы с данными рас
/// </summary>
/// <param cref="ILogger{RacesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="BiologyContext" name="context">Контекст базы данных биологии</param>
public class RacesDAO(ILogger<RacesDAO> logger, BiologyContext context) : IRacesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<RacesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных биологии
    /// </summary>
    private readonly BiologyContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка рас
    /// </summary>
    /// <returns cref="List{Race}">Список рас</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<Race>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListRacesMethod);

            //Получение данных из бд
            List<Race> data = await _context.Races.Where(x => x.DateDeleted == null).ToListAsync();

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
    /// Метод получения расы
    /// </summary>
    /// <param cref="long?" name="raceId">Идентификатор расы</param>
    /// <returns cref="Race?">Раса</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<Race?> GetItem(long? raceId)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredCheckDeletedRaceMethod);

            //Проверки
            if (raceId == null) throw new Exception(ErrorMessagesBiology.EmptyRace);

            //Получение данных из бд
            Race? data = await _context.Races.Where(x => x.Id == raceId).FirstOrDefaultAsync();

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
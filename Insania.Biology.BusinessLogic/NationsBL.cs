using Microsoft.Extensions.Logging;

using AutoMapper;

using Insania.Shared.Models.Responses.Base;

using Insania.Biology.Contracts.BusinessLogic;
using Insania.Biology.Contracts.DataAccess;
using Insania.Biology.Entities;
using Insania.Biology.Messages;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesBiology = Insania.Biology.Messages.ErrorMessages;

namespace Insania.Biology.BusinessLogic;

/// <summary>
/// Сервис работы с бизнес-логикой наций
/// </summary>
/// <param cref="ILogger{NationsBL}" name="logger">Сервис логгирования</param>
/// <param cref="IMapper" name="mapper">Сервис преобразования моделей</param>
/// <param cref="INationsDAO" name="nationsDAO">Сервис работы с данными наций</param>
/// <param cref="IRacesDAO" name="racesDAO">Сервис работы с данными рас</param>
public class NationsBL(ILogger<NationsBL> logger, IMapper mapper, INationsDAO nationsDAO, IRacesDAO racesDAO) : INationsBL
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<NationsBL> _logger = logger;

    /// <summary>
    /// Сервис преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Сервис работы с данными наций
    /// </summary>
    private readonly INationsDAO _nationsDAO = nationsDAO;

    /// <summary>
    /// Сервис работы с данными рас
    /// </summary>
    private readonly IRacesDAO _racesDAO = racesDAO;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка наций
    /// </summary>
    /// <param cref="long?" name="raceId">Идентификатор расы</param>
    /// <returns cref="BaseResponseList">Стандартный ответ</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<BaseResponseList> GetList(long? raceId)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListNationsMethod);

            //Проверки
            if (raceId == null) throw new Exception(ErrorMessagesBiology.EmptyRace);
            Race? race = await _racesDAO.GetItem(raceId) ?? throw new Exception(ErrorMessagesBiology.NotFoundRace);
            if (race.DateDeleted != null) throw new Exception(ErrorMessagesBiology.DeletedRace);


            //Получение данных
            List<Nation>? data = await _nationsDAO.GetList(raceId);

            //Формирование ответа
            BaseResponseList? response = null;
            if (data == null) response = new(false, null);
            else response = new(true, data?.Select(_mapper.Map<BaseResponseListItem>).ToList());

            //Возврат ответа
            return response;
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
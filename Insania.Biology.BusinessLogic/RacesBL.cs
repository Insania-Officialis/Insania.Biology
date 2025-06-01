using Microsoft.Extensions.Logging;

using AutoMapper;

using Insania.Biology.Contracts.BusinessLogic;
using Insania.Biology.Contracts.DataAccess;
using Insania.Biology.Entities;
using Insania.Biology.Messages;
using Insania.Shared.Models.Responses.Base;

namespace Insania.Biology.BusinessLogic;

/// <summary>
/// Сервис работы с бизнес-логикой рас
/// </summary>
/// <param cref="ILogger{RacesBL}" name="logger">Сервис логгирования</param>
/// <param cref="IMapper" name="mapper">Сервис преобразования моделей</param>
/// <param cref="IRacesDAO" name="racesDAO">Сервис работы с данными рас</param>
public class RacesBL(ILogger<RacesBL> logger, IMapper mapper, IRacesDAO racesDAO) : IRacesBL
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<RacesBL> _logger = logger;

    /// <summary>
    /// Сервис преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Сервис работы с данными рас
    /// </summary>
    private readonly IRacesDAO _racesDAO = racesDAO;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка рас
    /// </summary>
    /// <returns cref="BaseResponseList">Стандартный ответ</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<BaseResponseList> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListRacesMethod);

            //Получение данных
            List<Race>? data = await _racesDAO.GetList();

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
            _logger.LogError("{text}: {error}", ErrorMessages.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion
}
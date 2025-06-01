using Microsoft.AspNetCore.Mvc;

using Insania.Shared.Models.Responses.Base;

using Insania.Biology.Contracts.BusinessLogic;
using Insania.Biology.Messages;

namespace Insania.Biology.Api.Controllers;

/// <summary>
/// Контроллер работы с расами
/// </summary>
/// <param cref="ILogger" name="logger">Сервис логгирования</param>
/// <param cref="IRacesBL" name="racesService">Сервис работы с бизнес-логикой рас</param>
[Route("races")]
public class RacesController(ILogger<RacesController> logger, IRacesBL racesService) : Controller
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<RacesController> _logger = logger;

    /// <summary>
    /// Сервис работы с бизнес-логикой рас
    /// </summary>
    private readonly IRacesBL _racesService = racesService;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка наций
    /// </summary>
    /// <returns cref="OkResult">Список рас</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetList()
    {
        try
        {
            //Получение результата проверки логина
            BaseResponse? result = await _racesService.GetList();

            //Возврат ответа
            return Ok(result);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {ex}", ErrorMessages.Error, ex);

            //Возврат ошибки
            return BadRequest(new BaseResponseError(ex.Message));
        }
    }
    #endregion
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Biology.Contracts.DataAccess;
using Insania.Biology.Database.Contexts;
using Insania.Biology.Entities;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesBiology = Insania.Biology.Messages.ErrorMessages;
using InformationMessages = Insania.Biology.Messages.InformationMessages;

namespace Insania.Biology.DataAccess;

/// <summary>
/// Сервис работы с данными параметров
/// </summary>
/// <param cref="ILogger{ParametersDAO}" name="logger">Сервис логгирования</param>
/// <param cref="BiologyContext" name="context">Контекст базы данных новостей</param>
public class ParametersDAO(ILogger<ParametersDAO> logger, BiologyContext context) : IParametersDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<ParametersDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных новостей
    /// </summary>
    private readonly BiologyContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка параметров
    /// </summary>
    /// <returns cref="List{ParameterBiology}">Список параметров</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<ParameterBiology>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListParametersMethod);

            //Получение данных из бд
            List<ParameterBiology> data = await _context.Parameters.Where(x => x.DateDeleted == null).ToListAsync();

            //Возврат результата
            return data;
        }
        catch (Exception ex)
        {
            //Логгирование ошибки
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }

    /// <summary>
    /// Метод получения параметра по псевдониму
    /// </summary>
    /// <param cref="string" name="alias">Псевдоним параметра</param>
    /// <returns cref="ParameterBiology?">Параметр</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<ParameterBiology?> GetByAlias(string alias)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetParameterByAliasMethod);

            //Проверки
            if (alias == "") throw new Exception(ErrorMessagesShared.EmptyAlias);

            //Получение данных из бд
            ParameterBiology? data = await _context.Parameters.Where(x => x.DateDeleted == null && x.Alias == alias).FirstOrDefaultAsync();

            //Возврат результата
            return data;
        }
        catch (Exception ex)
        {
            //Логгирование ошибки
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion
}
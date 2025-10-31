using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;

using AutoMapper;

using Insania.Shared.Models.Responses.Base;

using Insania.Biology.Contracts.BusinessLogic;
using Insania.Biology.Contracts.DataAccess;
using Insania.Biology.Contracts.Services;
using Insania.Biology.Entities;
using Insania.Biology.Messages;
using Insania.Biology.Models.Responses.Races;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Biology.BusinessLogic;

/// <summary>
/// Сервис работы с бизнес-логикой рас
/// </summary>
/// <param cref="ILogger{RacesBL}" name="logger">Сервис логгирования</param>
/// <param cref="IMapper" name="mapper">Сервис преобразования моделей</param>
/// <param cref="IFilesSL" name="filesSL">Сервис работы с файлами</param>
/// <param cref="IRacesDAO" name="racesDAO">Сервис работы с данными рас</param>
public class RacesBL(ILogger<RacesBL> logger, IMapper mapper, IFilesSL filesSL, IRacesDAO racesDAO) : IRacesBL
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
    /// Сервис работы с файлами
    /// </summary>
    private readonly IFilesSL _filesSL = filesSL;

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

    /// <summary>
    /// Метод получения списка рас с нациями
    /// </summary>
    /// <returns cref="RacesWithNationsResponseList">Список рас с нациями</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<RacesWithNationsResponseList> GetListWithNations()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListRacesWithNationsMethod);

            //Получение данных
            List<Race>? data = await _racesDAO.GetList();

            //Инициализация файлового сервиса
            await _filesSL.Initialize();

            //Проверка наличия данных
            if (data == null) return new(false, []);

            //Создание коллекции для результатов
            ConcurrentBag<RacesWithNationsResponseListItem> races = [];

            //Создание семафоров для ограничения параллельного получения данных
            using var raceSemaphore = new SemaphoreSlim(4, 4);
            using var nationSemaphore = new SemaphoreSlim(4, 4);

            //Создание коллекции заданий для рас
            Task[] raceTasks = [.. data.Select(async race =>
            {
                //Блокировка потока
                await raceSemaphore.WaitAsync();

                try
                {
                    //Формирование переменной наций
                    ConcurrentBag<BaseResponseListItem> nations = [];

                    //Параллельная обработка при наличии наций
                    if (race.Nations != null && race.Nations.Count > 0)
                    {
                        //Создание коллекции заданий для наций
                        Task<BaseResponseListItem?>[] nationTasks = [.. race.Nations.Select(async nation =>
                        {
                            //Блокировка потока
                            await nationSemaphore.WaitAsync();

                            try
                            {
                                //Получение файлов нации
                                BaseResponseList? nationFiles = await _filesSL.GetNationFilesList(nation.Id);

                                //Пропуск при отсутствии файлов
                                if (nationFiles?.Items?.Count < 1) return null;
                                BaseResponseListItem? nationFile = nationFiles?.Items?.FirstOrDefault();
                                if (nationFile == null) return null;

                                //Возврат файла с нацией
                                return new BaseResponseListItem(nation.Id, nation.Name, nation.Description, nationFile.Name);
                            }
                            catch
                            {
                                throw;
                            }
                            finally
                            {
                                //Освобождение потока
                                nationSemaphore.Release();
                            }
                        })];

                        //Ожидание завершения всех задач по нациям
                        BaseResponseListItem?[]? nationResults = await Task.WhenAll(nationTasks);

                        //Добавление наций с файлами
                        foreach (var nationResult in nationResults)
                        {
                            if (nationResult != null) nations.Add(nationResult);
                        }
                    }

                    //Получение файлов расы
                    BaseResponseList? raceFiles = await _filesSL.GetRaceFilesList(race.Id);

                    //Пропуск при отсутствии файлов
                    if (raceFiles?.Items?.Count < 1) return;
                    BaseResponseListItem? raceFile = raceFiles?.Items?.FirstOrDefault();
                    if (raceFile == null || string.IsNullOrWhiteSpace(raceFile.Name)) return;

                    //Формирование объекта расы
                    RacesWithNationsResponseListItem raceItem = new(race.Id, race.Name, race.Description, raceFile.Name, [.. nations]);

                    //Добавление расы в коллекцию
                    races.Add(raceItem);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    raceSemaphore.Release();
                }
            })];

            //Ожидаем завершения всех задач по расам
            await Task.WhenAll(raceTasks);

            //Возврат ответа
            return new RacesWithNationsResponseList(true, [.. races]);
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
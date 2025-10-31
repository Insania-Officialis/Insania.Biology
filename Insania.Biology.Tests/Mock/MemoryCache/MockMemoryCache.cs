using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace Insania.Biology.Tests.Mock.MemoryCache;

/// <summary>
/// Тестовая реализация сервиса кэширования
/// </summary>
public class MockMemoryCache : IMemoryCache
{
    #region Поля
    /// <summary>
    /// Коллекция кэша
    /// </summary>
    private readonly ConcurrentDictionary<object, object> _cache = [];

    /// <summary>
    /// Время жизни записей
    /// </summary>
    private readonly ConcurrentDictionary<object, DateTimeOffset> _expirations = [];

    /// <summary>
    /// Флаг освобождения ресурсов
    /// </summary>
    private bool _disposed = false;
    #endregion

    #region Методы
    /// <summary>
    /// Метод создания кэша с указанным ключом
    /// </summary>
    /// <param cref="object" name="key">Ключ</param>
    /// <returns cref="ICacheEntry">Объект кэша</returns>
    public ICacheEntry CreateEntry(object key) => new MockCacheEntry(key, this);

    /// <summary>
    /// Метод удаления записи из кэша по ключу
    /// </summary>
    /// <param cref="object" name="key">Ключ</param>
    public void Remove(object key)
    {
        //Удаление значения из основного словаря кэша
        _cache.TryRemove(key, out _);

        //Удаление информации о времени жизни записи
        _expirations.TryRemove(key, out _);
    }

    /// <summary>
    /// Метод получения значения из кэша по ключу
    /// </summary>
    /// <param cref="object" name="key">Ключ</param>
    /// <param cref="object?" name="value">Значение</param>
    /// <returns cref="bool">Успешность получения</returns>
    public bool TryGetValue(object key, out object? value)
    {
        //Проверка наличия записи и истечения срока ее действия
        if (_expirations.TryGetValue(key, out var expiration) && expiration < DateTimeOffset.Now)
        {
            //Удаление просроченной записи
            Remove(key);

            //Запись значения
            value = null;

            //Возврат результата
            return false;
        }

        //Возврат результата
        return _cache.TryGetValue(key, out value);
    }

    /// <summary>
    /// Метод записи значения в кэш
    /// </summary>
    /// <param cref="object" name="key">Ключ</param>
    /// <param cref="object" name="value">Значение</param>
    /// <param cref="DateTimeOffset?" nname="absoluteExpiration">Время жизни</param>
    public void Set(object key, object value, DateTimeOffset? absoluteExpiration = null)
    {
        //Сохранение значения в основном словаре кэша
        _cache.AddOrUpdate(key, value, (k, v) => value);

        //Сохранение времени жизни, при его наличии
        if (absoluteExpiration.HasValue) _expirations.AddOrUpdate(key, absoluteExpiration.Value, (k, v) => absoluteExpiration.Value);
    }

    /// <summary>
    /// Метод освобождения ресурсов
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion

    #region Внутренние методы
    /// <summary>
    /// Освобождение ресурсов
    /// </summary>
    /// <param name="disposing">Флаг освобождения управляемых ресурсов</param>
    protected virtual void Dispose(bool disposing)
    {
        //Выход, если ресурсы освобождены
        if (_disposed) return;

        //Освобождение управляемых ресурсов
        if (disposing)
        {
            _cache.Clear();
            _expirations.Clear();
        }

        //Отметка об освобождении ресурсов
        _disposed = true;
    }
    #endregion
}
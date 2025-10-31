using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Insania.Biology.Tests.Mock.MemoryCache;

/// <summary>
/// Класс тестового объекта кэша
/// </summary>
/// <param cref="object" name="key">Ключ</param>
/// <param cref="MockMemoryCache" name="cache">Экземпляр кэша</param>
public class MockCacheEntry(object key, MockMemoryCache cache) : ICacheEntry
{
    #region Поля
    /// <summary>
    /// Ключ
    /// </summary>
    public object Key { get; } = key;

    /// <summary>
    /// Значение
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Абсолютное время жизни
    /// </summary>
    public DateTimeOffset? AbsoluteExpiration { get; set; }

    /// <summary>
    /// Время жизни относительно текущего момента
    /// </summary>
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

    /// <summary>
    /// Скользящее время жизни
    /// </summary>
    public TimeSpan? SlidingExpiration { get; set; }

    /// <summary>
    /// Токены отслеживания изменений
    /// </summary>
    public IList<IChangeToken> ExpirationTokens { get; } = [];

    /// <summary>
    /// Функции обратной связи после вытеснения
    /// </summary>
    public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; } = [];

    /// <summary>
    /// Приоритет записи в кэше
    /// </summary>
    public CacheItemPriority Priority { get; set; }

    /// <summary>
    /// Размер записи
    /// </summary>
    public long? Size { get; set; }

    /// <summary>
    /// Флаг освобождения ресурсов
    /// </summary>
    private bool _disposed = false;
    #endregion

    #region Зависимости
    /// <summary>
    /// Экземпляр кэша
    /// </summary>
    private readonly MockMemoryCache _cache = cache;
    #endregion

    #region Методы
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

        //Если значение было установлено
        if (Value != null)
        {
            //Получение времени жизни
            DateTimeOffset? expiration = AbsoluteExpiration;

            //Сохранение записи в кэше
            _cache.Set(Key, Value, expiration);
        }

        //Отметка об освобождении ресурсов
        _disposed = true;
    }
    #endregion
}
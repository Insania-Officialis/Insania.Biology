using Insania.Shared.Models.Responses.Base;

namespace Insania.Biology.Models.Responses.Races;

/// <summary>
/// Модель элемента ответа списком рас с нациями
/// </summary>
/// <param cref="long" name="id">Идентификатор расы</param>
/// <param cref="string" name="name">Наименование расы</param>
/// <param cref="string" name="description">Описание расы</param>
/// <param cref="string" name="file">Ссылка на файл расы</param>
/// <param cref="string" name="nations">Список наций</param>
public class RacesWithNationsResponseListItem(long id, string name, string description, string file, List<BaseResponseListItem> nations): BaseResponseListItem(id, name, description, file)
{
    #region Поля
    /// <summary>
    /// Список наций
    /// </summary>
    public List<BaseResponseListItem> Nations { get; set; } = nations;
    #endregion
}
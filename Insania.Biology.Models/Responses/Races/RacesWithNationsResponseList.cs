using Insania.Shared.Models.Responses.Base;

namespace Insania.Biology.Models.Responses.Races;

/// <summary>
/// Модель ответа списком рас с нациями
/// </summary>
/// <param cref="bool" name="success">Признак успешности ответа</param>
/// <param cref="List{RacesWithNationsResponseListItem}" name="items">Список рас с нациями</param>
public class RacesWithNationsResponseList(bool success, List<RacesWithNationsResponseListItem> items): BaseResponse(success)
{
    #region Поля
    /// <summary>
    /// Список рас с нациями
    /// </summary>
    public List<RacesWithNationsResponseListItem>? Items { get; set; } = items;
    #endregion
}
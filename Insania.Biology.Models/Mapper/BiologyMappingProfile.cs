using AutoMapper;

using Insania.Biology.Entities;
using Insania.Shared.Models.Responses.Base;

namespace Insania.Biology.Models.Mapper;

/// <summary>
/// Сервис преобразования моделей
/// </summary>
public class BiologyMappingProfile : Profile
{
    /// <summary>
    /// Конструктор сервиса преобразования моделей
    /// </summary>
    public BiologyMappingProfile()
    {
        //Преобразование модели сущности расы в базовую модель элемента ответа списком
        CreateMap<Race, BaseResponseListItem>();

        //Преобразование модели сущности нации в базовую модель элемента ответа списком
        CreateMap<Nation, BaseResponseListItem>();
    }
}
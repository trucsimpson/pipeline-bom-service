using AutoMapper;
using BOMService.Domain.Entities;
using BOMService.Infrastructure.Persistence.EFModels;

namespace BOMService.Infrastructure.MappingProfiles
{
    public class HouseProfile : Profile
    {
        public HouseProfile()
        {
            CreateMap<BuilderHouse, HouseModel>().ReverseMap();
        }
    }
}

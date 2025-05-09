using AutoMapper;
using BOMService.Domain.Entities;
using BOMService.Infrastructure.Persistence.EFModels;

namespace BOMService.Infrastructure.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductsProduct, ProductModel>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(t => t.ProductsId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(t => t.ProductsName))
                .ReverseMap();

            CreateMap<ProductsProductsToBuildingPhase, ProductToBuildingPhaseModel>()
                .ForMember(dest => dest.ProductToBuildingPhaseId, opt => opt.MapFrom(t => t.ProductsToBuildingPhasesId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(t => t.ProductsId))
                .ForMember(dest => dest.BuildingPhaseId, opt => opt.MapFrom(t => t.BuildingPhasesId))
                .ReverseMap();

            CreateMap<ProductsProductsToStyle, ProductsToStyleModel>()
                .ForMember(dest => dest.ProductToStyleId, opt => opt.MapFrom(t => t.ProductsToStylesId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(t => t.ProductsId))
                .ForMember(dest => dest.StyleId, opt => opt.MapFrom(t => t.StylesId))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(t => t.ProductsToStylesIsDefault))
                .ReverseMap();
        }
    }
}

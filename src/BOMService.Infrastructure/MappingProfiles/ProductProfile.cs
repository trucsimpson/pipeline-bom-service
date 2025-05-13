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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(t => t.ProductsId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(t => t.ProductsName))
                .ReverseMap();

            CreateMap<ProductsProductsToBuildingPhase, ProductToBuildingPhaseModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(t => t.ProductsToBuildingPhasesId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(t => t.ProductsId))
                .ForMember(dest => dest.BuildingPhaseId, opt => opt.MapFrom(t => t.BuildingPhasesId))
                .ReverseMap();

            CreateMap<ProductsProductsToStyle, ProductToStyleModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(t => t.ProductsToStylesId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(t => t.ProductsId))
                .ForMember(dest => dest.StyleId, opt => opt.MapFrom(t => t.StylesId))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(t => t.ProductsToStylesIsDefault))
                .ReverseMap();

            CreateMap<ProductsProductsToCategory, ProductToCategoryModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(t => t.ProductsToCategoriesId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(t => t.ProductsId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(t => t.CategoriesId))
                .ReverseMap();

            CreateMap<ProductsWithPtoBidAndPtoSid, ProductToBuildingPhaseAndStyleModel>()
                .ForMember(dest => dest.ProductToBuildingPhaseId, opt => opt.MapFrom(t => t.ProductsToBuildingPhasesId))
                .ForMember(dest => dest.ProductToStyleId, opt => opt.MapFrom(t => t.ProductsToStylesId))
                .ForMember(dest => dest.ProductStyleIsDefault, opt => opt.MapFrom(t => t.ProductsToStylesIsDefault))
                .ReverseMap();

            CreateMap<ProductsToProductPairing, ProductToProductPairingModel>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(t => t.ProductsId))
                .ForMember(dest => dest.PairedProductId, opt => opt.MapFrom(t => t.ProductsToProductPairingPairedProductId))
                .ReverseMap();

            CreateMap<ProductOrientation, ProductOrientationModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(t => t.ProductOrientationsId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(t => t.ProductOrientationsName))
                .ForMember(dest => dest.ShortDisplay, opt => opt.MapFrom(t => t.ProductOrientationsShortDisplay))
                .ReverseMap();
        }
    }
}

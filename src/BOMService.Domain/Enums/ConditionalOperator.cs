using System.ComponentModel.DataAnnotations;

namespace BOMService.Domain.Enums
{
    public enum ConditionalOperator
    {
        [Display(Name = "Division Name")]
        DivisionName = 1,
        [Display(Name = "Community Name")]
        CommunityName = 2,
        [Display(Name = "Community City")]
        CommunityCity = 3,
        [Display(Name = "Community County")]
        CommunityCounty = 4,
        [Display(Name = "Community State")]
        CommunityState = 5,
        [Display(Name = "Community Zip")]
        CommunityZip = 6,
        [Display(Name = "House Name")]
        HouseName = 7,
        [Display(Name = "House Plan No")]
        HousePlanNo = 8,
        [Display(Name = "House SQ FT Total")]
        HouseSQFTTotal = 9,
        [Display(Name = "House Style")]
        HouseStyle = 10,
        [Display(Name = "House Stories")]
        HouseStories = 11,
        [Display(Name = "House Bedrooms")]
        HouseBedrooms = 12,
        [Display(Name = "House Bathrooms")]
        HouseBathrooms = 13,
        [Display(Name = "House Garages")]
        HouseGarages = 14,
        [Display(Name = "Option Name")]
        OptionName = 15,
        [Display(Name = "Series Title")]
        SeriesTitle = 16,
        [Display(Name = "BuildingPhase Name")]
        BuildingPhaseName = 17,
        [Display(Name = "Product Name")]
        ProductName = 18,
        [Display(Name = "Style Name")]
        StyleName = 19,
        [Display(Name = "Category Name")]
        CategoryName = 20,
        [Display(Name = "House SQ FT Basement")]
        HouseSQFTBasement = 21,
        [Display(Name = "House SQ FT Floor 1")]
        HouseSQFTFloor1 = 22,
        [Display(Name = "House SQ FT Floor 2")]
        HouseSQFTFloor2 = 23,
        [Display(Name = "House SQ FT Heated")]
        HouseSQFTHeated = 24,
        [Display(Name = "Product Quantity")]
        ProductQuantity = 25,
    }
}

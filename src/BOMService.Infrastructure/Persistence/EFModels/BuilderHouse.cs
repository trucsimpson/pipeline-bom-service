using System;
using System.Collections.Generic;

namespace BOMService.Infrastructure.Persistence.EFModels;

public partial class BuilderHouse
{
    public int HousesId { get; set; }

    public string? HousesPlanNo { get; set; }

    public string? HousesName { get; set; }

    public decimal? HousesBasePrice { get; set; }

    public string? HousesBasementSqf { get; set; }

    public string? HousesHeatedSqf { get; set; }

    public string? HousesGarageSqf { get; set; }

    public string? HousesFirstSqf { get; set; }

    public string? HousesSecondSqf { get; set; }

    public string? HousesTotalSqf { get; set; }

    public bool HousesFloorPlanner { get; set; }

    public bool HousesExteriorDesigner { get; set; }

    public bool HousesInteriorDesigner { get; set; }

    public bool HousesMediaCenter { get; set; }

    public bool HousesOptions { get; set; }

    public string? HousesInfo { get; set; }

    public string? HousesPicFile { get; set; }

    public string? HousesPicFile2 { get; set; }

    public byte? HousesStories { get; set; }

    public string? HousesHomeStyle { get; set; }

    public string? HousesBedrooms { get; set; }

    public string? HousesBathrooms { get; set; }

    public string? HousesGarage { get; set; }

    public short SeriesId { get; set; }

    public string? HousesImgBasement { get; set; }

    public string? HousesImgFirstFloor { get; set; }

    public string? HousesImgSecondFloor { get; set; }

    public byte? HousesHomeStyleHouseComponentId { get; set; }

    public byte? HousesHomeStoryHouseComponentId { get; set; }

    public byte? HousesBedroomHouseComponentId { get; set; }

    public byte? HousesBathroomHouseComponentId { get; set; }

    public byte? HousesGarageHouseComponentId { get; set; }

    public virtual ICollection<BuilderHousesInCommunity> BuilderHousesInCommunities { get; set; } = new List<BuilderHousesInCommunity>();
}

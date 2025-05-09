using System;
using System.Collections.Generic;

namespace BOMService.Infrastructure.Persistence.EFModels;

public partial class BuilderHousesInCommunity
{
    public int HousesInCommunityId { get; set; }

    public int CommunitiesId { get; set; }

    public int HousesId { get; set; }

    public decimal? HousesInCommunityPrice { get; set; }

    public string? HousesInCommunityNotes { get; set; }

    public virtual BuilderHouse Houses { get; set; } = null!;
}

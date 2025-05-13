using System;
using System.Collections.Generic;

namespace BOMService.Infrastructure.Persistence.EFModels;

public partial class ProductsWithPtoBidAndPtoSid
{
    public int ProductsId { get; set; }

    public int ProductsToBuildingPhasesId { get; set; }

    public int ProductsToStylesId { get; set; }

    public int BuildingPhasesId { get; set; }

    public int StylesId { get; set; }

    public short ProductsToBuildingPhasesTaxable { get; set; }

    public string? ProductsToStylesProductCode { get; set; }

    public bool ProductsToStylesIsDefault { get; set; }
}

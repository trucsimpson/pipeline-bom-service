using System;
using System.Collections.Generic;

namespace BOMService.Infrastructure.Persistence.EFModels;

public partial class ProductsProductsToBuildingPhase
{
    public int ProductsToBuildingPhasesId { get; set; }

    public int ProductsId { get; set; }

    public int BuildingPhasesId { get; set; }

    public bool ProductsToBuildingPhasesIsDefault { get; set; }

    public short ProductsToBuildingPhasesTaxable { get; set; }

    public virtual ProductsProduct Products { get; set; } = null!;
}

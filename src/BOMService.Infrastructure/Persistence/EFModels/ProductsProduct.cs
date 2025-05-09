using System;
using System.Collections.Generic;

namespace BOMService.Infrastructure.Persistence.EFModels;

public partial class ProductsProduct
{
    public int ProductsId { get; set; }

    public string? ProductsName { get; set; }

    public string? ProductsDescription { get; set; }

    public string? ProductsNotes { get; set; }

    public short? ProductUnitTypesId { get; set; }

    public string? ProductsAccuracy { get; set; }

    public short? ProductsRounding { get; set; }

    public decimal? ProductsWaste { get; set; }

    public bool ProductsSupplementalToBid { get; set; }

    public decimal ProductsProjectedCost { get; set; }

    public bool? ProductsIsArchived { get; set; }

    public virtual ICollection<ProductsProductsToBuildingPhase> ProductsProductsToBuildingPhases { get; set; } = new List<ProductsProductsToBuildingPhase>();

    public virtual ICollection<ProductsProductsToStyle> ProductsProductsToStyles { get; set; } = new List<ProductsProductsToStyle>();
}

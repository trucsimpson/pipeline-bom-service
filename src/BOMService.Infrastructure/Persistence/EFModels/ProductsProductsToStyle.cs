using System;
using System.Collections.Generic;

namespace BOMService.Infrastructure.Persistence.EFModels;

public partial class ProductsProductsToStyle
{
    public int ProductsToStylesId { get; set; }

    public int ProductsId { get; set; }

    public int StylesId { get; set; }

    public string? ProductsToStylesProductCode { get; set; }

    public bool ProductsToStylesIsDefault { get; set; }

    public virtual ProductsProduct Products { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace BOMService.Infrastructure.Persistence.EFModels;

public partial class ProductsProductsToCategory
{
    public int ProductsToCategoriesId { get; set; }

    public int ProductsId { get; set; }

    public short CategoriesId { get; set; }

    public virtual ProductsProduct Products { get; set; } = null!;
}

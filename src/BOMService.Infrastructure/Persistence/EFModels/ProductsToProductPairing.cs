using System;
using System.Collections.Generic;

namespace BOMService.Infrastructure.Persistence.EFModels;

public partial class ProductsToProductPairing
{
    public int ProductsId { get; set; }

    public int ProductsToProductPairingPairedProductId { get; set; }
}

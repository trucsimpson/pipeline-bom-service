using System;
using System.Collections.Generic;

namespace BOMService.Infrastructure.Persistence.EFModels;

public partial class ProductOrientation
{
    public byte ProductOrientationsId { get; set; }

    public string ProductOrientationsName { get; set; } = null!;

    public string ProductOrientationsShortDisplay { get; set; } = null!;
}

using System.ComponentModel.DataAnnotations;

namespace BOMService.Domain.Enums
{
    public enum SystemOrientation
    {
        [Display(Name = "Standard")]
        Standard = 1,

        [Display(Name = "Reversed")]
        Reversed = 2,

        [Display(Name = "Both")]
        Both = 3,

        [Display(Name = "None")]
        None = 4
    }
}

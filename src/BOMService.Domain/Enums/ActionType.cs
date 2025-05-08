using System.ComponentModel.DataAnnotations;

namespace BOMService.Domain.Enums
{
    public enum ActionType
    {
        [Display(Name = "Added Product")]
        Added = 1,

        [Display(Name = "Conditioned Product")]
        Conditioned = 2
    }
}

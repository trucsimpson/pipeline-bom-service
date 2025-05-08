using System.ComponentModel.DataAnnotations;

namespace BOMService.Domain.Enums
{
    public enum RuleApplicationType
    {
        [Display(Name = "Pre Product Assembly")]
        Pre = 1,

        [Display(Name = "During Product Assembly")]
        During = 2,

        [Display(Name = "Post Product Assembly")]
        POST = 3,

        [Display(Name = "Post BOM Product Adjustment")]
        PostBomPA = 4
    }
}

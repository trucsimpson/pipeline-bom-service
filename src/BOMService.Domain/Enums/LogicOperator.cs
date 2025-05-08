using System.ComponentModel.DataAnnotations;

namespace BOMService.Domain.Enums
{
    public enum LogicOperator
    {
        EQUAL = 1,
        IN = 2,
        BETWEEN = 3,
        [Display(Name = "NOT IN")]
        NOTIN = 4,
        [Display(Name = "NOT EQUAL")]
        NOTEQUAL = 5,
        CONTAINS = 6
    }
}

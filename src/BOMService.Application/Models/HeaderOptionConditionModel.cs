namespace BOMService.Application.Models
{
    public class HeaderOptionConditionModel
    {
        public int OptionId { get; set; }
        public string DependentCondition { get; set; } = string.Empty;
    }
}

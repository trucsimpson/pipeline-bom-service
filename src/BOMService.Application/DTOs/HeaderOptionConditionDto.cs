namespace BOMService.Application.DTOs
{
    public class HeaderOptionConditionDto
    {
        public int OptionId { get; set; }
        public string DependentCondition { get; set; } = string.Empty;
    }
}

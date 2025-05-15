namespace BOMService.Application.DTOs
{
    public class HouseReportDetailDto
    {
        public int GeneratedReportId { get; set; }

        public int OptionId { get; set; }

        public string DependentCondition { get; set; } = string.Empty;
    }
}

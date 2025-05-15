namespace BOMService.Application.DTOs
{
    public class HouseOptionReportResultDto
    {
        public int GeneratedReportId { get; set; }

        public string ReportParams { get; set; }

        public int OptionId { get; set; }

        public string DependentCondition { get; set; }
    }
}

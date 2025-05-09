namespace BOMService.Application.Models
{
    public class HouseOptionReportResultModel
    {
        public int GeneratedReportId { get; set; }
        public string ReportParams { get; set; }
        public int OptionId { get; set; }
        public string DependentCondition { get; set; }
    }
}

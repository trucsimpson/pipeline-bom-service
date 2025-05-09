namespace BOMService.Application.Models
{
    public class HouseReportDetailModel
    {
        public int GeneratedReportId { get; set; }
        public int OptionId { get; set; }
        public string DependentCondition { get; set; } = string.Empty;
    }
}

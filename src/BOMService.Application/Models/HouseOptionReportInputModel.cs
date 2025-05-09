namespace BOMService.Application.Models
{
    public class HouseOptionReportInputModel
    {
        public int WorksheetId { get; set; }
        public int CustomOptionId { get; set; }
        public int JobId { get; set; }
        public int RunNumber { get; set; }
        public int LastConfigurationNumber { get; set; }
        public int CommunityId { get; set; }
        public int HouseId { get; set; }
        public int OptionId { get; set; }
        public string DependentCondition { get; set; } = string.Empty;
        public int lcfg { get; set; } //TODO: What is this property for?
        public int run { get; set; } //TODO: What is this property for?
    }
}

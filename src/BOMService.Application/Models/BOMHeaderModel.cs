namespace BOMService.Application.Models
{
    public class BOMHeaderModel
    {
        public int GeneratedReportId { get; set; }

        //TODO: Rename
        public HouseOptionReportInputModel HeaderArgs { get; set; } = new();
    }
}

namespace BOMService.Application.DTOs
{
    public class BOMHeaderDto
    {
        public int GeneratedReportId { get; set; }

        //TODO: Rename
        public HouseOptionReportInputDto HeaderArgs { get; set; } = new();
    }
}

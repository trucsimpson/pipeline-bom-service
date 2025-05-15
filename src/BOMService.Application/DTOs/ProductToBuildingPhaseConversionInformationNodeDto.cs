namespace BOMService.Application.DTOs
{
    /// <summary>
    /// Suggested name: ProductToBuildingPhaseNode
    /// </summary>
    public class ProductToBuildingPhaseConversionInformationNodeDto
    {
        public int BuildingPhaseId { get; set; }
        public int ProductId { get; set; }
    }

    //TODO: Lý do để đây là để so sánh với class ở trên, được thì thay luôn
    public record ProductPhaseKey(int ProductId, int BuildingPhaseId);
}

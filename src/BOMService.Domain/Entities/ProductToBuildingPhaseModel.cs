namespace BOMService.Domain.Entities
{
    public class ProductToBuildingPhaseModel
    {
        public int ProductToBuildingPhaseId { get; set; }

        public int ProductId { get; set; }

        public int BuildingPhaseId { get; set; }
    }
}

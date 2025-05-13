namespace BOMService.Domain.Entities
{
    public class ProductToBuildingPhaseAndStyleModel
    {
        public int ProductToBuildingPhaseId { get; set; }

        public int ProductToStyleId { get; set; }

        public bool ProductStyleIsDefault { get; set; }
    }
}

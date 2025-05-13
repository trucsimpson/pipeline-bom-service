namespace BOMService.Application.Models
{
    public class BOMGeneratingProductModel
    {
        public int AssetId { get; set; }

        public int OptionId { get; set; }

        public int CustomOptionId { get; set; }

        public string? DependentCondition { get; set; }

        public int ProductToBuildingPhaseId { get; set; }

        public int ProductToStyleId { get; set; }

        public decimal ProductQuantity { get; set; }

        public decimal ProductQuantityTotal { get; set; }

        public short KMSourceId { get; set; }

        public int KMTypeId { get; set; }

        public int BuildingPhaseId { get; set; }

        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public int UseId { get; set; }

        public int BOMGeneratedReportId { get; set; }

        public string? Parameters { get; set; }

        public int StyleId { get; set; }

        public int NodeId { get; set; }
    }
}

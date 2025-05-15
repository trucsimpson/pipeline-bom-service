namespace BOMService.Application.DTOs.BOMStepOutputs
{
    public class BOMPreparationDataDto
    {
        public Dictionary<int, string> ProductDict { get; set; } = new();

        // Suggested name: ProductBuildingPhaseMapById
        public Dictionary<int, ProductToBuildingPhaseConversionInformationNodeDto> ProductToBuildingPhaseConversionDict { get; set; } = new();

        public Dictionary<ProductPhaseKey, int> ProductToBuildingPhaseDict { get; set; } = new();

        public Dictionary<int, ProductToStyleLookupNodeDto> ProductToStyleDict { get; set; } = new();

        public Dictionary<int, Tuple<int, int>> DefaultProductToStyleDict { get; set; } = new();

        public Dictionary<int, ProductInverseStyleLookupNodeDto> ProductLookupForStyleDict { get; set; } = new();

        public Dictionary<ProductStyleKey, int> ProductToStyleIdDict { get; set; } = new();

        public Dictionary<Tuple<int, int>, int> ProductToCategoryDict { get; set; } = new();

        public Dictionary<int, List<int>> ProductLookupCategoryIdsDict { get; set; } = new();

        public Dictionary<int, int> ReverseProductDict { get; set; } = new();

        public Dictionary<string, Tuple<int, string>> SystemOrientationDict { get; set; } = new();

        public Dictionary<int, Tuple<string, string>> SystemOrientationIdKeyDict { get; set; } = new();

        public JobFlipStatusDto JobOrientation { get; set; } = new();

        // Suggested name: ProductsMissingStyle
        public List<string> ListProductsWithOutStyle { get; set; } = new();

        // Suggested name: GeneratingBOMProducts
        public LinkedList<BOMGeneratingProductDto> GeneratingProducts { get; set; } = new();
    }
}

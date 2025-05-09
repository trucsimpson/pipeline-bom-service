namespace BOMService.Application.Models
{
    public class BOMHeaderMappingResultModel
    {
        public List<BOMHeaderModel> Headers { get; set; } = new();
        public Dictionary<int, int> NodeGeneratorIds { get; set; } = new();
        public Dictionary<int, Dictionary<HeaderOptionConditionModel, bool>> HeaderOptionsAndConditions { get; set; } = new();
    }
}

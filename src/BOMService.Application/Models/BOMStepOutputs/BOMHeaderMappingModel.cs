namespace BOMService.Application.Models.BOMStepOutputs
{
    public class BOMHeaderMappingModel
    {
        public List<BOMHeaderModel> Headers { get; set; } = new();
        public Dictionary<int, int> DictionaryNodeGeneratorIds { get; set; } = new();
        public Dictionary<int, Dictionary<HeaderOptionConditionModel, bool>> HeaderOptionsAndConditions { get; set; } = new();
    }
}

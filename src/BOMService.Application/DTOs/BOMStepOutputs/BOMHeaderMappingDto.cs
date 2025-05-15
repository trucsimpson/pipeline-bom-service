namespace BOMService.Application.DTOs.BOMStepOutputs
{
    public class BOMHeaderMappingDto
    {
        public List<BOMHeaderDto> Headers { get; set; } = new();

        public Dictionary<int, int> DictionaryNodeGeneratorIds { get; set; } = new();

        public Dictionary<int, Dictionary<HeaderOptionConditionDto, bool>> HeaderOptionsAndConditions { get; set; } = new();
    }
}

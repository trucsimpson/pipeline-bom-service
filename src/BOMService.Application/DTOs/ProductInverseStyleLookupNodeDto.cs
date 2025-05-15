namespace BOMService.Application.DTOs
{
    /// <summary>
    /// Suggested name: StyleProductChainNode
    /// </summary>
    public class ProductInverseStyleLookupNodeDto
    {
        public int StyleId { get; set; }
        public int ProductToStyleId { get; set; }
        public ProductInverseStyleLookupNodeDto? Link { get; set; }
    }
}

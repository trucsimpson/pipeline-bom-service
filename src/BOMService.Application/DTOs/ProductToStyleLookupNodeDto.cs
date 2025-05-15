namespace BOMService.Application.DTOs
{
    /// <summary>
    /// Suggested name: ProductStyleNode
    /// </summary>
    public class ProductToStyleLookupNodeDto
    {
        public int ProductId { get; set; }
        public int StyleId { get; set; }
    }

    public record ProductStyleKey(int ProductId, int StyleId);
}

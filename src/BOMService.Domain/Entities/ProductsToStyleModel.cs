namespace BOMService.Domain.Entities
{
    public class ProductsToStyleModel
    {
        public int ProductToStyleId { get; set; }

        public int ProductId { get; set; }

        public int StyleId { get; set; }

        public bool IsDefault { get; set; }
    }
}

namespace BOMService.Domain.Entities
{
    public class ProductToStyleModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int StyleId { get; set; }

        public bool IsDefault { get; set; }
    }
}

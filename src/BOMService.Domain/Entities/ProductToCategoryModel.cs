namespace BOMService.Domain.Entities
{
    public class ProductToCategoryModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public short CategoryId { get; set; }
    }
}

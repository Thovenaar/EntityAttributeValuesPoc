namespace EntityAttributeValues.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public List<ProductAttribute> Attributes { get; set; } = new();
    }
}

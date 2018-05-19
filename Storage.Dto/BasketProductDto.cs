using System;
namespace Storage.Dto
{
    public class BasketProductDto
    {
        public string ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice
        {
            get {
                return Convert.ToDecimal(Product.Price) * Quantity;
            }
        }
    }
}

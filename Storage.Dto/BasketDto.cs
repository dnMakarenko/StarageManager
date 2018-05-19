using System.Collections.Generic;
using System.Linq;
namespace Storage.Dto
{
    public class BasketDto
    {
        public BasketDto()
        {
            Products = new List<BasketProductDto>();
        }
        public ICollection<BasketProductDto> Products { get; set; }
        public decimal TotalBasketPrice { get { return Products.Select(q => q.TotalPrice).Sum(); } }
    }
}

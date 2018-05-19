using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Storage.WebApi.Models
{
    public class Product
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [DefaultValue(1)]
        public long Quantity { get; set; }
    }

    public class Basket
    {
        public long Id { get; set; }

        public Basket()
        {
            if(Products==null)
                Products = new List<BasketProduct>();
        }

        public string UserId { get; set; }
        public ICollection<BasketProduct> Products { get; set; }
    }

    public class BasketProduct
    {
        public long Id { get; set; }

        [Required]
        public long BasketId { get; set; }
        public Basket Basket { get; set; }

        [Required]
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}
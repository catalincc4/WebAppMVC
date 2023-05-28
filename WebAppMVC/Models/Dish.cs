using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static IdentityServer4.Models.IdentityResources;

namespace WebAppMVC.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public decimal Price { get; set; }

        public ICollection<Photo> Photos { get; set; }
        public int Stock { get; set; }
        [BindNever]
        public ICollection<OrderDish> OrderDishes { get; set; }
        [BindNever]
        public ICollection<Orders> Orders { get; set; }
    }
  
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SaleLine
    {
        public string Id { get; set; } 

        public Product Product { get; set; }

        public double SubtotalPrice { get; set; }

        public int Quantity { get; set; }

        public ICollection<Product> ProductsList { get; set; } = new List<Product>();

    }
}

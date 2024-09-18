using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        public ICollection<Product> ProductsList { get; set; } = new List<Product>();

        public string Id { get; set; }

        public double TotalPrice { get; set; }

        public string TypePayment { get; set; }

        public bool IsPayabled { get; set; }


    }
}

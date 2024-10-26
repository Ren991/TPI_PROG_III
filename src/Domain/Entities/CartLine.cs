using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CartLine
    {
        public string Id { get; set; } 

        public Product Product { get; set; }

        public int ProductId {  get; set; }


        public double SubtotalPrice { get; set; }

        public int Quantity { get; set; }

        public int CartId { get; set; }  // Foreign key
        public Cart ?Cart { get; set; }

    }
}

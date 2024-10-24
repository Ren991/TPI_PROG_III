using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        public ICollection<CartLine> CartLineList { get; set; } = new List<CartLine>();

        public int Id { get; set; }

        public double TotalPrice { get; set; }

        public TypePayment TypePayment { get; set; }

        public bool IsPayabled { get; set; }

        public int UserId { get; set; } // Esta es la clave foránea que conecta con User

        public User User { get; set; }

    }
}

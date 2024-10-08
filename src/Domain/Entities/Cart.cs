﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        public ICollection<SaleLine> SaleLineList { get; set; } = new List<SaleLine>();

        public int Id { get; set; }

        public double TotalPrice { get; set; }

        public TypePayment TypePayment { get; set; }

        public bool IsPayabled { get; set; }

        public int UserId { get; set; } // Esta es la clave foránea que conecta con User

        public User User { get; set; }

        //public SaleLine SaleLine { get; set; }

    }
}

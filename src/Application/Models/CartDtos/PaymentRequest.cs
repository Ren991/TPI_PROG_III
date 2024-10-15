using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.CartDtos
{
    public class PaymentRequest
    {
        public TypePayment TypePayment { get; set; }
    }
}

using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.SaleLineDtos
{
    public class SaleLineDto
    {
        public string Id { get; set; }

        public int ProductId { get; set; }

        public double SubtotalPrice { get; set; }

        public int Quantity { get; set; }

        public int CartId { get; set; }

        public static SaleLineDto ToDto(SaleLine saleLine)
        {
            SaleLineDto saleLineDto = new SaleLineDto();
            saleLineDto.Id = saleLine.Id;
            saleLineDto.ProductId = saleLine.ProductId;
            saleLineDto.SubtotalPrice = saleLine.SubtotalPrice;
            saleLineDto.Quantity = saleLine.Quantity;
            saleLineDto.CartId = saleLine.CartId;

            return saleLineDto;
        }

        public static ICollection<SaleLineDto> ToCollectionDto(ICollection<SaleLine> saleLines)
        {
            return saleLines.Select(sl => ToDto(sl)).ToList();
        }
    }
}

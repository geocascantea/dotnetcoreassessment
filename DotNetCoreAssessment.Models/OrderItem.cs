using System;
using System.Collections.Generic;

namespace DotNetCoreAssessment.Models
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public string ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public ICollection<OrderItem> SubItems { get; set; }
    }
}
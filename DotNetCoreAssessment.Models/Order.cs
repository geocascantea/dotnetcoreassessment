using System;
using System.Collections.Generic;

namespace DotNetCoreAssessment.Models
{
    public class Order
    {
        public Order()
        {
            OrderId = Guid.NewGuid();
            OrderDate = DateTime.Now;
            OrderItems = new List<OrderItem>();
        }
        public Guid OrderId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public string CustomerId { get; set; }
        public decimal OrderSubTotal { get; set; }
        public DateTime OrderDate { get; set; }
    }
}

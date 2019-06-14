using DotNetCoreAssessment.Models;
using System;

namespace DotNetCoreAssessment.Services
{
    public class OrderService
    {
        public Order CreateOrder(string customerId)
        {
            Order order = new Order() {
                CustomerId = customerId
            };
            return order;
        }

        public void AddOrderItem(Order order, OrderItem orderItem)
        {
            order.OrderItems.Add(orderItem);
            RecalculateOrderTotal(order);
        }

        public void AddSubItem(Order order, Guid parentOrderItemId, OrderItem orderItem)
        {
            /*
            TODO: 

            1. Validate that OrderItem.ProductId is not already present in the Order.OrderItem nor in any subitem in 
            SubItems collection. If so it should throw an exception.

            2. Add the given OrderItem in the Subitems collection of the 
            OrderItem instance that matches the OrderItemId with the parentOrderItemId.
            */

            //Finally recalculate the order subtotal.
            RecalculateOrderTotal(order);
        }

        private void RecalculateOrderTotal(Order order)
        {
            /*
            TODO: 
            
            1. Go through every OrderItem and sum the OrderItem.Quantity * OrderItem.UnitPrice.
            Have in consideration that OrderItem has a collection of OrderItem instances in the 
            OrderItem.SubItems property.

            2. For SubItems it should consider the quantity of the parent. 
            For instace,
            If I have a parent item with quantity of 2 
            then the total quantity of a subitem of 2 quanties is: 2 * 2 = 4. 
            
            {Parents.Quantity} * ({SubItem.Quantity} * {SubItem.UnitPrice})

            */
        }


    }
}

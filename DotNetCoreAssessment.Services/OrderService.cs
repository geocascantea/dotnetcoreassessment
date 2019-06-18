using DotNetCoreAssessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
            order.OrderSubTotal = RecalculateOrderTotal(order.OrderItems);
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

            ValidateProductInOrder(order.OrderItems, orderItem.ProductId);

            var parent = GetCorrectParentOrderItem(order.OrderItems, parentOrderItemId);
            parent?.SubItems.Add(orderItem);

            //Finally recalculate the order subtotal.
            order.OrderSubTotal = RecalculateOrderTotal(order.OrderItems);
        }

        /// <summary>
        /// Checks if the product is already in the order (recursively)
        /// </summary>
        /// <param name="orderItems">OrderItems to check</param>
        /// <param name="productId">Product to check for</param>
        private void ValidateProductInOrder(IEnumerable<OrderItem> orderItems, string productId)
        {
            if (orderItems != null)
            {
                if (orderItems.Any(item => item.ProductId == productId))
                    throw new Exception("ProductId already in Order");

                foreach (var subitem in orderItems)
                    ValidateProductInOrder(subitem.SubItems, productId);
            }
        }

        /// <summary>
        /// Looks for the correct orderItem in the whole tree structure
        /// </summary>
        /// <param name="orderItems">OrderItems to check</param>
        /// <param name="parentOrderItemId">ParentId to look for</param>
        /// <returns>The parent with the correct id</returns>
        private OrderItem GetCorrectParentOrderItem(ICollection<OrderItem> orderItems, Guid parentOrderItemId)
        {
            OrderItem parent = null;

            if (orderItems != null)
            {
                foreach (var subitem in orderItems)
                {
                    parent = (subitem.OrderItemId == parentOrderItemId) ? subitem : GetCorrectParentOrderItem(subitem.SubItems, parentOrderItemId);
                    if (parent != null) break;
                }
            }

            if (parent != null && parent.SubItems == null)
                parent.SubItems = new List<OrderItem>();

            return parent;
        }

        /// <summary>
        /// Calculates the total amount of the order
        /// </summary>
        /// <param name="orderItems">OrderItems to calculate</param>
        /// <returns>The total amount of the order</returns>
        private decimal RecalculateOrderTotal(IEnumerable<OrderItem> orderItems)
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

            decimal orderTotal = 0;

            if (orderItems != null)
            {
                foreach (var subitem in orderItems)
                    orderTotal += (subitem.Quantity * subitem.UnitPrice) + (subitem.Quantity * RecalculateOrderTotal(subitem.SubItems));
            }

            return orderTotal;
        }
    }
}

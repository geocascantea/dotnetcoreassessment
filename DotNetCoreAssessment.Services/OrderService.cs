using DotNetCoreAssessment.Models;
using System;
using System.Collections.Generic;

namespace DotNetCoreAssessment.Services
{
    public class OrderService : IOrderService
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

            // Look if ProductId exists in any OrderItem or decendents SubItems
            LookForOrderItemProductId(order, orderItem);

            // Insert OrderItem as SubItem of its parentOrderItemId
            InsertOrderItemAsSubItem(order, parentOrderItemId, orderItem);

            //Finally recalculate the order subtotal.
            RecalculateOrderTotal(order);
        }

        /// <summary>
        /// Look in OrderItems and SubItems for a ProductId and throws an exception if it exists
        /// </summary>
        /// <param name="order">Parent Order</param>
        /// <param name="orderItem">OrderItem which contains the ProductId to look for</param>
        private void LookForOrderItemProductId(Order order, OrderItem orderItem)
        {
            foreach (var item in order.OrderItems)
            {
                if (LookForProductId(item, orderItem.ProductId))
                {
                    throw new ArgumentException("ProductId already exists");
                }
            }
        }

        /// <summary>
        /// Recursive method to look for a ProductId from the second level to the last level
        /// </summary>
        /// <param name="orderItem">Second level item where search starts</param>
        /// <param name="productId">ProductId to look for</param>
        /// <returns></returns>
        private bool LookForProductId(OrderItem orderItem, string productId)
        {
            if (orderItem.ProductId == productId)
                return true;

            if (orderItem.SubItems == null)
                return false;

            foreach (var subItem in orderItem.SubItems)
            {
                return LookForProductId(subItem, productId);
            }                        

            return false;
        }

        /// <summary>
        /// Insert orderItem as a SubItem of the order with the corresponding parentOrderItemId
        /// </summary>
        /// <param name="order">Parent Order</param>
        /// <param name="parentOrderItemId">OrderItemId to look for</param>
        /// <param name="orderItem">OrderItem to be added as a SubItem of order</param>
        private void InsertOrderItemAsSubItem(Order order, Guid parentOrderItemId, OrderItem orderItem)
        {
            foreach (var item in order.OrderItems)
            {
                InsertOrderItem(parentOrderItemId, item, orderItem);
            }
        }

        /// <summary>
        /// Recursive method used to insert orderItemToInsert as a SubItem of orderItem
        /// </summary>
        /// <param name="parentOrderItemId">OrderItemId to look for</param>
        /// <param name="orderItem">Parent Order of orderItemToInsert</param>
        /// <param name="orderItemToInsert">Element to be inserted as a SubItem of orderItem</param>
        private void InsertOrderItem(Guid parentOrderItemId, OrderItem orderItem, OrderItem orderItemToInsert)
        {            
            if (orderItem.OrderItemId == parentOrderItemId)
            {
                if (orderItem.SubItems == null)
                    orderItem.SubItems = new List<OrderItem>();
                orderItem.SubItems.Add(orderItemToInsert);
                return;
            }

            if (orderItem.SubItems == null)
                return;

            foreach (var subItem in orderItem.SubItems)
            {
                InsertOrderItem(parentOrderItemId, subItem, orderItemToInsert);
            }                        
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

            RecalculateByEveryOrderItemSubItems(order);
        }

        /// <summary>
        /// Calculate OrderItems and SubItems totals
        /// </summary>
        /// <param name="order">Parent Order to be calulated</param>
        private void RecalculateByEveryOrderItemSubItems(Order order)
        {
            decimal sum = 0;
            foreach (var orderItem in order.OrderItems)
            {
                sum += CalculateOrderItem(orderItem);
                sum += CalculateOrderItemSubItems(0, orderItem.Quantity, orderItem);
            }
            order.OrderSubTotal = sum;
        }

        /// <summary>
        /// Calculate total of one OrderItem
        /// </summary>
        /// <param name="orderItem">OrderItem to calculate value from</param>
        /// <returns></returns>
        private decimal CalculateOrderItem(OrderItem orderItem)
        {
            return orderItem.Quantity * orderItem.UnitPrice;
        }

        /// <summary>
        /// Recursive method to calculate SubItems total from the second level to the last one
        /// </summary>
        /// <param name="sum">Accumulator to return total value</param>
        /// <param name="parentQuantity">Quantity of the Parent</param>
        /// <param name="orderItem">SubItem to be calculated (2nd to last level)</param>
        /// <returns></returns>
        private decimal CalculateOrderItemSubItems(decimal sum, decimal parentQuantity, OrderItem orderItem)
        {
            if (orderItem.SubItems == null || orderItem.SubItems.Count == 0)
                return 0;
            foreach (var item in orderItem.SubItems)
            {
                sum += (parentQuantity * item.Quantity * item.UnitPrice) + CalculateOrderItemSubItems(sum, item.Quantity, item);
            }
            return sum;
        }
    }
}

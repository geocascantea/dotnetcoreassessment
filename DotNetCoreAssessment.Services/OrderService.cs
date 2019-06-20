using DotNetCoreAssessment.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreAssessment.Services
{
    public class OrderService : IOrderService
    {
        public Order CreateOrder(string customerId)
        {
            Order order = new Order()
            {
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
            ValidateIfProductIdExist(order.OrderItems, orderItem.ProductId);

            

            AddChildToSubItem(order.OrderItems, parentOrderItemId, orderItem);

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
            order.OrderSubTotal = GetOrderSubTotal(order.OrderItems);
        }

        /// <summary>Adds the child to sub item.</summary>
        /// <param name="orderItems">The order items.</param>
        /// <param name="parentOrderItemId">The parent order item identifier.</param>
        /// <param name="orderItem">The order item.</param>
        private void AddChildToSubItem(ICollection<OrderItem> orderItems, Guid parentOrderItemId, OrderItem orderItem)
        {
            foreach (var child in orderItems)
            {
                if (child.OrderItemId == parentOrderItemId)
                {
                    if (child.SubItems == null)
                        child.SubItems = new List<OrderItem>();
                    child.SubItems.Add(orderItem);
                }

                if (child.SubItems != null)
                {
                    AddChildToSubItem(child.SubItems, parentOrderItemId, orderItem);
                }
            }

            return;
        }

        /// <summary>Gets the order sub total.</summary>
        /// <param name="orderItems">The order items.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>order sub total</returns>
        private decimal GetOrderSubTotal(ICollection<OrderItem> orderItems, OrderItem parent = null)
        {
            decimal subItemTotal = 0;

            foreach (var child in orderItems)
            {
                if (child.SubItems != null)
                {
                    subItemTotal += GetOrderSubTotal(child.SubItems, child);

                    return (child.Quantity * child.UnitPrice) + subItemTotal;
                }

                if (parent != null)
                    subItemTotal += parent.Quantity * (child.Quantity * child.UnitPrice);
            }

            return subItemTotal;
        }

        /// <summary>Validates if product identifier exist.</summary>
        /// <param name="orderItems">The order items.</param>
        /// <param name="productId">The product identifier.</param>
        /// <exception cref="Exception">the order is already registered</exception>
        private void ValidateIfProductIdExist(ICollection<OrderItem> orderItems, string productId)
        {
            foreach (var child in orderItems)
            {
                if (child.SubItems != null)
                {
                    ValidateIfProductIdExist(child.SubItems, productId);
                }

                if (child.ProductId == productId)
                {
                    throw new Exception("the order is already registered");
                }
            }
        }
    }
}
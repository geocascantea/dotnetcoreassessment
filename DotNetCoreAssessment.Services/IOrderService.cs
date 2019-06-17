﻿using DotNetCoreAssessment.Models;
using System;

namespace DotNetCoreAssessment.Services
{
    public interface IOrderService
    {
        Order CreateOrder(string customerId);
        void AddOrderItem(Order order, OrderItem orderItem);
        void AddSubItem(Order order, Guid parentOrderItemId, OrderItem orderItem);
    }
}

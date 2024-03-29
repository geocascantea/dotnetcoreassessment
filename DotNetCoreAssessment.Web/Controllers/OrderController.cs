﻿using DotNetCoreAssessment.Models;
using DotNetCoreAssessment.Services;
using DotNetCoreAssessment.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreAssessment.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController()
        {
            _orderService = new OrderService();
        }
        // POST api/order
        /// <summary>
        /// Creates a new Order instance for the given customer identifier.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] string customerId)
        {
            Order order = _orderService.CreateOrder(customerId);
            return Ok(order);
        }

        // PUT api/order/OrderItem
        /// <summary>
        /// Add a new OrderItem into the given Order instance.
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("OrderItem")]
        public IActionResult AddOrderItem ([FromBody] AddOrderItemViewModel payload)
        {
            _orderService.AddOrderItem(payload.Order, payload.OrderItem);
            return Ok(payload.Order);
        }
    }
}

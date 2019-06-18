using DotNetCoreAssessment.Models;
using DotNetCoreAssessment.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace DotNetCoreAssessment.Tests.Services
{
    public class OrderServiceTest
    {
        private readonly OrderService _sut;
        private Guid _happyMealOrderItemId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private Guid _cheeseBurgerOrderItemId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        public OrderServiceTest()
        {
            _sut = new OrderService();
        }

        [Fact]
        public void Create_Parameterless_CreatesNewOrder()
        {
            //Act
            Order order = _sut.CreateOrder("Customer1");

            //Assert
            Assert.NotNull(order);
        }

        [Fact]
        public void AddOrderItem_GivenOrderItem_RecalculatesSubTotal()
        {
            //Arrange
            Order order = GenerateOrder();
            OrderItem orderItem = GenerateMcHappyMeal(2);
            decimal expectedSubTotal = 20;

            //Act
            _sut.AddOrderItem(order, orderItem);

            //Assert
            Assert.Equal(expectedSubTotal, order.OrderSubTotal);
        }

        [Fact]
        public void AddSubItem_Added1ExtraBacconTo2McHappyMeal_RecalculatesSubTotal()
        {
            //Arrange
            Order order = GenerateOrder();
            order.OrderItems.Add(GenerateMcHappyMeal(quantity: 2));
            OrderItem subItem = GenerateExtraBaccon(quantity: 1);
            decimal expectedSubTotal = 22;

            //Act
            _sut.AddSubItem(order, _happyMealOrderItemId, subItem);

            //Assert
            Assert.Equal(expectedSubTotal, order.OrderSubTotal);
        }

        [Fact]
        public void AddSubItem_Added2ExtraBacconTo2ChesseBurger_RecalculatesSubTotal()
        {
            //Arrange
            Order order = GenerateOrder();
            order.OrderItems.Add(GenerateMcHappyMeal(quantity: 1, cheeseBurgerQuantity: 2));
            OrderItem subItem = GenerateExtraBaccon(2);
            decimal expectedSubTotal = 14;

            //Act
            _sut.AddSubItem(order, _cheeseBurgerOrderItemId, subItem);

            //Assert
            Assert.Equal(expectedSubTotal, order.OrderSubTotal);
        }


        [Fact]
        public void AddSubItem_AddingMacHappyMealUnderChesseBurger_ThrowsException()
        {
            //Arrange
            Order order = GenerateOrder();
            order.OrderItems.Add(GenerateMcHappyMeal(quantity: 1, cheeseBurgerQuantity: 2));
            OrderItem subItem = GenerateMcHappyMeal();

            //Assert
            Assert.ThrowsAny<Exception>(() => _sut.AddSubItem(order, _cheeseBurgerOrderItemId, subItem));
        }

        private Order GenerateOrder()
        {
            return new Order()
            {
                CustomerId = "Customer1",
                OrderSubTotal = 0
            };
        }

        private OrderItem GenerateMcHappyMeal(decimal quantity = 1, decimal cheeseBurgerQuantity = 1)
        {
            return new OrderItem()
            {
                OrderItemId = _happyMealOrderItemId,
                ProductId = "McHappyMeal",
                Quantity = quantity,
                UnitPrice = 10,
                SubItems = new List<OrderItem> {
                            new OrderItem(){
                                OrderItemId = _cheeseBurgerOrderItemId,
                                ProductId = "Cheeseburger",
                                Quantity = cheeseBurgerQuantity,
                                UnitPrice = 0
                            },
                            new OrderItem(){
                                ProductId = "FrenchFries",
                                Quantity = 1,
                                UnitPrice = 0
                            },
                            new OrderItem(){
                                ProductId = "OrangeJuice",
                                Quantity = 1,
                                UnitPrice = 0
                            }
                        }
            };

        }

        private OrderItem GenerateExtraBaccon(decimal quantity = 1)
        {
            return new OrderItem()
            {
                ProductId = "ExtraBaccon",
                Quantity = quantity,
                UnitPrice = 1
            };
        }
    }




}

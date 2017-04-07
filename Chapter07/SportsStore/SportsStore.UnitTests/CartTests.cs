using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using Moq;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Add_New_Lines()
        {
            // 准备一些测试产品
            Product p1 = new Product {ProductID = 1,Name = "P1"};
            Product p2 = new Product {ProductID = 2,Name = "P2" };

            // 准备一个购物车
            Cart target = new Cart();
            
            // 动作
            target.AddItem(p1,1);
            target.AddItem(p2,1);

            CartLine[] results = target.Lines.ToArray();

            // 断言
            Assert.AreEqual(results.Length,2);
            Assert.AreEqual(results[0].Product,p1);
            Assert.AreEqual(results[1].Product,p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines() {
            // 准备一些测试产品
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            // 准备一个购物车
            Cart target = new Cart();

            // 动作
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1,10);
            CartLine[] results = target.Lines.OrderBy(c=>c.Product.ProductID).ToArray();

            // 断言
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // 准备一些测试产品
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            // 准备一个购物车
            Cart target = new Cart();

            
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2,1);
            // 动作
            target.RemoveLine(p2);

            // 断言
            Assert.AreEqual(target.Lines.Count(c=>c.Product==p2),0);
            Assert.AreEqual(target.Lines.Count(),2);
        }

        [TestMethod]
        public void Calculate_Cart_Total() {
            // 准备数据
            Product p1 = new Product {ProductID = 1,Name = "p1",Price = 100M};
            Product p2 = new Product {ProductID = 2,Name = "p2",Price = 50M };

            Cart target = new Cart();

            // act
            target.AddItem(p1,1);
            target.AddItem(p2,1);
            target.AddItem(p1,3);

            // Assert
            Assert.AreEqual(target.ComputeTotalValue(),450M);
        }

        [TestMethod]
        public void Can_Clear_Contest() {
            // 准备数据
            Product p1 = new Product { ProductID = 1, Name = "p1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "p2", Price = 50M };

            Cart target = new Cart();

            // act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);

            target.Clear();

            // Assert
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_TO_Cart()
        {
            // 准备一个模仿存储库
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ProductID=1,Name="P1",Category="Apples"},
            }.AsQueryable());

            // 准备-创建Cart
            Cart cart = new Cart();

            // 准备-创建Cart控制器
            CartController target = new CartController(mock.Object);

            // 动作--对Cart添加一个产品
            target.AddToCart(cart,1,null);

            // 断言
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID,1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            // 准备一个模仿存储库
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ProductID=1,Name="P1",Category="Apples"},
            }.AsQueryable());

            // 准备-创建Cart
            Cart cart = new Cart();

            // 准备-创建Cart控制器
            CartController target = new CartController(mock.Object);

            // 动作--对Cart添加一个产品
            RedirectToRouteResult result =  target.AddToCart(cart, 2, "myUrl");

            // 断言
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // 准备-创建Cart
            Cart cart = new Cart();

            // 准备-创建Cart控制器
            CartController target = new CartController(null);

            // 动作--调用Index动作方法
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart,"myUrl").ViewData.Model;

            // 断言
            Assert.AreEqual(result.Cart,cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
    }
}

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
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
    }
}

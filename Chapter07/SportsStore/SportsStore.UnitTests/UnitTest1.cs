using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Mvc;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void Can_Paginate() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"},
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // act
            ProductsListViewModel result = (ProductsListViewModel) controller.List(null,2).Model;
            // Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange - define an HTML helper - we need to do this
            // in order to apply the extension method
            HtmlHelper myHelper = null;
            // Arrange - create PagingInfo data
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            // Arrange - set up the delegate using a lambda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            // Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            // Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"+ @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
            + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
            result.ToString());
        }

        [TestMethod]
        public void Can_Create_Categories() {
            // 准备 创建模仿存储库
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m=>m.Products).Returns(new Product[] {
                new Product{ProductID = 1,Name = "P1",Category = "Apples"},
                new Product{ProductID = 2,Name = "P2",Category = "Apples"},
                new Product{ProductID = 3,Name = "P3",Category = "Plums"},
                new Product{ProductID = 4,Name = "P4",Category = "Oranges"},
            });

            // 准备-创建控制器
            NavController target = new NavController(mock.Object);

            //动作--获取分类集合
            string[] results = ((IEnumerable<string>) target.Menu().Model).ToArray();

            // 断言
            Assert.AreEqual(results.Length,3);
            Assert.AreEqual(results[0],"Apples");
            Assert.AreEqual(results[1],"Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        [TestMethod]
        public void Indicates_Seceted_Category()
        {
            // 准备 创建模仿存储库
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ProductID = 1,Name = "P1",Category = "Apples"},
                new Product{ProductID = 4,Name = "P4",Category = "Oranges"},
            });

            // 准备-创建控制器
            NavController target = new NavController(mock.Object);

            // 准备已选分类
            string categoryToSecelt = "Apples";
            
            //动作--获取分类集合
            string results = target.Menu(categoryToSecelt).ViewBag.SelectedCategory;

            // 断言
            Assert.AreEqual(categoryToSecelt, results);
        }
        /// <summary>
        ///  单元测试：特定分类的产品数
        /// </summary>
        [TestMethod]
        public void Generate_Category_Spectific_Product_Count()
        {
            // 准备 创建模仿存储库
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ProductID = 1,Name = "P1",Category = "Cat1"},
                new Product{ProductID = 2,Name = "P2",Category = "Cat2"},
                new Product{ProductID = 3,Name = "P3",Category = "Cat1"},
                new Product{ProductID = 4,Name = "P4",Category = "Cat2"},
                new Product{ProductID = 5,Name = "P5",Category = "Cat3"},
            });

            // 准备-创建控制器并使页面容纳3个物品
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            //动作--测试不同分类产品数
            int res1 = ((ProductsListViewModel) target.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)target.List("Cat2").Model).PagingInfo.TotalItems;
            int res3= ((ProductsListViewModel)target.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)target.List(null).Model).PagingInfo.TotalItems;
            // 断言
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }

    }
}
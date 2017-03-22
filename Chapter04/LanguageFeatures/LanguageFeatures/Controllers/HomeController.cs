using LanguageFeatures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller {
        // GET: Home
        public string Index() {
            return "Navigate to a URL to show an example";
        }

        public ViewResult AutoProperty() {
            Product myProduct = new Product();
            myProduct.Name = "Kayak";
            string productName = myProduct.Name;
            return View("Result", (object)String.Format("Product name: {0}", productName));
        }
        public ViewResult CreateProduct() {
            //Product myProduct = new Product();
            //// 设置属性
            //myProduct.ProductID = 100;
            //myProduct.Name = "Kayak";
            //myProduct.Description = "A boat for one person";
            //myProduct.Price = 275M;
            //myProduct.Category = "Watersports";

            // 使用对象初始化器特性
            Product myProduct = new Product {
                ProductID = 100,
                Name = "Kayak",
                Description = "A boat for one person",
                Price = 275M,
                Category = "Watersports"
            };
            return View("Result", (object)string.Format("Category: {0}", myProduct.Category));
        }

        public ViewResult CreateCollection() {
            string[] stringArray = { "apple", "orange", "plum" };
            List<int> intList = new List<int> { 10, 20, 30, 40 };
            Dictionary<string, int> myDict = new Dictionary<string, int> {
                { "apple",10},{ "orange",20},{ "plum",30}
            };
            return View("Result", (object)stringArray[1]);
        }

        public ViewResult UseExtension() {
            ShoppingCart cart = new ShoppingCart {
                Products = new List<Product> {
                    new Product {Name="Kayak",Price=275M},
                    new Product {Name="Lifejacket",Price=48.95M},
                    new Product {Name="Soccer ball",Price=19.50M},
                    new Product {Name="Corner flag",Price=34.95M},
                }
            };
            // 调用扩展方法
            decimal cartTotal = cart.TotalPrices();

            return View("Result", (object)string.Format("Total: {0:c}", cartTotal));
        }

        public ViewResult UseExtensionEnumerable() {
            IEnumerable<Product> products = new ShoppingCart {
                Products = new List<Product> {
                    new Product {Name="Kayak",Price=275M},
                    new Product {Name="Lifejacket",Price=48.95M},
                    new Product {Name="Soccer ball",Price=19.50M},
                    new Product {Name="Corner flag",Price=34.95M}
                }
            };

            Product[] productArray = {
                new Product {Name="Kayak",Price=275M},
                new Product {Name="Lifejacket",Price=48.95M},
                new Product {Name="Soccer ball",Price=19.50M},
                new Product {Name="Corner flag",Price=34.95M}
            };

            decimal cartTotal = products.TotalPrices();
            decimal arrayTotal = productArray.TotalPrices();

            return View("Result", (object)string.Format("Cart Total: {0}, Array Total: {1}", cartTotal, arrayTotal));
        }

        public ViewResult UseFilterExtensionMethod() {
            IEnumerable<Product> products = new ShoppingCart {
                Products = new List<Product> {
                    new Product {Name="Kayak", Category="Watersports", Price=275M},
                    new Product {Name="Lifejacket", Category="Watersports", Price=48.95M},
                    new Product {Name="Soccer ball", Category="Soccer", Price=19.50M},
                    new Product {Name="Corner flag", Category="Soccer", Price=34.95M}
                }
            };

            // 过滤委托
            //Func<Product, bool> categoryFilter = delegate (Product prod) {
            //    return prod.Category == "Soccer";
            //};

            // 使用lambda表达式代替委托定义
            //Func<Product, bool> categoryFilter = prod => prod.Category == "Soccer";

            decimal total = 0;
            //IEnumerable<Product> filterProducts = products.FilterByCategory("Soccer");
            //IEnumerable<Product> filterProducts = products.Filter(categoryFilter);

            // 无Func lambda形式
            IEnumerable<Product> filterProducts = products.Filter(prod => prod.Category == "Soccer" || prod.Price > 20);
            foreach (Product prod in filterProducts) {
                total += prod.Price;
            }

            /*just test 自动类型接口var*/
            var myVariable = new Product { Name = "Kayak", Category = "Watersports", Price = 275M };
            string name = myVariable.Name;
            //int count = myVariable.Count();

            /*匿名类型*/
            var myAnonType = new {
                Name = "MVC",
                Category = "Pattern"
            };

            return View("Result", (object)string.Format("Total:{0}, Count: {1}", total, filterProducts.Count()));
        }

        public ViewResult CreateAnonArray() {
            var oddsAndEnds = new[] {
                new { Name="MVC", Category="Pattern"},
                new { Name="Hat", Category="Clothing"},
                new { Name="Apple", Category="Fruit"},
            };

            StringBuilder result = new StringBuilder();
            foreach (var item in oddsAndEnds) {
                result.Append(item.Name).Append(" ");
            }
            return View("Result",(object)result.ToString());
        }

        
        public ViewResult FindProducts() {
            Product[] products = {
                new Product {Name="Kayak", Category="Watersports", Price=275M},
                new Product {Name="Lifejacket", Category="Watersports", Price=48.95M},
                new Product {Name="Soccer ball", Category="Soccer", Price=19.50M},
                new Product {Name="Corner flag", Category="Soccer", Price=34.95M}
            };

            //// 获取products 中最高的3个价格---传统方法
            //// 1.定义一个数组保存结果
            //Product[] foundProducts = new Product[3];
            //// 2.对products数组内容按价格进行排序
            //Array.Sort(products,(item1,item2)=> {
            //    return Comparer<decimal>.Default.Compare(item1.Price, item2.Price);
            //});
            //Array.Reverse(products);
            //// 3.获取products数组的前3项作为结果
            //Array.Copy(products,foundProducts,3);

            // 获取products 中最高的3个价格---LINQ
            //var foundProducts = from match in products
            //                    orderby match.Price descending
            //                    select new { match.Name, match.Price};
            // 使用点符号
            var foundProducts = products.OrderByDescending(e => e.Price)
                .Take(3)
                .Select(e=>new { e.Name,e.Price});
            int count = 0;
            StringBuilder result = new StringBuilder();
            foreach (var p in foundProducts) {
                result.AppendFormat("Product Name:{1} Price: {0}",p.Price,p.Name);
                //if (++count == 3) {
                //    break;
                //}
            }

            return View("Result",(object)result.ToString());
        }
    }
}
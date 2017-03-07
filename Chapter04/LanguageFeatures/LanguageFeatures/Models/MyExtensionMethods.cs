using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageFeatures.Models {
    public static class MyExtensionMethods {
        // this 把本方法标记味一个扩展方法//
        // 直接用ShoppingCart类对象访问本方法即访问扩展方法
        public static decimal TotalPrices1(this ShoppingCart cartParam) {
            decimal total = 0;
            foreach (Product prod in cartParam.Products) {
                total += prod.Price;
            }
            return total;
        }

        // 对接口运用扩展方法
        public static decimal TotalPrices(this IEnumerable<Product> productEnum) {
            decimal total = 0;
            foreach (Product prod in productEnum) {
                total += prod.Price;
            }
            return total;
        }

        // 创建过滤扩展方法
        public static IEnumerable<Product> FilterByCategory(this IEnumerable<Product> productEnum, string categoryParam) {
            foreach (Product prod in productEnum) {
                if (prod.Category == categoryParam) {
                    yield return prod;
                }
            }
        }

        public static IEnumerable<Product> Filter(
            this IEnumerable<Product> productEnum, 
            Func<Product,bool> selectorParam) {
            foreach (Product prod in productEnum) {
                if (selectorParam(prod)) {
                    yield return prod;
                }
            }
        }

    }
}
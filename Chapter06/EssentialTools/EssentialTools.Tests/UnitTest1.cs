using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EssentialTools.Models;

namespace EssentialTools.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private IDiscountHelper getTestObject()
        {
            return new MinimumDiscountHelper();
        }
       
        /*
         *  total大于100，折扣为10%
         *  介于10到100之间折扣是5
         *  小于10，无折扣
         *  为负值是抛出ArgumentOutOfRangeException
        */
        [TestMethod]
        public void Discount_Above_100()
        {
            // 准备
            IDiscountHelper target = getTestObject();
            decimal total = 200;

            // 动作
            var discountedTotal = target.ApplyDiscount(total);

            // 断言
            Assert.AreEqual(total *0.9M,discountedTotal);
        }

        [TestMethod]
        public void Discount_Between_10_And_100()
        {
            // 准备
            IDiscountHelper target = getTestObject();


            // 动作
            decimal TenDollarDiscount = target.ApplyDiscount(10);
            decimal HundredDollarDiscount = target.ApplyDiscount(100);
            decimal FiftyDollarDiscount = target.ApplyDiscount(50);

            // 断言
            Assert.AreEqual(5,TenDollarDiscount,"$10 discount iswrong");
            Assert.AreEqual(95, HundredDollarDiscount, "$100 discount iswrong");
            Assert.AreEqual(45, FiftyDollarDiscount, "$50 discount iswrong");
        }

        [TestMethod]
        public void Discount_Less_Than_10()
        {
            // 准备
            IDiscountHelper target = getTestObject();
     
            // 动作
            decimal discount5 = target.ApplyDiscount(5);
            decimal discount0 = target.ApplyDiscount(0);

            // 断言
            Assert.AreEqual(5, discount5);
            Assert.AreEqual(0, discount0);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Discount_Negative_Total()
        {
            // 准备
            IDiscountHelper target = getTestObject();
            // 动作
            target.ApplyDiscount(-1);
        }

    }
}

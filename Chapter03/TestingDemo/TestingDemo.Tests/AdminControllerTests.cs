using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingDemo.Models;
using System.Collections.Generic;
using System.Linq;
using TestingDemo.Controllers;

namespace TestingDemo.Tests{
    [TestClass]
    public class AdminControllerTests{
        [TestMethod]
        public void CanChangeLoginName() {
            // 准备--建立场景
            User user = new User() { LoginName="Bob"};
            FakeRepository repositioryParam = new FakeRepository();
            repositioryParam.Add(user);
            AdminController target = new AdminController(repositioryParam);
            string oldLoginName = user.LoginName;
            string newLoginName = "Joe";

            // 动作--尝试相应操作
            target.ChangeLoginName(oldLoginName,newLoginName);

            // 断言--验证结果
            Assert.AreEqual(newLoginName,user.LoginName);
            Assert.IsTrue(repositioryParam.DidSubmitChanges);
        }
    }

    class FakeRepository : IUserRepository {
        public List<User> Users = new List<User>();
        public bool DidSubmitChanges = false;
        public void Add(User newUser) {
            Users.Add(newUser);
        }
        public User FetchByLoginName(string loginName) {
            return Users.First(m=>m.LoginName==loginName);
        }
        public void SubmitChanges() {
            DidSubmitChanges = true;
        }
    }

}

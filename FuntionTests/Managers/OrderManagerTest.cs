using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using Assert = NUnit.Framework.Assert;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services.Managers;


namespace SkillBank.FunctionTests
{
    [TestClass]
    public class OrderManagerTests
    {
        private int _memberId;
        
        OrderManager _mgr;

        [TestInitialize]
        public void TestInitialize()
        {
            _memberId = 1;
            
            OrderRepository rep = new OrderRepository();
            _mgr = new OrderManager(rep);
        }

        [TestMethod]
        public void Should_GetOrderByListByStudentId()
        {
            var result = _mgr.GetOrderListByStudent(_memberId, false);
            Assert.IsNotNull(result);
        }
        
        [TestMethod]
        public void Should_GetOrderByListByStudentId_Correctly()
        {
            var result = _mgr.GetOrderListByStudent(_memberId, false);
            var resultAfterOrder = result.OrderByDescending(item=>item.OrderId).ToList();
            Assert.IsNotNull(result);
            for(int i=0;i<result.Count();i++)
            {
                Assert.AreEqual(resultAfterOrder[i].OrderId, result[i].OrderId);
            }
        }
        
        [TestMethod]
        public void Should_GetOrderByListByTeacherId()
        {
            var result = _mgr.GetOrderListByTeacher(_memberId, false);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Should_GetOrderByListByTeacherId_Correctly()
        {
            var result = _mgr.GetOrderListByTeacher(_memberId, false);
            var resultAfterOrder = result.OrderByDescending(item => item.OrderId).ToList();
            Assert.IsNotNull(result);
            for (int i = 0; i < result.Count(); i++)
            {
                Assert.AreEqual(resultAfterOrder[i].OrderId, result[i].OrderId);
            }
        }
    }
}
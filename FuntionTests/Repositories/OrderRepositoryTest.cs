using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.FunctionTests
{
    [TestClass]
    public class OrderRepositoryTests
    {

        private int _memberId;
        private int _memberBId;
        private int _classId;
        private int _orderId;
        private int _invalidMemberAId;
        //private int _invalidMemberBId;
        Byte _loadType;
        private OrderRepository _repository;
        private MemberRepository _memberRep;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new OrderRepository();
            _memberRep = new MemberRepository();

            _memberBId = 2;//student
            _memberId = 1;//teacher
            _classId = 1;
            _orderId = 1;
            _invalidMemberAId = 99999;
            //_invalidMemberBId = 99998;
            //Get from DB By Id
            _loadType = (Byte)Enums.DBAccess.OrderLoadType.ByOrderId;
        }

        

        [TestMethod]
        public void Should_GetOrderList_ByStudent()
        {
            var result = _repository.GetOrderListByStudent(_memberId, false);
            Assert.IsNotNull(result); 
        }

              
        //[TestMethod]
        //public void Should_GetOrder_ByStatusList_Correctly()//test in mySQL if status can use in(1,2,3) etc
        //{
        //    String statusList = "1,2,3";
        //    Byte loadType = (Byte)Enums.DBAccess.OrderLoadType.ByStatus;
        //    var orders = _repository.GetOrders(loadType, _classId,statusList, null, null);
        //    Assert.IsNotNull(orders);
        //    orders.Count.AssertIsGreaterThan(0);
        //    foreach (var orderItem in orders)
        //    {
        //        Assert.IsTrue(statusList.Contains(String.Format("{0},", orderItem.OrderStatus.ToString())));
        //    }
        //}

        //[TestMethod]
        //public void ShouldNot_GetOrder_ByIncorrectlyStatusList()//test in mySQL if status can use in(1,2,3) etc
        //{
        //    String statusList = "9,10";
        //    Byte loadType = (Byte)Enums.DBAccess.OrderLoadType.ByStatus;
        //    var orders = _repository.GetOrders(loadType, _classId, statusList, null, null);
        //    Assert.IsNull(orders);
        //}

        //[TestMethod]
        //public void Should_GetOrder_InCorrectlyOrder()// Order by BookedDate
        //{
        
        //}

        
        [TestMethod]
        public void ShouldNot_AddOrder_ForInvalidMemberId()
        {
            Order order = new Order();
            order.Student_Id = _invalidMemberAId;
            order.Class_Id = _classId;
            order.BookedDate = DateTime.Now.AddDays(1);
            order.Remark = "Here is remark";
            int orderId = _repository.AddOrder(order.Student_Id, order.Class_Id, order.BookedDate, order.Remark);

            Assert.AreEqual(orderId,-1);
        }

        //TO DO:Add later, if teacher and student id are same
        [TestMethod]
        public void ShouldNot_AddOrder_IfMemberIdAndStudentIdAreSame()
        {
        }
        

        [TestMethod]
        public void Should_LockCoins_Correctly()
        {
            var memberInfo = _memberRep.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberId, _memberId);
            int coinsBeforeLock = memberInfo.CoinsLocked;
            Random rd = new Random();
            Byte coinsToLock = (Byte)rd.Next(0, 5);
            _repository.LockCoinsByMemberId(_memberId, coinsToLock);
            memberInfo = _memberRep.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.BySocialAccount, "cathy.mu@hotmail.com", 1);
            int coinsAfterLock = memberInfo.CoinsLocked;
            Assert.AreEqual(coinsBeforeLock + coinsToLock, coinsAfterLock);
        }

        [TestMethod]
        public void Should_UnLockCoins_Correctly()
        {
            var memberInfo = _memberRep.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberId, _memberId);
            int coinsBeforeUnLock = memberInfo.CoinsLocked;
            Random rd = new Random();
            Byte coinsToUnLock = (Byte)rd.Next(0, 5);
            _repository.UnLockCoinsByMemberId(_memberId, coinsToUnLock);
            memberInfo = _memberRep.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberId, _memberId);
            int coinsAfterUnLock = memberInfo.CoinsLocked;
            Assert.AreEqual(coinsBeforeUnLock - coinsToUnLock, coinsAfterUnLock);
        }

        [TestMethod]
        public void Should_PayCoins_Correctly()
        {
            var memberInfo = _memberRep.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberId, _memberId);
            var teacherInfo = _memberRep.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberId, _memberBId);
            int coinsBeforePay = memberInfo.Coins;
            int teacheCoinsBeforePay = teacherInfo.Coins;
            Random rd = new Random();
            Byte coinsToPay = (Byte)rd.Next(0, 5);
            _repository.PayCoinsByMemberId(_memberId,_memberBId, coinsToPay);
            memberInfo = _memberRep.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberId, _memberId);
            teacherInfo = _memberRep.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberId, _memberBId);
            int coinsAfterPay = memberInfo.Coins;
            int teacheCoinsAfterPay = teacherInfo.Coins;
            Assert.AreEqual(coinsBeforePay - coinsToPay, coinsAfterPay);
            Assert.AreEqual(teacheCoinsBeforePay + coinsToPay, teacheCoinsAfterPay);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _repository.Dispose();
        } 
              
    }
}


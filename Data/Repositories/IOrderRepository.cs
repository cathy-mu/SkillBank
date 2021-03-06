﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

using SkillBank.Site.DataSource.Mapper;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.Site.DataSource.Data
{
    public interface IOrderRepository
    {
        Byte AddOrder(out String deviceToken, int studentId, int classId, DateTime bookDate, String remark);
        Byte UpdateOrderDate(int orderId, DateTime bookDate);
        Byte UpdateOrderStatus(out String deviceToken, int orderId, Byte orderStatus, int studentId, int teacherId);
        Byte UpdateOrderStatusWithCoins(out String deviceToken, int orderId, Byte orderStatus, int studentId, int teacherId);

        //List<Order> GetOrders(Byte loadType, int paraId);
        List<OrderItem> GetOrderListByStudent(int studentId, Boolean shouldCheck);
        List<OrderItem> GetOrderListByTeacher(int teacherId, Boolean shouldCheck);

        //void AddCoinsByMemberId(int memberId, Byte coinsToAdd);
        Byte LockCoinsByMemberId(int studentId, Byte coinsToLock);
        //void UnLockCoinsByMemberId(int studentId, Byte coinsToUnLock);
        Byte PayCoinsByMemberId(int studentId, int teacherId, Byte coinsToPay);
        void HandleMemberOrder(int memberId);
    }

    public class OrderRepository : Entities, IOrderRepository
    {
        #region Public Methods

        public OrderRepository()
        //: base("name=Entities")
        {
        }

        public void HandleMemberOrder(int memberId)
        {
            OrderStatus_ListHandler_p(memberId);
        }

        //Student chnage book date before accepted
        public Byte UpdateOrderDate(int orderId, DateTime bookDate)
        {
            Byte saveType = (Byte)Enums.DBAccess.OrderSaveType.UpdateBookedDate;
            String deviceToken; 
            //only need order id and booke date as para
            return Order_Save(out deviceToken, saveType, 0, 0, 0, bookDate, "", orderId);
        }

        public Byte AddOrder(out String deviceToken, int studentId, int classId, DateTime bookDate, String remark)
        {
            Byte status = (Byte)Enums.OrderStatus.Booked;
            Byte saveType = (Byte)Enums.DBAccess.OrderSaveType.AddNew;

            return Order_Save(out deviceToken, saveType, studentId, classId, status, bookDate, remark, 0);
        }
        
        /// <summary>
        /// Only chanage status tag, not coins update
        /// orderStatus == 2 || 3 || 5 || 6 || 8 || 12
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public Byte UpdateOrderStatus(out String deviceToken, int orderId, Byte orderStatus, int studentId, int teacherId = 0)
        {
            Byte saveType = (Byte)Enums.DBAccess.OrderSaveType.UpdateSatatus;
            return Order_Save(out deviceToken, saveType, studentId, 0, orderStatus, new DateTime(1900, 01, 01), "", orderId, teacherId);
        }

        //Coins related status update
        //orderStatus == 4 || 7 || 9 || 10 || 11
        public Byte UpdateOrderStatusWithCoins(out String deviceToken, int orderId, Byte status, int studentId, int teacherId)
        {
            return OrderCoin_Update_p(out deviceToken, orderId, status, studentId, teacherId);
        }

        //public List<Order> GetOrders(Byte loadType, int paraId)
        //{
        //    var loadTypeParameter = new ObjectParameter("LoadType", loadType);
        //    var idParameter = new ObjectParameter("ParaId", paraId);

        //    ObjectResult<Order_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Order_Load_p_Result>("Order_Load_p", MergeOption.NoTracking, loadTypeParameter, idParameter);
        //    return OrderMapper.Map(result);
        //}


        public List<OrderItem> GetOrderListByStudent(int studentId, Boolean shouldCheck)
        {
            var idParameter = new ObjectParameter("ParaId", studentId);
            var isCheckParameter = new ObjectParameter("isCheck", shouldCheck);

            ObjectResult<Order_LoadByStudent_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Order_LoadByStudent_p_Result>("Order_LoadByStudent_p", MergeOption.NoTracking, idParameter, isCheckParameter);
            return OrderMapper.Map(result);
        }


        public List<OrderItem> GetOrderListByTeacher(int teacherId, Boolean shouldCheck)
        {
            var idParameter = new ObjectParameter("ParaId", teacherId);
            var isCheckParameter = new ObjectParameter("isCheck", shouldCheck);

            ObjectResult<Order_LoadByTeacher_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Order_LoadByTeacher_p_Result>("Order_LoadByTeacher_p", MergeOption.NoTracking, idParameter, isCheckParameter);
            return OrderMapper.Map(result);
        }


        //public void AddCoinsByMemberId(int memberId, Byte coinsToAdd)
        //{
        //    Byte updateType = (Byte)Enums.DBAccess.CoinUpdateType.AddCoin;
        //    Coins_Update_p(updateType, memberId, 0, coinsToAdd);
        //}

        public Byte LockCoinsByMemberId(int studentId, Byte coinsToLock)
        {
            Byte updateType = (Byte)Enums.DBAccess.CoinUpdateType.LockCoin;
            return Coins_Update_p(updateType, studentId, 0, coinsToLock);
        }

        public void UnLockCoinsByMemberId(int studentId, Byte coinsToUnLock)
        {
            Byte updateType = (Byte)Enums.DBAccess.CoinUpdateType.UnLockCoin;
            Coins_Update_p(updateType, studentId, 0, coinsToUnLock);
        }

        public Byte PayCoinsByMemberId(int studentId, int teacherId, Byte coinsToPay)
        {
            Byte updateType = (Byte)Enums.DBAccess.CoinUpdateType.PayCoin;
            return Coins_Update_p(updateType, studentId, teacherId, coinsToPay);
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="studentId"></param>
        /// <param name="classId"></param>
        /// <param name="orderStatus"></param>
        /// <param name="bookedDate"></param>
        /// <param name="remark"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private Byte Order_Save(out String deviceToken, Byte saveType, int studentId, int classId, Byte orderStatus, DateTime bookedDate, String remark, int orderId, int teacherId = 0)
        {
            deviceToken = "";
            ObjectParameter paraId = new ObjectParameter("paraId", orderId);
            ObjectParameter studentIdParameter = new ObjectParameter("studentId", studentId);
            ObjectParameter teacherIdParameter = new ObjectParameter("teacherId", teacherId);
            ObjectParameter classIdParameter = new ObjectParameter("classId", classId);
            ObjectParameter orderStatusParameter = new ObjectParameter("status", orderStatus);
            ObjectParameter classDateParameter = new ObjectParameter("classDate", bookedDate);
            ObjectParameter saveTypeParameter = new ObjectParameter("saveType", saveType);
            ObjectParameter remarkParameter = new ObjectParameter("comment", String.IsNullOrEmpty(remark) ? "" : remark);
            ObjectParameter resultParameter = new ObjectParameter("result", 0);
            ObjectParameter deviceParameter = new ObjectParameter("deviceToken", deviceToken);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Order_Save_p", studentIdParameter, teacherIdParameter, classIdParameter, orderStatusParameter, classDateParameter, saveTypeParameter, remarkParameter, paraId, resultParameter, deviceParameter);
            deviceToken = (String)deviceParameter.Value;
            return (Byte)resultParameter.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="memberId"></param>
        /// <param name="teacherId"></param>
        /// <param name="amount"></param>
        /// <returns>1 SUCCESS ; 2 Fail ;3 Last coin for this member When try to lock coin</returns>
        public Byte Coins_Update_p(Byte saveType, int memberId, int teacherId, Byte amount)
        {
            ObjectParameter saveTypeParameter = new ObjectParameter("SaveType", saveType);
            ObjectParameter studentIdParameter = new ObjectParameter("StudentId", memberId);
            ObjectParameter teacherIdParameter = new ObjectParameter("TeacherId", teacherId);
            ObjectParameter amountParameter = new ObjectParameter("Amount", amount);
            ObjectParameter resultParameter = new ObjectParameter("Result", 0);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Coins_Update_p", saveTypeParameter, studentIdParameter, teacherIdParameter, amountParameter, resultParameter);
            return (Byte)resultParameter.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="status"></param>
        /// <param name="paraId"></param>
        /// <returns></returns>
        public Byte OrderCoin_Update_p(out String deviceToken, int orderId, Byte status, int studentId, int teacherId = 0)
        {
            deviceToken = "";
            ObjectParameter paraIdParameter = new ObjectParameter("paraId", orderId);
            ObjectParameter statusParameter = new ObjectParameter("status", status);
            ObjectParameter studentIdParameter = new ObjectParameter("studentId", studentId);
            ObjectParameter teacherIdParameter = new ObjectParameter("teacherId", teacherId);
            ObjectParameter resultParameter = new ObjectParameter("result", 0);
            ObjectParameter deviceParameter = new ObjectParameter("deviceToken", deviceToken);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("OrderCoin_Update_p", paraIdParameter, statusParameter, studentIdParameter, teacherIdParameter, resultParameter, deviceParameter);
            deviceToken = (String)deviceParameter.Value;
            return (Byte)resultParameter.Value;
        }

        public void OrderStatus_ListHandler_p(int memberId)
        {
            ObjectParameter memberIdParameter = new ObjectParameter("memberId", memberId);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("OrderStatus_ListHandler_p", memberIdParameter);
        }
    }
}


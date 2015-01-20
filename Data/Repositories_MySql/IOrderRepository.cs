using System;
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
        Byte AddOrder(int studentId, int classId, DateTime bookDate, String remark);
        void UpdateBookDate(int orderId, DateTime bookedDate);
        Byte UpdateOrderStatus(int orderId, Byte orderStatus);
        Byte UpdateOrderStatusWithCoins(int studentId, Byte status, int paraId);

        List<Order> GetOrders(Byte loadType, int paraId);
        List<OrderItem> GetOrderListByStudent(int studentId);
        List<OrderItem> GetOrderListByTeacher(int teacherId);

        void AddCoinsByMemberId(int memberId, Byte coinsToAdd);
        Byte LockCoinsByMemberId(int studentId, Byte coinsToLock);
        void UnLockCoinsByMemberId(int studentId, Byte coinsToUnLock);
        Byte PayCoinsByMemberId(int studentId, int teacherId, Byte coinsToPay);
        
     }

    public class OrderRepository : DbContext, IOrderRepository
    {
        #region Public Methods

        public OrderRepository()
            : base("name=Entities")
        {
        }

        public Byte AddOrder(int studentId, int classId, DateTime bookDate, String remark)
        {
            Byte status = (Byte)Enums.OrderStatus.Booked;
            Byte saveType = (Byte)Enums.DBAccess.OrderSaveType.AddNew;
            return Order_Save(saveType, studentId, classId, status, bookDate, remark, 0);
        }

        public void UpdateBookDate(int orderId, DateTime bookedDate)
        {
            Byte saveType = (Byte)Enums.DBAccess.OrderSaveType.UpdateBookedDate;
            Order order = new Order();
            order.OrderId = orderId;
            order.BookedDate = bookedDate;
            Order_Save(saveType, 0, 0, 0, order.BookedDate, "", order.OrderId);
        }

        public Byte UpdateOrderStatus(int orderId, Byte orderStatus)
        {
            //Only chanage status tag, not coins update
           
                Byte saveType = (Byte)Enums.DBAccess.OrderSaveType.UpdateSatatus;
                return Order_Save(saveType, 0, 0, orderStatus, default(DateTime), "", orderId);
        }

        //Coins related status update
        //orderStatus == 4 || orderStatus == 5 || orderStatus == 7
        public Byte UpdateOrderStatusWithCoins(int studentId, Byte status, int paraId) 
        {
            return OrderCoin_Update_p(studentId, status,paraId);
        }

        public List<Order> GetOrders(Byte loadType, int paraId)
        {
            var loadTypeParameter = new ObjectParameter("LoadType", loadType);
            var idParameter = new ObjectParameter("ParaId", paraId);

            ObjectResult<Order_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Order_Load_p_Result>("Order_Load_p", MergeOption.NoTracking, loadTypeParameter, idParameter);
            return OrderMapper.Map(result);
        }


        public List<OrderItem> GetOrderListByStudent(int studentId)
        {
            var idParameter = new ObjectParameter("ParaId", studentId);

            ObjectResult<Order_LoadByStudent_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Order_LoadByStudent_p_Result>("Order_LoadByStudent_p", MergeOption.NoTracking, idParameter);
            return OrderMapper.Map(result);
        }


        public List<OrderItem> GetOrderListByTeacher(int teacherId)
        {
            var idParameter = new ObjectParameter("ParaId", teacherId);

            ObjectResult<Order_LoadByTeacher_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Order_LoadByTeacher_p_Result>("Order_LoadByTeacher_p", MergeOption.NoTracking, idParameter);
            return OrderMapper.Map(result);
        }



     
    
        public void AddCoinsByMemberId(int memberId, Byte coinsToAdd)
        {
            Byte updateType = (Byte)Enums.DBAccess.CoinUpdateType.AddCoin;
            Coins_Update_p(updateType, memberId, 0, coinsToAdd);
        }

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
        private Byte Order_Save(Byte saveType, int studentId, int classId, Byte? orderStatus, DateTime bookedDate, String remark, int? orderId)
        {
            ObjectParameter paraId = new ObjectParameter("ParaId", orderId.HasValue ? orderId : 0);
            ObjectParameter studentIdParameter = new ObjectParameter("StudentId", studentId);
            ObjectParameter classIdParameter = new ObjectParameter("ClassId", classId);
            ObjectParameter orderStatusParameter = new ObjectParameter("Status", orderStatus.HasValue ? orderStatus : 0);
            ObjectParameter classDateParameter = new ObjectParameter("ClassDate", bookedDate);
            ObjectParameter saveTypeParameter = new ObjectParameter("SaveType", saveType);
            ObjectParameter remarkParameter = new ObjectParameter("Comment", String.IsNullOrEmpty(remark) ? "" : remark);
            ObjectParameter resultParameter = new ObjectParameter("Result", 0);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Order_Save_p", studentIdParameter, classIdParameter, orderStatusParameter, classDateParameter, saveTypeParameter, remarkParameter, paraId, resultParameter);
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
        public Byte OrderCoin_Update_p(int studentId, Byte status, int paraId)
        {
            ObjectParameter studentIdParameter = new ObjectParameter("StudentId", studentId);
            ObjectParameter statusParameter = new ObjectParameter("Status", status);
            ObjectParameter paraIdParameter = new ObjectParameter("ParaId", paraId);
            ObjectParameter resultParameter = new ObjectParameter("Result", 0);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Coins_Update_p", studentIdParameter, statusParameter, paraIdParameter, resultParameter);
            return (Byte)resultParameter.Value;
        }
    }
}


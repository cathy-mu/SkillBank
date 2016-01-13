using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.Site.Services.Managers
{
    public interface IOrderManager
    {
        Byte AddOrder(int studentId, int classId, DateTime bookDate, String remark);
        Byte UpdateOrderDate(int orderId, DateTime bookDate);
        Byte UpdateOrderStatusWithCoins(int orderId, Byte orderStatus, int studentId, int teacherId);
        Byte UpdateOrderStatus(int orderId, Byte orderStatus, int studentId = 0, int teacherId = 0);
        //void UpdateBookDate(int orderId, DateTime bookedDate);

        //List<Order> GetOrdersByStudentId(int memberId);
        //List<Order> GetOrdersByTeacherId(int memberId);
        List<OrderItem> GetOrderListByStudent(int studentId, Boolean shouldCheck);
        List<OrderItem> GetOrderListByTeacher(int teacherId, Boolean shouldCheck);

        void HandleMemberOrder(int memberId);
    }

    public class OrderManager : IOrderManager
    {
        private readonly IOrderRepository _orderRep;

        public OrderManager(IOrderRepository orderRep)
        {
            _orderRep = orderRep;
        }

        public void HandleMemberOrder(int memberId)
        {
            _orderRep.HandleMemberOrder(memberId);
        }

        public Byte UpdateOrderDate(int orderId, DateTime bookDate)
        {
            var result = _orderRep.UpdateOrderDate(orderId, bookDate);
            return result;
        }

        public Byte AddOrder(int studentId, int classId, DateTime bookDate, String remark)
        {
            var result = _orderRep.AddOrder(studentId, classId, bookDate, remark);
            return result;
        }

        //public List<Order> GetOrdersByStudentId(int memberId)
        //{
        //    Byte loadBy = (Byte)Enums.DBAccess.OrderLoadType.ByOrderId;
        //    return _orderRep.GetOrders(loadBy, memberId);
        //}

        //public List<Order> GetOrdersByTeacherId(int memberId)
        //{
        //    Byte loadBy = (Byte)Enums.DBAccess.OrderLoadType.ByTeacherId;
        //    return _orderRep.GetOrders(loadBy, memberId);
        //}

        public List<OrderItem> GetOrderListByTeacher(int teacherId, Boolean shouldCheck)
        {
            return _orderRep.GetOrderListByTeacher(teacherId, shouldCheck);
        }

        public List<OrderItem> GetOrderListByStudent(int studentId, Boolean shouldCheck)
        {
            return _orderRep.GetOrderListByStudent(studentId, shouldCheck);
        }

        public Byte UpdateOrderStatus(int orderId, Byte orderStatus, int studentId = 0, int teacherId = 0)
        {
           return _orderRep.UpdateOrderStatus(orderId, orderStatus, studentId, teacherId);
        }

        /// <summary>
        /// case Enums.OrderStatus.Accepted://4                  case Enums.OrderStatus.RefundProve://7                case Enums.OrderStatus.Confirmed://9                case Enums.OrderStatus.AutoConfirmed://10                case Enums.OrderStatus.AutoRefund://11
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <param name="studentId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public Byte UpdateOrderStatusWithCoins(int orderId, Byte orderStatus, int studentId, int teacherId)
        {
            return _orderRep.UpdateOrderStatusWithCoins(orderId, orderStatus, studentId, teacherId);
        }
        

    }

}

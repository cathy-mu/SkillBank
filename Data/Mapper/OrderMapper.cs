using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.DataSource.Mapper
{
    public class OrderMapper
    {
        //My SQL
        //public static List<Order> Map(ObjectResult<Order> objOrders)
        //{
        //    if (objOrders != null)
        //    {
        //        var orders = objOrders.Select(item => new Order() { OrderId = item.OrderId, BookedDate = item.BookedDate, Class_Id = item.Class_Id, CreatedDate = item.CreatedDate,  LastUpdateDate=item.LastUpdateDate, OrderStatus = item.OrderStatus, Student_Id = item.Student_Id}).ToList();
        //        return (orders.Count > 0) ? orders : null;
        //    }
        //    return null;
        //}

        //public static List<OrderItem> Map(ObjectResult<OrderItem> objOrders)
        //{
        //    if (objOrders != null)
        //    {
        //        var orders = objOrders.Select(item => new OrderItem() { OrderId = item.OrderId, ClassId = item.ClassId, LastUpdateDate = item.LastUpdateDate, OrderStatus = item.OrderStatus, BookedDate = item.BookedDate, Title = item.Title, MemberId = item.MemberId, MemberName = item.MemberName, Email = item.Email, Phone = item.Phone }).ToList();
        //        return (orders.Count > 0) ? orders : null;
        //    }
        //    return null;
        //}

        //SQL

        public static List<Order> Map(ObjectResult<Order_Load_p_Result> objOrders)
        {
            if (objOrders != null)
            {
                var orders = objOrders.Select(item => new Order() { OrderId = item.OrderId, BookedDate = item.BookedDate, Class_Id = item.Class_Id, CreatedDate = item.CreatedDate.Value, LastUpdateDate = item.LastUpdateDate, OrderStatus = item.OrderStatus, Student_Id = item.Student_Id }).ToList();
                return (orders.Count > 0) ? orders : null;
            }
            return null;
        }

        public static List<OrderItem> Map(ObjectResult<Order_LoadByTeacher_p_Result> objOrders)
        {
            if (objOrders != null)
            {
                var orders = objOrders.Select(item => new OrderItem() { OrderId = item.OrderId, ClassId = item.ClassId, LastUpdateDate = item.LastUpdateDate, OrderStatus = item.OrderStatus, BookedDate = item.BookedDate, Title = item.Title, MemberId = item.MemberId, MemberName = item.MemberName, Email = item.Email, Phone = item.Phone, Avatar = item.Avatar, HasReview = item.HasReview, Remark = item.Remark, NotifyTag = item.NotifyTag }).ToList();
                return (orders.Count > 0) ? orders : null;
            }
            return null;
        }

        public static List<OrderItem> Map(ObjectResult<Order_LoadByStudent_p_Result> objOrders)
        {
            if (objOrders != null)
            {
                var orders = objOrders.Select(item => new OrderItem() { OrderId = item.OrderId, ClassId = item.ClassId, LastUpdateDate = item.LastUpdateDate, OrderStatus = item.OrderStatus, BookedDate = item.BookedDate, Title = item.Title, MemberId = item.MemberId, MemberName = item.MemberName, Email = item.Email, Phone = item.Phone, Avatar = item.Avatar, HasReview = item.HasReview, Remark = item.Remark, NotifyTag = item.NotifyTag }).ToList();
                return (orders.Count > 0) ? orders : null;
            }
            return null;
        }
        

    }
}

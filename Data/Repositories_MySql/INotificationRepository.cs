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
    public interface INotificationRepository
    {
        void AddNotification(Byte type, int memberId, int relatedMemberId, int classOrderId);
        void SetNotificationAsRead(Byte type, int memberId, int relatedMemberId, int classOrderId);
        void SetNotificationAsPoped(Byte type, int memberId, int relatedMemberId, int classOrderId);
        void GetNotification(int memberId);
        void GetPopNotification(int memberId);
    }

    public class NotificationRepository : DbContext, INotificationRepository
    {
        #region Public Methods

        public NotificationRepository()
            : base("name=Entities")
        {
        }

        public void AddNotification(Byte type, int memberId, int relatedMemberId, int classOrderId)
        {
        }

        public void SetNotificationAsRead(Byte type, int memberId, int relatedMemberId, int classOrderId)
        {
        }

        public void SetNotificationAsPoped(Byte type, int memberId, int relatedMemberId, int classOrderId)
        {
        }

        //public List<Notification> GetNotification(int memberId)
        //{
        //    var result = Order_Load(loadType, paraId);
        //    return OrderMapper.Map(result);
        //}
        public void GetNotification(int memberId)
        {
        }
        public void GetPopNotification(int memberId)
        {
        }
    
    


        #endregion


        //private ObjectResult<Order> Notification_Load(SByte loadType, int paraId)
        //{
        //    var loadTypeParameter = new ObjectParameter("LoadType", loadType);
        //    var idParameter = new ObjectParameter("ParaId", paraId);

        //    ObjectResult<Order> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Order>("Order_Load_p", loadTypeParameter, idParameter);
        //    return result;
        //}

        private void Notification_Save(Byte saveType, int studentId, int classId, Byte orderStatus, DateTime bookedDate, String remark, int? orderId)
        {
            //ObjectParameter paraId = new ObjectParameter("ParaId", orderId.HasValue ? orderId : 0);
            //ObjectParameter studentIdParameter = new ObjectParameter("StudentId", studentId);
            //ObjectParameter classIdParameter = new ObjectParameter("ClassId", classId);
            //ObjectParameter orderStatusParameter = new ObjectParameter("Status", orderStatus.HasValue ? orderStatus : 0);
            //ObjectParameter classDateParameter = new ObjectParameter("ClassDate", bookedDate);
            //ObjectParameter saveTypeParameter = new ObjectParameter("SaveType", saveType);
            //ObjectParameter remarkParameter = new ObjectParameter("Comment", String.IsNullOrEmpty(remark) ? "" : remark);
            //ObjectParameter resultParameter = new ObjectParameter("Result", 0);

            //((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Order_Save_p", studentIdParameter, classIdParameter, orderStatusParameter, classDateParameter, saveTypeParameter, remarkParameter, paraId);
            //return (SByte)resultParameter.Value;
        }

        
    }
}


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
        //void AddNotification(Byte type, int memberId, int relatedMemberId, int classOrderId);
        void UpdateNotification(Byte saveType, int memberId, int paraId);
        List<NotificationItem> GetNotification(Byte loadType, int memberId);
        List<NotificationAlertItem> GetPopNotification(int memberId, Byte loadType);
    }

    public class NotificationRepository : Entities, INotificationRepository
    {
        #region Public Methods

        public NotificationRepository()
            //: base("name=Entities")
        {
        }

        public List<NotificationItem> GetNotification(Byte loadType, int memberId)
        {
            var result = Notification_Load(loadType, memberId);
            return NotificationMapper.Map(result);
        }

        public List<NotificationAlertItem> GetPopNotification(int memberId, Byte loadType)
        {
            var result = NotificationAlert_Load(memberId, loadType);
            return NotificationMapper.Map(result);
        }

        public void UpdateNotification(Byte saveType, int memberId, int paraId)
        {
            Notification_Save(saveType, memberId, paraId);
        }        

        #endregion


        private ObjectResult<Notification_Load_p_Result> Notification_Load(Byte loadType, int memberId)
        {
            var memberIdParameter = new ObjectParameter("memberId", memberId);
            var loadTypeParameter = new ObjectParameter("loadType", loadType);

            ObjectResult<Notification_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Notification_Load_p_Result>("Notification_Load_p", loadTypeParameter, memberIdParameter);
            return result;
        }

        private ObjectResult<NotificationAlert_Load_p_Result> NotificationAlert_Load(int memberId, Byte loadType)
        {
            var memberIdParameter = new ObjectParameter("memberId", memberId);
            var loadTypeParameter = new ObjectParameter("loadType", loadType);

            ObjectResult<NotificationAlert_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<NotificationAlert_Load_p_Result>("NotificationAlert_Load_p", loadTypeParameter, memberIdParameter);
            return result;
        }

        private void Notification_Save(Byte saveType, int memberId, int paraId)
        {
            var saveTypeParameter = new ObjectParameter("saveType", saveType);
            var memberIdParameter = new ObjectParameter("memberId", memberId);
            var paraIdParameter = new ObjectParameter("paraId", paraId);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Notification_Save_p", saveTypeParameter, memberIdParameter, paraIdParameter);
        }

        
    }
}


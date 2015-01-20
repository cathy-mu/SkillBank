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
        void SetNotificationAsRead(Byte saveType, int paraId);
        void SetNotificationAsClicked(int memberId); 
        List<NotificationItem> GetNotification(SByte loadBy, int memberId, Boolean isCheckStatus);
        List<NotificationAlertItem> GetPopNotification(int memberId, Boolean isCheckStatus);
    }

    public class NotificationRepository : Entities, INotificationRepository
    {
        #region Public Methods

        public NotificationRepository()
            //: base("name=Entities")
        {
        }

        public List<NotificationItem> GetNotification(SByte loadBy, int memberId, Boolean isCheckStatus)
        {
            var result = Notification_Load(loadBy, memberId, isCheckStatus);
            return NotificationMapper.Map(result);
        }

        public List<NotificationAlertItem> GetPopNotification(int memberId, Boolean isCheckStatus)
        {
            var result = NotificationAlert_Load(memberId, isCheckStatus);
            return NotificationMapper.Map(result);
        }

        public void SetNotificationAsRead(Byte saveType, int paraId)
        {
            Notification_Save(saveType, paraId);
        }

        public void SetNotificationAsClicked(int memberId)
        {
            Byte saveType = (Byte)Enums.DBAccess.NotificationTagUpdateType.SetPopTagAsClicked;
            Notification_Save(saveType, memberId);
        }  

        #endregion


        private ObjectResult<Notification_Load_p_Result> Notification_Load(SByte loadBy, int memberId, Boolean checkStauts)
        {
            var loadTypeParameter = new ObjectParameter("loadBy", loadBy);
            var memberIdParameter = new ObjectParameter("memberId", memberId);
            var checkStautsParameter = new ObjectParameter("checkStauts", checkStauts);

            ObjectResult<Notification_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Notification_Load_p_Result>("Notification_Load_p", loadTypeParameter, memberIdParameter, checkStautsParameter);
            return result;
        }

        private ObjectResult<NotificationAlert_Load_p_Result> NotificationAlert_Load(int memberId, Boolean checkStauts)
        {
            var memberIdParameter = new ObjectParameter("memberId", memberId);
            var checkStautsParameter = new ObjectParameter("checkStauts", checkStauts);

            ObjectResult<NotificationAlert_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<NotificationAlert_Load_p_Result>("NotificationAlert_Load_p", memberIdParameter, checkStautsParameter);
            return result;
        }

        private void Notification_Save(Byte saveType, int paraId)
        {
            var saveTypeParameter = new ObjectParameter("saveType", saveType);
            var memberIdParameter = new ObjectParameter("paraId", paraId);

            ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Notification_Save_p", saveTypeParameter, memberIdParameter);
        }

        
    }
}


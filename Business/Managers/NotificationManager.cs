using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.Common;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services.Net.SMS;


namespace SkillBank.Site.Services.Managers
{
    public interface INotificationManager
    {
        //void AddNotificationForNewMesage(int toMemberId, int fromMemberId);
        //void AddNotificationForOrderStatusUpdate(Enums.DBAccess.NotificationSaveType statusType, int toMemberId, int orderId);
        //void AddNotificationForClassStatusUpdate(Enums.DBAccess.NotificationSaveType statusType, int toMemberId, int classId);
        List<NotificationItem> GetNotification(Byte loadType, int memberId);
        List<NotificationAlertItem> GetPopNotification(int memberId, Byte loadType);
        void UpdateNotification(Byte saveType, int memberId, int paraId = 0);
        
        void SendOrderUpdateSMS(Byte statusType, String mobile, String className, Boolean sendSMS = true);
        void SendClassProveSMS(Boolean isProve, String mobile, String className, String link, Boolean sendSMS = true);
        void SendNewMessageSMS(String mobile, String name, String link, Boolean sendSMS = true);
    }

    public class NotificationManager : INotificationManager
    {
        private readonly INotificationRepository _repository;

        public NotificationManager(INotificationRepository repository)
        {
            _repository = repository;
        }

        public void SendOrderUpdateSMS(Byte statusType, String mobile, String className, Boolean sendSMS = true)
        {
            if (sendSMS)
            {
                YunPianSMS.SendOrderUpdateSms(statusType, mobile, className);
            }
        }

        public void SendClassProveSMS(Boolean isProve, String mobile, String className, String link, Boolean sendSMS = true)
        {
            if (sendSMS)
            {
                YunPianSMS.SendClassProveSms(isProve, mobile, className, link);
            }
        }

        public void SendNewMessageSMS(String mobile, String name, String link, Boolean sendSMS = true)
        {
            if (sendSMS)
            {
                YunPianSMS.SendNewMessageSMS(mobile, name, link);
            }
        }

        //public void AddNotificationForNewMesage(int toMemberId, int fromMemberId)
        //{
        //    _repository.AddNotification((Byte)Enums.DBAccess.NotificationSaveType.MessageAdd, toMemberId, fromMemberId, 0);
        //}

        //public void AddNotificationForOrderStatusUpdate(Enums.DBAccess.NotificationSaveType statusType, int toMemberId, int orderId)
        //{
        //    _repository.AddNotification((Byte)statusType, toMemberId, 0, orderId);
        //}

        //public void AddNotificationForClassStatusUpdate(Enums.DBAccess.NotificationSaveType statusType, int toMemberId, int classId)
        //{
        //    _repository.AddNotification((Byte)statusType, toMemberId, 0, classId);
        //}

        public List<NotificationAlertItem> GetPopNotification(int memberId, Byte loadType)
        {
            return _repository.GetPopNotification(memberId, loadType);
        }

        public List<NotificationItem> GetNotification(Byte loadType, int memberId)
        {
            //SByte loadBy = 1;
            return _repository.GetNotification(loadType, memberId);
        }

        public void UpdateNotification(Byte saveType, int memberId, int paraId = 0)
        {
            _repository.UpdateNotification(saveType, memberId, paraId);
        }


    }
}

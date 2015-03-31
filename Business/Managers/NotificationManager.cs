using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.Common;
using SkillBank.Site.DataSource.Data;


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
    }

    public class NotificationManager : INotificationManager
    {
        private readonly INotificationRepository _repository;

        public NotificationManager(INotificationRepository repository)
        {
            _repository = repository;
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

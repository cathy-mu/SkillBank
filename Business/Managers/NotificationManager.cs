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
        List<NotificationItem> GetNotification(int memberId, bool checkStatus);
        List<NotificationAlertItem> GetPopNotification(int memberId, bool checkStatus);
        void SetNotificationAsRead(Byte saveType, int paraId);
        void SetNotificationAsClicked(int memberId);
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

        public List<NotificationAlertItem> GetPopNotification(int memberId, bool checkStatus)
        {
            return _repository.GetPopNotification(memberId, checkStatus);
        }

        public List<NotificationItem> GetNotification(int memberId, bool checkStatus)
        {
            SByte loadBy = 1;
            return _repository.GetNotification(loadBy, memberId, checkStatus);
        }

        public void SetNotificationAsRead(Byte saveType, int paraId)
        {
            _repository.SetNotificationAsRead(saveType, paraId);
        }

        public void SetNotificationAsClicked(int memberId)
        {
            _repository.SetNotificationAsClicked(memberId);
        }


    }
}

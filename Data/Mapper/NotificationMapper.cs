using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.DataSource.Mapper
{
    public class NotificationMapper
    {
        //SQL
        public static List<NotificationItem> Map(ObjectResult<Notification_Load_p_Result> objOrders)
        {
            if (objOrders != null)
            {
                //var notifications = objOrders.Select(item => new NotificationItem() {  TypeId = item.TypeId,  ClassOrderId = item.ClassOrderId,  Name = item.Name }).ToList();
                var notifications = objOrders.Select(item => new NotificationItem() { NotificationId = item.NotificationId, TypeId = item.TypeId, TypeRank = item.TypeRank, RelatedMemberId = item.RelatedMemberId, ClassOrderId = item.ClassOrderId, Name = item.Name, Title = item.Title, Avatar = item.Avatar,LastUpdateDate = item.LastUpdateDate}).ToList();
                return (notifications.Count > 0) ? notifications : null;
            }
            return null;
        }

        public static List<NotificationAlertItem> Map(ObjectResult<NotificationAlert_Load_p_Result> objOrders)
        {
            if (objOrders != null)
            {
                var notificationAlerts = objOrders.Select(item => new NotificationAlertItem() { Type = item.Type, Number = item.Number.Value, Name = item.Name, PopNum = item.PopNum.Value }).ToList();
                return (notificationAlerts.Count > 0) ? notificationAlerts : null;
            }
            return null;
        }

        
        

    }
}

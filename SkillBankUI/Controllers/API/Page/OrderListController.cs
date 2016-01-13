using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.API
{
    public class OrderListController : ApiController
    {
        public readonly ICommonService _commonService;

        public class OrderListModel
        {
            public List<OrderItem> Orders;
            public Dictionary<String, int> Badge;
        }

        public OrderListController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// Get order list for Learning/Teaching page(Private)  (v1.1 15-11-05)
        /// For APP Only, My course 2,3 tab
        /// </summary>
        /// <param name="id">MemberId</param>
        /// <param name="type">0: get learning order 1: get teaching order</param>
        /// <returns></returns>
        public OrderListModel Get(int id = 0, Byte type = 0)
        {
            if (id > 0)
            {
                OrderListModel model = new OrderListModel();
                List<OrderItem> orders;
                Byte badgeLoadType;
                var shouldCheckOrder = false;// CheckOrderHandlerDate("TOrderHandleDate", memberId);
                if (type.Equals(0))
                {
                    orders = _commonService.GetOrderListByStudent(id, shouldCheckOrder);
                    badgeLoadType = (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileLearn;
                }
                else
                {
                    orders = _commonService.GetOrderListByTeacher(id, shouldCheckOrder);
                    badgeLoadType = (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileTeach;
                }
                model.Orders = orders;

                var alertList = _commonService.GetPopNotification(id, badgeLoadType);
                model.Badge = APIHelper.GetNotificationNums(alertList, false);

                return model;
            }

            return null;
        }

    }
}

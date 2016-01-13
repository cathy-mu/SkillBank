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

namespace SkillBankWeb.API
{
    public class InteractionController : ApiController
    {
        public readonly ICommonService _commonService;

        public class InteractionItem
        {
            public string Title { get; set; }
            public string Content { get; set; }
            public int RelatedMemberId { get; set; }
            public int ClassOrderId { get; set; }
            public string Avatar { get; set; }
            public DateTime LastUpdateDate { get; set; }
            public Boolean IsNew { get; set; }
            public byte TypeId { get; set; }
        }

        public class InteractionModel
        {
            public List<InteractionItem> Interaction;
            public Dictionary<String, int> Badge;
        }

        public InteractionController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InteractionModel Get(int id = 0)
        {
            InteractionModel model = new InteractionModel();
            //Step 1 load page content
            var notifications = _commonService.GetNotification(id, (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileInteration);
            var interactions = notifications.Select(item => new InteractionItem()
            {
                Avatar = item.Avatar,
                ClassOrderId = item.ClassOrderId,
                LastUpdateDate = item.LastUpdateDate,
                Title = item.Name,
                RelatedMemberId = item.RelatedMemberId,
                TypeId = item.TypeId,
                IsNew = item.TypeRank < 2,
                Content = GetInteractionText(item.TypeId,item.Title)
            }).ToList();
            model.Interaction = interactions;

            //Step 2 load menu badge, change data status
            var alertList = _commonService.GetPopNotification(id, (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileInteration);
            model.Badge = APIHelper.GetNotificationNums(alertList, false);

            return model;
        }

        private String GetInteractionText(Byte typeId, String text)
        {
            if (ConfigConstants.NotificationTextSetting.InteractionFormat.ContainsKey(typeId))
            {
                return String.Format(ConfigConstants.NotificationTextSetting.InteractionFormat[typeId],text);
            }
            return "";

        }

    }
}

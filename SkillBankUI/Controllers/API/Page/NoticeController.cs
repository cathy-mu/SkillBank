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
    public class NoticeController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public class NoticeItem
        {
            public string Title { get; set; }
            public string Image { get; set; }
            public string Content { get; set; }
            public DateTime LastUpdateDate { get; set; }
            public Boolean IsNew { get; set; }
        }

        public class NoticeModel
        {
            public List<NoticeItem> Notice;
            public Dictionary<String, int> Badge;
        }

        public NoticeController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NoticeModel Get(int id = 0)
        {
            NoticeModel model = new NoticeModel();
            //Step 1 load page content
            var notifications = _commonService.GetNotification(id, (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileSystem);
            if (notifications != null)
            {
                var notice = notifications.Select(item => new NoticeItem()
                {
                    Image = "",
                    LastUpdateDate = item.LastUpdateDate,
                    Title = GetNotificationText(item.TypeId),
                    IsNew = item.TypeRank < 2,
                    Content = GetNotificationText(item.TypeId, false)
                }).ToList();
                model.Notice = notice;
            }

            //Step 2 load menu badge, change data status
            var alertList = _commonService.GetPopNotification(id, (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileSystem);
            model.Badge = APIHelper.GetNotificationNums(alertList, true);

            return model;
        }

        private String GetNotificationText(Byte typeId, Boolean isTitle = true)
        {
            var notificationDic = _contentService.GetSystemNotificationLkp();
            if (notificationDic != null)
            {
                if (isTitle && notificationDic[typeId].TitleBlurbId.HasValue)
                {
                    return _contentService.GetTranslation(notificationDic[typeId].TitleBlurbId.Value);
                }
                else if (!isTitle && notificationDic[typeId].ContentBlurbId.HasValue)
                {
                    return _contentService.GetTranslation(notificationDic[typeId].ContentBlurbId.Value);
                }
            }

            return "";
        }
        

    }
}

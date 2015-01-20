using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.API
{
    public class LikeClassController : ApiController
    {
        //public readonly ICommonService _commonService;
        public readonly ICommonService _commonService;
        public class LikeClassItem
        {
            public int MemberId { get; set; }
            public int ClassId { get; set; }
            public Boolean IsLike { get; set; }
        }
        //
        // GET: /Message/

        public LikeClassController(ICommonService commonService)
        {
            //_contentService = contentService;
            _commonService = commonService;
        }
        
        ///// <summary>
        ///// GET api/FavoriteClass/1
        ///// </summary>
        ///// <param name="id">memberId</param>
        ///// <returns>Member's Favorite Class List</returns>
        //public List<MessageListItem> FavoriteClass(int id)
        //{
        //    int memberid = id;
        //    if (memberid > 0)
        //    {
        //        //LoadNotificationAlert(memberId);
        //        var messages = _commonService.GetMessageList(memberid);
        //        var unReadMessageNum = _commonService.GetMessageUnReadNum(memberid);
        //        foreach (var item in messages)
        //        {
        //            var contactId = item.From_Id.Equals(memberid) ? item.To_Id : item.From_Id;
        //            item.UnReadNumber = (unReadMessageNum!=null&&unReadMessageNum.ContainsKey(contactId)) ? unReadMessageNum[contactId] : 0;
        //        }
        //        return messages;
        //    }
        //    return null;
        //}

        public Boolean UpdateLikeTag(LikeClassItem item)
        {
            _commonService.UpdateClassLikeTag(item.MemberId, item.ClassId, item.IsLike);
            return true;
        }


    }
}

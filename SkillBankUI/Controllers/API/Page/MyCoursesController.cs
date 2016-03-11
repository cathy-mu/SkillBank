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
    public class MyCoursesController : ApiController
    {
        public readonly ICommonService _commonService;

        public class ClassEditListItem
        {
            public int ClassId;
            public String Title;
            public String Cover;
            public Byte Status;
        }
        
        public class MyCoursesModel
        {
            public List<ClassEditListItem> ClassList;
            public MemberBasicInfo MemberInfo;
            public Dictionary<String, int> Badge;
        }

        public MyCoursesController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// Get class list for mycourse page(Private) (v1.0 15-08-19)
        /// </summary>
        /// <param name="id">MemberId</param>
        /// <returns></returns>
        public MyCoursesModel Get(int id = 0)
        {
            if (!id.Equals(0))
            {
                var memberInfo = _commonService.GetMemberInfo(id);
                if (memberInfo != null)
                {
                    var model = new MyCoursesModel();
                    var classList = _commonService.GetClassEditInfoByMemberId(id, (Byte)Enums.DBAccess.ClassLoadType.ByTeacherId);
                    if (classList != null)
                    {
                        List<ClassEditListItem> classEditlist = classList.Select(i =>
                            new ClassEditListItem
                            {
                                ClassId = i.ClassId,
                                Title = (String.IsNullOrEmpty(i.Title) ? "一堂未开完的课程" : i.Title),
                                Cover = i.Cover,
                                Status = i.IsProved.Equals(0) ? (Byte)(i.PublishStatus + 4) : i.IsProved
                            }
                            ).ToList();

                        model.ClassList = classEditlist;
                    }
                    else
                    {
                        model.ClassList = null;
                    }
                    model.MemberInfo = new MemberBasicInfo() { MemberId = id, Avatar = memberInfo.Avatar, Name = memberInfo.Name };
                    
                    var alertList = _commonService.GetPopNotification(id, (Byte)Enums.DBAccess.NotificationAlterLoadType.MobileMyCourse);
                    model.Badge = APIHelper.GetNotificationNums(alertList,false);

                    return model;
                }
            }
            return null;
        }

    }
}

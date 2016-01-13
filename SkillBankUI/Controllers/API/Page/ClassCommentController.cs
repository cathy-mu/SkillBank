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
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.API
{
    public class ClassCommentController : ApiController
    {
        public readonly ICommonService _commonService;

        public class ClassCommentModel
        {
            public List<MemberReviewItem> Comment;
            public Int16 Status;
        }

        public ClassCommentController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">1:Update Notification to altered(Use server code ,but not API for now) 2:Update messages to altered</param>
        /// <returns></returns>
        public ClassCommentModel Get(int id, int mid = 0)
        {
            ClassCommentModel model = new ClassCommentModel();
            var classComments = _commonService.GetClassReviews((Byte)Enums.DBAccess.ReviewLoadType.ByClassComment, 0, id, 0, 0);
            model.Comment = classComments;
            model.Status = 200;
            return model;
        }

    }
}

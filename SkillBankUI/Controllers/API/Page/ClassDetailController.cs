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
    public class ClassDetailController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public class OwnerInfo
        {
            public int MemberId;
            public String Avatar;
            public String Name;
            public String CityName;
            public Boolean Gender;
            public Boolean IsFollow;
        }

        public class ClassDetailModel
        {
            public ClassEditItem Class;
            public OwnerInfo Owner;//teacher
            public List<MemberReviewItem> Review;
            
            public int LikeNum;
            public int ClassNum;
            public int StudentNum;
            public int ReviewNum;
            public int CommentNum;            
            public int Rank;

            public Boolean IsOwner;
            //public Int16 Status;
        }

        public ClassDetailController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">1:Update Notification to altered(Use server code ,but not API for now) 2:Update messages to altered</param>
        /// <returns></returns>
        public ClassDetailModel Get(int id, int mid = 0)
        {
            int memberId = mid;
            var classInfo = _commonService.GetClassInfoItem((Byte)Enums.DBAccess.ClassLoadType.ByClassAndCurrMemberId, id, memberId);
            ClassDetailModel classDetailModel = new ClassDetailModel();

            if (classInfo != null)
            {
                var teacherId = classInfo.Member_Id;
                OwnerInfo ownerInfo = null;//class owner,teacher
                //Class owner
                if (teacherId.Equals(memberId))
                {
                    classDetailModel.IsOwner = true;
                    //classDetailModel.IsLogin = true;
                }
                else
                {
                    classDetailModel.IsOwner = false;
                    //var teacherInfo = teacherId > 0 ? _commonService.GetMemberInfo(teacherId) : null;
                    var teacherInfo = teacherId > 0 ? _commonService.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberIdAndRelatedMemberId, teacherId, mid) : null;
                    if (teacherInfo != null)
                    {
                        var cityDic = _contentService.GetClassCities("cn");
                        ownerInfo = new OwnerInfo() { MemberId = teacherInfo.MemberId, Avatar = teacherInfo.Avatar, Name = teacherInfo.Name, CityName = cityDic.ContainsKey(teacherInfo.CityId) ? cityDic[teacherInfo.CityId].CityName : "", Gender = teacherInfo.Gender, IsFollow = teacherInfo.IsLike};
                    }
                    //memberInfo = memberId > 0 ? _commonService.GetMemberInfo(memberId) : null;
                    //classDetailModel.IsLogin = !memberId.Equals(0);
                }

                int reviewNum = 0;
                var studentReview = _commonService.GetClassReviews((Byte)Enums.DBAccess.ReviewLoadType.ByClassReview, 0, id, 0, 0);
                if (studentReview != null && studentReview.Count > 0)
                {
                    classDetailModel.Review = studentReview;
                    reviewNum = studentReview.Count();
                }

                //Set model data
                var numDic = _commonService.GetNumsByMemberClass(0, id, (Byte)Enums.DBAccess.MemberNumsLoadType.ByClassSummary);
                classDetailModel.ClassNum = numDic[Enums.NumberDictionaryKey.Class];
                classDetailModel.StudentNum = numDic[Enums.NumberDictionaryKey.Student];
                classDetailModel.Rank = numDic[Enums.NumberDictionaryKey.Rank];
                classDetailModel.CommentNum = numDic[Enums.NumberDictionaryKey.Comment];
                classDetailModel.LikeNum = classInfo.LikeNum;
                classDetailModel.ReviewNum = reviewNum;
                classDetailModel.Class = classInfo;
                classDetailModel.Owner = ownerInfo;
                //classDetailModel.Status = 200;
                return classDetailModel;

            }
            else
            {
                //classDetailModel.Status = 404;
                return null;
            }

        }

    }
}

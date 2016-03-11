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
    //TO DO: inprogress
    public class PersonalController : ApiController
    {
        public readonly ICommonService _commonService;
        public readonly IContentService _contentService;
        
        public class PersonalModel
        {
            public MemberProfileInfo Member;
            public List<ClassCollectionItem> ClassList;
            public List<MemberReviewItem> StuentReview;
            public List<MemberReviewItem> TeacherReview;
            public Dictionary<Enum, int> NumDic;

            //public String ContactMobile;
            //public String MobileText;
            public Boolean IsOwner;
            public Boolean IsLike;
            public Boolean IsLogin;
        }

        public PersonalController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// Personal page (public) v1.0
        /// TO DO: Before confirm final version
        /// </summary>
        /// <param name="id">page owner id</param>
        /// <param name="viewerId">current viewer member id</param>
        /// <returns></returns>
        public PersonalModel Get(int id, int viewerId = 0)
        {
            MemberInfo memberInfo = id > 0 ? _commonService.GetMemberInfo((Byte)Enums.DBAccess.MemberLoadType.ByMemberIdAndRelatedMemberId, id, viewerId) : null;
            
            if (memberInfo != null)
            {
                Boolean isMobileVerified = (memberInfo.NotifyTag & 1).Equals(1);
                String mobile = isMobileVerified ? memberInfo.Phone : "";//for send SMS notify
                String mobileText = isMobileVerified ? System.Text.RegularExpressions.Regex.Replace(memberInfo.Phone, @"(?im)(\d{3})(\d{6})(\d{2})", "$1******$3") : "";
                String cityName = (memberInfo.CityId > 0) ? _contentService.GetCityNameById(Constants.BizConfig.SingleLocalCode, memberInfo.CityId) : "";

                MemberProfileInfo member = new MemberProfileInfo()
                {
                    MemberId = id,
                    Name = memberInfo.Name,
                    Avatar = memberInfo.Avatar,
                    CityId = memberInfo.CityId,
                    CityName = cityName,
                    SocialType = memberInfo.SocialType,
                    SocialAccount = memberInfo.SocialType.Equals(1) ? memberInfo.SocialAccount : "",
                    Mobile = mobileText,
                    MobileText = mobileText,
                    JoinDate = memberInfo.CreatedDate.Date,
                    PosX = memberInfo.PosX,
                    PosY = memberInfo.PosY,
                    Email = memberInfo.Email,
                    SelfIntro = memberInfo.SelfIntro,
                    Gender = memberInfo.Gender
                };

                PersonalModel model = new PersonalModel();
                var classes = _commonService.GetClassCollection((Byte)Enums.DBAccess.ClassCollectionLoadType.ByTeacherId, id, 0);
                model.ClassList = DataMapper.Map(classes, null, ConfigConstants.ThirdPartySetting.UpYun.ClassCoverSize["ms"][0]);

                int sReviewNum = 0, tReviewNum = 0;
                var reviews = _commonService.GetMemberReviews((Byte)Enums.DBAccess.ReviewLoadType.ByMemberAll, id, 0, 0);
                if (reviews != null && reviews.Count > 0)
                {
                    var stuentReview = reviews.Where(r => r.TabId == 0).ToList();
                    if (stuentReview != null && stuentReview.Count() != 0)
                    {
                        model.StuentReview = stuentReview;
                        sReviewNum = stuentReview.Count();
                    }

                    var teacherReview = reviews.Where(r => r.TabId == 1).ToList();
                    if (teacherReview != null && teacherReview.Count() != 0)
                    {
                        model.TeacherReview = teacherReview;
                        sReviewNum = teacherReview.Count();
                    }
                }
                //set number dictionary
                var numDic = _commonService.GetNumsByMember(id, (Byte)Enums.DBAccess.MemberNumsLoadType.ByMemberSummary);
                if (memberInfo.SocialType.Equals(1) || memberInfo.SocialType.Equals(4))
                {
                    numDic[Enums.NumberDictionaryKey.Certification] += 1;
                }
                numDic.Add(Enums.NumberDictionaryKey.StudentReview, sReviewNum);
                numDic.Add(Enums.NumberDictionaryKey.TeacherReview, tReviewNum);

                model.NumDic = numDic;
                model.Member = member;//page owner


                model.IsLogin = !viewerId.Equals(0);
                model.IsOwner = id.Equals(viewerId);
                model.IsLike = memberInfo.IsLike;

                return model;
            }
            //member not exists or disactive
            return null;
        }

    }
}

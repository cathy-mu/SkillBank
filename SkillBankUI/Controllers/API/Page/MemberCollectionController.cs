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

namespace SkillBankWeb.API
{
    public class MemberCollectionController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public class MemberCollectionItem
        {
            public String Avatar;
            public String Title;
            public String Name;
            public int MemberId;
        }

        public class MemberCollectionModel
        {
            public String Avatar;
            public List<MemberCollectionItem> MemberList;
        }

        public MemberCollectionController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// Get Learnt/Tought class list page(Public) (v1.0 15-08-21)
        /// </summary>
        /// <param name="id">MemberId</param>
        /// <param name="type">4:Tought </param>
        /// <returns></returns>
        public MemberCollectionModel Get(int id = 0, Byte type = 4)
        {
            if (!id.Equals(0))
            {
                var memberInfo = _commonService.GetMemberInfo(id);
                if (memberInfo != null)
                {
                    var cityDic = _contentService.GetCities("cn");
                    Byte loadBy = type.Equals(1) ? (Byte)Enums.DBAccess.ClassCollectionLoadType.ByMemberLearnt : (Byte)Enums.DBAccess.ClassCollectionLoadType.ByMemberTought;
                    var list = _commonService.GetClassCollection(type, id);
                    if (list != null && list.Count > 0)
                    {
                        List<MemberCollectionItem> memberList= list.Select(i =>
                                new MemberCollectionItem
                                {
                                    MemberId = i.MemberId,
                                    Title = i.Title,
                                    Avatar = i.Cover,
                                    Name = i.Name
                                }
                                ).ToList();

                        MemberCollectionModel model = new MemberCollectionModel();
                        model.MemberList = memberList;
                        model.Avatar = memberInfo.Avatar;
                        return model;
                    }
                }
            }
            return null;
        }

    }
}

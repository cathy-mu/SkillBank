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
    public class CourseCollectionController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public CourseCollectionController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// Get Learnt/Tought class list page(Public) (v1.0 15-08-21)
        /// </summary>
        /// <param name="id">MemberId</param>
        /// <param name="type">1:Learnt   2:Tought </param>
        /// <returns></returns>
        public ClassCollectionModel Get(int id = 0, Byte type = 2)
        {
            if (!id.Equals(0))
            {
                var memberInfo = _commonService.GetMemberInfo(id);
                if (memberInfo != null)
                {
                    var cityDic = _contentService.GetClassCities("cn");
                    Byte loadBy = type.Equals(1) ? (Byte)Enums.DBAccess.ClassCollectionLoadType.ByMemberLearnt : (Byte)Enums.DBAccess.ClassCollectionLoadType.ByMemberTought;
                    var classes = _commonService.GetClassCollection(type, id);
                    if (classes != null && classes.Count > 0)
                    {
                        List<ClassCollectionItem> classList = DataMapper.Map(classes, cityDic, ConfigConstants.ThirdPartySetting.UpYun.ClassCoverSize["ms"][0]);
                         
                        ClassCollectionModel model = new ClassCollectionModel();
                        model.ClassList = classList;
                        model.Avatar = memberInfo.Avatar;
                        return model;
                    }
                }
            }
            return null;
        }

    }
}

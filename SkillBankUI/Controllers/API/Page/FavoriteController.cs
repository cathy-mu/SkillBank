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
    public class FavoriteController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public FavoriteController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// Get Favorite class list page(Public) (v1.0 15-08-20)
        /// </summary>
        /// <param name="id">MemberId</param>
        /// <param name="relatedId">ViewerId</param>
        /// <param name="type">1:Show Following by Member And ViewerId 2:Show Fans by Member And ViewerId</param>
        /// <returns></returns>
        public ClassCollectionModel Get(int id = 0)
        {
            var memberInfo = id > 0 ? _commonService.GetMemberInfo(id) : null;
            var classes = _commonService.GetClassCollection((Byte)Enums.DBAccess.ClassCollectionLoadType.ByMemberLiked, id);
            if (classes != null && classes.Count > 0)
            {
                var cityDic = _contentService.GetClassCities("cn");
                List<ClassCollectionItem> classList = DataMapper.Map(classes, cityDic, ConfigConstants.ThirdPartySetting.UpYun.ClassCoverSize["ms"][0]);

                ClassCollectionModel model = new ClassCollectionModel();
                model.ClassList = classList;
                return model;
            }
            return null;
        }

    }
}

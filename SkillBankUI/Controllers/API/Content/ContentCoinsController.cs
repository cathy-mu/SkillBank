using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;
using SkillBank.Site.Web.ViewModel;

namespace SkillBankWeb.API
{
    public class ContentCoinsController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        /// <summary>
        /// Return result
        /// </summary>
        /// 
        public class GetCoinsModel
        {
            public List<GetCoinsItem> Items;
            public String Version;
        }

        public class GetCoinsItem
        {
            public String Title;
            public String Desc;
            public String Coin;
        }

        public ContentCoinsController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public GetCoinsModel GetContentList(String version = "")
        {
            GetCoinsModel model = new GetCoinsModel();

            if (String.IsNullOrEmpty(version) || !version.Equals(Constants.ContentSiteVersion.HowToGetCoins))
            {
                List<GetCoinsItem> items = new List<GetCoinsItem>();
                GetCoinsItem item1 = new GetCoinsItem();
                item1.Title = _contentService.GetTranslation(668);
                item1.Desc = _contentService.GetTranslation(672);
                item1.Coin = "1";
                items.Add(item1);

                item1 = new GetCoinsItem();
                item1.Title = _contentService.GetTranslation(670);
                item1.Desc = "在\"个人\"页面分享技能银行到<br />\"朋友圈\"可获2枚课币";
                item1.Coin = "2";
                items.Add(item1);

                item1 = new GetCoinsItem();
                item1.Title = _contentService.GetTranslation(669);
                item1.Desc = _contentService.GetTranslation(673); 
                item1.Coin = "1";
                items.Add(item1);

                item1 = new GetCoinsItem();
                item1.Title = _contentService.GetTranslation(671);
                item1.Desc = _contentService.GetTranslation(675);
                item1.Coin = "N";
                items.Add(item1);

                model.Items = items;
            }
            else
            {
                model.Items = null;
            }
            model.Version = Constants.ContentSiteVersion.HowToGetCoins;

            return model;
        }

    }
}

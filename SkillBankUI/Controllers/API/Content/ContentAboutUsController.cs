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
    public class ContentAboutUsController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        /// <summary>
        /// Return result
        /// </summary>
        /// 
        public class AboutUsModel
        {
            public List<AboutUsItem> Items;
            public String Version;
        }

        public class AboutUsItem
        {
            public String Title;
            public List<String> Text;
        }

        public ContentAboutUsController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public AboutUsModel GetContentList(String version = "")
        {
            AboutUsModel model = new AboutUsModel();

            if (String.IsNullOrEmpty(version) || !version.Equals(Constants.ContentSiteVersion.HowToGetCoins))
            {
                List<AboutUsItem> items = new List<AboutUsItem>();
                AboutUsItem item1 = new AboutUsItem();
                item1.Title = "标题1";
                item1.Text = new List<String>() { "1内容1", "1内容2", "1内容3", "1内容4" };
                items.Add(item1);
                
                item1 = new AboutUsItem();
                item1.Title = "标题2";
                item1.Text = new List<String>() { "2内容1", "2内容2", "2内容3", "2内容4" };
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

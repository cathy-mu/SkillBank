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
    public class ContentFlowController : ApiController
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        /// <summary>
        /// Return result
        /// </summary>
        /// 
        public class FlowModel
        {
            public List<String> Summary;
            public List<FlowItem> Items;
            public String Version;
        }

        public class FlowItem
        {
            public String Title;
            public List<FlowSubItem> SubItems;
        }

        public class FlowSubItem
        {
            public String SubTitle;
            public List<String> Text;
        }

        public ContentFlowController(ICommonService commonService, IContentService contentService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public FlowModel GetContentList(String version = "")
        {
            FlowModel model = new FlowModel();

            if (String.IsNullOrEmpty(version) || !version.Equals(Constants.ContentSiteVersion.HowToGetCoins))
            {
                model.Summary = new List<String>() { "简介1", "简介2", "简介3", "简介4" };

                List<FlowItem> items = new List<FlowItem>();
                FlowItem item1 = new FlowItem();
                item1.Title = "标题1";

                List<FlowSubItem> subItems = new List<FlowSubItem>();

                FlowSubItem subItem1 = new FlowSubItem();
                subItem1.SubTitle = "1副标题 1";
                subItem1.Text= new List<String>() { "1 1 内容1", "1 1 内容2", "1 1 内容3", "1 1 内容4" };
                subItems.Add(subItem1);

                subItem1 = new FlowSubItem();
                subItem1.SubTitle = "1副标题 2";
                subItem1.Text= new List<String>() { "1 2 内容1", "1 2 内容2", "1 2 内容3" };
                subItems.Add(subItem1);

                item1.SubItems = subItems;
                items.Add(item1);




                item1 = new FlowItem();
                item1.Title = "标题2";

                subItems = new List<FlowSubItem>();

                subItem1.SubTitle = "2副标题 1";
                subItem1.Text = new List<String>() { "2 1 内容1", "2 1 内容2", "2 1 内容3"};
                subItems.Add(subItem1);

                subItem1 = new FlowSubItem();
                subItem1.SubTitle = "2副标题 2";
                subItem1.Text = new List<String>() { "2 2 内容1", "2 2 内容2", "2 2 内容3" };
                subItems.Add(subItem1);

                item1.SubItems = subItems;
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

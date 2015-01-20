using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using SkillBank.Site.DataSource;
using SkillBank.Site.Services;
using SkillBank.Site.Services.Models;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Services.CacheProviders;
using SkillBank.Site.Services.Managers;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.Web
{
    public static class MetaTagHelper
    { 
        public class PageMetaTagBlurb 
        {
        public int TitleBlurb { get; set; }
        public int KeywordsBlurb { get; set; }
        public int DescriptionBlurb { get; set; }
        }

        public static Dictionary<String,PageMetaTagBlurb> PageMetaTagDic;
        public static Dictionary<String, int> PageMetaTitleDic;
        static MetaTagHelper()
        {
            //TO DO Switch Description Blurb 
            PageMetaTagDic = new Dictionary<String, PageMetaTagBlurb>();
            PageMetaTagDic.Add("home", new PageMetaTagBlurb() { TitleBlurb = 527, KeywordsBlurb = 528, DescriptionBlurb = 529 });
            PageMetaTagDic.Add("classsearch", new PageMetaTagBlurb() { TitleBlurb = 530, KeywordsBlurb = 531, DescriptionBlurb = 532 });
            PageMetaTagDic.Add("beteacher", new PageMetaTagBlurb() { TitleBlurb = 533, KeywordsBlurb = 534, DescriptionBlurb = 535 });
            PageMetaTagDic.Add("classdetail", new PageMetaTagBlurb() { TitleBlurb = 536, KeywordsBlurb = 537, DescriptionBlurb = 538 });//536 class name
            PageMetaTagDic.Add("profile", new PageMetaTagBlurb() { TitleBlurb = 539, KeywordsBlurb = 540, DescriptionBlurb = 541 });//539 user id
            PageMetaTagDic.Add("aboutus", new PageMetaTagBlurb() { TitleBlurb = 542, KeywordsBlurb = 543, DescriptionBlurb = 544 });
            PageMetaTagDic.Add("qanda", new PageMetaTagBlurb() { TitleBlurb = 545, KeywordsBlurb = 546, DescriptionBlurb = 547 });


            PageMetaTitleDic = new Dictionary<string, int>();
            PageMetaTitleDic.Add("classadd", 564);
            PageMetaTitleDic.Add("terms", 565);
            PageMetaTitleDic.Add("dashboard", 563);
            PageMetaTitleDic.Add("classpublish", 548);

            PageMetaTitleDic.Add("classskill", 551);
            PageMetaTitleDic.Add("classpreview", 536);
            PageMetaTitleDic.Add("chat", 549);
            PageMetaTitleDic.Add("message", 550);
            PageMetaTitleDic.Add("teach", 552);
            PageMetaTitleDic.Add("learn", 553);
            PageMetaTitleDic.Add("memberinfo", 554);
            PageMetaTitleDic.Add("memberlocation", 555);
            PageMetaTitleDic.Add("memberphoto", 556);
            PageMetaTitleDic.Add("memberverification", 557);
            PageMetaTitleDic.Add("signup", 558);
        }

        public static String[] GetMetaTags(String pageKey)
        {
            String[] metaTags = new String[3];
            if (PageMetaTagDic.ContainsKey(pageKey))
            {
                var metaTagBlurbs = PageMetaTagDic[pageKey];
                metaTags[0] = ResourceHelper.GetTransText(metaTagBlurbs.TitleBlurb);
                metaTags[1] = ResourceHelper.GetTransText(metaTagBlurbs.KeywordsBlurb);
                metaTags[2] = ResourceHelper.GetTransText(metaTagBlurbs.DescriptionBlurb);
            }
            else
            {
                var metaTagBlurbs = PageMetaTagDic[pageKey];
                metaTags[0] = ResourceHelper.GetTransText(1);
                metaTags[1] = "";
                metaTags[2] = "";
            }
            return metaTags;
        }

        public static String GetMetaTitle(String pageKey)
        {
            if (PageMetaTitleDic.ContainsKey(pageKey))
            {
                return ResourceHelper.GetTransText(PageMetaTitleDic[pageKey]);
            }
            else
            {
                return  ResourceHelper.GetTransText(1);
            }
        }

        
    }
}

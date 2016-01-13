using System.Text;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using SkillBank.Site.DataSource.Data;


namespace SkillBank.Site.DataSource.Mapper
{
    public class LookupsMapper
    {

        public static List<SystemNotification> Map(ObjectResult<SystemNotification_LoadAll_p_Result> objectItems)
        {
            if (objectItems != null)
            {
                var items = objectItems.Select(item => new SystemNotification() { NotificationId = item.NotificationId, TitleBlurbId = item.TitleBlurbId, ContentBlurbId = item.ContentBlurbId, Link = item.Link, Banner = item.Banner, AddType = item.AddType, ShowType = item.ShowType, CreatedDate = item.CreatedDate }).ToList<SystemNotification>();
                return (items.Count > 0) ? items : null;
            }
            return null;
        }



        public static List<ClassCategory> Map(ObjectResult<ClassCategory_Load_p_Result> objectCategories)
        {
            if (objectCategories != null)
            {
                var categories = objectCategories.Select(item => new ClassCategory() { CategoryId = item.CategoryId, CategoryName = item.CategoryName, Blurb_Id = item.Blurb_Id, CategoryKey = item.CategoryKey }).ToList<ClassCategory>();
                return (categories.Count > 0) ? categories : null;
            }
            return null;
        }

        public static List<SkillCategory> Map(ObjectResult<SkillCategory_LoadAll_p_Result> objectCategories)
        {
            if (objectCategories != null)
            {
                var categories = objectCategories.Select(item => new SkillCategory() { CategoryId = item.CategoryId, CategoryName = item.CategoryName, Blurb_Id = item.Blurb_Id, Parent_CategoryId = item.Parent_CategoryId }).ToList<SkillCategory>();
                return (categories.Count > 0) ? categories : null;
            }
            return null;
        }

        public static List<CategoryTag> Map(ObjectResult<CategoryTag> objectTags)
        {
            if (objectTags != null)
            {
                var tags = objectTags.ToList<CategoryTag>();
                return (tags.Count > 0) ? tags : null;
            }
            return null;
        }

        public static List<EmailTemplate> Map(ObjectResult<EmailTemplate> objectTemplates)
        {
            if (objectTemplates != null)
            {
                var templates = objectTemplates.ToList<EmailTemplate>();
                return (templates.Count > 0) ? templates : null;
            }
            return null;
        }

        //public static Dictionary<string, MetaTag> Map(List<MetaTag> objectMetaTags)
        //{
        //    if (objectMetaTags != null)
        //    {
        //        var tags = objectMetaTags.ToDictionary(m => m.MetaKey, m => m);
        //        return (tags != null && tags.Count > 0) ? tags : null;
        //    }
        //    return null;
        //}

        public static Dictionary<string, MetaTag> Map(ObjectResult<MetaTag> objectMetaTags)
        {
            if (objectMetaTags != null)
            {
                var tags = objectMetaTags.ToDictionary(m => m.MetaKey, m => m);
                return (tags.Count > 0) ? tags : null;
            }
            return null;
        }

        public static List<CityInfo> Map(ObjectResult<CityInfo_LoadAll_p_Result> objectCities)
        {
            if (objectCities != null)
            {
                var cities = objectCities.Select(item => new CityInfo() { CityId = item.CityId, CityKey = item.CityKey, CityName = item.CityName, LocaleCode = item.LocaleCode, OrderRank = item.OrderRank }).ToList<CityInfo>();
                return (cities.Count > 0) ? cities : null;
            }
            return null;
        }

        #region Banner LKPS
        public static List<TopBanner> Map(ObjectResult<TopBanner_LoadAll_p_Result> objectItems)
        {
            if (objectItems != null)
            {
                var items = objectItems.Select(item => new TopBanner() { BannerId = item.BannerId, BannerImg = item.BannerImg, BannerKey = item.BannerKey, Link = item.Link, Rank = item.Rank, LinkType = item.LinkType }).ToList<TopBanner>();
                return (items.Count > 0) ? items : null;
            }
            return null;
        }

        public static List<PortalBanner> Map(ObjectResult<PortalBanner_LoadAll_p_Result> objectItems)
        {
            if (objectItems != null)
            {
                var items = objectItems.Select(item => new PortalBanner() { PortalBannerId = item.PortalBannerId, PortalImage = item.PortalImage, PortalKey = item.PortalKey, Link = item.Link, Rank = item.Rank, LinkType = item.LinkType }).ToList<PortalBanner>();
                return (items.Count > 0) ? items : null;
            }
            return null;
        }

        public static List<LinkMap> Map(ObjectResult<LinkMap_LoadAll_p_Result> objectItems)
        {
            if (objectItems != null)
            {
                var items = objectItems.Select(item => new LinkMap() { LinkMapId = item.LinkMapId, SourceLink = item.SourceLink, SourceId = item.SourceId, Link = item.Link, LinkType = item.LinkType }).ToList<LinkMap>();
                return (items.Count > 0) ? items : null;
            }
            return null;
        }

        #endregion
    }
}

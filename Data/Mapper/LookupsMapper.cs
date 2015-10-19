using System.Text;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using SkillBank.Site.DataSource.Data;


namespace SkillBank.Site.DataSource.Mapper
{
    public class LookupsMapper
    {
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

        //public static List<CityInfo> Map(ObjectResult<CityInfo> objectCities)
        //{
        //    if (objectCities != null)
        //    {
        //        var cities = objectCities.ToList<CityInfo>();
        //        return (cities.Count > 0) ? cities : null;
        //    }
        //    return null;
        //}

        public static List<CityInfo> Map(ObjectResult<CityInfo_LoadAll_p_Result> objectCities)
        {
            if (objectCities != null)
            {
                var cities = objectCities.Select(item => new CityInfo() { CityId = item.CityId, CityKey = item.CityKey, CityName = item.CityName, LocaleCode = item.LocaleCode, OrderRank = item.OrderRank }).ToList<CityInfo>();
                return (cities.Count > 0) ? cities : null;
            }
            return null;
        }

    }
}

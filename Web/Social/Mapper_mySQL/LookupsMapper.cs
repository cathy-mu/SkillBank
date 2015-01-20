using System.Text;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using SkillBank.Site.DataSource.Data;


namespace SkillBank.Site.DataSource.Mapper
{
    public class LookupsMapper
    {
        public static List<SkillCategory> Map(ObjectResult<SkillCategory> objectCategories)
        {
            if (objectCategories != null)
            {
                var categories = objectCategories.ToList<SkillCategory>();
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

        public static List<CityInfo> Map(ObjectResult<CityInfo> objectCities)
        {
            if (objectCities != null)
            {
                var cities = objectCities.ToList<CityInfo>();
                return (cities.Count > 0) ? cities : null;
            }
            return null;
        }

    }
}

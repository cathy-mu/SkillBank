using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Data.Entity;

using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using SkillBank.Site.DataSource.Mapper;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.DataSource.Data
{
    public interface ILookupsRepository
    {
        List<SkillCategory> GetSkillCategories();
        Dictionary<String, MetaTag> GetMetaTagLkp();
        List<CityInfo> GetCityLkp();
    }

    public class LookupsRepository : DbContext, ILookupsRepository
    {
        public LookupsRepository()
            : base("name=Entities")
        {
        }

        /// <summary>
        /// Get skill categories
        /// </summary>
        /// <returns></returns>
        public List<SkillCategory> GetSkillCategories()
        {
            return SkillCategory_LoadAll_p();
        }

        /// <summary>
        /// Load all skill categories
        /// </summary>
        /// <returns></returns>
        private List<SkillCategory> SkillCategory_LoadAll_p()
        {
            ObjectResult<SkillCategory> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SkillCategory>("SkillCategory_LoadAll_p");
            return LookupsMapper.Map(result);
        }

        /// <summary>
        /// Load Meta tags 
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, MetaTag> GetMetaTagLkp()
        {
            var result = MetaTag_LoadAll_p();
            return LookupsMapper.Map(result);
        }

        /// <summary>
        /// Load Meta tags 
        /// </summary>
        /// <returns></returns>
        private ObjectResult<MetaTag> MetaTag_LoadAll_p()
        {
            ObjectResult<MetaTag> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MetaTag>("MetaTag_LoadAll_p");
            return result;
        }

        /// <summary>
        /// Load Meta tags 
        /// </summary>
        /// <returns></returns>
        public List<CityInfo> GetCityLkp()
        {
            var result = CityInfo_LoadAll_p();
            return LookupsMapper.Map(result);;
        }

        /// <summary>
        /// Load City Info
        /// </summary>
        /// <returns></returns>
        private ObjectResult<CityInfo> CityInfo_LoadAll_p()
        {
            ObjectResult<CityInfo> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CityInfo>("CityInfo_LoadAll_p");
            return result;
        }

    }
}




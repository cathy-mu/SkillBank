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
        List<ClassCategory> GetClassCategories(int cityId);
        List<TopBanner> GetTopBanners();
        List<PortalBanner> GetPortalBanners();
        List<SystemNotification> GetSystemNotifications();
        List<LinkMap> GetLinkMap();
    }

    public class LookupsRepository : Entities, ILookupsRepository
    {
        public LookupsRepository()
           // : base("name=Entities")
        {
        }

        /// <summary>
        /// Get new categories by cityid
        /// </summary>
        /// <returns></returns>
        public List<ClassCategory> GetClassCategories(int cityId)
        {
            return ClassCategory_Load_p(cityId);
        }

        /// <summary>
        /// Load all skill categories
        /// </summary>
        /// <returns></returns>
        private List<ClassCategory> ClassCategory_Load_p(int cityId)
        {
            var cityIdParameter = new ObjectParameter("CityId", cityId);
            ObjectResult<ClassCategory_Load_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ClassCategory_Load_p_Result>("ClassCategory_Load_p", cityIdParameter);
            return LookupsMapper.Map(result);
        }

        /// <summary>
        /// Get skill categories
        /// </summary>
        /// <returns></returns>
        public List<SkillCategory> GetSkillCategories()
        {
            ObjectResult<SkillCategory_LoadAll_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SkillCategory_LoadAll_p_Result>("SkillCategory_LoadAll_p");
            return LookupsMapper.Map(result);
        }
        
        public List<SystemNotification> GetSystemNotifications()
        {
            ObjectResult<SystemNotification_LoadAll_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SystemNotification_LoadAll_p_Result>("SystemNotification_LoadAll_p");
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
            ObjectResult<CityInfo_LoadAll_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CityInfo_LoadAll_p_Result>("CityInfo_LoadAll_p");
            return LookupsMapper.Map(result); 
        }

        #region Banner LKPS

        /// <summary>
        /// Get skill categories
        /// </summary>
        /// <returns></returns>
        public List<TopBanner> GetTopBanners()
        {
            ObjectResult<TopBanner_LoadAll_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<TopBanner_LoadAll_p_Result>("TopBanner_LoadAll_p");
            return LookupsMapper.Map(result);
        }

        /// <summary>
        /// Get skill categories
        /// </summary>
        /// <returns></returns>
        public List<PortalBanner> GetPortalBanners()
        {
            ObjectResult<PortalBanner_LoadAll_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PortalBanner_LoadAll_p_Result>("PortalBanner_LoadAll_p");
            return LookupsMapper.Map(result);
        }

        public List<LinkMap> GetLinkMap()
        {
            ObjectResult<LinkMap_LoadAll_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LinkMap_LoadAll_p_Result>("LinkMap_LoadAll_p");
            return LookupsMapper.Map(result);
        }

        #endregion
    }
}




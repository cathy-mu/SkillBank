//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Data.Linq;
//using System.Data.Linq.Mapping;
//using System.Reflection;

//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Data.Objects;
//using System.Data.Objects.DataClasses;
//using SkillBank.Site.DataSource.Mapper;
//using SkillBank.Site.DataSource.Data;

//namespace SkillBank.Site.DataSource.Data
//{

//    public interface IMetaTagRepository
//    {
//        Dictionary<String, MetaTag> GetMetaTagLkp();
//    }

//    public class MetaTagRepository : DbContext, IMetaTagRepository
//    {
//        public MetaTagRepository()
//            : base("name=SkillBankConnection")
//        {
//        }

//        /// <summary>
//        /// Load Blurb text and id 
//        /// </summary>
//        /// <param name="language">LanguageCode</param>
//        /// <param name="version">SiteVertion</param>
//        /// <returns></returns>
//        public Dictionary<String, MetaTag> GetMetaTagLkp()
//        {
//            var result = MetaTag_LoadAll_p();
//            return LookupsMapper.Map(result);
//        }

//        /// <summary>
//        /// Load Blurb text and id 
//        /// </summary>
//        /// <param name="language">LanguageCode</param>
//        /// <param name="version">SiteVertion</param>
//        /// <returns></returns>
//        private ObjectResult<MetaTag> MetaTag_LoadAll_p()
//        {
//            ObjectResult<MetaTag> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<MetaTag>("MetaTag_LoadAll_p");
//            return result;
//        }

//    }
//}




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF.Frameworks.Common.ThreadingEF;
using EF.Frameworks.Common.CollectionsEF;
using EF.Frameworks.Common.ConfigurationEF;

//using EF.Frameworks.Common.CachingEF;
using SkillBank.Site.Common.Caching;
using EF.Frameworks.Common.FactoryEF;
using EF.Frameworks.Orpheus.ContentManagementEF;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.DataSource.Mapper;
using SkillBank.Site.Common;
using SkillBank.Site.Services.Models;

namespace SkillBank.Site.Services.CacheProviders
{

    public interface IMetaTagProvider
    {
        MetaTag GetMetaTag(String metaKey);
    }

    public class MetaTagProvider : CacheBase<object, Dictionary<String, MetaTag>>, ISingleton, IMetaTagProvider
    {
        private readonly ILookupsRepository _repository;
        private String keyPrefix = "MetaTag";

        #region Constructors

        public MetaTagProvider(ILookupsRepository repository)
            : base(RefreshOptions.Custom)
        {
            this._repository = repository;
        }

        #endregion

        public new void RefreshAll()
        {
            base.RefreshAll();
        }

        public String GetKey()
        {
            return keyPrefix;
        }

        public void RefreshItem()
        {
            base.RefreshItem(GetKey());
        }

        protected override ICacheManager<object, Dictionary<String,MetaTag>> GetCustomCacheManager(
            SynchronizedDictionary<object, CachedItemInfo<object, Dictionary<String, MetaTag>>> elementCache,
             KeyedReaderWriterLockSlim<object> contentLoadLock)
        {
            int timeOutMins = Constants.Setting.CacheTimeOut_MetaTagMins;
            var cacheMgr = new TimestampCacheManagerAsync<object, Dictionary<String, MetaTag>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override Dictionary<String, MetaTag> LoadItem(object key)
        {
            var metaTags = this._repository.GetMetaTagLkp();
            if (metaTags != null && metaTags.Count > 0)
            {
                return metaTags;
            }

            return null;
        }
                
        public MetaTag GetMetaTag(String metaTagKey)
        {
            var metaTagLkp = this.GetItem(GetKey());
            if (metaTagLkp != null && metaTagLkp.Count() > 0 && metaTagLkp.ContainsKey(metaTagKey))
            {
                return metaTagLkp[metaTagKey];
            }

            return null;
        }

    }
}
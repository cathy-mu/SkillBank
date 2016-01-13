using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF.Frameworks.Common.ThreadingEF;
using EF.Frameworks.Common.CollectionsEF;
using EF.Frameworks.Common.ConfigurationEF;

using SkillBank.Site.Common.Caching;
using EF.Frameworks.Common.FactoryEF;
using EF.Frameworks.Orpheus.ContentManagementEF;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services.Models;

namespace SkillBank.Site.Services.CacheProviders
{

    public interface IPortalBannerProvider
    {
        List<PortalBanner> GetPortalBannerLkp();
    }

    public class PortalBannerProvider : CacheBase<String, List<PortalBanner>>, ISingleton, IPortalBannerProvider
    {
        private readonly ILookupsRepository _repository;
        private String keyPrefix = "PortalBanner";

        //private static Dictionary<String, List<PortalBanner>> _categoryDic;
        //private static readonly object _locker = new object();

        #region Constructors

        public PortalBannerProvider(ILookupsRepository repository)
            : base(RefreshOptions.Custom)
        {
            this._repository = repository;
        }

        #endregion

        public new void RefreshAll()
        {
            base.RefreshAll();
        }

        public new void RefreshItem(String key)
        {
            base.RefreshItem(key);
        }

        public String GetKey()
        {
            return String.Format("{0}{1}{2}", keyPrefix, Constants.Setting.CacheKeySpliter, Constants.BizConfig.SingleLocalCode.ToLower());
        }

        protected override ICacheManager<String, List<PortalBanner>> GetCustomCacheManager(
            SynchronizedDictionary<String, CachedItemInfo<String, List<PortalBanner>>> elementCache,
             KeyedReaderWriterLockSlim<String> contentLoadLock)
        {
            int timeOutMins = Constants.Setting.CacheTimeOut_PortalBannerMins;
            var cacheMgr = new TimestampCacheManagerAsync<String, List<PortalBanner>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override List<PortalBanner> LoadItem(String key)
        {
            var categories = this._repository.GetPortalBanners();
            if (categories != null && categories.Count > 0)
            {
                return categories;
            }

            return null;
        }

        public List<PortalBanner> GetPortalBannerLkp()
        {
            String key = this.GetKey();
            var categoryLkp = this.GetItem(key);
            if (categoryLkp != null && categoryLkp.Count() > 0)
            {
                return categoryLkp;
            }

            return null;
        }

        
    }
}
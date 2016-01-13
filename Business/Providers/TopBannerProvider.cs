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

    public interface ITopBannerProvider
    {
        List<TopBanner> GetTopBannerLkp();
        //TopBanner GetTopBanner(String key);
    }

    public class TopBannerProvider : CacheBase<String, List<TopBanner>>, ISingleton, ITopBannerProvider
    {
        private readonly ILookupsRepository _repository;
        private String keyPrefix = "TopBanner";

        //private static Dictionary<String, List<TopBanner>> _categoryDic;
        //private static readonly object _locker = new object();

        #region Constructors

        public TopBannerProvider(ILookupsRepository repository)
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

        protected override ICacheManager<String, List<TopBanner>> GetCustomCacheManager(
            SynchronizedDictionary<String, CachedItemInfo<String, List<TopBanner>>> elementCache,
             KeyedReaderWriterLockSlim<String> contentLoadLock)
        {
            int timeOutMins = Constants.Setting.CacheTimeOut_TopBannerMins;
            var cacheMgr = new TimestampCacheManagerAsync<String, List<TopBanner>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override List<TopBanner> LoadItem(String key)
        {
            var categories = this._repository.GetTopBanners();
            if (categories != null && categories.Count > 0)
            {
                return categories;
            }

            return null;
        }

        public List<TopBanner> GetTopBannerLkp()
        {
            String key = this.GetKey();
            var bannerLkp = this.GetItem(key);
            if (bannerLkp != null && bannerLkp.Count() > 0)
            {
                return bannerLkp;
            }

            return null;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF.Frameworks.Common.ThreadingEF;
using EF.Frameworks.Common.CollectionsEF;
using EF.Frameworks.Common.ConfigurationEF;

using EF.Frameworks.Common.FactoryEF;
using EF.Frameworks.Orpheus.ContentManagementEF;
using SkillBank.Site.Common.Caching;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.Site.Services.CacheProviders
{

    public interface IBlurbsProvider
    {
        String GetBlurb(int blurbId, String languageCode, Byte siteVersion);
    }

    public class BlurbsProvider : CacheBase<object, IDictionary<int, String>>, ISingleton, IBlurbsProvider
    {
        private readonly IBlurbsRepository _repository;
        private String keyPrefix = "Blurbs";

        #region Constructors

        public BlurbsProvider(IBlurbsRepository repository)
            : base(RefreshOptions.Custom)
        {
            this._repository = repository;
        }

        #endregion

        public new void RefreshAll()
        {
            base.RefreshAll();
        }

        public String GetKey(String languageCode, Byte siteVersion)
        {
            return String.Format("{0}{1}{2}{3}", keyPrefix, languageCode.ToLower(), Constants.Setting.CacheKeySpliter, siteVersion);
        }
        
        public void RefreshItem(String languageCode, Byte siteVersion)
        {
           base.RefreshItem(GetKey(languageCode, siteVersion));
        }

        protected override ICacheManager<object, IDictionary<int, String>> GetCustomCacheManager(
            SynchronizedDictionary<object, CachedItemInfo<object, IDictionary<int, String>>> elementCache,
             KeyedReaderWriterLockSlim<object> contentLoadLock)
        {
            // context = new ETContextData { ServerName = Environment.MachineName, ContextDate = DateTime.Now };
            int timeOutMins = Constants.Setting.CacheTimeOut_LLMins;
            var cacheMgr = new TimestampCacheManagerAsync<object, IDictionary<int, String>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

       protected override IDictionary<int, String> LoadItem(object key)
        {
            String[] dicKeys = key.ToString().Split(Constants.Setting.CacheKeySpliter);
            if (dicKeys.Length == 2)
            {
                String languageCode = dicKeys[0];
                Byte siteVersion = Convert.ToByte(dicKeys[1]);
                var blurbsDic = this._repository.Blurbs_LoadByLanguageSiteVersion_p(languageCode, siteVersion);
                if (blurbsDic != null && blurbsDic.Count > 0)
                {
                    return blurbsDic;
                }
            }

            return null;
        }

       public String GetBlurb(int blurbId, String languageCode, Byte siteVersion)
       {
           var blurbs = this.GetItem(GetKey(languageCode, siteVersion));
           if (blurbs != null && blurbs.Count() > 0 && blurbs.ContainsKey(blurbId))
           {
               return blurbs[blurbId];
           }
           else
           {
               return String.Format("No Text [{0}]", blurbId);
           }
       }

    }
}

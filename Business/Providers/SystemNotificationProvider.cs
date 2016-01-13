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

    public interface ISystemNotificationProvider
    {
        Dictionary<int, SystemNotification> GetSystemNotificationLkp();//String Key
    }

    public class SystemNotificationProvider : CacheBase<String, List<SystemNotification>>, ISingleton, ISystemNotificationProvider
    {
        private readonly ILookupsRepository _repository;
        private String keyPrefix = "SystemNoti";

        #region Constructors

        public SystemNotificationProvider(ILookupsRepository repository)
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

        protected override ICacheManager<String, List<SystemNotification>> GetCustomCacheManager(
            SynchronizedDictionary<String, CachedItemInfo<String, List<SystemNotification>>> elementCache,
             KeyedReaderWriterLockSlim<String> contentLoadLock)
        {
            int timeOutMins = Constants.Setting.CacheTimeOut_TopBannerMins;
            var cacheMgr = new TimestampCacheManagerAsync<String, List<SystemNotification>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override List<SystemNotification> LoadItem(String key)
        {
            var result = this._repository.GetSystemNotifications();
            if (result != null && result.Count > 0)
            {
                return result;
            }

            return null;
        }

        public Dictionary<int, SystemNotification> GetSystemNotificationLkp()//String key
        {
            var result = this.GetItem(GetKey());
            if (result != null && result.Count() > 0)
            {
                Dictionary<int, SystemNotification> notiDic = new Dictionary<int, SystemNotification>();
                foreach(var item in result)
                {
                    notiDic.Add(item.NotificationId, item);
                }
                return notiDic;
            }

            return null;
        }


    }
}
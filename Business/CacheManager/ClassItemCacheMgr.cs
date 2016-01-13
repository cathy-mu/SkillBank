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

namespace SkillBank.Site.Services.Managers
{

    public interface IClassItemCacheMgr
    {
        ClassEditItem GetClassItem(int id);
        void RefreshItem(int key);
    }

    public class ClassItemCacheMgr : CacheBase<int, ClassEditItem>, ISingleton, IClassItemCacheMgr
    {
        private readonly IClassInfoRepository _repository;

        #region Constructors

        public ClassItemCacheMgr(IClassInfoRepository repository)
            : base(RefreshOptions.Custom)
        {
            this._repository = repository;
        }

        #endregion

        public new void RefreshAll()
        {
            base.RefreshAll();
        }

        public new void RefreshItem(int key)
        {
            base.RefreshItem(key);
        }

        protected override ICacheManager<int, ClassEditItem> GetCustomCacheManager(
            SynchronizedDictionary<int, CachedItemInfo<int, ClassEditItem>> elementCache,
             KeyedReaderWriterLockSlim<int> contentLoadLock)
        {
            int timeOutMins = Constants.CacheTimeOut.ClassListWebMins;
            var cacheMgr = new TimestampCacheManagerAsync<int, ClassEditItem>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override ClassEditItem LoadItem(int key)
        {
            var classInfo = _repository.GetClassEditInfo((Byte)Enums.DBAccess.ClassLoadType.ByClassAndCurrMemberId, key, 0);

            if (classInfo != null && classInfo.Count > 0)
            {
                return classInfo[0];
            }

            return null;
        }


        public ClassEditItem GetClassItem(int id)
        {
            return base.GetItem(id);
        }
    }
}

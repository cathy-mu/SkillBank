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

    public interface IClassListCacheMgr
    {
        List<ClassListItem> GetClassList(Byte loadType, int pageId, int pageSize, out int classNum);
    }

    public class ClassListCacheMgr : CacheBase<string, List<ClassListItem>>, ISingleton, IClassListCacheMgr
    {
        //public static readonly WidePromotionCacheMgr Instance = Factory<WidePromotionCacheMgr>.Create();
        //private static Dictionary<string, List<ClassListItem>> _classLists;
        //private static readonly object _locker = new object();

        private readonly IClassInfoRepository _repository;
        //private String keyPrefix = "ClassList";

        #region Constructors

        public ClassListCacheMgr(IClassInfoRepository repository)
            : base(RefreshOptions.Custom)
        {
            this._repository = repository;
        }

        #endregion

        public new void RefreshAll()
        {
            //_classLists = null;
            base.RefreshAll();
        }

        public String GetKey(Byte loadType)
        {
            return String.Format("{0}", loadType);
        }

        public void RefreshItem(Byte loadType)
        {
            base.RefreshItem(GetKey(loadType));
        }

        protected override ICacheManager<string, List<ClassListItem>> GetCustomCacheManager(
            SynchronizedDictionary<string, CachedItemInfo<string, List<ClassListItem>>> elementCache,
             KeyedReaderWriterLockSlim<string> contentLoadLock)
        {
            int timeOutMins = Constants.CacheTimeOut.ClassListWebMins;
            var cacheMgr = new TimestampCacheManagerAsync<string, List<ClassListItem>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override List<ClassListItem> LoadItem(String key)
        {
            int totalNum = 0;
            Byte loadType = (Byte)Enums.DBAccess.ClassListLoadType.WebAllClassCached;
            var classList = this._repository.GetClassList(loadType, 0, 0/*cityId*/, 0/*categoryId*/, false, 0, 0, 0, out totalNum, "", 0, 0);
            if (classList != null && classList.Count > 0)
            {
                return classList;
            }

            return null;
        }

        //private static Dictionary<string, List<ClassListItem>> GetClassListCache()
        //{
        //    var result = _classLists;
        //    // Double-checked lock
        //    if (result == null)
        //    {
        //        lock (_locker)
        //        {
        //            result = _classLists;
        //            if (result == null)
        //            {
        //                result = LoadGetClassListCache();
        //                _classLists = result;
        //            }
        //        }
        //    }

        //    return result;
        //}

        //private static Dictionary<string, List<ClassListItem>> LoadGetClassListCache()
        //{
        //    return null;
        //}

        //public List<ClassListItem> GetClassList(Byte loadType)
        //{
        //    var classLists = this.GetItem(GetKey());
        //    String dicKey = GetDicKey(loadType);
        //    if()
        //    {
        //    }
        //    return classLists;
        //}

        public List<ClassListItem> GetClassList(Byte loadType, int pageId, int pageSize, out int classNum)
        {

            String key = GetKey(loadType);
            var classList = base.GetItem(key);
            if (classList != null)
            {
                classNum = classList.Count();
                int minId = pageSize * (pageId - 1);
                if (classNum > minId)
                {
                    return classList.Skip(minId).Take(pageSize).ToList();
                }
            }
            classNum = 0;
            return null;
        }

    }
}

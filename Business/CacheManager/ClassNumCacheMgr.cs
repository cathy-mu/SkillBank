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

    public interface IClassNumCacheMgr
    {
        Dictionary<Byte,int> GetClassNums(int classId);
    }

    public class ClassNumCacheMgr : CacheBase<int, Dictionary<Byte, int>>, ISingleton, IClassNumCacheMgr
    {
        //public static readonly FavoriteLkpProvider Instance = Factory<FavoriteLkpProvider>.Create();
        private static Dictionary<int, Dictionary<Byte, int>> _numDic;

        private static readonly object _locker = new object();
        private readonly InteractiveRepository _repository;
        //private String keyPrefix = "Favorite";

        #region Constructors

        public ClassNumCacheMgr(InteractiveRepository repository)
            : base(RefreshOptions.Custom)
        {
            this._repository = repository;
        }

        #endregion

        public new void RefreshAll()
        {
            _numDic = null;
            base.RefreshAll();
        }

        public void RefreshItem(int classId, Byte type, Boolean isLike)
        {
            Dictionary<int, Dictionary<Byte, int>> numDic = GetClassNumCacheItem();
            
            // Double-checked lock
            if (isLike && numDic.ContainsKey(classId) && numDic[classId].ContainsKey(type))
            {
                lock (_locker)
                {
                    var result = _numDic;
                    if (result.ContainsKey(classId) && result[classId].ContainsKey(type))
                    {
                        int likeNum = result[classId][type] + 1;
                        _numDic[classId][type] = likeNum;
                    }
                }
            }
            
        }

        protected override ICacheManager<int, Dictionary<Byte, int>> GetCustomCacheManager(
            SynchronizedDictionary<int, CachedItemInfo<int, Dictionary<Byte, int>>> elementCache,
             KeyedReaderWriterLockSlim<int> contentLoadLock)
        {
            int timeOutMins = Constants.Setting.CacheTimeOut_LLMins;
            var cacheMgr = new TimestampCacheManagerAsync<int, Dictionary<Byte, int>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override Dictionary<Byte, int> LoadItem(int classId)
        {
            Dictionary<int, Dictionary<Byte, int>> numDic = GetClassNumCacheItem();

            //String[] dicKeys = key.ToString().Split(Constants.Setting.CacheKeySpliter);
            if (numDic != null && numDic.ContainsKey(classId))
            {
                return numDic[classId];
            }

            return null;
        }

        /// <summary>
        /// Get lists Cache
        /// </summary>
        private static Dictionary<int, Dictionary<Byte, int>> GetClassNumCacheItem()
        {
            var result = _numDic;
           
            // Double-checked lock
            if (result == null)
            {
                lock (_locker)
                {
                    result = _numDic;
                    if (result == null)
                    {
                        result = LoadClassNumCache();
                        _numDic = result;
                    }
                }
            }
            return result;
        }

        private static Dictionary<int, Dictionary<Byte, int>> LoadClassNumCache()
        {
            Dictionary<int, Dictionary<Byte, int>> result = new Dictionary<int, Dictionary<Byte, int>>();
            Dictionary<Byte, int> temp = new Dictionary<byte, int>();
            temp.Add(0, 10);
            temp.Add(1, 5);
            result.Add(1, temp);
            return result;
        }

        public Dictionary<Byte, int> GetClassNums(int classId)
        {
            var numDic = this.GetItem(classId);
            return numDic;
        }

    }
}

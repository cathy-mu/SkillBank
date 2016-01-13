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

    public interface IRecommendClassCacheMgr
    {
        List<ClassListItem> GetClassList(int classType, int pageSize, int pageId);
    }

    public class RecommendClassCacheMgr : CacheBase<string, List<ClassListItem>>, ISingleton, IRecommendClassCacheMgr
    {
        //public static readonly RecommendClassCacheMgr Instance = Factory<RecommendClassCacheMgr>.Create();
        private static List<ClassListItem> _classLists;
        private static readonly object _locker = new object();
        
        private static readonly IClassInfoRepository _repository = new ClassInfoRepository();
        private String keyPrefix = "ClassRecommend";

        #region Constructors

        public RecommendClassCacheMgr()//IClassInfoRepository repository
            : base(RefreshOptions.Custom)
        {
        }

        #endregion

        public new void RefreshAll()
        {
            base.RefreshAll();
        }

        public String GetKey(int classType, int pageSize, int pageId)
        {
            return String.Format("{0}_{1}_{2}", classType, pageSize, pageId);
        }
        
        public void RefreshItem(int classType, int pageSize, int pageId)
        {
           base.RefreshItem(GetKey(classType, pageSize, pageId));
        }

        protected override ICacheManager<string, List<ClassListItem>> GetCustomCacheManager(
            SynchronizedDictionary<string, CachedItemInfo<string, List<ClassListItem>>> elementCache,
             KeyedReaderWriterLockSlim<string> contentLoadLock)
        {
            // context = new ETContextData { ServerName = Environment.MachineName, ContextDate = DateTime.Now };
            int timeOutMins = Constants.CacheTimeOut.RecommendClassMins;
            var cacheMgr = new TimestampCacheManagerAsync<string, List<ClassListItem>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

       protected override List<ClassListItem> LoadItem(string key)
        {
            String[] keys = key.Split('_');
            if (keys.Length == 3)
            {
                int classType = Convert.ToInt32(keys[0]);
                int minId = Convert.ToInt32(keys[1]);
                int maxId = Convert.ToInt32(keys[2]);
                
                var classList = GetClassListCache();
                if (classList != null && classList.Count > 0)
                {
                    if (minId.Equals(0) && maxId.Equals(0))
                    {
                        var list = classList.Where(i => i.ClassNum == classType).ToList();
                        return list;
                    }
                    else if(maxId>minId)
                    {
                        var list = classList.Where(i => i.ClassNum == classType).Skip(minId-1).Take(maxId-minId+1).ToList();
                        return list;
                    }
                }

            }
            
            return null;
        }

       public List<ClassListItem> GetClassList(int classType, int minId, int maxId)
       {
           String key = GetKey(classType, minId, maxId);
           var classLists = this.GetItem(key);
           if (classLists != null)
           {
               return classLists;
           }
           
           return null;
       }
        

       private static List<ClassListItem> GetClassListCache()
       {
           var result = _classLists;
           // Double-checked lock
           if (result == null)
           {
               lock (_locker)
               {
                   result = _classLists;
                   if (result == null)
                   {
                       result = LoadClassListCache();
                       _classLists = result;
                   }
               }
           }
           return result;
       }

       private static List<ClassListItem> LoadClassListCache()
       {
           int totalNum = 0;
           Byte loadBy = (Byte)Enums.DBAccess.ClassListLoadType.ByRecommendationCached;
           var classList = _repository.GetClassList(loadBy, 0, 0/*cityId*/, 0/*categoryId*/, false, 0, 0, 0, out totalNum, "", 0, 0);
           if (classList != null && classList.Count > 0)
           {
               return classList;
           }
           return null;
       }

    }
}

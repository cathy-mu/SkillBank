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

    public interface ICategoryProvider
    {
        List<ClassCategory> GetCategoryLkp(int cityId);
        ClassCategory GetCategory(int categoryId);
    }

    public class CategoryProvider : CacheBase<int, List<ClassCategory>>, ISingleton, ICategoryProvider
    {
        private readonly ILookupsRepository _repository;

        private static Dictionary<int, ClassCategory> _categoryDic;
        private static readonly object _locker = new object();

        #region Constructors

        public CategoryProvider(ILookupsRepository repository)
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

        protected override ICacheManager<int, List<ClassCategory>> GetCustomCacheManager(
            SynchronizedDictionary<int, CachedItemInfo<int, List<ClassCategory>>> elementCache,
             KeyedReaderWriterLockSlim<int> contentLoadLock)
        {
            int timeOutMins = Constants.Setting.CacheTimeOut_CategoryMins;
            var cacheMgr = new TimestampCacheManagerAsync<int, List<ClassCategory>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override List<ClassCategory> LoadItem(int key)
        {
            var categories = this._repository.GetClassCategories(key);
            if (categories != null && categories.Count > 0)
            {
                return categories;
            }

            return null;
        }

        public List<ClassCategory> GetCategoryLkp(int cityId)
        {
            var categoryLkp = this.GetItem(cityId);
            if (categoryLkp != null && categoryLkp.Count() > 0)
            {
                return categoryLkp;
            }

            return null;
        }

        public ClassCategory GetCategory(int categoryId)
        {
            var categories = GetCategoryDicCache();
            if (categories != null && categories.Count() > 0)
            {
                if (categories.ContainsKey(categoryId))
                {
                    return categories[categoryId];
                }
            }

            return null;
        }


        private Dictionary<int, ClassCategory> GetCategoryDicCache()
        {
            var result = _categoryDic;
            // Double-checked lock
            if (result == null)
            {
                lock (_locker)
                {
                    result = _categoryDic;
                    if (result == null)
                    {
                        int key = 0;//all cities
                        var categories = this.GetItem(0);
                        if (categories != null && categories.Count > 0)
                        {
                            Dictionary<int, ClassCategory> dic = new Dictionary<int, ClassCategory>();
                            foreach (var item in categories)
                            {
                                dic.Add(item.CategoryId, item);
                            }
                            result = dic;
                        }
                        _categoryDic = result;
                    }
                }
            }

            return result;
        }

    }
}
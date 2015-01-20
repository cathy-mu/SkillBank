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

    public interface ICategoryLkpProvider
    {
        Dictionary<int, CategoryLkpItem> GetCategoryLkp();
        CategoryLkpItem GetCategoryLkpItem(int categoryId);
    }

    public class CategoryLkpProvider : CacheBase<object, Dictionary<int,CategoryLkpItem>>, ISingleton, ICategoryLkpProvider
    {
        private readonly ILookupsRepository _repository;
        private String keyPrefix = "Category";

        #region Constructors

        public CategoryLkpProvider(ILookupsRepository repository)
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
            return String.Format("{0}{1}{2}", keyPrefix, Constants.Setting.CacheKeySpliter, Constants.BizConfig.SingleLocalCode.ToLower());
        }

        public void RefreshItem()
        {
            base.RefreshItem(GetKey());
        }

        protected override ICacheManager<object, Dictionary<int,CategoryLkpItem>> GetCustomCacheManager(
            SynchronizedDictionary<object, CachedItemInfo<object, Dictionary<int,CategoryLkpItem>>> elementCache,
             KeyedReaderWriterLockSlim<object> contentLoadLock)
        {
            int timeOutMins = Constants.Setting.CacheTimeOut_CategoryMins;
            var cacheMgr = new TimestampCacheManagerAsync<object, Dictionary<int,CategoryLkpItem>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override Dictionary<int,CategoryLkpItem> LoadItem(object key)
        {
            var categories = this._repository.GetSkillCategories();
            if (categories != null && categories.Count > 0)
            {
                Dictionary<int, CategoryLkpItem> catagoryLkp = new Dictionary<int, CategoryLkpItem>();
                CategoryLkpItem categoryLkpItem;
                foreach (var item in categories)
                {
                    categoryLkpItem = new CategoryLkpItem();
                    categoryLkpItem.CategoryInfo = item;
                    var childCategories = categories.Where(c => c.Parent_CategoryId == item.CategoryId);
                    if (childCategories != null & childCategories.Count() > 0)
                    {
                        categoryLkpItem.ChildCategories = childCategories.ToList<SkillCategory>();
                    }
                    catagoryLkp.Add(item.CategoryId, categoryLkpItem);
                }
                return catagoryLkp;
            }

            return null;
        }

        public Dictionary<int, CategoryLkpItem> GetCategoryLkp()
        {
            var categoryLkp = this.GetItem(GetKey());
            if (categoryLkp != null && categoryLkp.Count() > 0)
            {
                return categoryLkp;
            }

            return null;
        }

        public CategoryLkpItem GetCategoryLkpItem(int categoryId)
        {
            var categories = this.GetItem(GetKey());
            if (categories != null && categories.Count() > 0)
            {
                if (categories.ContainsKey(categoryId))
                {
                return categories[categoryId];
                }
            }

            return null;
        }

    }
}
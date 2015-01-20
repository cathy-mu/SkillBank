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

    public interface ICategoryTagProvider
    {
        Dictionary<int, List<CategoryTag>> GetCategoryTags();
        List<CategoryTag> GetCategoryTagItems(int categoryId);
    }

    public class CategoryTagProvider : CacheBase<object, Dictionary<int, List<CategoryTag>>>, ISingleton, ICategoryTagProvider
    {
        private readonly ICategoryTagRepository _repository;
        private String keyPrefix = "CategoryTag";

        #region Constructors

        public CategoryTagProvider(ICategoryTagRepository repository)
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

        protected override ICacheManager<object, Dictionary<int, List<CategoryTag>>> GetCustomCacheManager(
            SynchronizedDictionary<object, CachedItemInfo<object, Dictionary<int, List<CategoryTag>>>> elementCache,
             KeyedReaderWriterLockSlim<object> contentLoadLock)
        {
            int timeOutMins = Constants.Setting.CacheTimeOut_CategoryTagMins;
            var cacheMgr = new TimestampCacheManagerAsync<object, Dictionary<int, List<CategoryTag>>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override Dictionary<int, List<CategoryTag>> LoadItem(object key)
        {
            var tags = this._repository.GetCategoryTags();
            if (tags != null && tags.Count > 0)
            {
                Dictionary<int, List<CategoryTag>> catagoryPopTags = new Dictionary<int, List<CategoryTag>>();
                List<int> categoryIds = tags.Select(t=>(int)t.Category_Id).Distinct().ToList();
                foreach (var idItem in categoryIds)
                {
                    var tagList = tags.Where(t => (t.Category_Id == idItem)).OrderBy(t => t.Category_Id).OrderByDescending(t => t.RankNo).ToList();
                    if (tagList != null && tagList.Count() > 0)
                    {
                        //TO DO:Set constanst for this tag size
                        if (tagList.Count() > 5)
                        {
                            catagoryPopTags.Add(idItem, tagList.Take(5).ToList<CategoryTag>());
                        }
                        else
                        {
                            catagoryPopTags.Add(idItem, tagList.ToList<CategoryTag>());
                        }
                    }
                }
                return catagoryPopTags;
            }

            return null;
        }

        public Dictionary<int, List<CategoryTag>> GetCategoryTags()
        {
            var categoryTags = this.GetItem(GetKey());
            if (categoryTags != null && categoryTags.Count() > 0)
            {
                return categoryTags;
            }

            return null;
        }

        public List<CategoryTag> GetCategoryTagItems(int categoryId)
        {
            var categoryTags = this.GetItem(GetKey());
            if (categoryTags != null && categoryTags.Count() > 0 && categoryTags.ContainsKey(categoryId))
            {
                return categoryTags[categoryId];
            }

            return null;
        }

    }
}
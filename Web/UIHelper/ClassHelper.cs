using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.DataSource;
using SkillBank.Site.Services;
using SkillBank.Site.Services.Models;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Services.CacheProviders;
using SkillBank.Site.Services.Managers;

namespace SkillBank.Site.Web
{
    public static class ClassHelper //: IClassHelper
    {



        public static String GetClassSearchTitle(SByte categoryId, int cityId, int recordNum)
        {
            //TO DO:Change to blurb and return blurb id
            String resultTitle = String.Format("{0} result for {1} in {2}", recordNum, categoryId, cityId);

            return resultTitle;
        }

        //public static string GetCategoryNameById(Dictionary<int, CategoryLkpItem> categoryLkp, int categoyId)
        //{
        //    var categoryName = "default";
        //    if (categoryLkp.ContainsKey(categoyId))
        //    {
        //        categoryName = categoryLkp[categoyId].CategoryInfo.CategoryName;
        //    }
        //    else
        //    {   
        //        //var parentCategoryId = categoryLkp.Where(i=>i.Value.ChildCategories!=null).Select(i=>i.Value.ChildCategories);
        //        var childCategoryLkp = categoryLkp.SelectMany(i => i.Value.ChildCategories).ToList();
        //        if (childCategoryLkp != null)
        //        {
        //            var childItem = childCategoryLkp.Where(i => i.CategoryId == categoyId).FirstOrDefault();
        //            categoryName = (childItem != null) ? childItem.CategoryName : categoryName;
        //        }
        //    }
        //    return categoryName;
        //}

        public static String GetCategoryNameById(Dictionary<int, CategoryLkpItem> categoryLkp, List<CategoryItem> categories, int categoryId)
        {
            var categoryName = "default";
            if (categoryLkp.ContainsKey(categoryId))
            {
                String subCateKey = String.Format(";{0},", categoryId);
                var parentCategory = categories.Where(i => (i.SubCategories != null && i.SubCategories.Contains(subCateKey))).FirstOrDefault();
                if (parentCategory == null)
                {
                    categoryName = categoryLkp[categoryId].CategoryInfo.CategoryName;
                }
                else
                {
                    categoryName = parentCategory.CateName;
                }

            }
            return categoryName;
        }

    }
}

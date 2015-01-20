using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource;
using SkillBank.Site.Services;
using SkillBank.Site.Services.Models;
using SkillBank.Site.Web.ViewModel;
using SkillBank.Site.Services.CacheProviders;
using SkillBank.Site.Services.Managers;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.Web
{
    public static class LookupHelper
    {

        public static List<CategoryItem> GetCatagory4Picker(Dictionary<int, CategoryLkpItem> categories)
        {
            List<CategoryItem> categotyList = new List<CategoryItem>();
            StringBuilder subCategories = new StringBuilder();
            int childNo = 0;
            foreach (var categoryItem in categories)
            {
                if (!categoryItem.Value.CategoryInfo.Parent_CategoryId.HasValue)
                {
                    CategoryItem item = new CategoryItem();
                    var categoryInfo = categoryItem.Value.CategoryInfo;
                    item.BlubId = categoryInfo.Blurb_Id;
                    item.CateId = categoryInfo.CategoryId;
                    item.CateName = categoryInfo.CategoryName;
                    if (categoryItem.Value.ChildCategories != null && categoryItem.Value.ChildCategories.Count > 0)
                    {
                        var childCategories = categoryItem.Value.ChildCategories;
                        subCategories.Append(String.Format(",{0};", ResourceHelper.GetTransText(251)));
                        foreach (var childCategory in childCategories)
                        {
                            childNo++;
                            subCategories.Append(String.Format("{0},{1}{2}", childCategory.CategoryId, ResourceHelper.GetTransText(childCategory.Blurb_Id), (childNo == childCategories.Count()) ? "" : ";"));
                        }
                        item.SubCategories = subCategories.ToString();

                    }
                    categotyList.Add(item);
                }
            }

            return categotyList;
        }

        /// <summary>
        /// Convert category list for dropdowm
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public static List<CityItem> GetCity4Picker(Dictionary<int, CityInfo> cities)
        {
            List<CityItem> cityList = new List<CityItem>();
            if (cities != null && cities.Count > 0)
            {
                foreach (var city in cities)
                {
                    cityList.Add(new CityItem() { Id = city.Value.CityId, Text = String.Format("{0}({1})", city.Value.CityName, city.Value.CityKey).Trim(), OrderRank = city.Value.OrderRank });
                }
            }
            return cityList;
        }
    }
}

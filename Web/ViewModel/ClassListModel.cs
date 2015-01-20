using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource.Data;
//using SkillBank.Site.Services.Models;

namespace SkillBank.Site.Web.ViewModel
{
    public class ClassListModel
    {
        //public List<ClassItem> ClassList;
        public List<ClassListItem> ClassList;
        public List<CategoryItem> ClassCategory;
        public List<CityInfo> ClassCity;
        public List<CategoryItem> CategoryLkp { get; set; }
        public Dictionary<int, CityInfo> CityLkp { get; set; }
        public int ClassNum;
        public int PageNum;
        public int PageId;
        public int PageIdMin;
        public int PageIdMax;
        public String SearchResultTitle;
        public int SelCityId;
        public Byte SelCategoryId;
        //public Byte SelParentCategoryId;
        public Byte SelSubCategoryId;
        public String OrderByKey;
        public Byte TabId;
    }
}

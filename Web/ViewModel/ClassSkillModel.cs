using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource;
using SkillBank.Site.Services.Models;

namespace SkillBank.Site.Web.ViewModel
{
    public class CityItem
    {
        public int Id { get; set; }
        public String Text { get; set; }
        public Byte OrderRank { get; set; }
    }

    public class ClassSkillModel
    {
        public Boolean hasMemberCityInfo;
        public List<CategoryItem> CategoryLkp { get; set; }
        //public List<CityItem> CityLkp { get; set; }
    }
}

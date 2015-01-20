using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services.Models;

namespace SkillBank.Site.Web.ViewModel
{
    public class ClassAddModel
    {
        //public List<CategoryItem> CategoryLkp { get; set; }
        public ClassInfo ClassInfo { get; set; }
        public MemberInfo MyInfo { get; set; }
        public Boolean isEdit { get; set; }
        public String CityForMap { get; set; }
        public String CategoryName { get; set; }
        public Boolean hasAboutMeInfo { get; set; }
        
    }
}

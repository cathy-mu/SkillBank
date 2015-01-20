using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.Web.ViewModel
{
    public class HomePageModel
    {
        public List<ClassListItem> LatestClassList;
        public List<ClassListItem> MasterClassList;
        public List<ClassListItem> OddClassList;
    }
}

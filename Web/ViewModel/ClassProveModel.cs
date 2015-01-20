using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource.Data;
//using SkillBank.Site.Services.Models;

namespace SkillBank.Site.Web.ViewModel
{
    public class ClassProveModel
    {
        public List<ClassInfo> ClassList;//TO DO:4 Class Edit Process 1.1, Remove after roll out
        public List<ClassEditItem> ClassEditList;
        public Boolean IsAdmin;
    }
}

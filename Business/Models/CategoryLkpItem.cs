using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.DataSource;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.Services.Models
{
    public class CategoryLkpItem
    {
        public SkillCategory CategoryInfo;
        public List<SkillCategory> ChildCategories;
    }
}

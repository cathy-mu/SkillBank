using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

using SkillBank.Site.DataSource.Mapper;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.Site.DataSource.Data
{
    public interface ICategoryTagRepository
    {
        List<CategoryTag> GetCategoryTags();
    }

    public class CategoryTagRepository : DbContext, ICategoryTagRepository
    {
        public CategoryTagRepository()
            : base("name=Entities")
        {
        }

        public List<CategoryTag> GetCategoryTags()
        {
            var result = CategoryPopTag_Load_p();
            return LookupsMapper.Map(result);
        }

        private ObjectResult<CategoryTag> CategoryPopTag_Load_p()
        {
            ObjectResult<CategoryTag> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CategoryTag>("CategoryPopTag_Load_p");
            return result;
        }

        
    }
}


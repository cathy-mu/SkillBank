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

namespace SkillBank.Site.DataSource.Data
{

    public interface IBlurbsRepository
    {
        Dictionary<int, String> Blurbs_LoadByLanguageSiteVersion_p(String language, Byte siteVersion);
    }

    public class BlurbsRepository : Entities, IBlurbsRepository
    {
        public BlurbsRepository()
            //: base("name=Entities")
        {
        }

        /// <summary>
        /// Load Blurb text and id 
        /// </summary>
        /// <param name="language">LanguageCode</param>
        /// <param name="version">SiteVertion</param>
        /// <returns></returns>
        public Dictionary<int, String> Blurbs_LoadByLanguageSiteVersion_p(String language, Byte version)
        {
            var versionParameter = new ObjectParameter("Version", version);

            var languageParameter = language != null ?
                new ObjectParameter("Language", language) :
                new ObjectParameter("Language", typeof(string));

            ObjectResult<Blurbs_LoadByLanguageSiteVersion_p_Result> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Blurbs_LoadByLanguageSiteVersion_p_Result>("Blurbs_LoadByLanguageSiteVersion_p", MergeOption.NoTracking, versionParameter, languageParameter);
            return BlurbMapper.Map(result);
        }
    }
}




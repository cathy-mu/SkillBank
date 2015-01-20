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

    public interface IMailTemplatesRepository
    {
        List<EmailTemplate> GetMailTemplates(Byte siteVersion);
    }

    public class MailTemplatesRepository : Entities, IMailTemplatesRepository
    {
        public MailTemplatesRepository()
         //   : base("name=Entities")
        {
        }

        /// <summary>
        /// Load Blurb text and id 
        /// </summary>
        /// <param name="language">LanguageCode</param>
        /// <param name="version">SiteVertion</param>
        /// <returns></returns>
        public List<EmailTemplate> GetMailTemplates(Byte siteVersion)
        {
            var result = MailTemplates_LoadBySiteVersion_p(siteVersion);
            return LookupsMapper.Map(result);
        }

        /// <summary>
        /// Load Blurb text and id 
        /// </summary>
        /// <param name="language">LanguageCode</param>
        /// <param name="version">SiteVertion</param>
        /// <returns></returns>
        private ObjectResult<EmailTemplate> MailTemplates_LoadBySiteVersion_p(Byte version)
        {
            var versionParameter = new ObjectParameter("Version", version);

            ObjectResult<EmailTemplate> result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<EmailTemplate>("EmailTemplate_Load_p", versionParameter);
            return result;
        }
    }
}




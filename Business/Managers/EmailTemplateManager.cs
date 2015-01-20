using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.DataSource.Data;
//using EF.Frameworks.Common.CachingEF;
//using EF.Frameworks.Orpheus.ContentManagementEF;


namespace SkillBank.Site.Services.Managers
{
    public interface IMailTemplateManager
    {
        Dictionary<int, String> GetBLurbs(String language, Byte siteVersion);
    }

    public class MailTemplateManager : IMailTemplateManager
    {
        private readonly IMailTemplateRepository _repository;

        public MailTemplateManager(IBlurbsRepository repository)
        {
            _repository = repository;
        }

        public Dictionary<String, String> GetBLurbs(String language, Byte siteVersion)
        {
            return _repository.Blurbs_LoadByLanguageSiteVersion_p(language, siteVersion);
        }
        
        
    }

}

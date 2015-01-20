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
    public interface IContentManager
    {
        Dictionary<int, String> GetBLurbs(String language, Byte siteVersion);
    }

    public class ContentManager : IContentManager
    {
        private readonly IBlurbsRepository _repository;

        public ContentManager(IBlurbsRepository repository)
        {
            _repository = repository;
        }

        public Dictionary<int, String> GetBLurbs(String language, Byte siteVersion)
        {
            return _repository.Blurbs_LoadByLanguageSiteVersion_p(language, siteVersion);
        }
    }
}

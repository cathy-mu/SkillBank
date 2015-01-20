using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF.Frameworks.Common.ThreadingEF;
using EF.Frameworks.Common.CollectionsEF;
using EF.Frameworks.Common.ConfigurationEF;

using EF.Frameworks.Common.FactoryEF;
using EF.Frameworks.Orpheus.ContentManagementEF;
using SkillBank.Site.Common.Caching;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;

namespace SkillBank.Site.Services.CacheProviders
{

    public interface IMailTemplatesProvider
    {
        String GetTemplate(String templateName, Byte siteVersion);
    }

    public class MailTemplatesProvider : CacheBase<object, IDictionary<String, String>>, ISingleton, IMailTemplatesProvider
    {
        private readonly IMailTemplatesRepository _repository;
        private String keyPrefix = "MailTemplates";

        #region Constructors

        public MailTemplatesProvider(IMailTemplatesRepository repository)
            : base(RefreshOptions.Custom)
        {
            this._repository = repository;
        }

        #endregion

        public new void RefreshAll()
        {
            base.RefreshAll();
        }

        public String GetKey(Byte siteVersion)
        {
            return String.Format("{0}{1}{2}", keyPrefix, Constants.Setting.CacheKeySpliter, siteVersion);
        }

        public void RefreshItem(Byte siteVersion)
        {
            base.RefreshItem(GetKey(siteVersion));
        }

        protected override ICacheManager<object, IDictionary<String, String>> GetCustomCacheManager(
            SynchronizedDictionary<object, CachedItemInfo<object, IDictionary<String, String>>> elementCache,
             KeyedReaderWriterLockSlim<object> contentLoadLock)
        {
            int timeOutMins = Constants.Setting.CacheTimeOut_MailTemplateMins;
            var cacheMgr = new TimestampCacheManagerAsync<object, IDictionary<String, String>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override IDictionary<String, String> LoadItem(object key)
        {
            var templates = this._repository.GetMailTemplates(Convert.ToByte(key));

            if (templates != null && templates.Count > 0)
            {
                return templates.ToDictionary(t => t.TemplateName, t => t.Content);
            }
            return null;
        }

        public String                     GetTemplate(String templateName, Byte siteVersion)
        {
            var templates = this.GetItem(GetKey(siteVersion));
            if (templates != null && templates.Count() > 0 && templates.ContainsKey(templateName))
            {
                return templates[templateName];
            }
            else
            {
                return String.Format("No content for[{0}]", templateName);
            }
        }

    }
}

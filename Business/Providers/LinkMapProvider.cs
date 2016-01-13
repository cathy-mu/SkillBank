using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF.Frameworks.Common.ThreadingEF;
using EF.Frameworks.Common.CollectionsEF;
using EF.Frameworks.Common.ConfigurationEF;

using SkillBank.Site.Common.Caching;
using EF.Frameworks.Common.FactoryEF;
using EF.Frameworks.Orpheus.ContentManagementEF;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services.Models;

namespace SkillBank.Site.Services.CacheProviders
{

    public interface ILinkMapProvider
    {
        List<LinkMap> GetLinkMapLkp(int sourceId);
    }

    public class LinkMapProvider : CacheBase<object, Dictionary<int, List<LinkMap>>>, ISingleton, ILinkMapProvider
    {
        private readonly ILookupsRepository _repository;
        private String keyPrefix = "LnkMap";

        #region Constructors

        public LinkMapProvider(ILookupsRepository repository)
            : base(RefreshOptions.Custom)
        {
            this._repository = repository;
        }

        #endregion

        public new void RefreshAll()
        {
            base.RefreshAll();
        }

        public String GetKey()
        {
            return keyPrefix;
        }

        public void RefreshItem()
        {
            base.RefreshItem(GetKey());
        }

        protected override ICacheManager<object, Dictionary<int, List<LinkMap>>> GetCustomCacheManager(
            SynchronizedDictionary<object, CachedItemInfo<object, Dictionary<int, List<LinkMap>>>> elementCache,
             KeyedReaderWriterLockSlim<object> contentLoadLock)
        {
            int timeOutMins = Constants.Setting.CacheTimeOut_CityInfoMins;
            var cacheMgr = new TimestampCacheManagerAsync<object, Dictionary<int, List<LinkMap>>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override Dictionary<int, List<LinkMap>> LoadItem(object key)
        {
            var lnkMapLkp = this._repository.GetLinkMap();
            if (lnkMapLkp != null && lnkMapLkp.Count > 0)
            {
                int sourceId = 0;
                Dictionary<int, List<LinkMap>> linkDic = new Dictionary<int, List<LinkMap>>();
                List<LinkMap> linkList = new List<LinkMap>();

                foreach (var item in lnkMapLkp)
                {
                    if (item.SourceId != sourceId)
                    {
                        if (linkList.Count > 0)
                        {
                            linkDic.Add(sourceId, linkList);
                            linkList = new List<LinkMap>();
                        }
                        sourceId = item.SourceId;
                    }
                    linkList.Add(item);
                }

                if (linkList.Count > 0)
                {
                    linkDic.Add(sourceId, linkList);
                }


                return linkDic;
            }

            return null;
        }


        public List<LinkMap> GetLinkMapLkp(int sourceId)
        {
            var links = this.GetItem(GetKey());
            if (links != null && links.Count() > 0 && links.ContainsKey(sourceId))
            {
                return links[sourceId];
            }

            return null;
        }

    }
}
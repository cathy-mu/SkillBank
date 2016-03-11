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

    public interface ICityLkpProvider
    {
        Dictionary<String, Dictionary<int, CityInfo>> GetCitys();
        //List<CityInfo> GetCityList(String localeCode);
        Dictionary<int, CityInfo> GetCityLkp(String localeCode);
        Dictionary<int, CityInfo> GetClassCityLkp(String localeCode);
    }

    public class CityLkpProvider : CacheBase<object, Dictionary<String, Dictionary<int, CityInfo>>>, ISingleton, ICityLkpProvider
    {
        private readonly ILookupsRepository _repository;
        private String keyPrefix = "City";

        #region Constructors

        public CityLkpProvider(ILookupsRepository repository)
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
            return String.Format("{0}{1}{2}", keyPrefix, Constants.Setting.CacheKeySpliter, Constants.BizConfig.SingleLocalCode.ToLower());
        }

        public void RefreshItem()
        {
            base.RefreshItem(GetKey());
        }

        protected override ICacheManager<object, Dictionary<String, Dictionary<int, CityInfo>>> GetCustomCacheManager(
            SynchronizedDictionary<object, CachedItemInfo<object, Dictionary<String, Dictionary<int, CityInfo>>>> elementCache,
             KeyedReaderWriterLockSlim<object> contentLoadLock)
        {
            int timeOutMins = Constants.Setting.CacheTimeOut_CityInfoMins;
            var cacheMgr = new TimestampCacheManagerAsync<object, Dictionary<String, Dictionary<int, CityInfo>>>(
                elementCache
                , contentLoadLock
                , LoadItem
                , TimeSpan.FromMinutes(timeOutMins)
                );

            return cacheMgr;
        }

        protected override Dictionary<String, Dictionary<int, CityInfo>> LoadItem(object key)
        {
            var cityLkp = this._repository.GetCityLkp();
            if (cityLkp != null && cityLkp.Count > 0)
            {
                Dictionary<String, Dictionary<int, CityInfo>> cityDic = new Dictionary<String, Dictionary<int, CityInfo>>();
                Dictionary<int, CityInfo> cities = new Dictionary<int, CityInfo>();
                Dictionary<int, CityInfo> classCities = new Dictionary<int, CityInfo>();
                String locale = String.Empty;
                //Hack for offline classes city
                classCities.Add(1000, new CityInfo() { CityId = 1000, CityKey = "Offline", CityName = "在线教授", LocaleCode = "cn", OrderRank = 100 });
                foreach (var city in cityLkp)
                {
                    if (city.LocaleCode != locale)
                    {
                        if (cities.Count > 0)
                        {
                            cityDic.Add(locale, cities);
                            cities = new Dictionary<int, CityInfo>();
                            classCities = new Dictionary<int, CityInfo>();
                        }
                        locale = city.LocaleCode;
                    }
                    cities.Add(city.CityId, city);
                    if (city.OrderRank > 0)
                    {
                        classCities.Add(city.CityId, city);
                    }

                }

                if (cities.Count > 0)
                {
                    cityDic.Add(locale, cities);
                    if (classCities.Count > 0)
                    {
                        cityDic.Add(String.Format("{0}{1}c", locale, Constants.Setting.CacheKeySpliter), classCities);
                    }
                }

                return cityDic;
            }

            return null;
        }

        public Dictionary<String, Dictionary<int, CityInfo>> GetCitys()
        {
            var citys = this.GetItem(GetKey());
            if (citys != null && citys.Count() > 0)
            {
                return citys;
            }

            return null;
        }

        //public List<CityInfo> GetCityList(String localeCode)
        //{
        //    var citys = this.GetItem(GetKey());
        //    if (citys != null && citys.Count() > 0 && citys.ContainsKey(localeCode))
        //    {
        //        var result = citys[localeCode];
        //        var list = result.Values.ToList<CityInfo>();
        //        return list;
        //    }

        //    return null;
        //}

        public Dictionary<int, CityInfo> GetCityLkp(String localeCode)
        {
            var citys = this.GetItem(GetKey());
            if (citys != null && citys.Count() > 0 && citys.ContainsKey(localeCode))
            {
                return citys[localeCode];
            }

            return null;
        }

        public Dictionary<int, CityInfo> GetClassCityLkp(String localeCode)
        {
            var citys = this.GetItem(GetKey());
            if (citys != null && citys.Count() > 0 && citys.ContainsKey(localeCode))
            {
                return citys[String.Format("{0}{1}c", localeCode, Constants.Setting.CacheKeySpliter)];
            }

            return null;
        }

    }
}
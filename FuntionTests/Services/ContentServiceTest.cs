using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using Assert = NUnit.Framework.Assert;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services;
using SkillBank.Site.Services.Managers;
using SkillBank.Site.Services.CacheProviders;

namespace SkillBank.FunctionTests
{
    [TestClass]
    public class ContentServiceTests
    {
        private int _memberId;
        private int _member2Id;
        private int _invalidMemberId;
        ContentService _svr;

        [TestInitialize]
        public void TestInitialize()
        {
            _memberId = 1;
            _member2Id = 2;
            _invalidMemberId = 9999;

            ContentManager _contentMgr = new ContentManager(new BlurbsRepository());
            BlurbsProvider _blurbProvider = new BlurbsProvider(new BlurbsRepository());
            CityLkpProvider _cityLkpProvider = new CityLkpProvider(new LookupsRepository());
            MetaTagProvider _metaTagProvider = new MetaTagProvider(new LookupsRepository());
            CategoryLkpProvider _categoryLkpProvider = new CategoryLkpProvider(new LookupsRepository());
            CategoryProvider _categoryProvider = new CategoryProvider(new LookupsRepository());
            SystemNotificationProvider _systemNotificationProvider = new SystemNotificationProvider(new LookupsRepository());
            TopBannerProvider _topBannerProvider = new TopBannerProvider(new LookupsRepository());
            PortalBannerProvider _portalBannerProvider = new PortalBannerProvider(new LookupsRepository());
            LinkMapProvider _linkMapProvider = new LinkMapProvider(new LookupsRepository());

            _svr = new ContentService(_contentMgr, _blurbProvider, _cityLkpProvider, _metaTagProvider, _categoryLkpProvider, _categoryProvider, _systemNotificationProvider, _topBannerProvider, _portalBannerProvider, _linkMapProvider);
        }

        #region Review Test Functions
        #endregion

        #region City Test Functions
        [TestMethod]
        public void Should_GetCitysByLocale()
        {
            String localeCode = "cn";
            var cities = _svr.GetCities(localeCode);
            Assert.IsNotNull(cities);
            cities.Count.AssertIsGreaterThan(0);
        }

        [TestMethod]
        public void Should_GetCitysByLocale_Correctly()
        {
            String localeCode = "cn";
            var cities = _svr.GetCities(localeCode);
            foreach (var city in cities)
            {
                Assert.IsNotNull(city.Value.CityKey);
                Assert.IsNotNull(city.Value.CityName);
                Assert.IsNotNull(city.Value.CityId);
            }
            String searchKey = "she";

            var result = cities.Where(c => c.Value.CityKey.ToLower().Contains(searchKey)).ToList();
            result.Count.AssertIsGreaterThan(0);
        }

        [TestMethod]
        public void Should_GetCityNameByCityId()
        {
            String localeCode = "cn";
            int cityId = 1;
            var cityName = _svr.GetCityNameById(localeCode, cityId);
            Assert.IsNotNull(cityName);
        }

        [TestMethod]
        public void Should_GetCityNameByCityId_Correctly()
        {
            String localeCode = "cn";
            int cityId = 1;
            var cityName = _svr.GetCityNameById(localeCode, cityId);
            Assert.IsNotNull(cityName);
            Assert.IsNotEmpty(cityName);
        }

        #endregion

        #region Category Test Functions
        [TestMethod]
        public void Should_GetCategoryByCityId()
        {
            int cityId = 0;
            var categories = _svr.GetCategories(cityId);
            Assert.IsNotNull(categories);
            categories.Count.AssertIsGreaterThan(0);
        }
        #endregion

        #region Banner Test Functions
        [TestMethod]
        public void Should_GetTopBanners()
        {
            var lkp = _svr.GetTopBannerLkp();
            Assert.IsNotNull(lkp);
            lkp.Count.AssertIsGreaterThan(0);
        }

        [TestMethod]
        public void Should_GetPrtalBanners()
        {
            var lkp = _svr.GetPortalBannerLkp();
            Assert.IsNotNull(lkp);
            lkp.Count.AssertIsGreaterThan(0);
        }

        [TestMethod]
        public void Should_GetLinkMap()
        {
            int sourceId = 4;
            var lkp = _svr.GetLinkMapLkp(sourceId);
            Assert.IsNotNull(lkp);
            lkp.Count.AssertIsGreaterThan(0);

            sourceId = 1;
            lkp = _svr.GetLinkMapLkp(sourceId);
            Assert.IsNull(lkp);
            
        }

        #endregion

        

        [TestCleanup]
        public void TestCleanup()
        {

        }
    }
}
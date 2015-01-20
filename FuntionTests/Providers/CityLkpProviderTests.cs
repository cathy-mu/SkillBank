using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services.CacheProviders;

namespace SkillBank.FunctionTests
{
    [TestClass]
    public class CityProviderTests
    {
        private LookupsRepository _repository;
        private CityLkpProvider _provider;
        private String _localeCode, _invalidLocaleCode;
        //private int _cityId, _invalidCityId;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new LookupsRepository(); 
            _provider = new CityLkpProvider(_repository);
            _localeCode = "cn";
            _invalidLocaleCode = "us";
            //_cityId = 1;
            //_invalidCityId = 999;
        }

        [TestMethod]
        public void Should_GetCities()
        {
            var cities = _provider.GetCityLkp(_localeCode);
            Assert.IsNotNull(cities);
        }

        [TestMethod]
        public void Should_GetCities_Correctly()
        {
            var cities = _provider.GetCityLkp(_localeCode);
            cities.Count.AssertIsGreaterThan(0);
            var cityItem = cities[0];
            Assert.IsNotNull(cityItem.LocaleCode);
            Assert.IsNotNull(cityItem.CityName);
        }

        [TestMethod]
        public void ShouldNot_GetCity_WithUnExistCityId()
        {
            var cities = _provider.GetCityLkp(_invalidLocaleCode);
            Assert.IsNull(cities);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _repository.Dispose();
        }
    }
}
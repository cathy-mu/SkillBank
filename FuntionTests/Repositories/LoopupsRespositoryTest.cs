using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Mocks;

using SkillBank.Site.DataSource.Data;

namespace SkillBank.FunctionTests
{
    [TestClass]
    public class LookupsRepositoryTests
    {
        [TestMethod]
        public void Should_GetAllCategoties()
        {
            LookupsRepository repository = new LookupsRepository();
            var result = repository.GetSkillCategories();
            Assert.IsNotNull(result);
            result.Count.AssertIsGreaterThan(0);
        }

        [TestMethod]
        public void Should_GetAllCategoties_Correctly()
        {
            LookupsRepository repository = new LookupsRepository();

            var result = repository.GetSkillCategories();
            Assert.IsNotNull(result[0].Blurb_Id);
            Assert.IsNotNull(result[0].CategoryId);
        }

        [TestMethod]
        public void Should_GetMetaTagLkp()
        {
            LookupsRepository repository = new LookupsRepository();
            var result = repository.GetMetaTagLkp();
            Assert.IsNotNull(result);
            result.Count.AssertIsGreaterThan(0);
        }

        [TestMethod]
        public void Should_GetMetaTagLkp_Correctly()
        {
            LookupsRepository repository = new LookupsRepository();

            var result = repository.GetMetaTagLkp();
            String metaKey1 = "default";
            String metaKey2 = "classadd";//"home";

            Assert.AreEqual(metaKey1,result[metaKey1].MetaKey);
            result[metaKey1].TitleBlurb.AssertIsGreaterThan(0);
            result[metaKey1].KeywordsBlurb.AssertIsGreaterThan(0); 
            result[metaKey1].DescriptionBlurb.AssertIsGreaterThan(0);

            Assert.AreEqual(metaKey2, result[metaKey2].MetaKey);
            result[metaKey2].TitleBlurb.AssertIsGreaterThan(0);
            result[metaKey2].KeywordsBlurb.AssertIsGreaterThan(0);
            result[metaKey2].DescriptionBlurb.AssertIsGreaterThan(0);
        }

        [TestMethod]
        public void Should_GetCityLkp()
        {
            LookupsRepository repository = new LookupsRepository();
            var result = repository.GetCityLkp();
            Assert.IsNotNull(result);
            result.Count.AssertIsGreaterThan(0);
        }

        [TestMethod]
        public void Should_GetCityLkp_Correctly()
        {
            LookupsRepository repository = new LookupsRepository();
            var result = repository.GetCityLkp();

            Assert.IsNotNull(result[0].LocaleCode);
            Assert.IsNotNull(result[0].CityKey);
            Assert.IsNotNull(result[0].CityName);
        }

    }
}

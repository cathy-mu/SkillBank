using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

using SkillBank.Site.DataSource.Data;

namespace SkillBank.FunctionTests
{
    [TestClass]
    public class BlurbsRepositoryTests
    {
        private Byte _siteVersion;
        private String _languageCode;
        BlurbsRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            _siteVersion = 1;
            _languageCode = "cs";
            _repository = new BlurbsRepository();
        }
        
        [TestMethod]
        public void Should_GetBlurbs_ByLanguageSiteVersion()
        {
            Dictionary<int, String> result = _repository.Blurbs_LoadByLanguageSiteVersion_p(_languageCode, _siteVersion);

            Assert.IsNotNull(result);
            result.Count.AssertIsGreaterThan(0);
        }

        [TestMethod]
        public void ShouldGetBlurbsList()
        {
            Dictionary<int, String> result = _repository.Blurbs_LoadByLanguageSiteVersion_p(_languageCode, _siteVersion);
            Assert.IsNotNull(result[1]);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _repository.Dispose();
        }
    }
}

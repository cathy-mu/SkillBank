﻿using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services.CacheProviders;

namespace SkillBank.FunctionTests
{
    [TestClass]
    public class CategoryProviderTests
    {
        private LookupsRepository _repository;
        private CategoryLkpProvider _provider;
        private int _categoryId,  _invalidCategoryId;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new LookupsRepository(); 
            _provider = new CategoryLkpProvider(_repository);
            _categoryId = 1;
            _invalidCategoryId = 100;
        }

        [TestMethod]
        public void Should_GetRepositories()
        {
            var categories =_provider.GetCategoryLkp();
            Assert.IsNotNull(categories);
        }

        [TestMethod]
        public void Should_GetCategories_Correctly()
        {
            var categories = _provider.GetCategoryLkp();
            var categoryItem = categories[_categoryId];
            Assert.IsNotNull(categoryItem.CategoryInfo);
            Assert.IsNotNull(categoryItem.CategoryInfo.Blurb_Id);
            Assert.IsNotNull(categoryItem.CategoryInfo.CategoryId);
            if (categoryItem.ChildCategories != null && categoryItem.ChildCategories.Count > 0)
            {
                var childCategoryItem = categoryItem.ChildCategories[0];
                Assert.IsNotNull(childCategoryItem.Blurb_Id);
                Assert.IsNotNull(childCategoryItem.CategoryId);
            }
        }

        [TestMethod]
        public void ShouldNot_GetCategory_WithUnExistCategoryId()
        {
            var categories = _provider.GetCategoryLkp();
            Assert.IsFalse(categories.ContainsKey(_invalidCategoryId));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _repository.Dispose();
        }
    }
}
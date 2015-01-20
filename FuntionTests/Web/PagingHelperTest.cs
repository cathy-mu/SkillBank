using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

using SkillBank.Site.Web;
using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services.CacheProviders;

namespace FunctionTests.Web
{
    [TestClass]
    public class PagingHelperTest
    {

        [TestMethod]
        public void Should_GetPageListIds()
        {
            int pageMinId;
            int pageMaxId;

            int pageNum = 8;
            int pageListSize = 5;
            int pageId = 1;

            PagingHelper.GetPagingListIds(pageNum, pageListSize, pageId, out pageMinId, out pageMaxId);
            Assert.AreEqual(1, pageMinId);
            Assert.AreEqual(5, pageMaxId);

            pageNum = 8;
            pageListSize = 5;
            pageId = 2;
            PagingHelper.GetPagingListIds(pageNum, pageListSize, pageId, out pageMinId, out pageMaxId);
            Assert.AreEqual(1, pageMinId);
            Assert.AreEqual(5, pageMaxId);

            pageId = 3;
            PagingHelper.GetPagingListIds(pageNum, pageListSize, pageId, out pageMinId, out pageMaxId);
            Assert.AreEqual(1, pageMinId);
            Assert.AreEqual(5, pageMaxId);
            
            pageId = 4;
            PagingHelper.GetPagingListIds(pageNum, pageListSize, pageId, out pageMinId, out pageMaxId);
            Assert.AreEqual(2, pageMinId);
            Assert.AreEqual(6, pageMaxId);

            pageId = 7;
            PagingHelper.GetPagingListIds(pageNum, pageListSize, pageId, out pageMinId, out pageMaxId);
            Assert.AreEqual(4, pageMinId);
            Assert.AreEqual(8, pageMaxId);


            pageNum = 3;
            pageListSize = 5;
            pageId = 7;
            PagingHelper.GetPagingListIds(pageNum, pageListSize, pageId, out pageMinId, out pageMaxId);
            Assert.AreEqual(1, pageMinId);
            Assert.AreEqual(3, pageMaxId);



            pageNum = 20;
            pageListSize = 17;
            pageId = 7;
            PagingHelper.GetPagingListIds(pageNum, pageListSize, pageId, out pageMinId, out pageMaxId);
            Assert.AreEqual(1, pageMinId);
            Assert.AreEqual(17, pageMaxId);

            pageId = 9;
            PagingHelper.GetPagingListIds(pageNum, pageListSize, pageId, out pageMinId, out pageMaxId);
            Assert.AreEqual(1, pageMinId);
            Assert.AreEqual(17, pageMaxId);

            pageId = 10;
            PagingHelper.GetPagingListIds(pageNum, pageListSize, pageId, out pageMinId, out pageMaxId);
            Assert.AreEqual(2, pageMinId);
            Assert.AreEqual(18, pageMaxId);

            pageId = 15;
            PagingHelper.GetPagingListIds(pageNum, pageListSize, pageId, out pageMinId, out pageMaxId);
            Assert.AreEqual(4, pageMinId);
            Assert.AreEqual(20, pageMaxId);
            
            pageId = 20;
            PagingHelper.GetPagingListIds(pageNum, pageListSize, pageId, out pageMinId, out pageMaxId);
            Assert.AreEqual(4, pageMinId);
            Assert.AreEqual(20, pageMaxId);

        }
    }

    
     

     
}

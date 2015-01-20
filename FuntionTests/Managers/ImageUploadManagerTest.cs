using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using Assert = NUnit.Framework.Assert;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Web.CDN;

namespace FunctionTests.Managers
{
    [TestClass]
    public class ImageUploadManagerTest
    {
        //private int _classId;
        //private int _invalidClassId;
        
        [TestInitialize]
        public void TestInitialize()
        {
       }

        //[TestMethod]
        //public void Should_UploadProfileImage()
        //{
        //    String uploadPath = @"D:\SkillBank\imgs\test.jpg";
        //    String imgPath = "1.jpg";
        //    String cropSetting = "50,100,300,300";
        //    var isSuccess = ImageUploadManager.UploadProfileImage(uploadPath, imgPath, cropSetting);
        //    Assert.IsTrue(isSuccess);
        //    //http://skillbank.b0.upaiyun.com/profile/m_1.jpg
        //    //http://bucket.b0.upaiyun.com/pic.jpg!30
        //}
        
        //[TestMethod]
        //public void Should_UploadClassCoverImage()
        //{
        //    String uploadPath = @"E:\SourceCode\_imgs\logo-115.png";
        //    String imgPath = "1.png";
        //    var isSuccess = ImageUploadManager.UploadClassCoverImage(uploadPath, imgPath);
        //    Assert.IsTrue(isSuccess);
        //    //http://skillbank.b0.upaiyun.com/class/c_1.png
        //}

        
       
    }
}

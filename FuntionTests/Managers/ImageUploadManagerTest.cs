using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using Assert = NUnit.Framework.Assert;
using System.Text;
using System.Security.Cryptography;

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

        [TestMethod]
        public void Should_GenerateCorrectParaTest1()
        {
            String savekey = "/img.jpg";
            String expiration = "1409200758";

            String bucket = "demobucket";
            String formAPI = "cAnyet74l9hdUag34h2dZu8z7gU=";
            //String paras = "{\"bucket\":\"demobucket\",\"expiration\":1409200758,\"save-key\":\"/img.jpg\"}";
            String paras = String.Concat("{\"bucket\":\"", bucket, "\",\"expiration\":", expiration, ",\"save-key\":\"", savekey, "\"}");
            byte[] bytes = Encoding.Default.GetBytes(paras);
            string policy = Convert.ToBase64String(bytes);
            Assert.AreEqual("eyJidWNrZXQiOiJkZW1vYnVja2V0IiwiZXhwaXJhdGlvbiI6MTQwOTIwMDc1OCwic2F2ZS1rZXkiOiIvaW1nLmpwZyJ9", policy);


            String encodeParas = String.Concat(policy, "&", formAPI);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(encodeParas));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            Assert.AreEqual("646a6a629c344ce0e6a10cadd49756d4", sb.ToString());
        }

        [TestMethod]
        public void Should_GenerateCorrectParaTest2()
        {
            String savekey = "/2/class/c_1.jpg";
            DateTime time = DateTime.Now.AddMinutes(30);

            String bucket = ConfigConstants.ThirdPartySetting.UpYun.SpaceName;
            long t = (time.Ticks - DateTime.Parse("1970-01-01 00:00:00").Ticks) / 10000000;
            String expiration = t.ToString();

            String paras = String.Concat("{\"bucket\":\"", bucket, "\",\"expiration\":", expiration, ",\"save-key\":\"", savekey, "\"}");
            byte[] bytes = Encoding.Default.GetBytes(paras);
            string policy = Convert.ToBase64String(bytes);
            String formAPI = ConfigConstants.ThirdPartySetting.UpYun.FromAPI;

            String encodeParas = String.Concat(policy, "&", formAPI);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(encodeParas));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            var signature = sb.ToString();
            Assert.IsNotEmpty(signature);
        }

        
       
    }
}

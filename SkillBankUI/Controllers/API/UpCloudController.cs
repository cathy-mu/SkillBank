using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Security.Cryptography;

using SkillBank.Site.Services;
using SkillBank.Site.Web;
using SkillBank.Site.Common;

namespace SkillBankWeb.Controllers.API
{
    public class UpCloudController : ApiController
    {
        public readonly IContentService _contentService;

        public class UploadPara
        {
            public String Url { get; set; }
            public String Policy { get; set; }
            public String Signature { get; set; }
            public String SaveKey { get; set; }
        }

        public UpCloudController(IContentService contentService)
        {
            _contentService = contentService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">{id}.{suffix}</param>
        /// <returns></returns>
        public UploadPara Post(String fileName, Boolean isAvatar)
        {
            DateTime time = DateTime.Now.AddMinutes(30);

            String bucket = ConfigConstants.ThirdPartySetting.UpYun.SpaceName;
            long t = (time.Ticks - DateTime.Parse("1970-01-01 00:00:00").Ticks) / 10000000;
            String expiration = t.ToString();
            String paras;
            //Get policy by 
            if (isAvatar)
            {
                fileName = String.Concat("/", System.Configuration.ConfigurationManager.AppSettings["ENV"], ConfigConstants.ThirdPartySetting.UpYun.AvatarPathPrefix, fileName);
                paras = String.Concat("{\"bucket\":\"", bucket, "\",\"expiration\":", expiration, ",\"save-key\":\"", fileName, "\",\"x-gmkerl-type\":\"fix_width\",\"x-gmkerl-type\":\"" + ConfigConstants.ThirdPartySetting.UpYun.AvatarImgSize["h"] + "\",\"x-gmkerl-crop\":\"0,0,180,180\"}");
            }
            else
            {
                fileName = String.Concat("/", System.Configuration.ConfigurationManager.AppSettings["ENV"], ConfigConstants.ThirdPartySetting.UpYun.ClassCoverPathPrefix, fileName);
                paras = String.Concat("{\"bucket\":\"", bucket, "\",\"expiration\":", expiration, ",\"save-key\":\"", fileName, "\",\"x-gmkerl-type\":\"fix_width\",\"x-gmkerl-type\":\"" + ConfigConstants.ThirdPartySetting.UpYun.ClassCoverSize["h"][0] +"\",\"x-gmkerl-crop\":\"0,0,640,480\"}");
            }
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

            var para = new UploadPara() { Url = ConfigConstants.ThirdPartySetting.UpYun.FromAPIHost, Policy = policy, Signature = signature, SaveKey = fileName };
            return para;
        }


    }

}

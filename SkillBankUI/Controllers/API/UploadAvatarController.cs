using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Threading;
using System.Web.UI;
using System.IO;

using SkillBank.Site.Common;
using SkillBank.Site.Web;
using SkillBank.Site.Web.CDN;
using SkillBank.Site.Services;
using SkillBank.Site.DataSource.Data;
using System.Configuration;

namespace MvcTesting.Controllers.WebApi
{
    public class UploadAvatarController : ApiController
    {
        //public readonly ICommonService _commonService;

        //public UploadController(ICommonService commonService)
        //{
        //    _commonService = commonService;
        //}
        
        // Enable both Get and Post so that our jquery call can send data, and get a status
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage UploadAvatar()
        {
            // Get a reference to the file that our jQuery sent.  Even with multiple files, they will all be their own request and be the 0 index
            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            // String corpSetting/*"top,left,width,height"*/ 
            //String corpSetting = String.Format("{0},{1},{2},{3}", HttpContext.Current.Request["imagefiletop"], HttpContext.Current.Request["imagefileleft"], HttpContext.Current.Request["imagefilew"], HttpContext.Current.Request["imagefileh"]);
            var memberId = HttpContext.Current.Request["imagefilename"];
            String fileName = String.Concat(memberId, HttpContext.Current.Request["imagefileext"]);
            Stream fileStream = file.InputStream;

            var avatarPath = String.Concat("/",System.Configuration.ConfigurationManager.AppSettings["ENV"],ConfigConstants.ThirdPartySetting.UpYun.AvatarPathPrefix, fileName);
            ImageUploadManager.UploadProfileImage(fileStream, avatarPath, HttpContext.Current.Request["imagefilesetting"]);//corpSetting
            MemberRepository _rep = new MemberRepository();
            _rep.UpdateMemberInfo(Convert.ToInt32(memberId), (Byte)Enums.DBAccess.MemberSaveType.UpdatePhoto, "", 0, "", "", true, "", (Decimal)0, (Decimal)0, new DateTime(1900, 1, 1), avatarPath);

            //String domain = UtilitiesModule.GetCookieDomain(HttpContext.Current.Request);
            //CookieManager.SetCookie(Constants.CookieKeys.MemberAvatar, avatarPath, /*isPersistent*/ true, domain, HttpContext.Current);
            // Now we need to wire up a response so that the calling script understands what happened
            HttpContext.Current.Response.ContentType = "text/plain";
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //var result = new { name = file.FileName };

            //HttpContext.Current.Response.Write(serializer.Serialize(result));

            HttpContext.Current.Response.Write(serializer.Serialize("done"));
            HttpContext.Current.Response.StatusCode = 200;

            // For compatibility with IE's "done" event we need to return a result as well as setting the context.response
            return new HttpResponseMessage(HttpStatusCode.OK);
            //http://skillbank.b0.upaiyun.com/profile/m_1.jpg
        }

        
        //[HttpGet]
        //[HttpPost]
        //public HttpResponseMessage Upload()
        //{
        //    // Get a reference to the file that our jQuery sent.  Even with multiple files, they will all be their own request and be the 0 index
        //    HttpPostedFile file = HttpContext.Current.Request.Files[0];

        //    // do something with the file in this space 
        //    // {....}
        //    // end of file doing

        //    Stream fileStream = file.InputStream;

        //    ImageUploadManager.UploadProfileImage(fileStream, "1.jpg", "");

        //    // Now we need to wire up a response so that the calling script understands what happened
        //    HttpContext.Current.Response.ContentType = "text/plain";
        //    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    var result = new { name = file.FileName };

        //    HttpContext.Current.Response.Write(serializer.Serialize(result));
        //    HttpContext.Current.Response.StatusCode = 200;

        //    // For compatibility with IE's "done" event we need to return a result as well as setting the context.response
        //    return new HttpResponseMessage(HttpStatusCode.OK);
       
       
        //}
    }
}

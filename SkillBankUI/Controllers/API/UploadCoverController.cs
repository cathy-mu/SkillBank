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
using SkillBank.Site.Web.CDN;
using SkillBank.Site.Services;
using SkillBank.Site.DataSource.Data;

namespace MvcTesting.Controllers.WebApi
{
    public class UploadCoverController : ApiController
    {
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage UploadCover()
        {
            // Get a reference to the file that our jQuery sent.  Even with multiple files, they will all be their own request and be the 0 index
            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            //String corpSetting = String.Format("{0},{1},{2},{3}", HttpContext.Current.Request["imagefiletop"], HttpContext.Current.Request["imagefileleft"], HttpContext.Current.Request["imagefilew"], HttpContext.Current.Request["imagefileh"]);
            // String corpSetting/*"top,left,width,height"*/, 
            var classId = HttpContext.Current.Request["imagefilename"];
            var isPublish = HttpContext.Current.Request["ispublish"];

            //String fileName = String.Concat(classId, "_", DateTime.Now.ToString("yyMMddhhmmss"), HttpContext.Current.Request["imagefileext"]);
            String fileName = String.Concat(classId, HttpContext.Current.Request["imagefileext"]);//For image upload testing
            Stream fileStream = file.InputStream;

            var coverPath = String.Concat("/", System.Configuration.ConfigurationManager.AppSettings["ENV"], ConfigConstants.ThirdPartySetting.UpYun.ClassCoverPathPrefix, fileName);
            ImageUploadManager.UploadClassCoverImage(fileStream, coverPath, HttpContext.Current.Request["imagefilesetting"]);//corpSetting"100,120,238,178"
            ClassInfoRepository _rep = new ClassInfoRepository();
            //Save cover and publish class
            if (isPublish != null && isPublish.Equals("1"))
            {
                _rep.UpdateClassEditInfo((Byte)Enums.DBAccess.ClassSaveType.UpdateStep3, Convert.ToInt32(classId), coverPath);
            }
            //save cover only
            else
            {
                _rep.UpdateClassEditInfo((Byte)Enums.DBAccess.ClassSaveType.UpdatePhoto, Convert.ToInt32(classId), coverPath);
            }

            // Now we need to wire up a response so that the calling script understands what happened
            HttpContext.Current.Response.ContentType = "text/plain";
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //var result = new { name = file.FileName };

            //HttpContext.Current.Response.Write(serializer.Serialize(result));

            HttpContext.Current.Response.Write(serializer.Serialize("done"));
            HttpContext.Current.Response.StatusCode = 200;

            // For compatibility with IE's "done" event we need to return a result as well as setting the context.response
            return new HttpResponseMessage(HttpStatusCode.OK);
            //http://skillbank.b0.upaiyun.com/class/c_1.jpg
        }


        
    }
}

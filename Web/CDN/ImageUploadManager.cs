using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;

using SkillBank.Site.Common;

namespace SkillBank.Site.Web.CDN
{
    public static class ImageUploadManager
    {
        ///// <summary>
        ///// For testing image upload function in Funtion test
        ///// </summary>
        ///// <param name="uploadPath"></param>
        ///// <param name="imgPath"></param>
        ///// <param name="cropSetting">"left,top,width,height" : "100,100,180,180"</param>
        ///// <returns></returns>
        //public static Boolean UploadProfileImage(String uploadPath, String imgPath,String cropSetting="")
        //{
        //    Hashtable headers = new Hashtable();
        //    headers.Add("x-gmkerl-type", "fix_width");
        //    headers.Add("x-gmkerl-value", ConfigConstants.ThirdPartySetting.UpYun.AvatarImgSize["b"]);
        //    if (!String.IsNullOrEmpty(cropSetting))
        //    {
        //        headers.Add("x-gmkerl-crop", cropSetting);
        //    }

        //    return UploadImage(uploadPath, String.Concat(ConfigConstants.ThirdPartySetting.UpYun.AvatarPathPrefix, imgPath), headers);
        //}

        /// <summary>
        /// For upload images on page
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="imgPath"></param>
        /// <param name="cropSetting"></param>
        /// <returns></returns>
        public static Boolean UploadProfileImage(Stream fs, String imgPath, String cropSetting = "")
        {
            Hashtable headers = new Hashtable();
            headers.Add("x-gmkerl-type", "fix_width");
            headers.Add("x-gmkerl-value", ConfigConstants.ThirdPartySetting.UpYun.AvatarImgSize["h"]);
            if (!String.IsNullOrEmpty(cropSetting))
            {
                headers.Add("x-gmkerl-crop", cropSetting);
            }

            return UploadImage(fs, imgPath, headers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uploadPath"></param>
        /// <param name="imgPath"></param>
        /// <param name="cropSetting">"left,top,width,height" : "100,100,238,178"</param>
        /// <returns></returns>
        public static Boolean UploadClassCoverImage(Stream fs, String imgPath, String cropSetting = "")
        {
            Hashtable headers = new Hashtable();
            headers.Add("x-gmkerl-type", "fix_width");


            //TO DO:update for release 1410
            headers.Add("x-gmkerl-value", ConfigConstants.ThirdPartySetting.UpYun.ClassCoverSize["h"][0]);
            if (!String.IsNullOrEmpty(cropSetting))
            {
                headers.Add("x-gmkerl-crop", cropSetting);//"top,left,width,height"  
            }
            return UploadImage(fs, imgPath, headers);
        }

        private static Boolean UploadImage(String uploadPath, String imgPath, Hashtable headers)
        {
            UpYun upyun = new UpYun(ConfigConstants.ThirdPartySetting.UpYun.SpaceName, ConfigConstants.ThirdPartySetting.UpYun.UserName, ConfigConstants.ThirdPartySetting.UpYun.Password);

            FileStream fs = new FileStream(uploadPath, FileMode.Open, FileAccess.Read);

            BinaryReader r = new BinaryReader(fs);
            byte[] postArray = r.ReadBytes((int)fs.Length);

            upyun.setContentMD5(UpYun.md5_file(uploadPath));
            bool b = upyun.writeFile(imgPath, postArray, true, headers);
            return b;
        }

        private static Boolean UploadImage(Stream fs, String imgPath, Hashtable headers)
        {
            UpYun upyun = new UpYun(ConfigConstants.ThirdPartySetting.UpYun.SpaceName, ConfigConstants.ThirdPartySetting.UpYun.UserName, ConfigConstants.ThirdPartySetting.UpYun.Password);

           // FileStream fs = new FileStream(uploadPath, FileMode.Open, FileAccess.Read);

            BinaryReader r = new BinaryReader(fs);

            byte[] postArray = r.ReadBytes((int)fs.Length);

            upyun.setContentMD5(UpYun.md5_file(postArray));

            bool b = upyun.writeFile(imgPath, postArray, true, headers);
            return b;
        }

        private static Boolean DeleteImage(String imgPath)
        {
            UpYun upyun = new UpYun(ConfigConstants.ThirdPartySetting.UpYun.SpaceName, ConfigConstants.ThirdPartySetting.UpYun.UserName, ConfigConstants.ThirdPartySetting.UpYun.Password);

            bool b = upyun.deleteFile(imgPath);
            return b;
        }

        
    }
}

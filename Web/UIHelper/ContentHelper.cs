using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services.CacheProviders;
using SkillBank.Site.Services.Managers;
using SkillBank.Site.Web.Context;
using System.Configuration;


namespace SkillBank.Site.Web
{
    public static class ContentHelper
    {
        //TO DO10:Change to service later
        private static readonly IBlurbsProvider _blurbsProvider;
        private static readonly ICityLkpProvider _cityProvider;

        static ContentHelper()
        {
            _blurbsProvider = new BlurbsProvider(new BlurbsRepository());
            _cityProvider = new CityLkpProvider(new LookupsRepository());
        }

        ///// <summary>
        ///// Get Text resource
        ///// </summary>
        ///// <param name="htmlHelper"></param>
        ///// <param name="BlurbId"></param>
        ///// <returns></returns>
        //public static String GetTrans(int BlurbId, Byte siteVersion)
        //{
        //    //LanguageCode is get from Context, siteversion get from webconfig
        //    String languageCode = WebContext.Current.LanguageCode;
        //    //Should show blurb id on page
        //    Boolean showBlurbId = WebContext.Current.ShowLL.Equals("y", StringComparison.OrdinalIgnoreCase);

        //    var blurbText = _blurbsProvider.GetBlurb(BlurbId, languageCode, siteVersion);
        //    blurbText = showBlurbId ? blurbText + "[" + BlurbId + "]" : blurbText;

        //    return blurbText;
        //}


        // <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="sizeType"></param>
        /// <returns></returns>
        public static String GetAvatarPath(string path, string size, byte socialType = 0)
        {
            String imgPath;
            if (String.IsNullOrEmpty(path))
                return "";

            if (path.Contains("http"))
            {
                if (socialType.Equals(1) || path.Contains("sina"))
                {
                    switch (size)
                    {
                        case "s":
                            path = path.Replace("/180/", "/30/");
                            break;
                        case "m":
                            path = path.Replace("/180/", "/50/");
                            break;
                       default://case b, case h
                            break;
                    }
                }
                else if (socialType.Equals(3) || path.Contains("qlogo"))
                {
                    switch (size)
                    {
                        case "s":
                            path = path + "/30";
                            break;
                        case "m":
                            path = path + "/50";
                            break;
                        default:
                            path = path + "/180";
                            break;
                    }
                }
                imgPath = path;
            }
            else if (ConfigConstants.ThirdPartySetting.UpYun.AvatarImgSize.ContainsKey(size))
            {
                imgPath = String.Format("{0}{1}!{2}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, path, ConfigConstants.ThirdPartySetting.UpYun.AvatarImgSize[size]);
            }
            else
            {
                imgPath = String.Format("{0}{1}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, path);
            }

            return imgPath;
        }

       
        public static String GetClassCoverPath(string imgPath, string sizeType)
        {
            if (String.IsNullOrEmpty(imgPath))
            {
                return Constants.DefaultSetting.ClassCoverImg;
            }
            
            if (ConfigConstants.ThirdPartySetting.UpYun.ClassCoverSize.ContainsKey(sizeType))
            {
                imgPath = String.Format("{0}{1}!{2}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, imgPath, ConfigConstants.ThirdPartySetting.UpYun.ClassCoverSize[sizeType][0]);
            }
            else
            {
                imgPath = String.Format("{0}{1}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, imgPath);
            }

            return imgPath;
        }

        public static String GetPrivateInfo(string info, int showLen, bool hidePrefix = true)
        {
            string displayInfo = "";
            int len = String.IsNullOrEmpty(info) ? 0 : info.Length;
            if (len > showLen)
            {
                int startIdx = len - showLen;
                if (hidePrefix)
                {
                    displayInfo = info.Substring(startIdx, showLen);
                    displayInfo = displayInfo.PadLeft(len, '*');
                }
                else
                {
                    displayInfo = info.Substring(0, showLen);
                    displayInfo = displayInfo.PadRight(len, '*');
                }
            }
            return displayInfo;
        }




    }
}
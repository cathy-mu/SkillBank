using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

using io.rong;
using SkillBank.Site.Common;
using Newtonsoft.Json;

namespace SkillBank.Site.Services.Utility
{
    public class RongCloudHelper
    {

        public RongCloudHelper()
        {}


        public class GetTokenResult
        {
            public int code { get; set; }
            public String userId { get; set; }
            public String token { get; set; }
        }

        public class RefreshUserInfoResult
        {
            public int code { get; set; }
        }

        //public class RCUser
        //{
        //    public String id { get; set; }
        //    public String name { get; set; }
        //    public String icon { get; set; }
        //}

        public class RCContent
        {
            public String content { get; set; }
        }



        #region Public method

        public static String GetToken(String envName, int id, string name = Constants.DefaultSetting.UserName, string avatar = "")
        {
            avatar = ResetAvatarUrl(avatar);


            String rcToken = "";
            string tokenResult = RongCloudServer.GetToken(GetRongCloudAppKey(envName), GetRongCloudAppSecret(envName), id.ToString(), name, avatar);
            GetTokenResult result = JsonConvert.DeserializeObject<GetTokenResult>(tokenResult);
            if (result != null && result.code.Equals(200))
            {
                rcToken = result.token;
            }
            return rcToken;
        }

        public static Boolean RefreshUserInfo(String envName, int id, string name = Constants.DefaultSetting.UserName, string avatar = "")
        {
            avatar = ResetAvatarUrl(avatar);
            string refreshResult = RongCloudServer.RefreshUserInfo(GetRongCloudAppKey(envName), GetRongCloudAppSecret(envName), id.ToString(), name, avatar);
            RefreshUserInfoResult result = JsonConvert.DeserializeObject<RefreshUserInfoResult>(refreshResult);
            if (result != null && result.code.Equals(200))
            {
                return true;
            }
            return false;
         }
        
        public static Boolean PushMessage(String envName, String fromId, String toId, String message)
        {
            RCContent contentObj = new RCContent() { content = message};
            String contentData = JsonConvert.SerializeObject(contentObj);
            string refreshResult = RongCloudServer.PublishMessage(GetRongCloudAppKey(envName), GetRongCloudAppSecret(envName), fromId, toId, ConfigConstants.ThirdPartySetting.RongCloudSetting.MessageTypeText, contentData);
            RefreshUserInfoResult result = JsonConvert.DeserializeObject<RefreshUserInfoResult>(refreshResult);
            if (result != null && result.code.Equals(200))
            {
                return true;
            }
            return false;
        }

        #endregion

        


        #region Non-Public method

        private static String ResetAvatarUrl(String avatar)
        {
            if (String.IsNullOrEmpty(avatar))
            {
                avatar = String.Concat(ConfigConstants.EnvSetting.SiteDomain[ConfigConstants.EnvSetting.LiveEnvName], Constants.DefaultSetting.AvaterImg);
            }
            else if (!avatar.ToLower().StartsWith("http"))
            {
                avatar = String.Format("{0}{1}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, avatar);
            }
            return avatar;
        }

        private static String GetRongCloudAppKey(String envName)
        {
            if (ConfigConstants.EnvSetting.LiveEnvName.Equals(envName))
            {
                return ConfigConstants.ThirdPartySetting.RongCloudSetting.AppKey[envName];
            }
            else
            {
                return ConfigConstants.ThirdPartySetting.RongCloudSetting.AppKey[ConfigConstants.EnvSetting.Web1EnvName];
            }
        }

        private static String GetRongCloudAppSecret(String envName)
        {
            if (ConfigConstants.EnvSetting.LiveEnvName.Equals(envName))
            {
                return ConfigConstants.ThirdPartySetting.RongCloudSetting.AppSecret[envName];
            }
            else
            {
                return ConfigConstants.ThirdPartySetting.RongCloudSetting.AppSecret[ConfigConstants.EnvSetting.Web1EnvName];
            }
        }

        #endregion

    }


}


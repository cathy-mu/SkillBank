using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

using RongCloudServerSDK.rong;
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

        
        public static String GetToken(int id, string name = "User", string avatar = "http://skillbank.b0.upaiyun.com/0/profile/m_1.jpg")
        {
            if (!avatar.ToLower().StartsWith("http"))
            {
                avatar = String.Format("{0}{1}", ConfigConstants.ThirdPartySetting.UpYun.SpaceHost, avatar);
            }
            
            String rcToken = "";
            string tokenResult = RongCloudServer.GetToken(ConfigConstants.ThirdPartySetting.RongCloudSetting.AppKey, ConfigConstants.ThirdPartySetting.RongCloudSetting.AppSecret, id.ToString(), name, avatar);
            //string tokenResult = "{\"code\":200,\"userId\":\"1\",\"token\":\"gVHKgV3tP3132Lm5G2ZGVMp/R7nLdwyeVE5OI2q+Zh+JlVqcReQFwlOr92eUFvcJ2JRgqlNj6ADOfRelAAwEBA==\"}";
            GetTokenResult result = JsonConvert.DeserializeObject<GetTokenResult>(tokenResult);
            if (result!=null && result.code.Equals(200))
            {
                rcToken = result.token;
            }
            return rcToken;
        }
    }


}


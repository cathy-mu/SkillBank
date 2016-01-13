using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBank.Site.Common
{
    public static class ConfigConstants
    {
        public static class EnvSetting
        {
            public const Boolean IsTesting = true;
            public const String LiveEnvName = "0";
            public const String Web1EnvName = "1";
            public const String LocalEnvName = "2";

            public static readonly Dictionary<String, String> MobileHome = new Dictionary<String, String>() 
            { 
                { "0", "http://m.skillbank.cn/m" }, 
                { "1", "http://m.skill-bank.com/m"}, 
                { "2", "http://localhost/m" }
            };

            public static readonly Dictionary<String, String> SiteHome = new Dictionary<String, String>() 
            { 
                { "0", "http://www.skillbank.cn" }, 
                { "1", "http://www.skillbank.cn" }, 
                { "2", "http://localhost" }
            };
        }

            
        public static class MailSetting
        {
            //String fromAddress = "cathy.mu@qq.com";
            //String smtpServer = "smtp.qq.com";
            //int smtpPort = 465;

            //String fromAddress = "cathy_mu@126.com";
            //String smtpServer = "smtp.126.com";
            //int smtpPort = 587;
            //String fromPassword = "2011mail";

            //public const String FromAddress = "postmaster@skillbank.sendcloud.org";
            //public const String SenderName = "Skill Bank";// TO DO : change to blurb later
            //public const String Password = "WrBzxjT1";
            //public const String ServerHost = "smtpcloud.sohu.com";
            //public const int ServerPort = 25;

            public const String FromAddress = "contact@skill-bank.com";
            public const String SenderName = "Skill Bank";// TO DO : change to blurb later
            public const String Password = "21qw!@QW";
            public const String ServerHost = "smtp.skill-bank.com";
            public const int ServerPort = 25;

            public const String ToAddress4Testing1 = "elaine11@163.com";
            public const String ToAddress4Testing2 = "cathy.mu@qq.com";
            public const String ToAddress4Testing3 = "cathy_mu@126.com";
            public const String ToAddress4Testing4 = "cathy.mu@hotmail.com";
            public const String ToAddress4Testing5 = "1964367060@qq.com";
        }

        public static class SendCloudMailSetting
        {

            public const String API_User = "postmaster@skillbank.sendcloud.org";
            public const String API_ListUser = "postmaster@skill-bank.sendcloud.org";
            public const String API_Key = "FdflUbRKok8D3vkA";
            public const String Sender_Address = "noreply@skill-bank.com";
            public const String Sender_Name = "技能银行";
            
            public const String Welcome_TemplateName = "Welcome_Mail";
            public const String Welcome_EmailSubject = "欢迎加入技能银行，开始你的分享之旅";
            public const String ProveClass_TemplateName = "class_accept";
            public const String ProveClass_EmailSubject = "恭喜，你的课程已经通过审核啦";
            public const String RejectClass_TemplateName = "class_reject";
            public const String RejectClass_EmailSubject = "很抱歉，你的课程没能通过审核";
            public const String MessageReceive_TemplateName = "new_message";
            public const String MessageReceive_EmailSubject = "你有新私信！";
            public const String OrderStatusChanged_TemplateName = "class_messagenew";//class_message
            public const String OrderStatusChanged_EmailSubject = "你有新的课程消息！";
        }

        public static class NotificationTextSetting
        {
            public static readonly Dictionary<Byte, String> InteractionFormat = new Dictionary<Byte, String>() 
            { 
                { 6, "在你的课程《 {0} 》上留言" },//class title 
                { 7, "评价了你教授的课程《 {0} 》" }, //class title 
                { 8, "评价了你学习的课程《 {0} 》" }, //class title 
                { 9, "喜欢了你的课程《 {0} 》" }, //class title 
                { 30, "关注了你" }//class title 
            };

        }

        public static class ThirdPartySetting
        {
            public static class DBMap
            {
                public const String AK = "RZG8k5YBPYBRvU64kWTVLDsK";
                public const String OutputType = "json";
                public const int PageSize = 10;
                public const int PageNo = 1;
                public const String Scope = "1";//Base info
                public const String SearchPlaceQuery = "http://api.map.baidu.com/place/v2/search?ak={0}&output={1}&query={2}&page_size={3}&page_num={4}&scope={5}&region={6}";
            }

            public static class UpYun
            {
                public const String SpaceHost = "http://skillbank.b0.upaiyun.com";
                public const String SpaceName = "skillbank";
                public const String UserName = "cathy";
                public const String Password = "12qw!@QW";
                public const String AvatarPathPrefix = "/profile/m_";
                public const String ClassCoverPathPrefix = "/class/c_";
                public const String FromAPIHost = "http://v0.api.upyun.com/skillbank";
                public const String FromAPI = "2dv+g6Driz0BjAKKdkm3UPy2vlw=";
                public static readonly Dictionary<String, int> AvatarImgSize = new Dictionary<string, int> { 
                { "s", 30 }, 
                { "m", 40 }, 
                { "b", 100 }, 
                { "h", 180 }, 
                { "ms", 64 },
                { "mm", 100 },
                { "mb", 150 }
                };
                public static readonly Dictionary<String, int[]> ClassCoverSize = new Dictionary<String, int[]>() { 
                { "s", new int[] { 100, 75 } }, 
                { "m", new int[] { 220, 165 } }, 
                { "b", new int[] { 240, 180 } }, 
                { "h", new int[] { 320, 240 } }, 
                { "ms", new int[] { 520, 390 } }, //moblie member class
                { "mm", new int[] { 600, 450 } }, //moblie class list
                { "mb", new int[] { 640, 480 } } //moblie class detail
                };
            }

            public static class SocialNetwork
            {
                public static readonly Dictionary<Byte, String> SkillBankAccountName = new Dictionary<Byte, String> { { 1, "@技能银行微博 " }, { 2, "@技能银行 " }, { 3, "@skill-bank " }, { 4, "" } };

                public static readonly String QQ_APPKey = "801504346";
                public static readonly String QQ_APPSecret = "7f937514830a46ce132a30aa3253073d";
                
                public static readonly String Sina_APPKey = "111240964";
                public static readonly String Sina_APPSecret = "3a9bd6965729abbb3b048a6dfd4670e2";
                
            }
            
            public static class WeChatSetting
            {
                public const String Token = "skillbank2015";
                public const String AppID = "wxa10c0a8a413081b6";
                public const String Secret = "aa43e002d3f7b6d583ac9f4019cbb7bc";
                public const String EncodingAESKey = "wjxKHVotmZCB89vuCAWt0CBOAEah0Yi0Xo2UMWvNfVF";
            }


            public static class RongCloudSetting
            {
                public const String AppKey = "vnroth0kr9rjo";
                public const String AppSecret = "Ak3CNPqQJOZKtp";
            }

        }


    }
}
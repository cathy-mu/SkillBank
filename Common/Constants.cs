using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBank.Site.Common
{
    public static class Constants
    {

        public class Setting
        {
            public const Char CacheKeySpliter = '_';
            public const Char DBDataDelimiter = ',';
            public const int CacheTimeOut_LLMins = 600;//10h*60min
            public const int CacheTimeOut_MailTemplateMins = 600;//10h*60min
            public const int CacheTimeOut_CategoryMins = 1440;//24h*60min
            public const int CacheTimeOut_CategoryTagMins = 300;//5h*60min
            public const int CacheTimeOut_CityInfoMins = 60;//1h*60min
            public const int CacheTimeOut_MetaTagMins = 1440;//24h*60min;

            public const int CacheTimeOut_TopBannerMins = 2880;//48h*60min
            public const int CacheTimeOut_PortalBannerMins = 2880;//48h*60min
        }

        public class CacheItemKey
        {
            public const String RecommendClassListPrefix = "cr";
            public const String ClassListPrefix = "cl";
        }

        public class CacheTimeOut
        {
            public const int RecommendClassMins = 60;//1h*60min
            public const int ClassListWebMins = 30;//0.5h*60min
            public const int ClassListNumsMins = 30;//0.5h*60min
            public const int MemberFavoriteDicMins = 30;//0.5h*60min
        }

        public class CookieName
        {
            public const String ShowLL = "ShowLL";
            public const String MemberId = "MemberId";
        }

        public static class LookUps
        {
            public static readonly List<String> MarketLkp = new List<String>() { "cn" };
            public static readonly List<String> LanguageLkp = new List<String>() { "cs" };
            public static readonly List<Enums.MemberType> MemberTypeLkp = new List<Enums.MemberType>() { Enums.MemberType.Visiter, Enums.MemberType.Register, Enums.MemberType.ClassOwner, Enums.MemberType.Member };
        }

        public static class QueryStringKeys
        {
            public const String Market = "ctr";
            public const String Language = "lng";
            public const String Etag = "etag";
            public const String DebugLL = "debugll";
            public const String MemberType = "mtype";
        }
        
        public static class DefaultSetting
        {   //TO DO:1.0 Change memeber id back after testing
            public const String MarketCode = "cn";
            public const String LanguageCode = "cs"; 
            public const int MemberId = 0;//if member code not exists,not login 
            public const Enums.MemberType MemberType = Enums.MemberType.Visiter;
            public const Boolean ShowBlurbId = true;
            public const Byte SocialType = 0;

            public const String ClassCoverImg = "/img/default_cover.png";
            public const String AvaterImg =  "/img/icons/icon-17.png";//"/mock/avatar-2-180.jpg";
            public const String UserName = "用户";
        }

        public static class CookieKeys
        {
            public const String Market = "ctr";
            public const String Language = "lng";
            public const String Etag = "etag";
            public const String MemberType = "mtype";
            public const String MemberId = "mid";
            public const String ServerName = "svr";
            public const String DebugLL = "debugll";
            public const String SocialType = "stype";
            public const String SocialId = "sid";
            public const String OpenId = "soid";
            //public const String MemberName = "mname";
            //public const String MemberAvatar = "mavatar";
            public const String OrderHandleDate = "ohdate";
            public const String SocialAccessInfo = "sai";
            public const String ClassListCity = "clcity";//0 for all city
            public const String ClassListCategory = "clcate";//0 for all cate
            public const String ClassListOrder = "clorder";//orderby and isasc  

        }


        public static class CacheDicKeys
        {
            public const String WeChatAccessToken = "we_access_token";
            public const String WeChatJsapiTicket = "we_jsapi_ticket";
        }

        public static class BizConfig
        {
            public const int ConfirmTimeOutMins = 1440;//24*60min
            public const int RefundimeOutMins = 2880;//48*60min
            public const string SingleLocalCode = "CN";
            public const int ClassFinishedTag = 15;//4Step ClassManager.CheckClassSteps: 1+2+4+8;
            public const int AdminMemberId = 59;
            public const string OverwriteEtagPrefix = "";//"Promote_Ref_Oct";
            public const int FreeCoinForShareClass = 2;
            public const int FreeCoinForRegister = 1;
        }

        public static class BizLogic
        {
            //public const Dictionary<int, List<int>> OrderStatusDic = new Dictionary<int, List<int>>() { { 1,{1,2} } };
            //public const Dictionary<int, string> OrderStatusDic = new Dictionary<int, string>() { { 1, "1,2" }, { 2, "1,2" } };
            //1-->2,3,4
            //3-->5,6
            //6-->7,8
            //n member not check coin
            //1 class check student coins(<3 , 3 book for same class)
            //1 class check student coins(>=3 , student coins for times for same class booking)
        }

        public static class ContentPageSize
        {
            public const int DashboardMessage = 4;
        }

        public static class PageURL
        {
            public const String MessagePage = "http://www.skillbank.cn/message";
            public const String MobileClassPage = "http://m.skillbank.cn/m/course/";
            public const String MobilePersonalPage = "http://m.skillbank.cn/m/personal/";
            public const String MobileTeachPage = "http://m.skillbank.cn/m/teaching";
            public const String MobileLearnPage = "http://m.skillbank.cn/m/learning";
            public const String MobileMyCoursePage = "http://m.skillbank.cn/m/mycourses";
            public const String MobileMessagePage = "http://m.skillbank.cn/m/message";
            public const String WebDashboard = "http://www.skillbank.cn/member";
            public const String MobileDashboard = "http://m.skillbank.cn/m/dashboard";
            public const String MobileInteractionPage = "http://m.skillbank.cn/m/message?tabid=1";
        }

        public static class ValidationExpressions
        {
            public const String Mobile = @"^[1]+\d{10}";
            public const String ValidationCode = @"^\d{6}$";
            public const String Password = @"^[\w!@#$%\^&\*\(\)_]{6,16}$";
        }

        public static class ContentSiteVersion
        {
            public const String CityCategoryLookup = "15110101";
            public const String HowToGetCoins = "15110101";
        }

        public static class APIStatusCode
        {
            public const Int16 Success = 200;
            public const Int16 NotFound = 404;
            public const Int16 InvalidPass = 403;
            public const Int16 InvalidMobileAccount = 406;
        }
                        

    }
    
}

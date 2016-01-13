using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Web.UI;
using System.Text;
using System.Web.Security;


using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services.Managers;
using dotNetDR_OAuth2.APIs.WeChat;
using SkillBank.Site.Services.Tencent;
using SkillBank.Site.Services.Providers;
using SkillBank.Site.Common;
using SkillBank.Site.Services.Utility;

using SkillBank.Site.Services;
using SkillBank.Site.Web;
using SkillBank.Site.Web.Context;


namespace SkillBankWeb.Controllers
{
    public class WeChatController : Controller
    {
        public readonly IContentService _contentService;
        public readonly ICommonService _commonService;

        public WeChatController(IContentService contentService, ICommonService commonService)
        {
            _contentService = contentService;
            _commonService = commonService;
        }

        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get()
        {
            //验证签名
            string echoStr = Request.QueryString["echoStr"].ToString();
            //if (CheckSignature())
            //{
                if (!string.IsNullOrEmpty(echoStr))
                {
                    Response.Write(echoStr);
                    Response.End();
                }
            //}
            return View();
        }


        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post()
        {
            using (Stream stream = Request.InputStream)
            {
                var safeMode = (Request.QueryString.Get("encrypt_type") == "aes");
                if (safeMode)
                {
                    var decryptMsg = string.Empty;
                    var streamReader = new StreamReader(stream);
                    var msg = streamReader.ReadToEnd();
                    
                    var msg_signature = Request.QueryString.Get("msg_signature");
                    String timestamp = Request.QueryString.Get("timestamp");
                    String nonce = Request.QueryString.Get("nonce");
                    string sToken = ConfigConstants.ThirdPartySetting.WeChatSetting.Token;
                    string sAppID = ConfigConstants.ThirdPartySetting.WeChatSetting.AppID;
                    string sEncodingAESKey = ConfigConstants.ThirdPartySetting.WeChatSetting.EncodingAESKey;
                    WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);

                    var ret = wxcpt.DecryptMsg(msg_signature, timestamp, nonce, msg, ref decryptMsg);

                    WeChatBaseMessage wechatMessage = XMLHelper.Deserialize<WeChatBaseMessage>(decryptMsg);
                    ResponseMsg(wechatMessage);
                }
                else
                {
                    WeChatBaseMessage wechatMessage = XMLHelper.Deserialize<WeChatBaseMessage>(stream);
                    ResponseMsg(wechatMessage);
                }
            }

            return View();
        }


        public void ResponseMsg(WeChatBaseMessage message)
        {
            Boolean shouldResponse = false;
            var safeMode = (Request.QueryString.Get("encrypt_type") == "aes");
            String timestamp = Request.QueryString.Get("timestamp");
            String nonce = Request.QueryString.Get("nonce");
            
            WeChatBaseMessage resMessage = new WeChatBaseMessage();
            resMessage.Content = "";
                
            if (!string.IsNullOrEmpty(message.Event) && message.Event.Trim().ToLower() == "subscribe")
            {
                if (!message.EventKey.Contains("qrscene_"))
                {
                    resMessage.Content = "技能银行 SkillBank － 用技能拉近彼此的距离！\n";
                    resMessage.Content += "欢迎关注技能银行服务号！\n";
                    resMessage.Content += "我们会每周推送平台上好玩的新技能给你，点击右下角菜单 “进入平台” 就能访问我们";
                    shouldResponse = true;
                }
                else
                {
                    resMessage.Content = GetMessageResponse(message);
                    shouldResponse = true;
                }
            }
            else if (!string.IsNullOrEmpty(message.Event) && message.Event.Trim().ToLower() == "scan")
            {
                resMessage.Content = GetMessageResponse(message);
                shouldResponse = true;
            }
            else if (!string.IsNullOrEmpty(message.Event) && message.Event.Trim().ToLower() == "click")
            {
                if (message.EventKey == "menu_contactus")
                {
                    resMessage.Content = "商务合作或建议反馈： contact@skill-bank.com\n";
                    resMessage.Content += "加入我们： recruiter@skill-bank.com";
                    shouldResponse = true;
                }
            }
            if (shouldResponse)
            {
                resMessage.ToUserName = message.FromUserName;
                resMessage.FromUserName = message.ToUserName;
                resMessage.CreateTime = (int)DateTime.Now.Ticks;
                resMessage.MsgType = "text";

                String res = XMLHelper.Serializer<WeChatBaseMessage>(resMessage);

                //just test
                string sToken = ConfigConstants.ThirdPartySetting.WeChatSetting.Token;
                string sAppID = ConfigConstants.ThirdPartySetting.WeChatSetting.AppID;
                string sEncodingAESKey = ConfigConstants.ThirdPartySetting.WeChatSetting.EncodingAESKey;
                WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
                String sEncryptMsg = ""; //xml格式的密文
                int ret = wxcpt.EncryptMsg(res, timestamp, nonce, ref sEncryptMsg);

                Response.ContentType = "text/xml";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Write(safeMode ? sEncryptMsg : res);
                Response.End();
            }
            else
            {
                Response.ContentType = "text/xml";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Write("");
                Response.End();
            }
        }


        public ActionResult ResponseMsg(String sReqData)
        {
            //公众平台上开发者设置的token, appID, EncodingAESKey
            string sToken = ConfigConstants.ThirdPartySetting.WeChatSetting.Token;
            string sAppID = ConfigConstants.ThirdPartySetting.WeChatSetting.AppID;
            //String sSecret = ConfigConstants.ThirdPartySetting.WeChatSetting.Secret;
            string sEncodingAESKey = ConfigConstants.ThirdPartySetting.WeChatSetting.EncodingAESKey;

            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);

            /* 1. 对用户回复的数据进行解密。
            * 用户回复消息或者点击事件响应时，企业会收到回调消息，假设企业收到的推送消息：
            * 	POST /cgi-bin/wxpush? msg_signature=477715d11cdb4164915debcba66cb864d751f3e6&timestamp=1409659813&nonce=1372623149 HTTP/1.1
               Host: qy.weixin.qq.com
               Content-Length: 613
            *
            * 	<xml>
                   <ToUserName><![CDATA[wx5823bf96d3bd56c7]]></ToUserName>
                   <Encrypt><![CDATA[RypEvHKD8QQKFhvQ6QleEB4J58tiPdvo+rtK1I9qca6aM/wvqnLSV5zEPeusUiX5L5X/0lWfrf0QADHHhGd3QczcdCUpj911L3vg3W/sYYvuJTs3TUUkSUXxaccAS0qhxchrRYt66wiSpGLYL42aM6A8dTT+6k4aSknmPj48kzJs8qLjvd4Xgpue06DOdnLxAUHzM6+kDZ+HMZfJYuR+LtwGc2hgf5gsijff0ekUNXZiqATP7PF5mZxZ3Izoun1s4zG4LUMnvw2r+KqCKIw+3IQH03v+BCA9nMELNqbSf6tiWSrXJB3LAVGUcallcrw8V2t9EL4EhzJWrQUax5wLVMNS0+rUPA3k22Ncx4XXZS9o0MBH27Bo6BpNelZpS+/uh9KsNlY6bHCmJU9p8g7m3fVKn28H3KDYA5Pl/T8Z1ptDAVe0lXdQ2YoyyH2uyPIGHBZZIs2pDBS8R07+qN+E7Q==]]></Encrypt>
                </xml>
            */
            String sReqTimeStamp = WeChatHelper.GetTimestamp().ToString();
            String sReqNonce = WeChatHelper.GetNonceStr();
            String CurrUrl = Request.Url.AbsoluteUri;
            String jsAPITicket = GetJsAPITicket(GetAccessToken());

            String str4SHA1 = WeChatHelper.GenerateString4Signature(jsAPITicket, sReqNonce, sReqTimeStamp, CurrUrl);
            String sReqMsgSig = FormsAuthentication.HashPasswordForStoringInConfigFile(str4SHA1, "SHA1").ToLower();

            //string sReqData = "<xml><ToUserName><![CDATA[wx5823bf96d3bd56c7]]></ToUserName><Encrypt><![CDATA[RypEvHKD8QQKFhvQ6QleEB4J58tiPdvo+rtK1I9qca6aM/wvqnLSV5zEPeusUiX5L5X/0lWfrf0QADHHhGd3QczcdCUpj911L3vg3W/sYYvuJTs3TUUkSUXxaccAS0qhxchrRYt66wiSpGLYL42aM6A8dTT+6k4aSknmPj48kzJs8qLjvd4Xgpue06DOdnLxAUHzM6+kDZ+HMZfJYuR+LtwGc2hgf5gsijff0ekUNXZiqATP7PF5mZxZ3Izoun1s4zG4LUMnvw2r+KqCKIw+3IQH03v+BCA9nMELNqbSf6tiWSrXJB3LAVGUcallcrw8V2t9EL4EhzJWrQUax5wLVMNS0+rUPA3k22Ncx4XXZS9o0MBH27Bo6BpNelZpS+/uh9KsNlY6bHCmJU9p8g7m3fVKn28H3KDYA5Pl/T8Z1ptDAVe0lXdQ2YoyyH2uyPIGHBZZIs2pDBS8R07+qN+E7Q==]]></Encrypt></xml>";
            string sMsg = "";  //解析之后的明文
            int ret = 0;
            ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);
            if (ret != 0)
            {
                System.Console.WriteLine("ERR: Decrypt fail, ret: " + ret);
                return View();
            }
            //System.Console.WriteLine(sMsg);
            //Response.Write(sMsg);

            /*
             * 2. 企业回复用户消息也需要加密和拼接xml字符串。
             * 假设企业需要回复用户的消息为：
             * 		<xml>
             * 		<ToUserName><![CDATA[mycreate]]></ToUserName>
             * 		<FromUserName><![CDATA[wx5823bf96d3bd56c7]]></FromUserName>
             * 		<CreateTime>1348831860</CreateTime>
                    <MsgType><![CDATA[text]]></MsgType>
             *      <Content><![CDATA[this is a test]]></Content>
             *      <MsgId>1234567890123456</MsgId>
             *      </xml>
             * 生成xml格式的加密消息过程为：
             */
            WeChatBaseMessage wechatMessage = XMLHelper.Deserialize<WeChatBaseMessage>(sMsg);

            string sRespData = "<xml><ToUserName><![CDATA[" + wechatMessage.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + wechatMessage.ToUserName + "]]></FromUserName><CreateTime>1348831860</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[欢迎关注技能银行服务号]]></Content></xml>";
            string sEncryptMsg = ""; //xml格式的密文
            ret = wxcpt.EncryptMsg(sRespData, sReqTimeStamp, sReqNonce, ref sEncryptMsg);
            //System.Console.WriteLine(sEncryptMsg);

            Response.ContentType = "text/xml";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(ret);
            Response.End();

            /*测试：
             * 将sEncryptMsg解密看看是否是原文
             * */
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(sEncryptMsg);
            //XmlNode root = doc.FirstChild;
            //string sig = root["MsgSignature"].InnerText;
            //string enc = root["Encrypt"].InnerText;
            //string timestamp = root["TimeStamp"].InnerText;
            //string nonce = root["Nonce"].InnerText;
            //string stmp = "";
            //ret = wxcpt.DecryptMsg(sig, timestamp, nonce, sEncryptMsg, ref stmp);
            //System.Console.WriteLine("stemp");
            //System.Console.WriteLine(stmp + ret);

            return View();
        }

        /// <summary>
        /// Cache provider test
        /// </summary>
        /// <returns></returns>
        public ActionResult Test()
        {
            String accessTonken = GetAccessToken();
            //String news = WeChatHelper.GetMediaList(accessTonken);

            ViewBag.Test = accessTonken;// +"  :  " + news;
            //String type = "news";
            //int offset = 0;
            //int count = 20;
            ////ViewBag.Test = string.Format("{\"type\": \"{0}\" \"offset\": {1}, \"count\":{2}}", type, offset, count);
            ////ViewBag.Test =  string.Format("{\"type\": {0}, \"offset\": {1}, \"count\":{2}}", type, offset, type);
            //ViewBag.Test = "{\"type\": \"" + type + "\", \"offset\":"+offset.ToString()+", \"count\":"+count+"}";
            
            return View();
        }

        #region Non-Action method

        private bool CheckSignature()
        {
            string sToken = ConfigConstants.ThirdPartySetting.WeChatSetting.Token;
            string signature = Request.QueryString["signature"].ToString();
            string timestamp = Request.QueryString["timestamp"].ToString();
            string nonce = Request.QueryString["nonce"].ToString();
            string[] ArrTmp = { sToken, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序  
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetAccessToken()
        {

            //String token = null;
            //ConfigureInfo configInfo = _configureManager.GetConfigureByKey("wechat_token");
            //if (configInfo.ExpireDate < DateTime.Now)
            //{
            //    DateTime expireDate = new DateTime();
            //    token = WeChatHelper.GetToken(out expireDate);
            //    _configureManager.UpdateConfigureInfo("wechat_token", token, expireDate);
            //}
            //else
            //{
            //    token = configInfo.InfoValue;
            //}

            SocialCacheItem cachedToken;
            var accessToken = StaticCacheProvider.GetObject(Constants.CacheDicKeys.WeChatAccessToken);
            if (accessToken == null)
            {
                DateTime expireDate = new DateTime();
                String token = WeChatHelper.GetToken(out expireDate);
                cachedToken = new SocialCacheItem() { ItemValue = token, ExpireDate = expireDate };
                StaticCacheProvider.SetObject(Constants.CacheDicKeys.WeChatAccessToken, cachedToken);
                return token;
            }
            else
            {
                cachedToken = (SocialCacheItem)StaticCacheProvider.GetObject(Constants.CacheDicKeys.WeChatAccessToken);
                return cachedToken.ItemValue;
            }
        }

        private string GetJsAPITicket(String accessToken)
        {
            SocialCacheItem cachedTicket;
            var apiTicket = StaticCacheProvider.GetObject(Constants.CacheDicKeys.WeChatJsapiTicket);
            if (apiTicket == null)
            {
                DateTime expireDate = new DateTime();
                String ticket = WeChatHelper.GetJsAPITicket(accessToken, out expireDate);
                cachedTicket = new SocialCacheItem() { ItemValue = ticket, ExpireDate = expireDate };
                StaticCacheProvider.SetObject(Constants.CacheDicKeys.WeChatJsapiTicket, cachedTicket);
                return ticket;
            }
            else
            {
                cachedTicket = (SocialCacheItem)StaticCacheProvider.GetObject(Constants.CacheDicKeys.WeChatJsapiTicket);
                return cachedTicket.ItemValue;
            }
        }

        private string GetMessageResponse(WeChatBaseMessage message)
        {
            String content = default(String);
            String scenceId = message.EventKey.Replace("qrscene_", "");
            if (!String.IsNullOrEmpty(message.Ticket))
            {
                var result = _commonService.SaveWeChatEvent((Byte)Enums.DBAccess.WeChatEventSaveType.UpdateEvent, 0, message.FromUserName, scenceId, message.Ticket);
                if (result.Equals(3))
                {
                    content = "很抱歉，你的微信账号已经在技能银行注册过，无法绑定你的微博账号！\n";
                    content += "如果想绑定两个账号，请在服务号里留言给我们。\n";
                }
                else if (result.Equals(2))
                {
                    content = "很抱歉，未查询到相关二维码，请稍后再试";
                }
                else if (result.Equals(1))
                {
                    content = "你的微信账号已和微博账号成功绑定！\n";
                    content += "点击右下角菜单 “进入平台” ，用微信登录就能随时访问我们了。\n";
                }
            }
            else
            {
                content = "很抱歉，二维码无法读取，请稍后再试";
            }
            return content;
        }

        #endregion

    }
}
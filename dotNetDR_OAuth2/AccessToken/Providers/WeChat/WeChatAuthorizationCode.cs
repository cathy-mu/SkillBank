using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Dynamic;

using dotNetDR_OAuth2.Net;

namespace dotNetDR_OAuth2.AccessToken.Providers.WeChat
{
    /// <summary>
    /// 腾讯Authorization Code授权机制的实现类
    /// </summary>
    public class WeChatAuthorizationCode : IAuthorizationCodeBase
    {
        private static AppConfig _appConfig = AppConfigs.GetWeChat();

        #region IGetCode 成员

        public string GenerateCodeUrl(string redirectUrl)
        {
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=SCOPE&state=STATE#wechat_redirect", _appConfig.AppKey, redirectUrl);
        }

        #endregion

        #region IGetAccessToken 成员

        public string GenerateAccessTokenUrl(string redirectUrl, string code)
        {
            return string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", _appConfig.AppKey, _appConfig.AppSecret, code);
        }

        #endregion

        #region IAccessToken 成员

        public dynamic GetResult(string getAccessTokenUrl)
        {
            ClientRequest cr = new ClientRequest(getAccessTokenUrl);
            cr.HttpMethod = WebRequestMethods.Http.Get;

            return NetQuick.GetResponseForDynamic(cr);
        }

        #endregion
    }
}
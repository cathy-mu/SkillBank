using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using dotNetDR_OAuth2.Net;

namespace dotNetDR_OAuth2.AccessToken.Providers.Sina
{
    /// <summary>
    /// 新浪Authorization Code授权机制的实现类
    /// </summary>
    public class SinaAuthorizationCode : IAuthorizationCodeBase
    {
        private static AppConfig _appConfig = AppConfigs.GetSina();

        #region IGetCode 成员

        public string GenerateCodeUrl(string redirectUrl)
        {
            return string.Format("https://api.weibo.com/oauth2/authorize?client_id={0}&response_type=code&redirect_uri=", _appConfig.AppKey) + redirectUrl;
        }

        #endregion

        #region IGetAccessToken 成员

        public string GenerateAccessTokenUrl(string redirectUrl, string code)
        {
            return string.Format("https://api.weibo.com/oauth2/access_token?client_id={0}&client_secret={1}&grant_type=authorization_code&redirect_uri=", _appConfig.AppKey, _appConfig.AppSecret) + redirectUrl + "&code=" + code;
        }

        #endregion

        #region IAccessToken 成员

        public dynamic GetResult(string getAccessTokenUrl)
        {
            ClientRequest cr = new ClientRequest(getAccessTokenUrl);
            cr.HttpMethod = WebRequestMethods.Http.Post;

            return NetQuick.GetResponseForDynamic(cr);
        }

        #endregion
    }
}

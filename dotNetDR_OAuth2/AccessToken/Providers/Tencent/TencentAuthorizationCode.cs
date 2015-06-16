using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Dynamic;

using dotNetDR_OAuth2.Net;

namespace dotNetDR_OAuth2.AccessToken.Providers.Tencent
{
    /// <summary>
    /// 腾讯Authorization Code授权机制的实现类
    /// </summary>
    public class TencentAuthorizationCode : IAuthorizationCodeBase
    {
        private static AppConfig _appConfig = AppConfigs.GetTencent();

        #region IGetCode 成员

        public string GenerateCodeUrl(string redirectUrl)
        {
            return string.Format("https://open.t.qq.com/cgi-bin/oauth2/authorize?client_id={0}&response_type=code&redirect_uri=", _appConfig.AppKey) + redirectUrl;
        }

        #endregion

        #region IGetAccessToken 成员

        public string GenerateAccessTokenUrl(string redirectUrl, string code)
        {
            return string.Format("https://open.t.qq.com/cgi-bin/oauth2/access_token?client_id={0}&client_secret={1}&redirect_uri=", _appConfig.AppKey, _appConfig.AppSecret) + redirectUrl + "&grant_type=authorization_code&code=" + code;
        }

        #endregion

        #region IAccessToken 成员

        public dynamic GetResult(string getAccessTokenUrl)
        {
            ClientRequest cr = new ClientRequest(getAccessTokenUrl);
            cr.HttpMethod = WebRequestMethods.Http.Get; //腾讯微创新，走GET路线~~新浪POST靠边站

            string src = NetQuick.GetResponseForText(cr); //返回值也微创新了！ {key}={value}&{key..n}={value..n}

            //var arr = src.Split('&');
            //var list = new Dictionary<string, string>();
            //foreach (var item in arr)
            //{
            //    var tmp = item.Split('=');
            //    list.Add(tmp[0], tmp[1]);
            //}

            //var access_token = list["access_token"];
            //var expires_in = list["expires_in"];
            //var refresh_token = list["refresh_token"];
            //var name = list["name"];
            //var nick = list["nick"];

            //dynamic result = new DynamicDictionary();
            //result.access_token = access_token;
            //result.expires_in = expires_in;
            //result.refresh_token = refresh_token;
            //result.name = name;
            //result.nick = nick;

            return src;
        }

        #endregion
    }
}

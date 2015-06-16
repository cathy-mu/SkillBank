using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using dotNetDR_OAuth2.AccessToken.Providers.Tencent;
using dotNetDR_OAuth2.AccessToken.Providers.Sina;
using dotNetDR_OAuth2.AccessToken.Providers.WeChat;
using dotNetDR_OAuth2.AccessToken.Providers;

namespace dotNetDR_OAuth2.AccessToken
{
    /// <summary>
    /// AccessToken Toolkit
    /// </summary>
    public class AccessTokenToolkit
    {
        /// <summary>
        /// 生成主机地址路径如 http://www.abc.com:8080
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GenerateHostPath(HttpRequest request)
        {
            return GenerateHostPath(request.Url);
        }

        /// <summary>
        /// 生成主机地址路径如 http://www.abc.com:8080
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GenerateHostPath(Uri uri)
        {
            var result = uri.Scheme + Uri.SchemeDelimiter + uri.Host + (uri.Port != 80 ? ":" + uri.Port : "");
            return result;
        }
    }
}

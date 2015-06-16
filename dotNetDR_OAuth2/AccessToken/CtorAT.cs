using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using dotNetDR_OAuth2.AccessToken.Providers.Sina;
using dotNetDR_OAuth2.AccessToken.Providers.Tencent;
using dotNetDR_OAuth2.AccessToken.Providers.WeChat;

namespace dotNetDR_OAuth2.AccessToken
{
    /// <summary>
    /// AccessToken接口构造机制
    /// </summary>
    public class CtorAT
    {
        /// <summary>
        /// 新浪微博
        /// </summary>
        /// <returns></returns>
        public static IAuthorizationCodeBase Sina()
        {
            return new SinaAuthorizationCode();
        }

        /// <summary>
        /// 腾讯微博
        /// </summary>
        /// <returns></returns>
        public static IAuthorizationCodeBase Tencent()
        {
            return new TencentAuthorizationCode();
        }

        /// <summary>
        /// 腾讯微博
        /// </summary>
        /// <returns></returns>
        public static IAuthorizationCodeBase WeChat()
        {
            return new WeChatAuthorizationCode();
        }
    }
}

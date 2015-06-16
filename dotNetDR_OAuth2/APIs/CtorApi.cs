using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using dotNetDR_OAuth2.APIs.Providers.Sina;
using dotNetDR_OAuth2.APIs.Providers.Tencent;
using dotNetDR_OAuth2.APIs.Providers.WeChat;

namespace dotNetDR_OAuth2.APIs
{
    /// <summary>
    /// Api接口构造机制
    /// </summary>
    public class CtorApi
    {
        /// <summary>
        /// 新浪微博
        /// </summary>
        /// <returns></returns>
        public static IApi Sina()
        {
            return new SinaApi();
        }

        /// <summary>
        /// 腾讯微博
        /// </summary>
        /// <returns></returns>
        public static IApi Tencent()
        {
            return new TencentApi();
        }

        /// <summary>
        /// 腾讯微博
        /// </summary>
        /// <returns></returns>
        public static IApi WeChat()
        {
            return new WeChatApi();
        }
    }
}

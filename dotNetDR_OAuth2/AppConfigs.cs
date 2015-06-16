using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace dotNetDR_OAuth2
{
    /// <summary>
    /// web.config配置文件的AppConfig对象设置
    /// </summary>
    public class AppConfigs
    {
        private const string _APP_KEY_NAME = ".weibo.appkey";
        private const string _APP_SECRET_NAME = ".weibo.appsecret";

        /// <summary>
        /// 获取指定微博的key, secret
        /// </summary>
        /// <param name="provider">微博提供商</param>
        /// <returns></returns>
        public static AppConfig Get(string provider)
        {
            if (string.IsNullOrEmpty(provider))
            {
                throw new ArgumentNullException("provider");
            }

            var result = new AppConfig();

            provider = provider.ToLower();

            result.AppKey = ConfigurationManager.AppSettings[provider + _APP_KEY_NAME];
            result.AppSecret = ConfigurationManager.AppSettings[provider + _APP_SECRET_NAME];

            if (result.AppKey == null)
            {
                throw new Exception("节点 appSettings 下 <add key=\"" + provider + _APP_KEY_NAME + "\" /> 没有被找到!");
            }

            if (result.AppSecret == null)
            {
                throw new Exception("节点 appSettings 下 <add key=\"" + provider + _APP_SECRET_NAME + "\" /> 没有被找到!");
            }

            return result;
        }

        /// <summary>
        /// 获取腾讯微博的key, secret
        /// </summary>
        /// <returns></returns>
        public static AppConfig GetTencent()
        {
            return Get(DefaultAppConfigs.Tencent);
        }

        /// <summary>
        /// 获取新浪微博的key, secret
        /// </summary>
        /// <returns></returns>
        public static AppConfig GetSina()
        {
            return Get(DefaultAppConfigs.Sina);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static AppConfig GetWeChat()
        {
            return Get(DefaultAppConfigs.WeChat);
        }
    }
}

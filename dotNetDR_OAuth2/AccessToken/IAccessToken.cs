using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNetDR_OAuth2.AccessToken
{
    /// <summary>
    /// 获取Access Token的接口
    /// </summary>
    public interface IAccessToken
    {
        /// <summary>
        /// 根据请求的access_token url返回访问令牌
        /// </summary>
        /// <param name="getAccessTokenUrl">请求的access_token url</param>
        /// <returns></returns>
        dynamic GetResult(string getAccessTokenUrl);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNetDR_OAuth2.AccessToken
{
    /// <summary>
    /// 生成访问authorize的url接口
    /// </summary>
    public interface IGetCode
    {
        /// <summary>
        /// 生成访问authorize的url
        /// 文档地址
        /// 新浪：http://open.t.sina.com.cn/wiki/Oauth2/authorize
        /// 腾讯：http://wiki.open.t.qq.com/index.php/OAuth%E6%8E%88%E6%9D%83/%E7%94%A8%E6%88%B7%E6%8E%88%E6%9D%83request_token
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        string GenerateCodeUrl(string redirectUrl);
    }
}

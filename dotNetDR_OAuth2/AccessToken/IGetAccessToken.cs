using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNetDR_OAuth2.AccessToken
{
    /// <summary>
    /// 生成访问access_token的url接口
    /// </summary>
    public interface IGetAccessToken
    {
        /// <summary>
        /// 生成访问access_token的url
        /// 文档地址
        /// 新浪：http://open.t.sina.com.cn/wiki/OAuth2/access_token
        /// 腾讯：http://wiki.open.t.qq.com/index.php/OAuth%E6%8E%88%E6%9D%83/%E4%BA%A4%E6%8D%A2access_token
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        string GenerateAccessTokenUrl(string redirectUrl, string code);
    }
}

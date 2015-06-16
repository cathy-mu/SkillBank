using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNetDR_OAuth2.APIs
{
    /// <summary>
    /// 新浪错误码的列表
    /// http://open.t.sina.com.cn/wiki/Error_code
    /// </summary>
    public class SinaErrorCode
    {
        /// <summary>
        /// 访问令牌无效
        /// </summary>
        public const int invalid_access_token = 21332;
        /// <summary>
        /// 提交相同的信息
        /// </summary>
        public const int repeat_content = 20019;
    }
}

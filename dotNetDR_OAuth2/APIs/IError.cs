using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNetDR_OAuth2.APIs
{
    /// <summary>
    /// 访问资源服务器发生错误时返回的错误接口类型
    /// </summary>
    public interface IError
    {
        /// <summary>
        /// 错误码
        /// </summary>
        int ErrorCode { get; set; }
    }
}

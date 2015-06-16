using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNetDR_OAuth2.APIs
{
    /// <summary>
    /// 新浪错误码的实现
    /// </summary>
    public class WeChatError : IError
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 请求的接口名称
        /// </summary>
        public string request { get; set; }

        #region IError 成员

        public string errmsg { get; set; }
        public string errcode { get; set; }

        #endregion

        /// <summary>
        /// 返回json
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = string.Empty;

            result = string.Format("{{\"errcode\":\"{0}\",\"errmsg\":{1},\"request\":\"{2}\"}}", errcode, errmsg, request);

            return result;
        }
    }
}


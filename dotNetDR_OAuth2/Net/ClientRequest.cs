using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

using dotNetDR_OAuth2.Net;

namespace dotNetDR_OAuth2.Net
{
    public class ClientRequest
    {
        public string Url { get; set; }
        public string HttpMethod { get; set; }
        public CookieContainer CookieContainer { get; set; }
        string Accept { get; set; }
        /// <summary>
        /// 获取或设置 GetResponse 和 GetRequestStream 方法的超时值（毫秒）
        /// </summary>
        public int Timeout { get; set; }
        public string Referer { get; set; }
        /// <summary>
        /// 获取或设置写入或读取流时的超时（以毫秒为单位）。
        /// </summary>
        public int ReadWriteTimeout { get; set; }
        public Version ProtocolVersion { get; set; }
        public bool KeepAlive { get; set; }
        public DateTime IfModifiedSince { get; set; }
        public string Expect { get; set; }
        public string ContentType { get; set; }
        string UserAgent { get; set; }
        public IDictionary<string, string> FormData { get; set; }

        #region 文件上传
        /// <summary>
        /// 以post方式上传带附件的请求时使用
        /// </summary>
        public string Boundary { get; set; }
        public IDictionary<string, BinaryData> BinaryData { get; set; }  
        #endregion



        public ClientRequest(string url)
        {
            Url = url;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNetDR_OAuth2.APIs
{
    /// <summary>
    /// 腾讯错误码的实现
    /// http://wiki.open.t.qq.com/index.php/%E8%BF%94%E5%9B%9E%E9%94%99%E8%AF%AF%E7%A0%81%E8%AF%B4%E6%98%8E
    /// </summary>
    public class TencentError : IError
    {
        /// <summary>
        /// 一级错误信息
        /// 0为返回成功
        /// 非0为失败
        /// </summary>
        public int ret { get; set; }

        #region IError 成员

        public int ErrorCode { get; set; }

        #endregion

        /// <summary>
        /// 错误信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 相关数据(有时为null)
        /// </summary>
        public dynamic data { get; set; }

        /// <summary>
        /// 返回json
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = string.Empty;

            var data = string.Empty;
            data = (this.data == null) ? "null" : this.data.ToString();

            result = string.Format("{{\"data\":{0},\"errcode\":{1},\"msg\":\"{2}\",\"ret\":{3}}}", data, ErrorCode.ToString(), msg, ret.ToString());

            return result;
        }
    }
}
/*
 *  Example
    {
        data: null,
        detailerrinfo: {
            accesstoken: "",
            apiname: "weibo.t.show",
            appkey: "",
            clientip: "119.255.26.231",
            cmd: 0,
            proctime: 0,
            ret1: 20,
            ret2: 1,
            ret3: 20,
            ret4: 2300806410,
            timestamp: 1346219805
        },
        errcode: 20,
        msg: "error id param",
        ret: 1,
        seqid: 5781970035711489000
    }
 * ret为一级错误码
 * errcode为二级错误码
 * 
 * http://wiki.open.t.qq.com/index.php/%E8%BF%94%E5%9B%9E%E9%94%99%E8%AF%AF%E7%A0%81%E8%AF%B4%E6%98%8E
 * http://wiki.open.t.qq.com/index.php/%E9%94%99%E8%AF%AF%E7%A0%81%E8%AF%B4%E6%98%8E
 */
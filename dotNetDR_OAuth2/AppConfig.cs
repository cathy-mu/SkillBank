using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNetDR_OAuth2
{
    /// <summary>
    /// 服务商AppKey,AppSecret配置
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// AppKey标示第三方应用的id
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// AppSecret标示第三方应用持有人的密钥（此参数请不要对外公开）
        /// </summary>
        public string AppSecret { get; set; }
    }
}

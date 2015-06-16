using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNetDR_OAuth2
{
    /// <summary>
    /// UnityFactory
    /// </summary>
    public static class Uf
    {
        /// <summary>
        /// 创建接口的实例化
        /// </summary>
        /// <typeparam name="T">返回的接口类型</typeparam>
        /// <param name="createInstanceCallBack">具体创建接口实例化的函数</param>
        /// <returns></returns>
        public static T C<T>(Func<T> createInstanceCallBack)
        {
            return createInstanceCallBack();
        }
    }
}

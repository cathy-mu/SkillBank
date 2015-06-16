using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

using dotNetDR_OAuth2.Net;

namespace dotNetDR_OAuth2.JSON
{
    /// <summary>
    /// JSON序列化，反序列化的静态类
    /// </summary>
    public static class JsonQuick
    {
        /// <summary>
        /// 将json反序列化成.NET 4.0的dynamic对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static dynamic Deserialize(string json)
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

            dynamic result = serializer.Deserialize(json, typeof(object));

            return result;
        }

        /// <summary>
        /// 将.NET 4.0的dynamic对象序列化成json（待测试）
        /// </summary>
        /// <param name="jsonObj"></param>
        /// <returns></returns>
        public static string Serializer(dynamic jsonObj)
        {
            var result = string.Empty;

            if (jsonObj is DynamicDictionary)
            {
                result = jsonObj.ToString();
            }
            else
            {
                var serializer = new JavaScriptSerializer();
                //serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

                result = serializer.Serialize(jsonObj);
            }

            return result;
        }
    }
}

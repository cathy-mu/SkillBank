using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using dotNetDR_OAuth2.Net;
using dotNetDR_OAuth2.JSON;

namespace dotNetDR_OAuth2.APIs.Providers.Sina
{
    public class SinaApi : IApi
    {
        public const string API_BASE_URL = "https://api.weibo.com/2/";

        private dynamic Call(string url, string httpMethod, string accessToken, IDictionary<string, string> formData, IDictionary<string, Net.BinaryData> binaryData, bool returnJson = false)
        {
            var linkChar = url.IndexOf('?') > 0 ? "&" : "?";
            var queryStringAccessToken = linkChar + "access_token=" + accessToken;
            if (!url.StartsWith("http"))
            {
                url = API_BASE_URL + url;
            }
            ClientRequest cr = new ClientRequest(url + queryStringAccessToken);
            cr.HttpMethod = httpMethod;

            if (formData != null)
            {
                cr.FormData = formData;
                cr.ContentType = "application/x-www-form-urlencoded";
            }

            if (binaryData != null)
            {
                cr.BinaryData = binaryData;
                cr.ContentType = "multipart/form-data";

                cr.KeepAlive = true;
            }

            dynamic result = null;

            if (returnJson)
            {
                result = NetQuick.GetResponseForText(cr);
            }
            else
            {
                result = NetQuick.GetResponseForDynamic(cr);
            }

            return result;
        }

        #region IApi 成员

        #region Get
        public dynamic CallGet(string url, string accessToken, bool returnJson = false, IDictionary<string, object> paramsExt = null)
        {
            return Call(url, WebRequestMethods.Http.Get, accessToken, null, null, returnJson);
        }
        #endregion

        #region Post
        public dynamic CallPost(string url, string accessToken, IDictionary<string, string> formData = null, bool returnJson = false, IDictionary<string, object> paramsExt = null)
        {
            return Call(url, WebRequestMethods.Http.Post, accessToken, formData, null, returnJson);
        }

        public dynamic CallPost(string url, string accessToken, IDictionary<string, string> formData = null, IDictionary<string, BinaryData> binaryData = null, bool returnJson = false, IDictionary<string, object> paramsExt = null)
        {
            return Call(url, WebRequestMethods.Http.Post, accessToken, formData, binaryData, returnJson);
        }
        #endregion


        #region HandlerError
        public bool WasError<T>(dynamic obj, out T error) where T : class, IError
        {
            bool result = false;

            if (!WasError(obj))
            {
                error = default(T);
            }
            else
            {
                if (typeof(T) != typeof(SinaError))
                {
                    throw new Exception("泛型T的类型必须为 dotNetDR_OAuth2.APIs.Providers.Sina.SinaError");
                }

                var err = new SinaError();
                err.ErrorCode = obj.error_code;
                err.error = obj.error;
                err.request = obj.request;

                error = err as T;
                result = true;
            }
            
            return result;
        }

        private bool WasError(dynamic obj)
        {
            return obj.error_code != null;
        }

        public T HandlerException<T>(Exception ex, System.Web.HttpContextBase context) where T : class, IError
        {
            var webEx = ex as WebException;

            if (ex == null)
            {
                throw ex;
            }

            var err = new SinaError();

            var json = string.Empty;

            using (var stream = webEx.Response.GetResponseStream())
            {
                using (var readerStream = new StreamReader(stream, Encoding.UTF8))
                {
                    json = readerStream.ReadToEnd();
                }
            }

            var errObj = JsonQuick.Deserialize(json);
            err.ErrorCode = errObj.error_code;
            err.error = errObj.error;
            err.request = errObj.request;

            T result = err as T;

            return result;
        }
        #endregion
        
        #endregion
    }
}

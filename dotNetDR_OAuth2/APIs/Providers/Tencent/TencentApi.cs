using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using dotNetDR_OAuth2.Net;
using dotNetDR_OAuth2.JSON;

namespace dotNetDR_OAuth2.APIs.Providers.Tencent
{
    public class TencentApi : IApi
    {
        public const string API_BASE_URL = "https://open.t.qq.com/api/";
        private static AppConfig _APP_CONFIG = AppConfigs.GetTencent();

        private dynamic Call(string url, string httpMethod, string accessToken, string openid, IDictionary<string, string> formData, IDictionary<string, BinaryData> binaryData, bool returnJson = false)
        {
            var linkChar = url.IndexOf('?') > 0 ? "&" : "?";

            var queryStringAccessToken = linkChar + "access_token=" + accessToken;
            var queryStringOAuthConsumerKey = "&oauth_consumer_key=" + _APP_CONFIG.AppKey; //TX专注创新30年
            var queryStringOAuthVersion = "&oauth_version=2.a&scope=all";
            var queryStringOpenid = "&openid=" + openid; //TX专注创新30年

            ClientRequest cr = new ClientRequest(API_BASE_URL + url + queryStringAccessToken + queryStringOAuthConsumerKey + queryStringOAuthVersion + queryStringOpenid);
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

        #region GET
        public dynamic CallGet(string url, string accessToken, bool returnJson = false, IDictionary<string, object> paramsExt = null)
        {
            var openid = paramsExt["openid"].ToString();

            return Call(url, WebRequestMethods.Http.Get, accessToken, openid, null, null, returnJson);
        }
        #endregion

        #region POST
        public dynamic CallPost(string url, string accessToken, IDictionary<string, string> formData = null, bool returnJson = false, IDictionary<string, object> paramsExt = null)
        {
            var openid = paramsExt["openid"].ToString();

            return Call(url, WebRequestMethods.Http.Post, accessToken, openid, formData, null, returnJson);
        }

        public dynamic CallPost(string url, string accessToken, IDictionary<string, string> formData = null, IDictionary<string, BinaryData> binaryData = null, bool returnJson = false, IDictionary<string, object> paramsExt = null)
        {
            var openid = paramsExt["openid"].ToString();

            return Call(url, WebRequestMethods.Http.Post, accessToken, openid, formData, binaryData, returnJson);
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
                if (typeof(T) != typeof(TencentError))
                {
                    throw new Exception("泛型T的类型必须为 dotNetDR_OAuth2.APIs.Providers.Tencent.TencentError");
                }

                var err = new TencentError();
                err.ErrorCode = obj.errcode;
                err.ret = obj.ret;
                err.msg = obj.msg;
                err.data = obj.data;

                error = err as T;
                result = true;
            }

            return result;
        }

        private bool WasError(dynamic obj)
        {
            if (obj.ret == null)
            {
                return false;
            }
            else
            {
                return obj.ret != 0;
            }
            
        }

        public T HandlerException<T>(Exception ex, System.Web.HttpContextBase context) where T : class, IError
        {
            var webEx = ex as WebException;

            if (ex == null)
            {
                throw ex;
            }

            var err = new TencentError();

            var json = string.Empty;

            using (var stream = webEx.Response.GetResponseStream())
            {
                using (var readerStream = new StreamReader(stream, Encoding.UTF8))
                {
                    json = readerStream.ReadToEnd();
                }
            }

            var errObj = JsonQuick.Deserialize(json);
            err.ErrorCode = errObj.errcode;
            err.ret = errObj.ret;
            err.msg = errObj.msg;
            err.data = errObj.data;

            T result = err as T;

            return result;
        }
        #endregion
        
        #endregion

        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using dotNetDR_OAuth2.Net;

namespace dotNetDR_OAuth2.APIs
{
    /// <summary>
    /// 访问OAuth2协议API的接口
    /// </summary>
    public interface IApi
    {
        #region Get
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">api地址</param>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="returnJson">true:返回json  false:返回dynamic</param>
        /// <param name="paramsExt">扩展参数</param>
        /// <returns></returns>
		dynamic CallGet(string url, string accessToken, bool returnJson = false, IDictionary<string, object> paramsExt = null);
	    #endregion


        #region Post
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">api地址</param>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="formData">表单数据</param>
        /// <param name="returnJson">true:返回json  false:返回dynamic</param>
        /// <param name="paramsExt">扩展参数</param>
        /// <returns></returns>
        dynamic CallPost(string url, string accessToken, IDictionary<string, string> formData = null, bool returnJson = false, IDictionary<string, object> paramsExt = null); 

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">api地址</param>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="formData">表单数据</param>
        /// <param name="binaryData">上传文件数据</param>
        /// <param name="returnJson">true:返回json  false:返回dynamic</param>
        /// <param name="paramsExt">扩展参数</param>
        /// <returns></returns>
        dynamic CallPost(string url, string accessToken, IDictionary<string, string> formData = null, IDictionary<string, BinaryData> binaryData = null, bool returnJson = false, IDictionary<string, object> paramsExt = null); 
        #endregion

        #region HandlerError
        /// <summary>
        /// 返回的json是否为错误
        /// </summary>
        /// <typeparam name="T">此泛型请传入实现IError的具体类型（如SinaError,TencentError）</typeparam>
        /// <param name="obj">接口返回的结果</param>
        /// <param name="error">若此方法返回true则此输出参数将不为null</param>
        /// <returns></returns>
        bool WasError<T>(dynamic obj, out T error) where T : class, IError;

        /// <summary>
        /// 调用api接口时引发异常的处理方式
        /// </summary>
        /// <typeparam name="T">此泛型请传入实现IError的具体类型（如SinaError,TencentError）</typeparam>
        /// <param name="ex">调用api抛出的异常</param>
        /// <param name="context">当前http上下文</param>
        /// <returns></returns>
        T HandlerException<T>(Exception ex, HttpContextBase context) where T : class, IError;
        #endregion
    }   
}

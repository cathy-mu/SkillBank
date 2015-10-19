using System;
using System.Web;
using System.Collections.Generic;

using SkillBank.Site.Common;

namespace SkillBank.Site.Web.Context
{
    public class WebContextModule : WebContextModuleBase<WebContext, WebContextBuilder>
    {
        #region Overrides

        /// <summary>
        /// </summary>
        /// <param name = "request"></param>
        /// <returns></returns>
        /// 
        protected override bool IsValidForRequest(HttpRequest request)
        {
            string path = request.Url.AbsolutePath.ToLowerInvariant();
            //TO DO:Update isValid function
            if (path.Contains("/content/") || path.Contains("/images/") || path.Contains("/mock/") || path.Contains("/css/") || path.Contains("/js/") || path.Contains("/scripts/"))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Load the Etown context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override WebContext LoadContext(HttpContext context)
        {
            //bool isNewSession = false;
            WebContext webContext = default(WebContext);
            WebContextBuilder contextBuilder = System.Activator.CreateInstance<WebContextBuilder>();
            try
            {
                base.LoadContext(context);
                WebContextModule.SetMemberId(contextBuilder, context.Request);
                WebContextModule.SetBebugLL(contextBuilder, context.Request);
                WebContextModule.SetEtag(contextBuilder, context.Request);
                WebContextModule.SetOrderHandleDate(contextBuilder, context.Request);
                WebContextModule.SetSocialAccount(contextBuilder, context.Request);
                WebContextModule.SetSocialType(contextBuilder, context.Request);
                WebContextModule.SetSocialAccessInfo(contextBuilder, context.Request);
                WebContextModule.SetOpenId(contextBuilder, context.Request);
                this.SetCustomProperties(contextBuilder, context);
                webContext = contextBuilder.GetContext();
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }

            context.Trace.Write("ETContextModule", "End LoadContext");
            return webContext;
        }

        /// <summary>
        /// </summary>
        /// <param name = "context"></param>
        /// <param name = "httpContext"></param>
        protected override void SaveContext(WebContext context, HttpContext httpContext)
        {
            String domain = UtilitiesModule.GetCookieDomain(httpContext.Request);

            var url = httpContext.Request.Url.ToString();
            var rawUrl = httpContext.Request.RawUrl;

            // Recall base method
            base.SaveContext(context, httpContext);

            //// Member Type
            //HttpCookie cookie = httpContext.Request.Cookies[Constants.CookieKeys.MemberType];
            //var reqVal = (cookie == null ? null : cookie.Value);
            //var newVal = context.MemberType.ToString();
            //if (reqVal != newVal)
            //{
            //    CookieManager.SetCookie(Constants.CookieKeys.MemberType, newVal, /*isPersistent*/ true, domain, httpContext);
            //}

            // Social Type
            HttpCookie cookie = httpContext.Request.Cookies[Constants.CookieKeys.MemberId];
            var reqVal = (cookie == null ? null : cookie.Value);
            var newVal = context.MemberId.ToString();
            if (reqVal != newVal)
            {
                CookieManager.SetCookie(Constants.CookieKeys.MemberId, newVal, /*isPersistent*/ true, domain, httpContext);
            }

            // Etag, set etag into cookie if etag is not null or empty
            cookie = httpContext.Request.Cookies[Constants.CookieKeys.Etag];
            reqVal = (cookie == null ? null : cookie.Value);
            newVal = context.Etag;

            if (!String.IsNullOrWhiteSpace(newVal) && reqVal != newVal)
            {
                CookieManager.SetCookie(Constants.CookieKeys.Etag, newVal, /*isPersistent*/ true, domain, httpContext);
            }

            // Show language layer blurb id
            cookie = httpContext.Request.Cookies[Constants.CookieKeys.DebugLL];
            reqVal = (cookie == null ? null : cookie.Value);
            newVal = context.ShowLL.ToString();
            if (reqVal != newVal)
            {
                CookieManager.SetCookie(Constants.CookieKeys.DebugLL, newVal, /*isPersistent*/ true, domain, httpContext);
            }

            // Order auto handler date
            cookie = httpContext.Request.Cookies[Constants.CookieKeys.OrderHandleDate];
            reqVal = (cookie == null ? null : cookie.Value);
            newVal = context.OrderHandleDate;
            if (reqVal != newVal)
            {
                CookieManager.SetCookie(Constants.CookieKeys.OrderHandleDate, newVal, /*isPersistent*/ true, domain, httpContext);
            }

            //Social access token and key
            cookie = httpContext.Request.Cookies[Constants.CookieKeys.SocialAccessInfo];
            reqVal = (cookie == null ? null : cookie.Value);
            newVal = context.SocialAccessInfo;
            if (reqVal != newVal)
            {
                CookieManager.SetCookie(Constants.CookieKeys.SocialAccessInfo, newVal, /*isPersistent*/ true, domain, httpContext);
            }

            cookie = httpContext.Request.Cookies[Constants.CookieKeys.SocialId];
            reqVal = (cookie == null ? null : cookie.Value);
            newVal = context.SocialAccount;
            if (reqVal != newVal)
            {
                CookieManager.SetCookie(Constants.CookieKeys.SocialId, newVal, /*isPersistent*/ true, domain, httpContext);
            }

            cookie = httpContext.Request.Cookies[Constants.CookieKeys.SocialType];
            reqVal = (cookie == null ? null : cookie.Value);
            newVal = context.SocialType.ToString(); 
            if (reqVal != newVal)
            {
                CookieManager.SetCookie(Constants.CookieKeys.SocialType, newVal, /*isPersistent*/ true, domain, httpContext);
            }
            
            cookie = httpContext.Request.Cookies[Constants.CookieKeys.OpenId];
            reqVal = (cookie == null ? null : cookie.Value);
            newVal = context.OpenId.ToString();
            if (reqVal != newVal)
            {
                CookieManager.SetCookie(Constants.CookieKeys.OpenId, newVal, /*isPersistent*/ true, domain, httpContext);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name = "contextBuilder"></param>
        /// <param name = "context"></param>
        protected override void SetCustomProperties(WebContextBuilder contextBuilder, HttpContext context)
        {
            // Recall base method
            base.SetCustomProperties(contextBuilder, context);
            SetMemberId(contextBuilder, context.Request);
            SetEtag(contextBuilder, context.Request);
            SetBebugLL(contextBuilder, context.Request);
            SetOrderHandleDate(contextBuilder, context.Request);
            SetSocialAccessInfo(contextBuilder, context.Request);
            SetSocialAccount(contextBuilder, context.Request);
            SetSocialType(contextBuilder, context.Request);
            SetOpenId(contextBuilder, context.Request);
            //TO DO:Phase2 Set server property later
            //SetServer(contextBuilder, context.Request);
        }

        /// <summary>
        /// Check if it's valid member type
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        private static bool IsValidMemberType(String memberTypeCode)
        {
            bool result;
            if (String.IsNullOrEmpty(memberTypeCode))
            {
                result = false;
            }
            else
            {
                var memberType = (Enums.MemberType)Enum.Parse(typeof(Enums.MemberType), memberTypeCode);
                result = (Constants.LookUps.MemberTypeLkp.Contains(memberType));
            }
            return result;
        }

        private static bool IsValidDebugLLFlag(String debugLL)
        {
            bool result = false;
            if (!String.IsNullOrWhiteSpace(debugLL) && (String.Equals(debugLL, "y", StringComparison.OrdinalIgnoreCase) || String.Equals(debugLL, "n", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
            return result;
        }
                
        private static void SetSocialType(WebContextBuilder contextBuilder, HttpRequest request)
        {
            String socialType = CookieManager.GetCookie(Constants.CookieKeys.SocialType, request);
            //if (!WebContextModule.IsValidMemberType(memberTypeCode))
            //{
            //    memberTypeCode = null;
            //}

            if (string.IsNullOrEmpty(socialType))
            {
                socialType = Constants.DefaultSetting.SocialType.ToString();
            }
            contextBuilder.SocialType = Convert.ToByte(socialType);
        }
   

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextBuilder"></param>
        /// <param name="request"></param>
        private static void SetMemberId(WebContextBuilder contextBuilder, HttpRequest request)
        {
            String cookieMemberId = CookieManager.GetCookie(Constants.CookieKeys.MemberId, request);
            int memberId;
            if (String.IsNullOrEmpty(cookieMemberId) || !Int32.TryParse(cookieMemberId, out memberId))
            {
                memberId = Constants.DefaultSetting.MemberId;
            }
            contextBuilder.MemberId = memberId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextBuilder"></param>
        /// <param name="request"></param>
        private static void SetBebugLL(WebContextBuilder contextBuilder, HttpRequest request)
        {
            string debugLL = request.QueryString[Constants.QueryStringKeys.DebugLL];
            Boolean showLL = false;
            //Has debug query string for LL
            if (WebContextModule.IsValidDebugLLFlag(debugLL))
            {
                showLL = debugLL.Equals("y", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                debugLL = CookieManager.GetCookie(Constants.CookieKeys.DebugLL, request);
                if (WebContextModule.IsValidDebugLLFlag(debugLL))
                {
                    showLL = debugLL.Equals("y", StringComparison.OrdinalIgnoreCase);
                }
            }
            debugLL = showLL ? "y" : "n";
            contextBuilder.ShowLL = debugLL;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextBuilder"></param>
        /// <param name="request"></param>
        private static void SetEtag(WebContextBuilder contextBuilder, HttpRequest request)
        {
            String etag = request.QueryString[Constants.QueryStringKeys.Etag];
            if (!string.IsNullOrEmpty(Constants.BizConfig.OverwriteEtagPrefix) && !string.IsNullOrEmpty(etag) && etag.Contains(Constants.BizConfig.OverwriteEtagPrefix))
            {

            }
            else
            {
                // Check if exist etag
                etag = CookieManager.GetCookie(Constants.CookieKeys.Etag, request);

                if (string.IsNullOrWhiteSpace(etag))
                {
                    etag = request.QueryString[Constants.QueryStringKeys.Etag];
                }

                if (string.IsNullOrWhiteSpace(etag))
                {
                    etag = "";
                }
            }
            contextBuilder.Etag = etag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextBuilder"></param>
        /// <param name="request"></param>
        private static void SetOrderHandleDate(WebContextBuilder contextBuilder, HttpRequest request)
        {
            String orderHandleDate = CookieManager.GetCookie(Constants.CookieKeys.OrderHandleDate, request);
            if (string.IsNullOrWhiteSpace(orderHandleDate))
            {
                orderHandleDate = "";
            }
            contextBuilder.OrderHandleDate = orderHandleDate;
        }

        private static void SetSocialAccount(WebContextBuilder contextBuilder, HttpRequest request)
        {
            String socialAccount = CookieManager.GetCookie(Constants.CookieKeys.SocialId, request);
            if (string.IsNullOrWhiteSpace(socialAccount))
            {
                socialAccount = "";
            }
            contextBuilder.SocialAccount = socialAccount;


        }

        private static void SetOpenId(WebContextBuilder contextBuilder, HttpRequest request)
        {
            String openId = CookieManager.GetCookie(Constants.CookieKeys.OpenId, request);
            if (string.IsNullOrWhiteSpace(openId))
            {
                openId = "";
            }
            contextBuilder.OpenId = openId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextBuilder"></param>
        /// <param name="request"></param>
        private static void SetSocialAccessInfo(WebContextBuilder contextBuilder, HttpRequest request)
        {
            String socialAccessInfo = CookieManager.GetCookie(Constants.CookieKeys.SocialAccessInfo, request);
            if (string.IsNullOrWhiteSpace(socialAccessInfo))
            {
                socialAccessInfo = "";
            }
            contextBuilder.SocialAccessInfo = socialAccessInfo;
        }

        #endregion
    }
}
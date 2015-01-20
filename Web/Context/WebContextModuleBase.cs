using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;

using SkillBank.Site.Common;

namespace SkillBank.Site.Web.Context
{
    public abstract class WebContextModuleBase<TContext, TContextBuilder> : IHttpModule
        where TContext : WebContextBase<TContext>
        where TContextBuilder : WebContextBuilderBase<TContext>, new()
	{
		[DebuggerNonUserCode]
        protected WebContextModuleBase()
		{
		}

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual void Dispose()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual void Init(HttpApplication application)
        {
            application.BeginRequest += new EventHandler(this.Application_BeginRequest);
            application.PreSendRequestHeaders += new EventHandler(this.Application_PreSendRequestHeaders);
        }

        #region Un-Public Methods

        protected abstract bool IsValidForRequest(HttpRequest request);
		
		private void Application_BeginRequest(object sender, EventArgs e)
		{
			HttpApplication application = (HttpApplication)sender;
			bool flag = this.IsValidForRequest(application.Request);
			if (flag)
			{
				TContext context = this.LoadContext(application.Context);
				ContextManager<TContext>.SetContext(context, application.Context);
			}
		}
		private void Application_PreSendRequestHeaders(object sender, EventArgs e)
		{
			HttpApplication application = (HttpApplication)sender;
			bool flag = this.IsValidForRequest(application.Request);
			if (flag)
			{
				TContext context = ContextManager<TContext>.GetContext(application.Context);
				this.SaveContext(context, application.Context);
			}
		}
        
        /// <summary>
        /// Load the Etown context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual TContext LoadContext(HttpContext context)
        {
            //bool isNewSession = false;
            TContext tContext = default(TContext);
            TContextBuilder contextBuilder = System.Activator.CreateInstance<TContextBuilder>();
            try
            {
            //    ETContextSessionCookieData cookie = ETContextModuleBase<TContext, TContextBuilder>.GetSessionCookie(context);
            //    //WebDebugFlagProvider debugFlags = WebDebugFlagProvider.Current;
            //    WebContextModuleBase<TContext, TContextBuilder>.SetSession(contextBuilder, context, ref isNewSession);
            //    WebContextModuleBase<TContext, TContextBuilder>.SetContextDate(contextBuilder, debugFlags);
            WebContextModuleBase<TContext, TContextBuilder>.SetMarketCode(contextBuilder, context.Request);
            WebContextModuleBase<TContext, TContextBuilder>.SetLanguageCode(contextBuilder, context.Request);
            this.SetCustomProperties(contextBuilder, context);
            tContext = contextBuilder.GetContext();
            //WebContextModuleBase<TContext, TContextBuilder>.CheckBrowserCompatibility(etContext, context.Response);
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }

            context.Trace.Write("ETContextModule", "End LoadContext");
            return tContext;
        }


        /// <summary>
        /// Save the Etown context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContext"></param>
        protected virtual void SaveContext(TContext context, HttpContext httpContext)
        {
            try
            {
                //httpContext.Trace.Write("WebContextModule", "Begin SaveContext");
                if (context != null)
                {
                    WebContextModuleBase<TContext, TContextBuilder>.SaveToCookie(context, httpContext);
                }
                //else
                //{
                //    WebContextModuleBase<TContext, TContextBuilder>.LogNullSaveContext(httpContext);
                //}
                //httpContext.Trace.Write("WebContextModule", "End SaveContext");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private static void SaveToCookie(TContext context, HttpContext httpContext)
        {
            String domain = UtilitiesModule.GetCookieDomain(httpContext.Request);

            string cookie = CookieManager.GetCookie(Constants.CookieKeys.Market, httpContext.Request);
            if (cookie != context.MarketCode)
            {
                bool isPersistent = true;
                CookieManager.SetCookie(Constants.CookieKeys.Market, context.MarketCode, isPersistent, domain, httpContext);
            }
            cookie = CookieManager.GetCookie(Constants.CookieKeys.Language, httpContext.Request);
            if (cookie != context.LanguageCode)
            {
                bool isPersistent = true;
                CookieManager.SetCookie(Constants.CookieKeys.Language, context.LanguageCode, isPersistent, domain, httpContext);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        private static bool IsValidLanguage(string languageCode)
        {
            bool result;
            if (string.IsNullOrEmpty(languageCode))
            {
                result = false;
            }
            else
            {
                result = (Constants.LookUps.LanguageLkp.Contains(languageCode));
            }
            return result;
        }

        private static void SetLanguageCode(TContextBuilder contextBuilder, HttpRequest request)
        {
            string languageCode = request.QueryString[Constants.QueryStringKeys.Language];
            if (!WebContextModuleBase<TContext, TContextBuilder>.IsValidLanguage(languageCode))
            {
                languageCode = null;
            }
            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = CookieManager.GetCookie(Constants.CookieKeys.Language, request);
                if (!WebContextModuleBase<TContext, TContextBuilder>.IsValidLanguage(languageCode))
                {
                    languageCode = null;
                }
            }
            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = Constants.DefaultSetting.LanguageCode;
            
            }
            
            contextBuilder.LanguageCode = languageCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        private static bool IsValidMarket(string marketCode)
        {
            bool result;
            if (string.IsNullOrEmpty(marketCode))
            {
                result = false;
            }
            else
            {
                result = (Constants.LookUps.MarketLkp.Contains(marketCode));
            }
            return result;
        }

        private static void SetMarketCode(TContextBuilder contextBuilder, HttpRequest request)
        {
            string marketCode = request.QueryString[Constants.QueryStringKeys.Market];
            if (!WebContextModuleBase<TContext, TContextBuilder>.IsValidMarket(marketCode))
            {
                marketCode = null;
            }
            if (string.IsNullOrEmpty(marketCode))
            {
                marketCode = CookieManager.GetCookie(Constants.CookieKeys.Market, request);
                if (!WebContextModuleBase<TContext, TContextBuilder>.IsValidMarket(marketCode))
                {
                    marketCode = null;
                }
            }
            if (string.IsNullOrEmpty(marketCode))
            {
                marketCode = Constants.DefaultSetting.MarketCode;

            }

            contextBuilder.MarketCode = marketCode;
        }

        protected virtual void SetCustomProperties(TContextBuilder contextBuilder, HttpContext context)
        {
            SetLanguageCode(contextBuilder, context.Request);
            SetMarketCode(contextBuilder, context.Request);
        }

        #endregion

    }
}


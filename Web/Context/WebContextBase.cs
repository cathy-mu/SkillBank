//using EF.Frameworks.Orpheus.ContentManagementEF;
//using EF.Frameworks.Orpheus.Web.ContextEF;
//using EFSchools.Englishtown.ContentConfigurationET;
//using EFSchools.Englishtown.ContentManagementET;
//using EFSchools.Englishtown.LookupsET;
//using EFSchools.Englishtown.MembersET;
using System;
using System.Web;
using System.ComponentModel;

using SkillBank.Site.Web.ContentConfiguration;

namespace SkillBank.Site.Web.Context
{
	/// <summary>
	/// Base implementation of the Etown context.
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
    public abstract class WebContextBase<TContext> : ILanguageContext, IMarketContext where TContext : class
    {
        #region Public Properties

        public string LanguageCode
		{
			get;
			private set;
        }

        public string MarketCode
		{
			get;
			private set;
        }

        
        public static TContext Current
        {
            get
            {
                return ContextManager<TContext>.GetContext(HttpContext.Current);
            }
            set
            {
                ContextManager<TContext>.SetContext(value, HttpContext.Current);
            }
        }

        #endregion

        /// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="builder"></param>
        protected WebContextBase(String marketCode, String languageCode)
        {
            this.MarketCode = marketCode;
            this.LanguageCode = languageCode;
        }
	}
}


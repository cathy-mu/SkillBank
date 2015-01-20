using System;

namespace SkillBank.Site.Web.Context
{
	/// <summary>
	/// Base implementation of the Etown context builder
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
    public abstract class WebContextBuilderBase<TContext> where TContext : WebContextBase<TContext>
    {

        #region Public Properties
        
        /// <summary>
		///
		/// </summary>
		public string LanguageCode
		{
			get;
			set;
		}
			
		/// <summary>
		///
		/// </summary>
		public string MarketCode
		{
			get;
			set;
		}

        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract TContext GetContext();
	
		/// <summary>
		/// Reads the values from an existing context.
		/// </summary>
		/// <param name="context"></param>
        public void ReadContext(WebContextBase<TContext> context)
        {
            this.LanguageCode = context.LanguageCode;
            this.MarketCode = context.MarketCode;
        }
                
        #endregion
    }
}
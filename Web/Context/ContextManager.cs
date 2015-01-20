using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;

namespace SkillBank.Site.Web.Context
{
    /// <summary>
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
	/// <remarks></remarks>
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public class ContextManager<TContext> where TContext : class
	{
		[DebuggerNonUserCode]
		public ContextManager()
		{
		}
		/// <summary>
		///  Gets the current ef context
		///  </summary>
		/// <param name="httpContext"></param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static TContext GetContext(HttpContext httpContext)
		{
			bool flag = httpContext == null;
			if (flag)
			{
				httpContext = HttpContext.Current;
				flag = (httpContext == null);
				if (flag)
				{
					throw new ArgumentNullException("httpContext");
				}
			}
			return httpContext.Items[typeof(TContext)] as TContext;
		}
		public static void SetContext(TContext webContext, HttpContext httpContext)
		{
			bool flag = httpContext == null;
			if (flag)
			{
				httpContext = HttpContext.Current;
				flag = (httpContext == null);
				if (flag)
				{
					throw new ArgumentNullException("httpContext");
				}
			}
            httpContext.Items[typeof(TContext)] = webContext;
		}
	}
}

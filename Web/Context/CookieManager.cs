using System;
using System.ComponentModel;
using System.Web;

namespace SkillBank.Site.Web.Context
{
 public class CookieManager
	{
        ///// <summary>
        ///// The different parts of the CMus cookie
        ///// </summary>
        //public enum CMusCookieParts
        //{
        //    /// <summary>
        //    ///
        //    /// </summary>
        //    Member_id,
        //    /// <summary>
        //    ///
        //    /// </summary>
        //    TimeStamp,
        //    /// <summary>
        //    ///
        //    /// </summary>
        //    UserName,
        //    /// <summary>
        //    ///
        //    /// </summary>
        //    MemberType
        //}
        ///// <summary>
        /////
        ///// </summary>
        //public sealed class CookieKeys : EFSchools.Englishtown.Web.CookieKeys
        //{
        //    /// <summary>
        //    ///
        //    /// </summary>
        //    [EditorBrowsable(EditorBrowsableState.Never), System.Obsolete("Please use Session_id instead.", true)]
        //    public static readonly string SessionId = "SessionId";
        //    private CookieKeys()
        //    {
        //    }
        //}
		private CookieManager()
		{
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="cookieName"></param>
		/// <param name="request"></param>
		/// <returns></returns>
		public static string GetCookie(string cookieName, HttpRequest request)
		{
			string result;
			try
			{
				HttpCookie cookie = request.Cookies[cookieName];
				if (cookie != null)
				{
					result = cookie.Value;
				}
				else
				{
					result = null;
				}
			}
			catch (System.IndexOutOfRangeException)
			{
				result = null;
			}
			return result;
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="cookieName"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public static string GetCookie(string cookieName, HttpContext context)
		{
			return CookieManager.GetCookie(cookieName, context.Request);
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="cookieName"></param>
		/// <returns></returns>
		public static string GetCookie(string cookieName)
		{
			HttpContext context = HttpContext.Current;
			return CookieManager.GetCookie(cookieName, context.Request);
		}
        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="cookieIndex"></param>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public static string ReadCMusCookie(CookieManager.CMusCookieParts cookieIndex, HttpContext context)
        //{
        //    HttpCookie cookie = context.Request.Cookies[EFSchools.Englishtown.Web.CookieKeys.CMus];
        //    string result;
        //    if (cookie != null)
        //    {
        //        result = CookieManager.ReadCMusCookie(cookie.Value, cookieIndex);
        //    }
        //    else
        //    {
        //        result = "-1";
        //    }
        //    return result;
        //}
        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="cookieIndex"></param>
        ///// <returns></returns>
        //public static string ReadCMusCookie(CookieManager.CMusCookieParts cookieIndex)
        //{
        //    HttpContext context = HttpContext.Current;
        //    return CookieManager.ReadCMusCookie(cookieIndex, context);
        //}
        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="cmusCookieText"></param>
        ///// <param name="cookiePart"></param>
        ///// <returns></returns>
        //public static string ReadCMusCookie(string cmusCookieText, CookieManager.CMusCookieParts cookiePart)
        //{
        //    string result;
        //    if (!string.IsNullOrEmpty(cmusCookieText))
        //    {
        //        cmusCookieText = HttpUtility.UrlDecode(cmusCookieText);
        //        string decodedCookie = CookieManager.CookieDecode(cmusCookieText);
        //        string[] parts = decodedCookie.Split(new char[]
        //        {
        //            '|'
        //        });
        //        if (parts.Length >= 4)
        //        {
        //            result = parts[(int)cookiePart].ToString();
        //        }
        //        else
        //        {
        //            result = "-1";
        //        }
        //    }
        //    else
        //    {
        //        result = "-1";
        //    }
        //    return result;
        //}
		/// <summary>
		///
		/// </summary>
		/// <param name="cookieName"></param>
		/// <param name="context"></param>
		public static void ClearCookie(string cookieName, HttpContext context)
		{
			HttpCookie cookie = new HttpCookie(cookieName);
			cookie.Expires = System.DateTime.Now.AddDays(-1.0);
			if (context != null)
			{
				HttpContext.Current.Response.Cookies.Remove(cookie.Name);
				HttpContext.Current.Response.Cookies.Add(cookie);
			}
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="cookieName"></param>
		public static void ClearCookie(string cookieName)
		{
            CookieManager.ClearCookie(cookieName, HttpContext.Current);
		}
		/// <summary>
		/// Set cookie value and also you can pass in Domain name as parameter.
		/// Refer Utilities.GetDomain() function - It gives appropriate domain name
		/// You can also pass Nothing to the domainName 
		/// If you are not sure about the domainName parameter just use the overloaded 
		///     the Function SetCookie without DomainName parameter
		/// </summary>
		/// <param name="cookieName"></param>
		/// <param name="cookieValue"></param>
		/// <param name="isPersistent"></param>
		/// <param name="domainName"></param>
		/// <param name="context"></param>
		/// <remarks></remarks>
		public static void SetCookie(string cookieName, string cookieValue, bool isPersistent, string domainName, HttpContext context)
		{
			System.TimeSpan cookieTimeSpan = default(System.TimeSpan);
			if (isPersistent)
			{
				cookieTimeSpan = new System.TimeSpan(1000, 0, 0, 0);
			}
			else
			{
				cookieTimeSpan = System.TimeSpan.Zero;
			}
            CookieManager.SetCookie(cookieName, cookieValue, isPersistent, cookieTimeSpan, domainName, context);
		}
		/// <summary>
		/// Set cookie value and also you can pass in Domain name as parameter.
		/// Refer Utilities.GetDomain() function - It gives appropriate domain name
		/// You can also pass Nothing to the domainName 
		/// If you are not sure about the domainName parameter just use the overloaded 
		///     the Function SetCookie without DomainName parameter
		/// </summary>
		/// <param name="cookieName"></param>
		/// <param name="cookieValue"></param>
		/// <param name="isPersistent"></param>
		/// <param name="domainName"></param>
		/// <remarks></remarks>
		public static void SetCookie(string cookieName, string cookieValue, bool isPersistent, string domainName)
		{
			System.TimeSpan cookieTimeSpan = default(System.TimeSpan);
			if (isPersistent)
			{
				cookieTimeSpan = new System.TimeSpan(1000, 0, 0, 0);
			}
			else
			{
				cookieTimeSpan = System.TimeSpan.Zero;
			}
            CookieManager.SetCookie(cookieName, cookieValue, isPersistent, cookieTimeSpan, domainName);
		}
		/// <summary>
		/// Set cookie value and set value for Domain obtained using GetDomain method.
		/// If isPersistent is set to true it keep the expiry date for Cookie to 1000 days from now.
		/// </summary>
		/// <param name="cookieName"></param>
		/// <param name="cookieValue"></param>
		/// <param name="isPersistent"></param>
		/// <param name="context"></param>
		/// <remarks></remarks>
		public static void SetCookie(string cookieName, string cookieValue, bool isPersistent, HttpContext context)
		{
			string domainName = UtilitiesModule.GetCookieDomain(context.Request);
            CookieManager.SetCookie(cookieName, cookieValue, isPersistent, domainName);
		}
		/// <summary>
		/// Set cookie value and set value for Domain obtained using GetDomain method.
		/// If isPersistent is set to true it keep the expiry date for Cookie to 1000 days from now.
		/// </summary>
		/// <param name="cookieName"></param>
		/// <param name="cookieValue"></param>
		/// <param name="isPersistent"></param>
		/// <remarks></remarks>
		public static void SetCookie(string cookieName, string cookieValue, bool isPersistent)
		{
            CookieManager.SetCookie(cookieName, cookieValue, isPersistent, UtilitiesModule.GetCookieDomain());
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="cookieName"></param>
		/// <param name="cookieValue"></param>
		/// <param name="cookieLife"></param>
		public static void SetCookie(string cookieName, string cookieValue, System.TimeSpan cookieLife)
		{
            CookieManager.SetCookie(cookieName, cookieValue, true, cookieLife, UtilitiesModule.GetCookieDomain());
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="cookieName"></param>
		/// <param name="cookieValue"></param>
		/// <param name="cookieLife"></param>
		/// <param name="context"></param>
		public static void SetCookie(string cookieName, string cookieValue, System.TimeSpan cookieLife, HttpContext context)
		{
            CookieManager.SetCookie(cookieName, cookieValue, true, cookieLife, UtilitiesModule.GetCookieDomain(), context);
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="cookieName"></param>
		/// <param name="cookieValue"></param>
		/// <param name="isPersistent"></param>
		/// <param name="cookieLife"></param>
		/// <param name="domainName"></param>
		public static void SetCookie(string cookieName, string cookieValue, bool isPersistent, System.TimeSpan cookieLife, string domainName)
		{
			HttpContext context = HttpContext.Current;
			if (context == null)
			{
				throw new System.InvalidOperationException("Cannot SetCookie() when current HttpContext is null.  If this is an async process use overload providing the context.");
			}
            CookieManager.SetCookie(cookieName, cookieValue, isPersistent, cookieLife, domainName, context);
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="cookieName"></param>
		/// <param name="cookieValue"></param>
		/// <param name="isPersistent"></param>
		/// <param name="cookieLife"></param>
		/// <param name="domainName"></param>
		/// <param name="context"></param>
        public static void SetCookie(string cookieName, string cookieValue, bool isPersistent, System.TimeSpan cookieLife, string domainName, HttpContext context)
        {
            System.DateTime dt = System.DateTime.Now;
            HttpCookie cookie = context.Request.Cookies[cookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName, cookieValue);
            }
            cookie.Value = cookieValue;
            cookie.Path = "/";
            cookie.Domain = domainName;
            if (isPersistent)
            {
                cookie.Expires = dt.Add(cookieLife);
            }
            context.Response.SetCookie(cookie);
        }
        //private static string CookieEncode(string text)
        //{
        //    return LegacyCookieEncoder.Encode6Bit(text);
        //}
        //private static string CookieDecode(string text)
        //{
        //    return LegacyCookieEncoder.Decode6Bit(text);
        //}
	}
}

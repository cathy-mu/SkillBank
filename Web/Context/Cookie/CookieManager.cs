using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkillBank.Site.Web
{
    public class CookieManager
    {
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

        
        public static void SetCookie(string cookieName, string cookieValue, HttpContext context)
        {
            CookieManager.SetCookie(cookieName, cookieValue, true, "localhost", context);
        }

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

        public static void SetCookie(string cookieName, string cookieValue, bool isPersistent, TimeSpan cookieTimeSpan, string domainName, HttpContext context)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Value = cookieValue;
            cookie.Expires = DateTime.Now.Add(cookieTimeSpan);
            context.Response.SetCookie(cookie);
        }

        public static Byte GetCookieByteValue(string cookieName, HttpRequestBase request)
        {
            Byte result = 0;
            HttpCookie cookie = request.Cookies[cookieName];
                if (cookie != null)
                {
                    Byte value = 0;
                    bool isByte = Byte.TryParse(cookie.Value, out value);
                    if (isByte)
                    {
                        result = value;
                    }
                }
            return result;
        }

        public static Int32 GetCookieIntValue(string cookieName, HttpRequestBase request)
        {
            Int32 result = 0;
            HttpCookie cookie = request.Cookies[cookieName];
            if (cookie != null)
            {
                Int32 value = 0;
                bool isByte = Int32.TryParse(cookie.Value, out value);
                if (isByte)
                {
                    result = value;
                }
            }
            return result;
        }

        public static Byte GetCookieByteBoolValue(string cookieName, HttpRequestBase request, Char spliter, out Boolean isTrue)
        {
            Byte result = 0;
            isTrue = false;
            HttpCookie cookie = request.Cookies[cookieName];
            if (cookie != null)
            {
                Byte value0;
                var values = cookie.Value.Split(spliter);
                if (values.Length == 2)
                {
                    bool isByte = Byte.TryParse(values[0], out value0);
                    bool isBool = (values[1].Equals("0") || values[1].Equals("1"));
                    if (isByte && isBool)
                    {
                        isTrue = values[1].Equals("1");
                        result = value0;
                    }
                }
            }
            return result;
        }


    }
}

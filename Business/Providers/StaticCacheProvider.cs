using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkillBank.Site.Services.Providers
{
    public class StaticCacheProvider
    {
        private static IDictionary<string, object> _dic;
        private static object locker = new object();

        private static IDictionary<string, object> CachedDic
        {
            get
            {
                if (_dic == null)
                {
                    lock (locker)
                    {
                        if (_dic == null)
                        {
                            _dic = new Dictionary<string, object>();
                        }
                    }
                }

                return _dic;
            }
        }

        public static object GetObject(string key)
        {
            if (CachedDic.ContainsKey(key))
            {
                return CachedDic[key];
            }
            else
            {
                return null;
            }
        }

        public static void SetObject(string key, object obj)
        {
            lock (locker)
            {
                CachedDic[key] = obj;
            }
        }
    }

}




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Common;
using SkillBank.Site.Services.CacheProviders;
using SkillBank.Site.Services.Managers;
using SkillBank.Site.Web.Context;
using System.Configuration;
using SkillBank.Site.Services.Utility;

namespace SkillBank.Site.Web
{
    public static class TagHelper
    {
        private static readonly ICityLkpProvider _cityProvider;

        static TagHelper()
        {
            _cityProvider = new CityLkpProvider(new LookupsRepository());
        }

        public static String GetTextSubSting(String text, int length, Boolean addBlankIfEmpty = false, String suffix = " ...")
        {
            if (String.IsNullOrEmpty(text))
            {
                if (addBlankIfEmpty)
                {
                    return "&nbsp;&nbsp;&nbsp;";
                }
            }
            else if (text.Length > length)
            {
                return text.Substring(0, length) + suffix;
            }
            
            return text;
        }
        
        public static int MapNumer(int numKey, Dictionary<int, int> numDic)
        {
            if (numDic == null || numDic.Count() == 0)
            {
                if (numDic.ContainsKey(numKey))
                {
                    return numDic[numKey];
                }

            }

            return 0;
        }


        public static String GetCityName(int cityId)
        {
            var result = "";
            if (cityId > 0)
            {
                String localCode = WebContext.Current.MarketCode;
                var cities = _cityProvider.GetCityLkp(localCode);
                if (cities == null || cities.Count == 0)
                {
                    result = String.Format("CTR {0} {1}", localCode, cityId);
                }
                else if (cities.ContainsKey(cityId))
                {
                    result = cities[cityId].CityName;
                }
                else
                {
                    result = cityId.ToString();
                }
            }
            return result;
        }

        public static String GetNewMessageTag(Dictionary<int,int> MessageUnReadDic, int contactId)
        {
            if (MessageUnReadDic!=null && MessageUnReadDic.ContainsKey(contactId))
            {
                string result = String.Format("<span class=\"label label-msg\">{0}</span>", MessageUnReadDic[contactId]);

                return result;
            }
            else
            {
                return "";
            }
           
        }

        public static Boolean GetIsLike(String likeList, int classId)
        {
            //if (!String.IsNullOrEmpty(likeList))
            //{
            //    var classTag = String.Concat(classId, ",");
            //    if (likeList.StartsWith(classTag) || likeList.Contains(String.Concat(",", classTag)))
            //    {
            //        return true;
            //    }
            //}
            //return false;
            return DataTagHelper.GetIsLike(likeList, classId);
        }


    }
    
        
}

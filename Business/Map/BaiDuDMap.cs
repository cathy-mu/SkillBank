using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBank.Site.Common;

namespace SkillBank.Site.Services
{
    public static class BaiDuDMap
    {
        public static String SearchPlace()
        {
            String region = "上海";
            String queryTag = "创智天地";

            String requestURL = String.Format(ConfigConstants.ThirdPartySetting.DBMap.SearchPlaceQuery, ConfigConstants.ThirdPartySetting.DBMap.AK, ConfigConstants.ThirdPartySetting.DBMap.OutputType, queryTag, ConfigConstants.ThirdPartySetting.DBMap.PageSize, ConfigConstants.ThirdPartySetting.DBMap.PageNo, ConfigConstants.ThirdPartySetting.DBMap.Scope, region);
            return requestURL;
        }

        public static void GetSuggestionByTag()
        {
        }
    }
}

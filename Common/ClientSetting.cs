using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkillBank.Site.Common
{
    public class ClientSetting
    {
        /// <summary>
        /// For show class list page
        /// </summary>
        public enum ClassListOrderType
        {
            ByLastUpdate = 0,
            ByDisctince = 1,//default for login member
            ByLevel = 2,
            ByRank = 3
        }

        public static class ClassListOrderDefaultAsc
        {
            public const bool ByLastUpdate = false;
            public const bool ByDisctince = true;
            public const bool ByLevel = true;
            public const bool ByRank = false;
        }

    }
}

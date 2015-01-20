using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.DataSource.Mapper
{
    public class MemberMapper
    {
        public static MemberInfo Map(ObjectResult<MemberInfo> objectUser)
        {
            if (objectUser != null && objectUser.Count() != 0)
            {
                List<MemberInfo> userInfos = objectUser.ToList<MemberInfo>();
                return userInfos[0];
            }
            return null;
        }

        
    }
}

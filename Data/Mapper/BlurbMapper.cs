using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Objects;
using SkillBank.Site.DataSource.Data;

namespace SkillBank.Site.DataSource.Mapper
{
    public class BlurbMapper
    {
        //My SQL
        //public static Dictionary<int, String> Map(ObjectResult<Blurb> objectBlurbs)
        //{
        //    if (objectBlurbs != null)
        //    {
        //        var blurbs = objectBlurbs.ToDictionary(b => b.BlurbId, b => b.BlurbText);
        //        return (blurbs.Count > 0) ? blurbs : null;
        //    }
        //    return null;
        //}

        
        //SQL
        public static Dictionary<int, String> Map(ObjectResult<Blurbs_LoadByLanguageSiteVersion_p_Result> objectBlurbs)
        {
            if (objectBlurbs != null)
            {
                var blurbs = objectBlurbs.ToDictionary(b => b.BlurbId, b => b.BlurbText);
                return (blurbs.Count > 0) ? blurbs : null;
            }
            return null;
        }
        
        
    }
}

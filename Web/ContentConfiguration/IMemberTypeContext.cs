using System;
using SkillBank.Site.Common;

namespace SkillBank.Site.Web.ContentConfiguration
{
    public interface IMemberTypeContext
    {
        /// <summary>
        ///
        /// </summary>
        Enums.MemberType MemberType
        {
            get;
        }
    }
}
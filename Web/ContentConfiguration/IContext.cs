using System;
namespace SkillBank.Site.Web.ContentConfiguration
{
	/// <summary>
	/// Combines the following interfaces: IAreaContext, IMarketContext, IMemberTypeContext, IPartnerContext, IServerContext, IValidDateRangeContext, ISplitRunContext, ContentManagementET.ICmsContext
	/// </summary>
    public interface IETContext : IMemberTypeContext, ILanguageContext, IMarketContext, IServerContext
	{
	}
}

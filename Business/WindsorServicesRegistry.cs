using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using Castle.Facilities.Logging;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Core.Internal;

using SkillBank.Site.DataSource.Data;
using SkillBank.Site.Services;
using SkillBank.Site.Services.Managers;
using SkillBank.Site.Services.CacheProviders;


namespace SkillBank.Site.Services
{
    public class WindsorServicesRegistry
    {
        public static void Register(IWindsorContainer container)
        {
            RegisterInterceptors(container);
            RegisterContentServiceComponents(container);
        }

        private static void RegisterContentServiceComponents(IWindsorContainer container)
        {

            container.Register(Component.For<IBlurbsRepository>().ImplementedBy<BlurbsRepository>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<IClassInfoRepository>().ImplementedBy<ClassInfoRepository>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<IClassTagRepository>().ImplementedBy<ClassTagRepository>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<ILookupsRepository>().ImplementedBy<LookupsRepository>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<IMessageRepository>().ImplementedBy<MessageRepository>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<IOrderRepository>().ImplementedBy<OrderRepository>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<IStudentReviewRepository>().ImplementedBy<StudentReviewRepository>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<ITeacherReviewRepository>().ImplementedBy<TeacherReviewRepository>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<IMemberRepository>().ImplementedBy<MemberRepository>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<INotificationRepository>().ImplementedBy<NotificationRepository>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<IInteractiveRepository>().ImplementedBy<InteractiveRepository>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<IReportToolsRepository>().ImplementedBy<ReportToolsRepository>().LifeStyle.Is(LifestyleType.Singleton));
            
            container.Register(Component.For<IBlurbsProvider>().ImplementedBy<BlurbsProvider>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<ICategoryProvider>().ImplementedBy<CategoryProvider>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<ICategoryLkpProvider>().ImplementedBy<CategoryLkpProvider>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<ICategoryTagRepository>().ImplementedBy<CategoryTagRepository>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<ICityLkpProvider>().ImplementedBy<CityLkpProvider>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<IMetaTagProvider>().ImplementedBy<MetaTagProvider>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<ISystemNotificationProvider>().ImplementedBy<SystemNotificationProvider>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<ITopBannerProvider>().ImplementedBy<TopBannerProvider>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<IPortalBannerProvider>().ImplementedBy<PortalBannerProvider>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<ILinkMapProvider>().ImplementedBy<LinkMapProvider>().LifeStyle.Is(LifestyleType.Singleton));
                                              
            container.Register(Component.For<IRecommendClassCacheMgr>().ImplementedBy<RecommendClassCacheMgr>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<IClassNumCacheMgr>().ImplementedBy<ClassNumCacheMgr>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<IClassListCacheMgr>().ImplementedBy<ClassListCacheMgr>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(Component.For<IClassItemCacheMgr>().ImplementedBy<ClassItemCacheMgr>().LifeStyle.Is(LifestyleType.Singleton));
            
            container.Register(Component.For<IContentManager>().ImplementedBy<ContentManager>().LifeStyle.Is(LifestyleType.Transient));
            container.Register(Component.For<IMemberManager>().ImplementedBy<MemberManager>().LifeStyle.Is(LifestyleType.Transient));
            container.Register(Component.For<IClassManager>().ImplementedBy<ClassManager>().LifeStyle.Is(LifestyleType.Transient));
            container.Register(Component.For<IFeedBackManager>().ImplementedBy<FeedBackManager>().LifeStyle.Is(LifestyleType.Transient));
            container.Register(Component.For<IOrderManager>().ImplementedBy<OrderManager>().LifeStyle.Is(LifestyleType.Transient));
            container.Register(Component.For<IMessageManager>().ImplementedBy<MessageManager>().LifeStyle.Is(LifestyleType.Transient));
            container.Register(Component.For<INotificationManager>().ImplementedBy<NotificationManager>().LifeStyle.Is(LifestyleType.Transient));
            container.Register(Component.For<IReportToolsManager>().ImplementedBy<ReportToolsManager>().LifeStyle.Is(LifestyleType.Transient));
            container.Register(Component.For<ICacheContentManager>().ImplementedBy<CacheContentManager>().LifeStyle.Is(LifestyleType.Transient));
            

            container.Register(Component.For<ICommonService>().ImplementedBy<CommonService>().LifeStyle.Is(LifestyleType.Transient));
            container.Register(Component.For<IContentService>().ImplementedBy<ContentService>().LifeStyle.Is(LifestyleType.Transient));
        }

        
        private static void RegisterInterceptors(IWindsorContainer container)
        {
            //container.AddFacility<LoggingFacility>(f => f.LogUsing(LoggerImplementation.Log4net));
            //container.Register(Component.For<LoggingInterceptor>().ImplementedBy<LoggingInterceptor>().LifeStyle.Is(LifestyleType.Transient));
        }


        //private static IList<IStudyPlanRecommendationRule> GetRules(IKernel kernel, params string[] rules)
        //{
        //    return rules.Select(kernel.Resolve<IStudyPlanRecommendationRule>).ToList();
        //}

        //private static IActivitySaveHandler GetActivitySaveHandler(IKernel kernel, string handlerName)
        //{
        //    return kernel.Resolve<IActivitySaveHandler>(handlerName);
        //}

        //public static void GetAchievements(IKernel kernel, IDictionary dictionary)
        //{
        //    dictionary["coinBasedAchievements"] = new List<IAchievement>(kernel.ResolveAll<ICoinBasedAchievement>().ToList());
        //    dictionary["progressBasedAchievements"] = new List<IAchievement>(kernel.ResolveAll<IProgressBasedAchievement>().ToList());
        //}
    }
}

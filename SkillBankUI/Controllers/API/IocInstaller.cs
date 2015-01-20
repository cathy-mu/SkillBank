using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

using Castle.Core;
using Castle.Facilities.Logging;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Core.Internal;

namespace SkillBankWeb.API
{
    //public class IoCInstaller : IWindsorInstaller
    //{
    //    public void Install(IWindsorContainer container, IConfigurationStore store)
    //    {
    //        container.Register(Component.For<castledemoContext>().LifeStyle.PerWebRequest);

    //        container.Register(Classes.FromThisAssembly()
    //         .InNamespace("CastleDemo.DataAccess.RepositoryImpl", true)
    //         .LifestylePerWebRequest()
    //        .WithService.AllInterfaces());
    //    }
    //}
}
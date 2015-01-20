using System.Web.Mvc;
using System.Web.Http.Controllers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using SkillBank.Controllers;

namespace SkillBank.Windsor
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());
            container.Register(Classes.FromThisAssembly().BasedOn<IHttpController>().LifestyleTransient());
            container.Register(Classes.FromThisAssembly().BasedOn<AuthorizeAttribute>().LifestyleTransient());
        }
    }
}
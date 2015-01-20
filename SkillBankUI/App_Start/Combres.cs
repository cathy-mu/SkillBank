[assembly: WebActivator.PreApplicationStartMethod(typeof(SkillBankWeb.App_Start.Combres), "PreStart")]
namespace SkillBankWeb.App_Start {
	using System.Web.Routing;
	using global::Combres;
	
    public static class Combres {
        public static void PreStart() {
            RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}
using System.Web;
using SQuadro.App_Start;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using AspNetHaack;

[assembly: PreApplicationStartMethod(typeof(FormsAuthenticationConfig), "Register")]
namespace SQuadro.App_Start {
    public static class FormsAuthenticationConfig {
        public static void Register() {
            DynamicModuleUtility.RegisterModule(typeof(SuppressFormsAuthenticationRedirectModule));
        }
    }
}

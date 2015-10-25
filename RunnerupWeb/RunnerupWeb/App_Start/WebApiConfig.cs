using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using TinyIoC;

namespace RunnerupWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var container = new TinyIoCContainer();
            container.Register<IGoogleUploader,GoogleUpLoader>().AsSingleton();
            var module = container.Resolve<IGoogleUploader>();
            module.CertificatePath = HttpRuntime.AppDomainAppPath;
            //            container.Register(new GoogleUpLoader(HttpRuntime.AppDomainAppPath));

            GlobalConfiguration.Configuration.DependencyResolver = new TinyIoCDependencyResolver(container);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

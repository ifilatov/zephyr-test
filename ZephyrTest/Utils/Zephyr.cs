using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using System.Text.RegularExpressions;

namespace ZephyrTest.Utils
{
    class Zephyr
    {
        public static IConfigurationRoot ZephyrConfiguration = Tools.Configurator.GetConfiguration("Zephyr");
        public static string GetUrlValue(string url) => ZephyrConfiguration.GetSection("Urls:"+url).Value;
        public static string GetIdValue(string id) => ZephyrConfiguration.GetSection("Ids:" + id).Value;
        private static IRestClient Client = new RestClient(ZephyrConfiguration.GetSection("Urls:BaseUrl").Value)
        {
            Authenticator = new HttpBasicAuthenticator(ZephyrConfiguration["Uname"], ZephyrConfiguration["Pass"])
        };

        public static IRestResponse<T> Execute<T>(IRestRequest request) where T: new() => 
            Client.Execute<T>(request);

        public static IRestResponse Execute(IRestRequest request) => 
            Client.Execute(request);

        public static IRestRequest GetAuthorizedRequest(string url, Method method) =>
            new RestRequest(url, method)
                .AddHeader("Content-type", "application/json")
                .AddHeader("AO-7DEABF", GetZephyrToken())
                .AddHeader("X-AUSERNAME", ZephyrConfiguration["Uname"]);

        private static string GetZephyrToken() =>
            Regex.Match(Execute(new RestRequest(string.Empty,Method.GET)).Content.ToString(), "(?<=zEncKeyVal = \").*(?=\")").Value;
    }
}

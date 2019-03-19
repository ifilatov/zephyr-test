using Microsoft.Extensions.Configuration;
using ZephyrTest.Models.WebServices;
using ZephyrTest.Tools;

namespace ZephyrTest.Factories
{
    class ConfigFactory
    {
        public static WSEnvironment GetEnvironment(string env)
        {
            IConfigurationRoot Configuration = Configurator.GetConfiguration(env);
            return new WSEnvironment
            {
                Shelf = Configuration.GetSection("Urls:Shelf").Value,
                Authorization = Configuration.GetSection("Urls:Authorization").Value,
                Outlet = Configuration.GetSection("Urls:Outlet").Value
            };
        }

        public static WSAuthData GetAuthBody(string env)
        {
            IConfigurationRoot Configuration = Configurator.GetConfiguration(env);
            return new WSAuthData
            {
                ClientId = Configuration.GetSection("Authorization:ClientId").Value,
                Username = Configuration.GetSection("Authorization:Username").Value,
                Password = Configuration.GetSection("Authorization:Password").Value
            };
        }

        public static string GetRcNumber(string env)
        {
            IConfigurationRoot Configuration = Configurator.GetConfiguration(env);
            return Configuration.GetSection("Authorization:RcNumber").Value;
        }

        public static string GetWicketId(string env)
        {
            IConfigurationRoot Configuration = Configurator.GetConfiguration(env);
            return Configuration.GetSection("Authorization:WicketId").Value;
        }
    }
}

using log4net;
using Newtonsoft.Json;
using RestSharp;
using ZephyrTest.Factories;
using ZephyrTest.Models.WebServices;

namespace ZephyrTest.Tools
{
    class AuthManager
    {
        public static ILog Log = LogManager.GetLogger(typeof(AuthManager));

        public static string GetAuthToken()
        {
            WSEnvironment env = ConfigFactory.GetEnvironment("WSConfig");
            IRestClient Client = new RestClient(env.Authorization);
            IRestRequest request = new RestRequest("/Token/CreateToken/", Method.POST)
                .AddQueryParameter("rcNumber", ConfigFactory.GetRcNumber("WSConfig"))
                .AddQueryParameter("wicketId", ConfigFactory.GetWicketId("WSConfig"))
                .AddHeader("Content-type", "application/json")
                .AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(ConfigFactory.GetAuthBody("WSConfig")), ParameterType.RequestBody);
            var response = Client.Execute<WSAuthToken>(request);
            Log.Debug(response.Data.Token);
            return response.Data.Token;
        }
    }
}

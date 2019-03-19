using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using ZephyrTest.Factories;
using ZephyrTest.Models.WebServices;
using ZephyrTest.Tools;


namespace ZephyrTest.Core.Steps
{
    class WebServiceSteps
    {

        private static Dictionary<string, Type> Types => new Dictionary<string, Type>{
            {"Shelf", typeof(Shelf)}
        };

        public static string CreateRequest(string data) => SendRequest(Method.POST, data);

        public static string ReadRequest(string data) => SendRequest(Method.GET, data);

        public static string UpdateRequest(string data) => SendRequest(Method.PUT, data);

        public static string DeleteRequest(string data) => SendRequest(Method.DELETE, data);

        public static string HealthCheck(string data)
        {
            try
            {
                string[] parsedData = data.Split(' ');
                IRestClient client = new RestClient(Configurator.GetConfiguration("LegacyConfig").GetSection(parsedData[0] + ":Url").Value)
                {
                    Authenticator = new HttpBasicAuthenticator(
                        Configurator.GetConfiguration("LegacyConfig").GetSection(parsedData[0] + ":Username").Value,
                        Configurator.GetConfiguration("LegacyConfig").GetSection(parsedData[0] + ":Password").Value)
                };
                Console.WriteLine(client.BaseUrl);
                IRestRequest request = new RestRequest(string.Empty, Method.GET);
                
                IRestResponse response = client.Execute(request);
                Console.WriteLine(JsonConvert.SerializeObject(response));
                return response.StatusCode.ToString("D");
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string ReadAll(string data)
        {
            //try
            //{
                string[] parsedData = data.Split(' ');
                Type type = Types[parsedData[0]];
                IRestClient client = new RestClient(Configurator.GetConfiguration("WSConfig").GetSection("Urls:" + parsedData[0]).Value);
                IRestRequest request = new RestRequest(parsedData[1], Method.GET)
                    .AddHeader("Authorization", "Bearer " + AuthManager.GetAuthToken())
                    .AddHeader("Content-type", "application/json");
                IRestResponse<List<object>> response = client.Execute<List<object>>(request);
                Console.WriteLine(JsonConvert.SerializeObject(response));
                Assert.That(response.Data.Count>0);
                return response.StatusCode.ToString("D");
            //}
            //catch (Exception e)
            //{
            //    return e.Message;
            //}
        }

        public static string SendRequest(Method method, string data)
        {
            try
            {
                string[] parsedData = data.Split(' ');
                Type type = Types[parsedData[1]];
                IWebServiceEntity entity = (IWebServiceEntity)typeof(WebServiceSteps)
                    .GetMethod("GetTestEntity")
                    .MakeGenericMethod(type)
                    .Invoke(null, new object[]{parsedData[0]});
                IRestClient client = new RestClient(Configurator.GetConfiguration("WSConfig").GetSection("Urls:" + parsedData[1]).Value);
                IRestRequest request = new RestRequest(parsedData[2], method)
                    .AddHeader("Authorization", "Bearer " + AuthManager.GetAuthToken())
                    .AddHeader("Content-type", "application/json")
                    .AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(entity), ParameterType.RequestBody)
                    .AddUrlSegment("name", entity.GetName())
                    .AddUrlSegment("id", entity.GetId());
                dynamic response = typeof(WebServiceSteps)
                    .GetMethod("Execute")
                    .MakeGenericMethod(type)
                    .Invoke(null, new object[]{client, request});
                if (response.Data != null)
                {
                    entity.AssertEquals(response.Data);
                }
                return response.StatusCode.ToString("D");
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static T GetTestEntity<T>(string modifier) where T: IWebServiceEntity, new()
        {
            switch (modifier)
            {
                case "new"      : return WSFactory<T>.GenerateEntity<T>();
                case "invalid"  : return WSFactory<T>.InvalidateEntity();
                case "current"  : return WSFactory<T>.GetEntity();
                case "null"     : return WSFactory<T>.CloneEntity();
                case "updated"  : return WSFactory<T>.UpdateEntity();
                default         : return default(T);
            }
        }

        public static IRestResponse<T> Execute<T>(IRestClient client, IRestRequest request) where T: new() =>
            client.Execute<T>(request);

    }
}

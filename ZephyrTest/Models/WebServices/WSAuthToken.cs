using Newtonsoft.Json;

namespace ZephyrTest.Models.WebServices
{
    class WSAuthToken
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}

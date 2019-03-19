using Newtonsoft.Json;

namespace ZephyrTest.Models.WebServices
{
    class WSAuthData
    {
        [JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }
        [JsonProperty(PropertyName = "rcNumber")]
        public string RcNumber { get; set; }
        [JsonProperty(PropertyName = "wicketId")]
        public string WicketId { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}

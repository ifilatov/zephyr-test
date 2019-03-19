using Newtonsoft.Json;

namespace ZephyrTest.Models.Data
{
    class FilePath
    {
        [JsonProperty(PropertyName = "file_path")]
        public string Filename { get; set; }
    }
}

using Newtonsoft.Json;

namespace SchematBlokowy.Application
{
    public class Data
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}

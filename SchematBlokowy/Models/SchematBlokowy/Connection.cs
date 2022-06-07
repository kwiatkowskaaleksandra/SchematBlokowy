using Newtonsoft.Json;

namespace SchematBlokowy.Application
{
    public class Connection
    {
        [JsonProperty("node")]
        public int Node { get; set; }
    }
}

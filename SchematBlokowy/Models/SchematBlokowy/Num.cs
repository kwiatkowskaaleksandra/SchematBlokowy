using Newtonsoft.Json;

namespace SchematBlokowy.Application
{
    public class Num
    {
        [JsonProperty("connections")]
        public Connection[] Connections { get; set; }
    }
}

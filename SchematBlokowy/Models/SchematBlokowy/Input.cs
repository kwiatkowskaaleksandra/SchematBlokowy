using Newtonsoft.Json;

namespace SchematBlokowy.Application
{
    public class Input
    {
        [JsonProperty("num")]
        public Num Num { get; set; }
    }
}

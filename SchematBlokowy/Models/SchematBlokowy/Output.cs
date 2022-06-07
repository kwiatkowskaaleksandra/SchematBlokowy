using Newtonsoft.Json;

namespace SchematBlokowy.Application
{
    public class Output
    {
        [JsonProperty("num")]
        public Num Num { get; set; }

        [JsonProperty("out1")]
        public Num Out1 { get; set; }

        [JsonProperty("out2")]
        public Num Out2 { get; set; }
    }
}

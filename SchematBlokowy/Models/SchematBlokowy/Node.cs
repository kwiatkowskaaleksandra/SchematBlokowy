using Newtonsoft.Json;

namespace SchematBlokowy.Application
{
    public class Node
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("inputs")]
        public Input Inputs { get; set; }

        [JsonProperty("outputs")]
        public Output Outputs { get; set; }

        [JsonProperty("position")]
        public decimal[] Position { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

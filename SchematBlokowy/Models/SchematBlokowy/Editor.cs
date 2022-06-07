using Newtonsoft.Json;
using System.Collections.Generic;

namespace SchematBlokowy.Application
{
    public class Editor
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nodes")]
        public Dictionary<string, Node> Nodes { get; set; }
    }
}

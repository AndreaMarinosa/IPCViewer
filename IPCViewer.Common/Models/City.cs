namespace IPCViewer.Common.Models
{
    using Newtonsoft.Json;

    class City
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

    }
}

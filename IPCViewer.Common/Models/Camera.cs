namespace IPCViewer.Common.Models
{
    using System;
    using Newtonsoft.Json;

    public class Camera
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("imageFullPath")]
        public Uri ImageFullPath { get; set; }

        [JsonProperty("cityId")]
        public int CityId { get; set; }

        public override string ToString()
        {
            return $"{this.Name} {this.Latitude:C6} {this.Longitude:C6}";
        }
    }
}

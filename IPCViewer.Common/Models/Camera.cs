﻿namespace IPCViewer.Common.Models
{
    using Newtonsoft.Json;
    using System;

    public class Camera
    {
        [JsonProperty("id")]
        public int Id { get; set; }

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

        [JsonProperty("city")]
        public City City { get; set; }

        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("imageFullPath")]
        public string ImageFullPath { get; set; }

        public byte[] ImageArray { get; set; }

       
    }
}
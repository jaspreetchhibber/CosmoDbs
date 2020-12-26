using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PlantApp.Core.Photos
{
    public class Photo
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "photoMetaData")]
        public List<PhotoMetaData> PhotoMetaData { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }
    }

}


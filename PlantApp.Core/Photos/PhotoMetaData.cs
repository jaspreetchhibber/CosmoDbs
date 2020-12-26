using Newtonsoft.Json;
using System;

namespace PlantApp.Core.Photos
{
    public class PhotoMetaData
    {

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "speed")]
        public string Speed { get; set; }

    }
}

using Newtonsoft.Json;
using System;

namespace App.Model
{
    public class BaseEntity
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Deleted { get; set; }

        [JsonProperty(PropertyName = "entityType")]
        public string EntityType { get; set; }
    }
}
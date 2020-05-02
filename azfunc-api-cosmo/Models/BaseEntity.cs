using Newtonsoft.Json;
using System;

namespace App.Models
{
    public class BaseEntity
    {
        [JsonIgnore]
        public DateTime Created { get; set; }

        [JsonIgnore]
        public DateTime Updated { get; set; }

        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}
using App.Models.Users;
using Newtonsoft.Json;
using System;

namespace App.Models.Rankings
{
    public class Ranking : BaseEntity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public int Position { get; set; }
        public User Player { get; set; }
        public int Points { get; set; }
        public int TournametsPlayed { get; set; }
    }
}

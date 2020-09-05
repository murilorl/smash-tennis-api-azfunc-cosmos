using Newtonsoft.Json;
using System;

namespace App.Model
{
    public class Tournament : BaseEntity
    {
        private const string EntityTypeTournament = "tournament";

        public Tournament()
        {
            this.EntityType = EntityTypeTournament;
            IsDeleted = false;
        }
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        
        [JsonIgnore]
        public Boolean IsDeleted { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}

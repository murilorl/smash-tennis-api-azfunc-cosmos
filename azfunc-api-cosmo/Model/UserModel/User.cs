using App.Model.UserModel;
using Newtonsoft.Json;
using System;

namespace App.Model
{
    public class User : BaseEntity
    {
        private const string ENTITY_TYPE = "user";

        public User()
        {
            EntityType = ENTITY_TYPE;
            Id = Guid.NewGuid();
            Created = DateTime.Now;
            Updated = DateTime.Now;
            Deleted = false;
        }

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ShortName { get; set; }
        public DateTime? Birthday { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }

        [JsonProperty(PropertyName = "facebookProfile")]
        public FacebookProfile FacebookProfile { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}

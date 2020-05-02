using Newtonsoft.Json;
using System;

namespace App.Models.Users
{
    public class User : BaseEntity
    {
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
        public string FacebookId { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}

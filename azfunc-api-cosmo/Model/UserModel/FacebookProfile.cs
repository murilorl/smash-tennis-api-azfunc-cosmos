using Newtonsoft.Json;
using System;


namespace App.Model.UserModel
{
    public class FacebookProfile
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string ProfilePicURL { get; set; }

        [JsonProperty(PropertyName = "accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expiresAt")]
        public long ExpiresAt { get; set; }


    }
}

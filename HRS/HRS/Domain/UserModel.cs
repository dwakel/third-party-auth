using System;
using System.Text.Json.Serialization;

namespace HRS.Domain
{
    public class UserModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

    }

    public class UserDTO : UserModel
    {
        public string Key { get; set; }
    }
}

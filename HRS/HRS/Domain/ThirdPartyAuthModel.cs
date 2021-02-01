using System;

namespace HRS.Domain
{
    public class ThirdPartyAuthModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Key { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

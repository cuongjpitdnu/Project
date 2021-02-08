using System;

namespace KeyMgnt.Models
{
    public class MUser
    {
        public string UserId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Password { get; set; }

        public short Role { get; set; }

        public string RoleDisplay { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }
    }
}

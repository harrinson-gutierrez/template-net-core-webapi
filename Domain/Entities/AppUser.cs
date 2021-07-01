using Dapper;
using System.Collections.Generic;

namespace Domain.Entities
{
    [Table("users")]
    public class AppUser
    {
        [Key]
        public int user_id { get; set; }

        public string username { get; set; }

        public string email { get; set; }

        public bool email_confirmed { get; set; }

        public string password_hash { get; set; }

        public string security_stamp { get; set; }

        public List<AppRole> Roles { get; set; }

        public string name { get; set; }

        public string last_name { get; set; }

        public int? country_id { get; set; }

        public bool enabled { get; set; }

        public AppUser()
        {
            Roles = new List<AppRole>();
        }
    }
}

using Dapper;

namespace Domain.Entities
{
    [Table("users_logins")]
    public class AppUserLogin
    {
        [Key]
        public string login_provider { get; set; }

        [Key]
        public string provider_key { get; set; }

        public string provider_displayname { get; set; }

        public int user_id { get; set; }
    }
}

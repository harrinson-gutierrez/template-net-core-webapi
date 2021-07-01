using Dapper;
using Domain.Common;
using System;

namespace Domain.Entities
{
    [Table("refresh_tokens")]
    public class RefreshToken : BaseEntity
    {
        [Key]
        public Guid token { get; set; }

        public string jwt_id { get; set; }

        public DateTime creation_date { get; set; }

        public DateTime expired_date { get; set; }

        public bool used { get; set; }

        public bool invalidated { get; set; }

        public int user_id { get; set; }
    }
}

using Dapper;
using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    [Table("roles")]
    public class AppRole : BaseEntity
    {
        [Key]
        public int role_id { get; set; }

        public string role_name { get; set; }

        public List<AppUser> Users { get; set; }
    }
}

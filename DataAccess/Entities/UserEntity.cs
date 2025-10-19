using DomainModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Role RoleOfUser { get; set; }
        public string HashPassword { get; set; }
        public string About {  get; set; }
    }
}

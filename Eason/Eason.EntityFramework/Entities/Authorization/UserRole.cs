using Eason.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eason.EntityFramework.Entities.Authorization
{

    public class UserRole : EasonEntity
    {
        public long userId { get; set; }
        public User user { get; set; }
        public long roleId { get; set; }
        public Role role { get; set; }
    }
}

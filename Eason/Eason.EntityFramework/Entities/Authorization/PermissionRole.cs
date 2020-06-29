using Eason.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eason.EntityFramework.Entities.Authorization
{
    public class PermissionRole : EasonEntity
    {
        public long permissionId { get; set; }

        public Permission permission { get; set; }
        public long roleId { get; set; }
        public Role role { get; set; }

    }
}

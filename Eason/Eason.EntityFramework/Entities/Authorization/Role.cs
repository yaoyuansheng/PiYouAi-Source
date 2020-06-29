using Eason.Domain.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eason.EntityFramework.Entities.Authorization
{

    public class Role : EasonEntity
    {
        [MaxLength(length: 20)]
        public string name { get; set; }

    }
}

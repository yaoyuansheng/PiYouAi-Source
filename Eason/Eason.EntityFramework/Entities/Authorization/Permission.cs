using Eason.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eason.EntityFramework.Entities.Authorization
{
    public class Permission :EasonEntity
    {
        [MaxLength(length: 100)]
        [Required]
        public string url { get; set; }
    }
}

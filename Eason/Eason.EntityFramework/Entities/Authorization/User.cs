using Eason.Domain.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eason.EntityFramework.Entities.Authorization
{

    public class User : Entity<long>
    {
       
        [Required]
        public override long id { get; set; }
        [MaxLength(length: 20)]
        public string name { get; set; }
        [MaxLength(length: 128)]
        [Required]
        public string password { get; set; }
        [MaxLength(length: 50)]
        public string email { get; set; }
        [MaxLength(length: 15)]
        public string telephone { get; set; }
      
        [DataType( DataType.ImageUrl)]
        public string imgUrl { get; set; }
        [MaxLength(length: 50)]
        public string openid { get; set; }
        [DefaultValue(0)]
        [Required]
        public byte isDelete { get; set; }
        public DateTime lastLoginTime { get; set; }
        [DefaultValue(0)]
        public byte isActive { get; set; }
        [DefaultValue(0)]
        public byte isAdmin { get; set; }
        public DateTime creationTime { get; set; }

    }
}

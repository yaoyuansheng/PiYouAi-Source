using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eason.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "用户名不允许为空！")]
      
        [MaxLength(20, ErrorMessage = "用户名不得高于16位")]
        public string account { get; set; }
        [Required(ErrorMessage = "密码不允许为空!")]
        [MinLength(6, ErrorMessage = "密码不得低于6位")]
        [MaxLength(20, ErrorMessage = "密码不得高于20位")]
        public string password { get; set; }
    }
}
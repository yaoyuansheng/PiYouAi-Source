using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eason.Web.Models
{
    public class RegisterModel
    {
        public string name { get; set; }
        [Required(ErrorMessage = "密码不允许为空!")]
        [MinLength(6, ErrorMessage = "密码不得低于6位")]
        [MaxLength(20, ErrorMessage = "密码不得高于20位")]
        public string password { get; set; }
        public string email { get; set; }
        public string openid { get; set; }
        [Required(ErrorMessage = "手机号不允许为空!")]
        public string telephone { get; set; }

        public string imgUrl { get; set; }
        [Required(ErrorMessage = "请输入短信验证码!")]
        public int code { get; set; }
        /// <summary>
        /// 0 普通注册
        /// 1微信注册
        /// </summary>
        /// <value>From.</value>
        public int from { get; set; }
    }
}
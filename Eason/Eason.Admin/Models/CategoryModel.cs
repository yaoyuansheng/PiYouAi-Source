using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eason.Admin.Models
{
    public class CategoryModel
    {
        [Required( ErrorMessage ="请输入标题")]
        [MinLength(2,ErrorMessage ="分类最低两个文字")]
        [MaxLength(20, ErrorMessage = "分类最多20文字")]
        public string title { get; set; }
     
        public long parentId { get; set; }

        public long id { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Eason.Core.News
{
    public class PublishArticle
    {
        public long id { get; set; }
        [Required]
        public string title { get; set; }
      
        [AllowHtml]
        public string contents { get; set; }
        [Required]
        public long categoryId { get; set; }
        // [Required]
        // public long typeId { get; set; }

        [Required]
        public byte status { get; set; }
        [MaxLength(100)]
        public string mTitle { get; set; }

        [DataType(DataType.ImageUrl)]
        public string imageUrl { get; set; }
        [AllowHtml]

        public string videoUrl { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(length: 300)]
        public string desc { get; set; }
        [DataType(DataType.Url)]
        public string outLink { get; set; }



    }
}

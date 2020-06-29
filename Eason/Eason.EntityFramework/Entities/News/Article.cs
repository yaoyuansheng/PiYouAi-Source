using Eason.Domain.Entities;
using Eason.EntityFramework.Entities.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eason.EntityFramework.Entities.News
{
    public class Article : EasonEntity
    {
        [MaxLength(100)]
        public string title { get; set; }
        [MaxLength(100)]
        public string mTitle { get; set; }

        [DataType(DataType.ImageUrl)]
        public string imageUrl { get; set; }

        [DataType(DataType.Html)]
        public string videoUrl { get; set; }
        [DataType(DataType.Url)]
        public string outLink { get; set; }
        [MaxLength(length: 300)]

        [DataType(DataType.MultilineText)]
        public string desc { get; set; }

        [DataType(DataType.Html)]
        public string contents { get; set; }

        public ArticleCategory category { get; set; }

        public long categoryId { get; set; }
        public string categoryName { get; set; }

        public long readNum { get; set; }

        // public ArticleChannel channel { get; set; }
        //  public long channelId { get; set; }

        // public ArticleType type { get; set; }
        // public long typeId { get; set; }




    }
}

using Eason.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eason.EntityFramework.Entities.News
{
    public class ArticleComment : EasonEntity
    {
        public long parentId { get; set; }
        [DataType(DataType.MultilineText)]
        public string contents { get; set; }
        //  public Article article { get; set; }

        public long articleId { get; set; }
        [DataType(DataType.ImageUrl)]
        public string imgUrl { get; set; }

    }
}

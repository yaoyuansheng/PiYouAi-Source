using Eason.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eason.EntityFramework.Entities.News
{
    public class ArticleCategory : EasonEntity
    {
        [MaxLength(length: 20)]
        public string title { get; set; }
        public int sort { get; set; }

        public int depth { get; set; }
        public long parentId { get; set; }
    }
}

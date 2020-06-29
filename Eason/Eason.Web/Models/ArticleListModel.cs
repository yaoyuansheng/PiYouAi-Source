using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eason.Web.Models
{
    public class ArticleListModel
    {
        public long Id { get; set; }
        public long CategoryCode { get; set; }
        public string PicUrl { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string mTitle { get; set; }

        public string Author { get; set; }
        public long AuthorCode { get; set; }
        public string OutLink { get; set; }
        public string Time { get; set; }
        public string TitleClass { get; set; }
    }
}
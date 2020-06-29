using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eason.Web.Models
{
    public class BannerModel
    {
        public long Id { get; set; }
        public string PicUrl { get; set; }
        public string Title { get; set; }
        public string OutLink { get; set; }
        public string Time { get; set; }
        public string TitleClass { get; set; }
    }
}
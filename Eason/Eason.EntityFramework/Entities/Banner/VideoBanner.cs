using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eason.EntityFramework.Entities.Banner
{
    public class VideoBanner: EasonEntity
    {
        public string title { get; set; }
        public string bpic { get; set; }
        public string burl { get; set; }
    }
}

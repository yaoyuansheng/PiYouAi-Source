using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eason.EntityFramework.Entities.School
{
    public class Teacher : EasonEntity
    {
        // 导师图片
        public string dpic { get; set; }
        //导师名字
        public string dname { get; set; }
        //导师昵称
        public string dnichen { get; set; }
        //导师级别
        public string djibie { get; set; }
        //导师介绍
        public string djies { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eason.EntityFramework.Entities.School
{
    public class Course : EasonEntity
    {
        //课程图片 
        public string kpic { get; set; }
        //课程标题 
        public string ktitle { get; set; }
        //课程外链
        public string kurl { get; set; }
        //课程学习人数
        public string kpop { get; set; }
        //(这里是数组 返回最多6张图片)
        //课程小图展示 
        public string kspic { get; set; }

        //课程导师名字
        public string kteach { get; set; }
        //课程时节 
        public string kshi { get; set; }
        //课程描述 
        public string kmiao { get; set; }

        //课程价位 
        public string kmony { get; set; }
        /// <summary>
        ///课程类型
        /// </summary>
        /// <value>The ty.</value>
        public string ty { get; set; }
        //(//3种状态
        //1. 988.00
        //2. 5000.00 - 32000.00
        //3. 8000.00/学员价 8000.00
    }
}

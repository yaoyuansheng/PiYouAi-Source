using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eason.Web.Models
{
    public class ResultModel
    {
        public int Code { get; set; }
        public string Message { get; set; }


        public long allpageNum { get; set; }

        public Object Data { get; set; }
        public Object ChildName { get; set; }
    }
}
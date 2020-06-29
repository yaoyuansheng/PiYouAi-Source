using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eason.EntityFramework.Entities.Message
{
    public class ShortMessage : EasonEntity
    {
        [MaxLength(15)]
        public string telephone { get; set; }
        [MaxLength(200)]
        public string content { get; set; }
       
        public int code { get; set; }
        [MaxLength(20)]
        public string ip { get; set; }
    }
}

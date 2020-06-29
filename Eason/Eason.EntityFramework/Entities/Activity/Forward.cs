using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eason.EntityFramework.Entities.Activity
{
    public class Forward:EasonEntity
    {
     
        [MaxLength(length: 200)]
       
        public string hdname { get; set; }
        public int hdnum { get; set; }
        
    }
}

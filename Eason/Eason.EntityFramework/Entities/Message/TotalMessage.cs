using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eason.EntityFramework.Entities.Message
{
    public class TotalMessage:EasonEntity
    {
        /// <summary>
        /// 充值数量
        /// </summary>
        /// <value>The count.</value>
        public int count { get; set; }
        /// <summary>
        /// 充值备注
        /// </summary>
        /// <value>The remark.</value>
        public string remark { get; set; }
    }
}

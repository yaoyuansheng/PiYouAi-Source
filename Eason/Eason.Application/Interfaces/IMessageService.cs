using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eason.Application.Interfaces
{
    public interface IMessageService
    {
        void Register(string code, string telephone);
    }
}

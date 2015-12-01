using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Common.Message
{
    class HelloMessage
    {
        public string Message { get; private set; }

        public HelloMessage(string msg)
        {
            Message = msg;
        }
        
    }
}

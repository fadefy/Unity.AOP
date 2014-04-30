using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.AOP.Demo.Services
{
    public interface ISoapSender
    {
        void Send(SoapRequest request);
    }
}

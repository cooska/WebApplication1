using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Service
{
    public interface ISmsSend
    {
        void SendSms(string mobile);
    }
}

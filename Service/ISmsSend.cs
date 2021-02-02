using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Service
{
    public interface ISmsSend
    {
        string SendSms(string mobile);
        string SendCmSms(string mobile);
    }
}

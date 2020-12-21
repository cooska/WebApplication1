using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapi.Service
{
    public interface ISmsSend
    {
        string SendSms(string mobile);
    }
}

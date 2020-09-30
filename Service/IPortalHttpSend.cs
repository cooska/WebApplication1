using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Service {
    public interface IPortalHttpSend<in TReq, TResp>
        where TReq : IReqBase
        where TResp : IRespBase {
        /// <summary>
        /// 同步接口调用
        /// </summary>
        /// <param name="req">请求</param>
        /// <param name="resp">响应</param>
        /// <param name="returnMsg">错误消息</param>
        /// <returns>flase 调用失败,true调用成功</returns>
        bool GetJsonData(TReq req, out TResp resp);
    }
}

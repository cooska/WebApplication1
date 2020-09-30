using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.Attributes;

namespace WebApplication1.Service {
    public class PortalHttpSendService<TReq, TResp> : IPortalHttpSend<TReq, TResp>
    where TReq : IReqBase
    where TResp : IRespBase {
        //开发环境：https://devwisp.sunjee.cn
        //测试环境：https://wisp.sunjee.cn
        private readonly string _domainUrl = "https://wisp.sunjee.cn";
        public bool GetJsonData(TReq req, out TResp resp) {
            var attr = GetAttribute(req.GetType());
            if (attr == null) {
                resp = default;
                return false; 
            }
            return Send<TReq, TResp>(req, attr.PortalName, out resp, new WebHeaderCollection(), attr.MethodEnum);
        }

        private bool Send<TReq, TResp>(TReq req, string portalUrl, out TResp resp, WebHeaderCollection headers, HttpMethodEnum method)
    where TReq : IReqBase
    where TResp : IRespBase {
            string reqJson = JsonConvert.SerializeObject(req);//如果请求体为对象，则需转换成json语句
            string returnJson = null;
                returnJson = SendHttpRequest(portalUrl, reqJson, headers, method.ToString());
            resp = JsonConvert.DeserializeObject<TResp>(returnJson);
            //入库
            return resp != null;
        }

        private string SendHttpRequest(string url, string req, WebHeaderCollection webheader, string method) {

            string resultJson;
            try {
                System.Net.ServicePointManager.Expect100Continue = false;
                byte[] postData = Encoding.UTF8.GetBytes(req); // 将提交的字符串数据转换成字节数组 

                if (method == "GET") {
                    var list = JsonConvert.DeserializeObject<Dictionary<string, string>>(req);
                    string retStr = "";
                    foreach (var key in list) {
                        retStr += string.Format("{0}={1}&", key.Key.Replace("_", "."), key.Value);
                    }
                    retStr = retStr.TrimEnd('&');
                    url = url.Contains("?") ? url + retStr : url + "?" + retStr;
                    url = url.TrimEnd('?');
                } else if (method.Contains("LINE")) {
                    var list = JsonConvert.DeserializeObject<Dictionary<string, string>>(req);
                    foreach (var key in list) {
                        url = url.Replace("{" + key.Key + "}", key.Value);
                    }
                }



                var request = WebRequest.Create(url) as HttpWebRequest;
                if (request != null) {
                    request.Headers = webheader;//指定请求头

                    request.Timeout = 50000;
                    request.Method = method.Replace("LINE", "");
                    request.KeepAlive = false;
                    request.AllowAutoRedirect = true;
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; " +
                                        ".NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";

                    //读写超时
                    request.ReadWriteTimeout = 10000;
                    #region 提交请求数据
                    if (method == "POST") {
                        request.ContentType = "application/json; charset=UTF-8";
                        request.ContentLength = postData.Length;
                        //读写超时
                        #region 提交请求数据
                        using (Stream outputStream = request.GetRequestStream()) {
                            outputStream.Write(postData, 0, postData.Length);
                            outputStream.Close();
                        }
                        #endregion
                    }
                    #endregion
                }


                #region 获取响应json
                HttpWebResponse response;
                try { response = (HttpWebResponse)request.GetResponse(); } catch (WebException ex) { response = (HttpWebResponse)ex.Response; }
                using (Stream responseStream = response.GetResponseStream()) {
                    //尝试修改流不可读的问题
                    if (!response.GetResponseStream().CanRead) {
                        resultJson = "{\"code\":-1,\"message\":\"流不可读\"}";
                    } else {
                        using (var reader = new StreamReader(responseStream, Encoding.UTF8)) {
                            resultJson = reader.ReadToEnd();
                        }
                    }
                    responseStream.Flush();
                }
                #endregion
            } catch (Exception ex) {
                resultJson = "{\"code\":-1,\"message\":\"" + ex.Message + "\"}";
            }
            return resultJson;
        }

        public AttributeModel GetAttribute(Type req) {
            var gi = req.GetInterface("IReqBase");
            if (gi !=null) {
                var attrList = Attribute.GetCustomAttributes(req);
                foreach (var attr in attrList) {
                    var resourceEnvironmentAttribute = attr as PortalAttribute;
                    if (resourceEnvironmentAttribute == null) {
                        continue;
                    }
                    var protalInfo = new AttributeModel {
                        PortalName = _domainUrl + "/" + resourceEnvironmentAttribute.ProtalName.Trim('/'),
                        MethodEnum = resourceEnvironmentAttribute.HttpMethod

                    };
                    return protalInfo;
                }
            }
            return null;
        }

    }
}

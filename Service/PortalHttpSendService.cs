using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.Sig;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.Attributes;
using WebApplication1.Tools;

namespace WebApplication1.Service {
    public class PortalHttpSendService<TReq, TResp> : IPortalHttpSend<TReq, TResp>
    where TReq : IReqBase
    where TResp : IRespBase {
        //开发环境：https://devwisp.sunjee.cn
        //测试环境：https://wisp.sunjee.cn
        private readonly string _domainUrl = "https://qyapi.weixin.qq.com";
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

                var list = JsonConvert.DeserializeObject<Dictionary<string, object>>(req);
                if (method == "GET") {
                    string retStr = "";
                    foreach (var key in list) {
                        retStr += string.Format("{0}={1}&", key.Key, key.Value);
                    }
                    retStr = retStr.TrimEnd('&');
                    url = url.Contains("?") ? url + retStr : url + "?" + retStr;
                    url = url.TrimEnd('?');
                } else if (method.Contains("LINE")) {
                    foreach (var key in list) {
                        if (key.Value != null)
                            url = url.Replace("{" + key.Key + "}", key.Value.ToString());
                    }
                }
                    list.Remove("access_token");
                    list.Remove("errcode");
                    list.Remove("errmsg");
                    var datastr = JsonConvert.SerializeObject(list);
                    postData = Encoding.UTF8.GetBytes(datastr);
//#if DEBUG
//                url += "&debug=1";
//#endif



                var request = WebRequest.Create(url) as HttpWebRequest;
                if (request != null) {
                    request.Method = method.Replace("LINE", "");
                    #region 提交请求数据
                    if (method.Contains("POST")) {
                        request.ContentType = "application/json; charset=UTF-8";
                        request.ContentLength = postData.Length;
                        
                        if (url.Contains("ims.jsu.edu.cn")) {
                            webheader.Add("Accept", "*/*");
                            webheader.Add("apikey", "portal");
                            webheader.Add("timestamp", DateTime.UtcNow.Ticks.ToString());
                            var sign = SignatureHelper.createSignature(webheader, "/ims/api/account/update/" + list["userid"] + "/" + "userpassword",
                            "MIIBSwIBADCCASwGByqGSM44BAEwggEfAoGBAP1_U4EddRIpUt9KnC7s5Of2EbdSPO9EAMMeP4C2USZpRV1AIlH7WT2NWPq_xfW6MPbLm1Vs14E7gB00b_JmYLdrmVClpJ-f6AR7ECLCT7up1_63xhv4O1fnxqimFQ8E-4P208UewwI1VBNaFpEy9nXzrith1yrv8iIDGZ3RSAHHAhUAl2BQjxUjC8yykrmCouuEC_BYHPUCgYEA9-GghdabPd7LvKtcNrhXuXmUr7v6OuqC-VdMCz0HgmdRWVeOutRZT-ZxBxCBgLRJFnEj6EwoFhO3zwkyjMim4TwWeotUfI0o4KOuHiuzpnWRbqN_C_ohNWLx-2J6ASQ7zKTxvqhRkImog9_hWuWfBpKLZl6Ae1UlZAFMO_7PSSoEFgIUKCKbCTC3wcDaQv25MjXrv4vtiF0");
                            webheader.Add("signature", sign);
                        }

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

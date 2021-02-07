using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Tools;

namespace WebApplication1.Service
{
    public class SmsSendServer : ISmsSend
    {
        private string GetVerify(int length) {
            // 随机生成6位验证码
            var rd = new Random();
            var code = rd.Next(100000, 999999);
            return code.ToString();
        }
        public string SendSms(string mobile)
        {
            // accesskeyId、secret对应你的阿里云产品id
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", "LTAI4FyC1dXXSskjcDZCCbgj", "Z6NcYfqJgDMmq3oW1TgsjarWoQhe0M");
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP
            // 随机生成6位验证码
            var rd = new Random();
            var code = new
            {
                code = rd.Next(100000, 999999)
            };
            request.AddQueryParameters("PhoneNumbers", mobile);
            request.AddQueryParameters("SignName", "信息网络中心");
            //request.AddQueryParameters("TemplateCode", "SMS_204110082");
            request.AddQueryParameters("TemplateCode", "SMS_207496185");
            // 验证码参数，code 转json格式
            request.AddBodyParameters("TemplateParam", JsonConvert.SerializeObject(code));
            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                var data = JsonConvert.DeserializeObject<AliSmsResponse>(response.Data);
                if (data.Code == "OK")
                    //Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
                    return code.code.ToString();
                else
                    return "";
            }
            catch (ServerException e)
            {
                Console.WriteLine(e);
                return "";
            }
            catch (ClientException e)
            {
                Console.WriteLine(e);
                return "";
            }
        }

        public string SendCmSms(string mobile) {
            WeInfo.WsSmsService wsSms = new WeInfo.WsSmsServiceClient();
            var verify = GetVerify(0);
            var req = new CMSmsRequest {
                apId = "usereg",
                secretKey = "Reg@202102041103",
                sign = "Sc9pavWGe",
                templateId = "bb17998bd477441184703717bce54e47",
                ecName = "吉首大学",
                mobiles = new List<Cstring> { new Cstring { @string = mobile} },
                @params = new List<Cstring> { new Cstring { @string =  verify} },
                addSerial = ""
            };
            try { 
                var md5str = req.ecName + req.apId + req.secretKey + req.templateId + req.mobiles[0].@string + req.@params[0].@string + req.sign + req.addSerial;
                var mac = GetMD5(md5str);
                req.mac = mac;
                var xml = XmlHelper.Serialize(req);
                var res = wsSms.sendTplSmsAsync(new WeInfo.sendTplSmsRequest {
                    Body = new WeInfo.sendTplSmsRequestBody {
                        arg0 = xml
                    }
                });
                var ret = res.Result.Body.@return;
                var retobj = XmlHelper.Deserialize<CMSmsResponse>(ret);
                if (retobj.success != "true")
                    return "";
                return verify;
            } catch(Exception ex) {
                return "";
            }
        }

        private string GetMD5(string str) {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            string result = BitConverter.ToString(md5.ComputeHash(bytes));
            return result.Replace("-", "").ToLower();
        }
    }
}

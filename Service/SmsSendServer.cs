using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class SmsSendServer : ISmsSend
    {
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
    }
}

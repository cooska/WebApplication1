﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.WeInfo;

namespace WebApplication1.Service {
    public class WeInfoService {
        /// <summary>
        /// 获取token授权
        /// </summary>
        /// <returns></returns>
        public static GetDevTokenResp GetToken() {
            GetDevTokenResp resp;
            var serve = new PortalHttpSendService<GetDevTokenReq, GetDevTokenResp>();
            serve.GetJsonData(new GetDevTokenReq {
                 corpid = "ww808efd4c41a2f2b9",
                 corpsecret = "_AMn3g47MHjGP5ZFpTHGxsyp0hxB3oNPxoCB4JEW7HI"
            }, out resp);
            return resp;
        }

        public static GetUserInfoResp GetUserInfo(string token, string uid) {
            GetUserInfoResp resp;
            var serve = new PortalHttpSendService<GetUserInfoReq, GetUserInfoResp>();
            serve.GetJsonData(new GetUserInfoReq {
                access_token = token,
                userid = uid
            }, out resp);
            return resp;
        }

        public static bool CreateUserInfo(AddUserReq user) {
            AddUserResp resp;
            var serve = new PortalHttpSendService<AddUserReq, AddUserResp>();
            serve.GetJsonData(user, out resp);
            return resp != null && resp.errcode ==0;
        }


        public static bool UpdateUserInfo(UpdateUserInfoReq user) {
            UpdateUserInfoResp resp;
            var serve = new PortalHttpSendService<UpdateUserInfoReq, UpdateUserInfoResp>();
            serve.GetJsonData(user, out resp);
            return resp != null && resp.errcode == 0;
        }

    }
}

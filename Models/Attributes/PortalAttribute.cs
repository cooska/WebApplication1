using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapi.Models.Attributes {

    public enum HttpMethodEnum {
        POST,
        GET,
        PUT,
        UPDATE,
        DELETE,
        /// <summary>
        /// post方法提交，url用参数路由
        /// </summary>
        POSTLINE,
        /// <summary>
        /// get方法提交，url用参数路由
        /// </summary>
        GETLINE
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class PortalAttribute : Attribute {
        /// <summary>
        /// 接口文档上的接口名称
        /// </summary>
        public string ProtalName { get; set; }
        /// <summary>
        /// 是否在授课预览是过滤该接口
        /// </summary>
        public bool IsFilterWhenPreivewTeach { get; set; }

        public HttpMethodEnum HttpMethod { get; set; }

        // This is a positional argument
        public PortalAttribute() {
            HttpMethod = HttpMethodEnum.POST;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bus.Core
{
    /// <summary>
    /// 输出Ajax Json类
    /// </summary>
    public class AjaxJson
    {
        #region 构造函数
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 返回的消息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 执行完后，Callback函数
        /// </summary>
        public string callback { get; set; }
        /// <summary>
        /// 扩展输出信息1
        /// </summary>
        public string expand1 { get; set; }
        /// <summary>
        /// 扩展输出信息2
        /// </summary>
        public string expand2 { get; set; }
        /// <summary>
        /// 扩展输出信息3
        /// </summary>
        public string expand3 { get; set; }
        #endregion
        public AjaxJson() { }
        /// <summary>
        /// 初始化类
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        /// <param name="expand1"></param>
        /// <param name="expand2"></param>
        /// <param name="expand3"></param>
        public AjaxJson(bool success, string message, string callback, string expand1, string expand2, string expand3)
        {
            this.callback = callback;
            this.expand1 = expand1;
            this.expand2 = expand2;
            this.expand3 = expand3;
            this.message = message;
            this.success = success;
        }
        /// <summary>
        /// 输出JSON格式文件
        /// </summary>
        public void Write()
        {
            if (string.IsNullOrEmpty(this.message))
            {
                //如果没有传入Message
                this.message = (this.success) ? "操作成功." : "操作失败.";
            }
            string jsonFormat = "{\"success\":" + this.success.ToString().ToLower() + ",\"message\":\"" + this.message + "\",\"callback\":\"" + this.callback + "\",\"expand1\":\"" + this.expand1 + "\",\"expand2\":\"" + expand2 + "\",\"expand3\":\"" + expand3 + "\"}";
            try
            {
                System.Web.HttpContext.Current.ApplicationInstance.Response.Clear();
                System.Web.HttpContext.Current.Response.Write(jsonFormat);
            }
            finally
            {
                System.Web.HttpContext.Current.Response.End();
            }
        }
    }
}

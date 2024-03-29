﻿using System;
using System.IO;
using System.Text;
using Senparc.Weixin.MP.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Helpers;
using Bus.Weixin.CommonService;

namespace Bus.WeiXin.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public partial class CustomMessageHandler : MessageHandler<MessageContext>
    {
        /*
         * 重要提示：v1.5起，MessageHandler提供了一个DefaultResponseMessage的抽象方法，
         * DefaultResponseMessage必须在子类中重写，用于返回没有处理过的消息类型（也可以用于默认消息，如帮助信息等）；
         * 其中所有原OnXX的抽象方法已经都改为虚方法，可以不必每个都重写。若不重写，默认返回DefaultResponseMessage方法中的结果。
         */

        public CustomMessageHandler(Stream inputStream)
            : base(inputStream)
        {
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            //比如MessageHandler<MessageContext>.GlobalWeixinContext.ExpireMinutes = 3。
            WeixinContext.ExpireMinutes = 3;
        }

        public override void OnExecuting()
        {
            //测试MessageContext.StorageData
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
            base.OnExecuting();
        }

        public override void OnExecuted()
        {
            base.OnExecuted();
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            //TODO:这里的逻辑可以交给Service处理具体信息，参考OnLocationRequest方法或/Service/LocationSercice.cs

            //方法一（v0.1），此方法调用太过繁琐，已过时（但仍是所有方法的核心基础），建议使用方法二到四
            //var responseMessage =
            //    ResponseMessageBase.CreateFromRequestMessage(RequestMessage, ResponseMsgType.Text) as
            //    ResponseMessageText;

            //方法二（v0.4）
            //var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(RequestMessage);

            //方法三（v0.4），扩展方法，需要using Senparc.Weixin.MP.Helpers;
            //var responseMessage = RequestMessage.CreateResponseMessage<ResponseMessageText>();

            //方法四（v0.6+），仅适合在HandlerMessage内部使用，本质上是对方法三的封装
            //注意：下面泛型ResponseMessageText即返回给客户端的类型，可以根据自己的需要填写ResponseMessageNews等不同类型。
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            if (requestMessage.Content == "约束")
            {
                responseMessage.Content = "<a href=\"http://weixin.senparc.com/FilterTest/\">点击这里</a>进行客户端约束测试（地址：http://weixin.senparc.com/FilterTest/）。";
            }
            else
            {
                var content = requestMessage.Content;
                var list = Common.queryLine(content);
                if (list.Count > 0) {
                    var news = CreateResponseMessage<ResponseMessageNews>();
                    var index = 1;
                    foreach (var item in list)
                    {
                        if (index <= 8)
                        {
                            news.Articles.Add(new Article()
                            {
                                Title = item.LineName,
                                Description = "",
                                PicUrl = "http://www.orderbus.cn/Content/orderbus.jpg",
                                Url = "http://www.orderbus.cn/Home/BusLineInfo?id="+item.ID
                            });
                        }
                        index++;
                    }
                    return news;
                }
                else
                {
                    responseMessage.Content = "没有查询到有关线路.";
                }
                //string result = "";
                //System.IO.StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("/")+"/result.txt");
                //result = sr.ReadToEnd();
                //sr.Close();
                //responseMessage.Content = result;
                //var result = new StringBuilder();
                //result.AppendFormat("您刚才发送了文字信息：{0}\r\n\r\n", requestMessage.Content);

                //if (CurrentMessageContext.RequestMessages.Count > 1)
                //{
                //    result.AppendFormat("您刚才还发送了如下消息（{0}）：\r\n", CurrentMessageContext.StorageData);
                //    for (int i = CurrentMessageContext.RequestMessages.Count - 2; i >= 0; i--)
                //    {
                //        var historyMessage = CurrentMessageContext.RequestMessages[i];
                //        result.AppendFormat("{0} 【{1}】{2}\r\n",
                //                            historyMessage.CreateTime.ToShortTimeString(),
                //                            historyMessage.MsgType.ToString(),
                //                            (historyMessage is RequestMessageText)
                //                                ? (historyMessage as RequestMessageText).Content
                //                                : "[非文字类型]"
                //            );
                //    }
                //    result.AppendLine("\r\n");
                //}

                //result.AppendFormat("如果您在{0}分钟内连续发送消息，记录将被自动保留。过期后记录将会自动清除。\r\n", WeixinContext.ExpireMinutes);
                //result.AppendLine("\r\n");
                //result.AppendLine("您还可以发送【位置】【图片】【语音】等类型的信息，查看不同格式的回复。\r\nSDK官方地址：http://weixin.senparc.com");

                //responseMessage.Content = result.ToString();
            }
            return responseMessage;
        }

        /// <summary>
        /// 处理位置请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var locationService = new LocationService();
            var responseMessage = locationService.GetResponseMessage(requestMessage as RequestMessageLocation);
            return responseMessage;
        }

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageNews>();
            responseMessage.Articles.Add(new Article()
            {
                Title = "您刚才发送了图片信息",
                Description = "您发送的图片将会显示在边上",
                PicUrl = requestMessage.PicUrl,
                Url = ""
            });
            //responseMessage.Articles.Add(new Article()
            //{
            //    Title = "第二条",
            //    Description = "第二条带连接的内容",
            //    PicUrl = requestMessage.PicUrl,
            //    Url = "http://weixin.senparc.com"
            //});
            return responseMessage;
        }

        /// <summary>
        /// 处理语音请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageMusic>();
            responseMessage.Music.MusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
            return responseMessage;
        }

        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = string.Format(@"您发送了一条连接信息：
Title：{0}
Description:{1}
Url:{2}", requestMessage.Title, requestMessage.Description, requestMessage.Url);
            return responseMessage;
        }

        /// <summary>
        /// 处理事件请求（这个方法一般不用重写，这里仅作为示例出现。除非需要在判断具体Event类型以外对Event信息进行统一操作
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEventRequest(RequestMessageEventBase requestMessage)
        {
            var eventResponseMessage = base.OnEventRequest(requestMessage);//对于Event下属分类的重写方法，见：CustomerMessageHandler_Events.cs
            //TODO: 对Event信息进行统一操作
            return eventResponseMessage;
        }


        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这条消息来自DefaultResponseMessage。";
            return responseMessage;
        }
    }
}

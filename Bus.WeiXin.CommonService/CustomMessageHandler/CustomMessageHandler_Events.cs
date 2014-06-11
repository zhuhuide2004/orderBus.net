using System;
using System.Diagnostics;
using System.Web;
using Senparc.Weixin.MP.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using TheCMS.Linq;

namespace Bus.WeiXin.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler : MessageHandler<MessageContext>
    {
        string SiteUrl = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"].ToString();
        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase reponseMessage = null;
            var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            reponseMessage = strongResponseMessage;
            if (requestMessage.EventKey == "keyquery")
            {
                strongResponseMessage.Content = "请点击微信左下角图标，在输入框输入要查询线路的关键字即可搜索.";
            }
            else if (requestMessage.EventKey == "busmap")
            {
                strongResponseMessage.Content = "乘车站点地图正在开发..";
            }
            else if (requestMessage.EventKey == "editmap")
            {
                var _strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                reponseMessage = _strongResponseMessage;
                _strongResponseMessage.Articles.Add(new Article()
                {
                    Title = "设置您的乘车位置.",
                    Description = "",
                    PicUrl = "http://www.orderbus.cn/editmap.jpg",
                    Url = "http://www.orderbus.cn/imap?k="+requestMessage.FromUserName
                });
            }
            else
            {
                strongResponseMessage.Content = "对不起，此功能暂时不能使用，菜单功能开发中。。。。。";
            }
            //菜单点击，需要跟创建菜单时的Key匹配
            //switch (requestMessage.EventKey)
            //{
            //    case "OneClick":
            //        {
            //            var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            //            reponseMessage = strongResponseMessage;
            //            strongResponseMessage.Content = "您点击了底部按钮。";
            //        }
            //        break;
            //    case "SubClickRoot_Text":
            //        {
            //            var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
            //            reponseMessage = strongResponseMessage;
            //            strongResponseMessage.Content = "您点击了子菜单按钮。";
            //        }
            //        break;
            //    case "SubClickRoot_News":
            //        {
            //            var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
            //            reponseMessage = strongResponseMessage;
            //            strongResponseMessage.Articles.Add(new Article()
            //            {
            //                Title = "您点击了子菜单图文按钮",
            //                Description = "您点击了子菜单图文按钮，这是一条图文信息。",
            //                PicUrl = "http://weixin.senparc.com/Images/qrcode.jpg",
            //                Url = "http://weixin.senparc.com"
            //            });
            //        }
            //        break;
            //    case "SubClickRoot_Music":
            //        {
            //            var strongResponseMessage = CreateResponseMessage<ResponseMessageMusic>();
            //            reponseMessage = strongResponseMessage;
            //            strongResponseMessage.Music.MusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
            //        }
            //        break;
            //}

            return reponseMessage;
        }

        public override IResponseMessageBase OnEvent_EnterRequest(RequestMessageEvent_Enter requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "您刚才发送了ENTER事件请求。";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            throw new Exception("暂不可用");
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            //var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);

            #region 订阅
            var OpenID = requestMessage.FromUserName;
            var q = QueryBuilder.Create<Data.WXUsers>().Equals(x => x.OpenID, OpenID);
            var model = Data.WXUsersDB.GETWXUsers(q);
            if (model == null)
            {
                //=不存在该用户
                //var umodel = Common.GetUserInfo(OpenID);
                var _model = new Data.WXUsers();
                _model.CreateTime = DateTime.Now;
                _model.OpenID = OpenID;
                _model.Sex = 3;
                var resultid = Data.WXUsersDB.AddWXUsers(_model);
                if (resultid > 0)
                {
                    //订阅成功，退出图文信息。
                    var art = Data.MyContentDB.GETMyContent("reg_myprofile");
                    var res = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
                    res.Articles.Add(new Article { Title = art.Title, Description = art.SubContent, PicUrl = SiteUrl + art.Thumb, Url = art.LinkUrl + "?WxID=" + resultid });
                    res.ToUserName = OpenID;
                    return res;
                }
                else
                {
                    //订阅不成功
                    var strongResponseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
                    strongResponseMessage.Content = "发生异常，订阅不成功。";
                    return strongResponseMessage;
                }
            }
            else
            {
                //已经存在该用户
                var strongResponseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
                strongResponseMessage.Content = "您已经关注了，不需要重新关注。";
                return strongResponseMessage;
            }
            #endregion

            //return responseMessage;
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "有空再来";
            return responseMessage;
        }
    }
}

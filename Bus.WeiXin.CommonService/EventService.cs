using System;
using System.Diagnostics;
using System.Web;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP;
using TheCMS.Linq;

namespace Bus.WeiXin.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 事件处理程序，此代码的简化MessageHandler方法已由/CustomerMessageHandler/CustomerMessageHandler_Event.cs完成，
    /// 此文件不再更新。
    /// </summary>
    public class EventService
    {
        public ResponseMessageBase GetResponseMessage(RequestMessageEventBase requestMessage)
        {
            string SiteUrl = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"].ToString();
            ResponseMessageBase responseMessage = null;
            switch (requestMessage.Event)
            {
                case Event.ENTER:
                    {
                        var strongResponseMessage = requestMessage.CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "您刚才发送了ENTER事件请求。";
                        responseMessage = strongResponseMessage;
                        break;
                    }
                case Event.LOCATION:
                    throw new Exception("暂不可用");
                //break;
                case Event.subscribe://订阅
                    {
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
                            var resultid=Data.WXUsersDB.AddWXUsers(_model);
                            if (resultid > 0)
                            {
                                //订阅成功，退出图文信息。
                                var art = Data.MyContentDB.GETMyContent("reg_myprofile");
                                var res = requestMessage.CreateResponseMessage<ResponseMessageNews>();
                                res.Articles.Add(new Article { Title = art.Title, Description = art.SubContent, PicUrl = SiteUrl+art.Thumb, Url = art.LinkUrl+"?WxID="+resultid });
                                res.ToUserName = OpenID;
                                return res;
                            }
                            else
                            {
                             //订阅不成功
                                var strongResponseMessage = requestMessage.CreateResponseMessage<ResponseMessageText>();
                                strongResponseMessage.Content = "发生异常，订阅不成功。";
                                return strongResponseMessage;
                            }
                        }
                        else
                        {
                            //已经存在该用户
                            var strongResponseMessage = requestMessage.CreateResponseMessage<ResponseMessageText>();
                            strongResponseMessage.Content = "您已经关注了，不需要重新关注。";
                            return strongResponseMessage;
                        }
                        #endregion
                        break;
                    }
                case Event.unsubscribe://退订
                    {
                        //实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
                        //unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。
                        var strongResponseMessage = requestMessage.CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "有空再来";
                        responseMessage = strongResponseMessage;
                        break;
                    }
                case Event.CLICK://菜单点击事件，根据自己需要修改
                    throw new Exception("Demo中还没有加入CLICK的测试！");
                //break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return responseMessage;
        }
    }
}
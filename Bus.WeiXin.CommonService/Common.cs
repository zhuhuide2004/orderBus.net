using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheCMS.Linq;
namespace Bus.WeiXin.CommonService
{
   public class Common
    {
        private static string AppId = System.Configuration.ConfigurationManager.AppSettings["AppId"].ToString();//换成你的信息
        private static string AppSecret = System.Configuration.ConfigurationManager.AppSettings["AppSecret"].ToString();//换成你的信息
        protected static AccessTokenResult tokenResult = new AccessTokenResult()
        {
            /* 由于获取accessToken有次数限制，为了节约请求，
             * 可以到 http://weixin.senparc.com/Menu 获取Token之后填入下方，
             * 使用当前可用Token直接进行测试。
             */
            access_token = ""
        };

        protected static AccessTokenResult LoadToken()
        {
            if (tokenResult == null || string.IsNullOrEmpty(tokenResult.access_token))
            {
                //正确数据，请填写微信公众账号后台的AppId及AppSecret
                tokenResult = CommonApi.GetToken(AppId, AppSecret);
            }
            return tokenResult;
        }
        public static void GetToken()
        {                
            LoadToken();            
        }
        #region 获取用户
        public static WeixinUserInfoResult GetUserInfo(string OpenID)
        {
            try
            {
                GetToken();
                var result = CommonApi.GetUserInfo(tokenResult.access_token, OpenID);
                return result;
            }
            catch (Exception ex)
            {
                //如果不参加内测，只是“服务号”，这类接口仍然不能使用，会抛出异常：错误代码：45009：api freq out of limit
                throw new Exception(ex + "///"+"获取用户信息出错，可能无权限。");
            }
        }
        #endregion
        #region 查询线路
        public static List<Data.BusLine> queryLine(string word)
        {
            var q = QueryBuilder.Create<Data.BusLine>().Like(x => x.LineName, word).Equals(x => x.TypeID, 1);
            var list = Data.BusLineDB.BusLineList(q);
            return list;
        }
        #endregion


    }
}

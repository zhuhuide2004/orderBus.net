using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Bus.Core
{
   public class SMS
    {
       /// <summary>
       /// 
       /// </summary>
       /// <param name="Phone"></param>
       /// <param name="Code"></param>
       /// <param name="t">0或者不输入为默认，1为注册时发送验证码</param>
       public static void Send(string Phone, string Code = "",int t=0)
       {
           try
           {
               string content = "您的密码已经重置为：" + Code + "，如果不是您本人操作，请马上修改密码。";
               content = "尊敬的会员：" + Phone + " 您已经成功修改密码，您的新密码：" + Code + " 请牢记新密码！【班车定制网】";
               if (t == 1) {
                   content = "您的手机号："+Phone+"，注册验证码："+Code+"，十分钟内有效！【班车定制网】";
               }
               Encoding myEncoding = Encoding.GetEncoding("gb2312");
               string param = HttpUtility.UrlEncode("username", myEncoding) + "=" + HttpUtility.UrlEncode("orderbus", myEncoding) + "&" + HttpUtility.UrlEncode("password", myEncoding) + "=" + HttpUtility.UrlEncode("vjgZ0AMQ", myEncoding) + "&" + HttpUtility.UrlEncode("receiver", myEncoding) + "=" + HttpUtility.UrlEncode(Phone, myEncoding) + "&" + HttpUtility.UrlEncode("content", myEncoding) + "=" + HttpUtility.UrlEncode(content, myEncoding);
               byte[] postBytes = Encoding.ASCII.GetBytes(param);
               String postReturn = doPostRequest("http://api.chanyoo.cn/gbk/interface/send_sms.aspx", postBytes);
           }
           catch (Exception)
           {
           }
       }
       public static void Send2(string Phone, string Content = "")
       {
           try
           {
               string content = Content+"【班车定制网】";
               Encoding myEncoding = Encoding.GetEncoding("gb2312");
               string param = HttpUtility.UrlEncode("username", myEncoding) + "=" + HttpUtility.UrlEncode("orderbus", myEncoding) + "&" + HttpUtility.UrlEncode("password", myEncoding) + "=" + HttpUtility.UrlEncode("vjgZ0AMQ", myEncoding) + "&" + HttpUtility.UrlEncode("receiver", myEncoding) + "=" + HttpUtility.UrlEncode(Phone, myEncoding) + "&" + HttpUtility.UrlEncode("content", myEncoding) + "=" + HttpUtility.UrlEncode(content, myEncoding);
               byte[] postBytes = Encoding.ASCII.GetBytes(param);
               String postReturn = doPostRequest("http://api.chanyoo.cn/gbk/interface/send_sms.aspx", postBytes);
           }
           catch (Exception)
           {
           }
       }
       public static string CreateCode()
       {
           Random rd = new Random();
           return rd.Next(100000, 999999).ToString();
       }
       private static String doPostRequest(string url, byte[] bData)
       {
           System.Net.HttpWebRequest hwRequest;
           System.Net.HttpWebResponse hwResponse;

           string strResult = string.Empty;
           try
           {
               hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
               hwRequest.Timeout = 5000;
               hwRequest.Method = "POST";
               hwRequest.ContentType = "application/x-www-form-urlencoded";
               hwRequest.ContentLength = bData.Length;
               System.IO.Stream smWrite = hwRequest.GetRequestStream();
               smWrite.Write(bData, 0, bData.Length);
               smWrite.Close();
           }
           catch (System.Exception err)
           {
               return strResult;
           }


           //get response
           try
           {
               hwResponse = (HttpWebResponse)hwRequest.GetResponse();
               StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.Default);
               strResult = srReader.ReadToEnd();
               srReader.Close();
               hwResponse.Close();
           }
           catch (System.Exception err)
           {
               return err.Message.ToString();
           }

           return strResult;
       }
    }
}

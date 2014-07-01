using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheCMS.Linq;

namespace Bus.Core
{
    public class Common
    {
        #region 计算距离
        private const double EARTH_RADIUS = 6378.137;//地球半径
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        /// <summary>
        /// 计算两个地址距离
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lng1"></param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }


        static double DEF_PI = 3.14159265359; // PI
        static double DEF_2PI = 6.28318530712; // 2*PI
        static double DEF_PI180 = 0.01745329252; // PI/180.0
        static double DEF_R = 6370693.5; // radius of earth
        public static double GetShortDistance(double lon1, double lat1, double lon2, double lat2)
        {
            double ew1, ns1, ew2, ns2;
            double dx, dy, dew;
            double distance;
            // 角度转换为弧度
            ew1 = lon1 * DEF_PI180;
            ns1 = lat1 * DEF_PI180;
            ew2 = lon2 * DEF_PI180;
            ns2 = lat2 * DEF_PI180;
            // 经度差
            dew = ew1 - ew2;
            // 若跨东经和西经180 度，进行调整
            if (dew > DEF_PI)
                dew = DEF_2PI - dew;
            else if (dew < -DEF_PI)
                dew = DEF_2PI + dew;
            dx = DEF_R * Math.Cos(ns1) * dew; // 东西方向长度(在纬度圈上的投影长度)
            dy = DEF_R * (ns1 - ns2); // 南北方向长度(在经度圈上的投影长度)
            // 勾股定理求斜边长 
            distance = Math.Sqrt(dx * dx + dy * dy);
            return distance;
        }
        public double GetLongDistance(double lon1, double lat1, double lon2, double lat2)
        {
            double ew1, ns1, ew2, ns2;
            double distance;
            // 角度转换为弧度
            ew1 = lon1 * DEF_PI180;
            ns1 = lat1 * DEF_PI180;
            ew2 = lon2 * DEF_PI180;
            ns2 = lat2 * DEF_PI180;
            // 求大圆劣弧与球心所夹的角(弧度)
            distance = Math.Sin(ns1) * Math.Sin(ns2) + Math.Cos(ns1) * Math.Cos(ns2) * Math.Cos(ew1 - ew2);
            // 调整到[-1..1]范围内，避免溢出
            if (distance > 1.0)
                distance = 1.0;
            else if (distance < -1.0)
                distance = -1.0;
            // 求大圆劣弧长度
            distance = DEF_R * Math.Acos(distance);
            return distance;
        }
        #endregion
        /// <summary>
        /// C#检测上传图片文件
        /// </summary>
        /// <param name="stream">上传文件流</param>
        /// <param name="fileType">fileType 文件类型</param>
        /// <param name="fileclass">返回文件真实扩展名</param>
        /// <param name="DisposeStream">是否关闭当前资源</param>
        /// <returns></returns>
        public static bool IsAllowedFileExtension(System.IO.Stream stream, string[] fileType, out string fileclass, bool DisposeStream)
        {
            bool ret = false;

            //System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            //System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
            System.IO.BinaryReader r = new System.IO.BinaryReader(stream);
            fileclass = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();
            }
            catch
            {
                return false;
            }
            finally
            {
                //r.Close();
                //r.Dispose();
                //fs.Close();
                //fs.Dispose();
                if (DisposeStream)
                {
                    r.Close();
                    r.Dispose();
                    stream.Close();
                    stream.Dispose();
                }
            }
            /*文件扩展名说明
             *4946/104116 txt
             *7173        gif 
             *255216      jpg
             *13780       png
             *6677        bmp
             *239187      txt,aspx,asp,sql
             *208207      xls.doc.ppt
             *6063        xml
             *6033        htm,html
             *4742        js
             *8075        xlsx,zip,pptx,mmap,zip
             *8297        rar   
             *01          accdb,mdb
             *7790        exe,dll           
             *5666        psd 
             *255254      rdp 
             *10056       bt种子 
             *64101       bat 
             *4059        sgf
             */

            //String[] fileType = { "255216", "7173", "6677", "13780", "8297", "5549", "870", "87111", "8075" };

            //纯图片
            //String[] fileType = { 
            //    "7173",    //gif
            //    "255216",  //jpg
            //    "13780"    //png
            //};

            for (int i = 0; i < fileType.Length; i++)
            {
                if (fileclass == fileType[i])
                {
                    ret = true;
                    break;
                }
            }
            //Response.Write(fileclass);//可以在这里输出你不知道的文件类型的扩展名
            return ret;
        }
        #region 获取当前登录的用户信息
        /// <summary>
        /// 获取当前登录的用户信息
        /// </summary>
        /// <returns></returns>
        public static Data.Users GetCurrentUsers()
        {
            if (TheCMS.Common.Cookie.GetCookie("UserID") == null || TheCMS.Common.Cookie.GetCookie("UserID").ToString() == "")
            {
                return null;
            }
            else
            {
                int UID = Convert.ToInt32(TheCMS.Common.Cookie.GetCookie("UserID").ToString());
                var model = Data.UsersDB.GETUsers(UID);
                return model == null ? null : model;
            }
        }
        #endregion
        #region 周围已经预定用户
        public static List<Data.Users> GetUserList(double Lng, double Lat,int LineID)
        {
            var alluserList = Data.UsersDB.UsersList();
            var list = new List<Data.Users>();
            foreach (var item in alluserList)
            {
                var juli = Common.GetShortDistance(item.StartLong, item.StartLat, Lng, Lat);
                if (juli <= 1000)
                {
                    var q = QueryBuilder.Create<Data.Order>().Equals(x => x.UserID, item.ID).Equals(x => x.LineID, LineID);
                    var _list = Data.OrderDB.OrderList(q);
                    if (_list.Count > 0)
                    {
                        if (!list.Contains(item))
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }
        #endregion
        #region 返回地点周围用户列表
        public static List<Data.Users> GetUserList(double Lng, double Lat)
        {
            var alluserList = Data.UsersDB.UsersList();
            var list = new List<Data.Users>();
            foreach (var item in alluserList)
            {
                var juli = Common.GetShortDistance(item.StartLong, item.StartLat, Lng, Lat);
                if (juli <= 1000)
                {
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }
        #endregion

        #region 返回前月/当月/下月 信息列表
        public static List<string> GetYearMonthList()
        {
            var list = new List<string>();

            list.Add(DateTime.Now.AddMonths(-1).ToString("yyyyMM"));
            list.Add(DateTime.Now.ToString("yyyyMM"));
            list.Add(DateTime.Now.AddMonths(1).ToString("yyyyMM"));

            return list;
        }
        #endregion
    }
}

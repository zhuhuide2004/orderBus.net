using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bus.Data;
using TheCMS.Linq;
using TheCMS.Common;
using Bus.Core;

namespace Bus.Web.Controllers
{
    public class LineInfo {
        public int LineID { get; set; }
        public string LineName { get; set; }
        public DateTime STime { get; set; }
        public DateTime ETime { get; set; }
        public decimal Price { get; set; }
        public string SAddress { get; set; }
        public string EAddress { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }

    }
    public class HomeController : BaseController
    {

        //
        // GET: /Home/
        //[UserLogin] 
        public ActionResult Index()
        {                         
            return View();
        }
        #region 用户基本资料
        [UserLogin]
        public ActionResult MyInfo()
        {
            return View(umodel);
        }
        [UserLogin]
        [HttpPost]
        public ActionResult MyInfo(FormCollection fc)
        {
            var model = new Data.Users();
            model.ID = umodel.ID;
            model.Names = fc["Names"];
            model.Phone = fc["Phone"];
            model.Sex = TypeConverter.StrToInt(fc["Sex"]);
            model.EMail = fc["EMail"];
            model.QQ = fc["QQ"];
            model.CompanyName = fc["CompanyName"];
            var flag = Data.UsersDB.SaveEditUsersMyInfo(model);
            return Json(new { success = flag }, JsonRequestBehavior.AllowGet);
        }

        [UserLogin]
        public ActionResult EditMyInfo()
        {
            return View(umodel);
        }
        [UserLogin]
        [HttpPost]
        public ActionResult EditMyInfo(FormCollection fc)
        {
            var model = new Data.Users();
            model.ID = umodel.ID;
            model.Names = fc["Names"];
            model.Phone = fc["Phone"];
            model.Sex = TypeConverter.StrToInt(fc["Sex"]);
            model.EMail = fc["EMail"];
            model.QQ = fc["QQ"];
            model.CompanyName = fc["CompanyName"];

            string _starttime = DateTime.Now.ToString("yyyy-MM-dd");
            _starttime = _starttime + " " + fc["StartTime1"] + ":" + fc["StartTime2"] + ":00";
            model.StartTime = TypeConverter.StrToDateTime(_starttime);

            string _endtime = DateTime.Now.ToString("yyyy-MM-dd");
            _endtime = _endtime + " " + fc["EndTime1"] + ":" + fc["EndTime2"] + ":00";
            model.EndTime = TypeConverter.StrToDateTime(_endtime);




            var flag = Data.UsersDB.SaveEditUsersMyInfo(model);
            return Json(new { success = flag }, JsonRequestBehavior.AllowGet);
        }


        public static string GetNumber()
        {
            var dt = DateTime.Now;
            var q = QueryBuilder.Create<Data.Users>().Equals(x => x.CreateTime.Year, dt.Year).Equals(x => x.CreateTime.Month, dt.Month).Equals(x => x.CreateTime.Day, dt.Day);
            var list = Data.UsersDB.UsersList(q);
            int count = list.Count+1;
            var strdt = dt.ToString("yyMMdd");
            var allcount = Data.UsersDB.UsersList().Count+1;
            string result = "";
            if (allcount < 1000) {
                result= strdt + count.ToString().PadLeft(3, '0');
            }
            else if (allcount < 10000)
            {
                result = strdt + count.ToString().PadLeft(4, '0');
            }
            else if (allcount < 100000)
            {
                result = strdt + count.ToString().PadLeft(5, '0');
            }
            return result;
        }
        //public static bool IsMobileDevice()
        //{
        //    string u = Request.ServerVariables["HTTP_USER_AGENT"];
        //    Regex b = new Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino|ucweb|mqqbrowser", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //    Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //    if (b.IsMatch(u))
        //    {
        //        return true;
        //    }
        //    return v.IsMatch(u.Substring(0, 4));
        //}
        [UserLogin]
        public ActionResult MyProfile(int ID = 0)
        {
            return View(umodel);
        }
        [UserLogin]
        [HttpPost]
        public ActionResult MyProfile(FormCollection fc)
        {
            var model = new Data.Users();
            model.ID = TypeConverter.StrToInt(fc["hidUserID"]); 
            model.StartLat = TypeConverter.StrToDouble(fc["StartLat"], 0);     
            model.EndLong = TypeConverter.StrToDouble(fc["EndLong"], 0);      
            model.EndLat = TypeConverter.StrToDouble(fc["EndLat"], 0);
            model.CreateTime = DateTime.Now; 
            model.EndAddress = fc["EndAddress"];
            model.ParentUserID = iRequest.GetQueryInt("ParentID", 0);

            model.WXUserID = 0;
            model.isFinal = true;
            model.Address = fc["Address"];
            string _starttime = DateTime.Now.ToString("yyyy-MM-dd");
            _starttime = _starttime + " " + fc["StartTime1"] + ":" + fc["StartTime2"] + ":00";
            model.StartTime = TypeConverter.StrToDateTime(_starttime);

            string _endtime = DateTime.Now.ToString("yyyy-MM-dd");
            _endtime = _endtime + " " + fc["EndTime1"] + ":" + fc["EndTime2"] + ":00";
            model.EndTime = TypeConverter.StrToDateTime(_endtime);
            model.StartLong = TypeConverter.StrToDouble(fc["StartLong"], 0);
            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.UsersDB.SaveEditUsers(model);
            }
            else
            {
                aj.success = Data.UsersDB.AddUsers(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }

         //=============
        [UserLogin]
        public ActionResult EditMyProfile(int ID = 0)
        {
            return View(umodel);
        }
        [UserLogin]
        [HttpPost]
        public ActionResult EditMyProfile(FormCollection fc)
        {
            var model = new Data.Users();
            model.ID = TypeConverter.StrToInt(fc["hidUserID"]);
            //model.StartLat = TypeConverter.StrToDouble(fc["StartLat"], 0);
            //model.EndLong = TypeConverter.StrToDouble(fc["EndLong"], 0);
            //model.EndLat = TypeConverter.StrToDouble(fc["EndLat"], 0);
            model.CreateTime = DateTime.Now;
            model.EndAddress = fc["EndAddress"];
            model.ParentUserID = iRequest.GetQueryInt("ParentID", 0);

            model.WXUserID = 0;
            model.isFinal = true;
            model.Address = fc["Address"];
            string _starttime = DateTime.Now.ToString("yyyy-MM-dd");
            _starttime = _starttime + " " + fc["StartTime1"] + ":" + fc["StartTime2"] + ":00";
            model.StartTime = TypeConverter.StrToDateTime(_starttime);

            string _endtime = DateTime.Now.ToString("yyyy-MM-dd");
            _endtime = _endtime + " " + fc["EndTime1"] + ":" + fc["EndTime2"] + ":00";
            model.EndTime = TypeConverter.StrToDateTime(_endtime);
            //model.StartLong = TypeConverter.StrToDouble(fc["StartLong"], 0);
            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.UsersDB.SaveEditUsers_NoMap(model);
            }
            else
            {
                aj.success = Data.UsersDB.AddUsers(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        [UserLogin]
        public ActionResult EditMap2(int ID = 0)
        {
            return View(umodel);
        }
        [HttpPost]
        [UserLogin]
        public ActionResult EditMap2(FormCollection fc)
        {
            var model = new Data.Users();
            model.ID = TypeConverter.StrToInt(fc["hidUserID"]);
            model.EndAddress = fc["add"];
            //model.StartLat = TypeConverter.StrToDouble(fc["StartLat"], 0);
            model.EndLong = TypeConverter.StrToDouble(fc["EndLong"], 0);
            model.EndLat = TypeConverter.StrToDouble(fc["EndLat"], 0);
            //model.StartLong = TypeConverter.StrToDouble(fc["StartLong"], 0);
            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.UsersDB.SaveEditUsers_Map2(model);
            }
            //else
            //{
            //    aj.success = Data.UsersDB.AddUsers(model) > 0;
            //}
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult iMap2()
        {
            return umodel == null ? View() : View(umodel);
        }
        [HttpPost]
        public ActionResult iMap2(FormCollection fc)
        {
            if (umodel == null || umodel.ID < 1)
            {
                return Json(new { success = false, msg = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //var model = new Data.Users();
                //model.ID = TypeConverter.StrToInt(fc["hidUserID"]);
                //model.StartLat = TypeConverter.StrToDouble(fc["StartLat"], 0);
                //model.Address = fc["add"];
                //model.StartLong = TypeConverter.StrToDouble(fc["StartLong"], 0);
                //AjaxJson aj = new AjaxJson();
                //if (model.ID > 0)
                //{
                //    aj.success = Data.UsersDB.SaveEditUsers_Map(model);
                //}
                SetLocal(fc, umodel.ID);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult iMap()
        {
            return umodel == null ? View() : View(umodel);
        }
        [HttpPost]
        public ActionResult iMap(FormCollection fc)
        {
            if (umodel == null||umodel.ID<1)
            {
                return Json(new { success = false, msg = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var model = new Data.Users();
                model.ID = TypeConverter.StrToInt(fc["hidUserID"]);
                model.StartLat = TypeConverter.StrToDouble(fc["StartLat"], 0);
                model.Address = fc["add"];
                model.StartLong = TypeConverter.StrToDouble(fc["StartLong"], 0);
                AjaxJson aj = new AjaxJson();
                if (model.ID > 0)
                {
                    aj.success = Data.UsersDB.SaveEditUsers_Map(model);
                }
                return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
            }
        }

        [UserLogin]
        public ActionResult EditMap(int ID = 0)
        {
            return View(umodel);
        }
        [HttpPost]
        [UserLogin]
        public ActionResult EditMap(FormCollection fc)
        {
            var model = new Data.Users();
            model.ID = TypeConverter.StrToInt(fc["hidUserID"]);
            model.StartLat = TypeConverter.StrToDouble(fc["StartLat"], 0);
            model.Address = fc["add"];
            model.StartLong = TypeConverter.StrToDouble(fc["StartLong"], 0);
            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.UsersDB.SaveEditUsers_Map(model);
            }
            //else
            //{
            //    aj.success = Data.UsersDB.AddUsers(model) > 0;
            //}
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }








        #endregion
        #region 设置用户位置信息
        void SetLocal(FormCollection f, int UID)
        {

                var um = new Data.Users();
                um.ID = UID;
                um.Address = f["hidSAddress"];
                um.EndAddress = f["hidEAddress"];

                um.StartLat = TypeConverter.StrToDouble(f["hidSLat"], 0);
                um.StartLong = TypeConverter.StrToDouble(f["hidSLong"], 0);
                um.EndLat = TypeConverter.StrToDouble(f["hidELat"], 0);
                um.EndLong = TypeConverter.StrToDouble(f["hidELong"], 0);
                if (um.Address != "" && um.EndAddress != "" && um.StartLat > 0 && um.StartLong > 0 && um.EndLat > 0 && um.EndLong > 0)
                {
                    Data.UsersDB.SetUserLocal(um);
                }

        }
        #endregion
        #region 等录

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult MLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            var phone = fc["Phone"];
            var p = Encrypt.DES.Des_Encrypt(fc["Password"]);
            var q = QueryBuilder.Create<Data.Users>().Equals(x => x.Password, p).Equals(x => x.Phone, phone);
            var model = Data.UsersDB.GETUsers(q);
            if (model == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                SetLocal(fc, model.ID);
                Cookie.WriteCookie("UserID", model.ID.ToString());
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult MLogin(FormCollection fc)
        {
            var phone = fc["Phone"];
            var p = Encrypt.DES.Des_Encrypt(fc["Password"]);
            var q = QueryBuilder.Create<Data.Users>().Equals(x => x.Password, p).Equals(x => x.Phone, phone);
            var model = Data.UsersDB.GETUsers(q);
            if (model == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                SetLocal(fc, model.ID);
                Cookie.WriteCookie("UserID", model.ID.ToString());
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 注册
        public static bool SendCodes(string Phone="", string Code="")
        {
            try
            {
                Random rd = new Random();
                Code = rd.Next(1000, 9999).ToString();
                SMS.Send(Phone, Code,1);
                var smsModel = new Data.SMSCode();
                smsModel.Code = Code;
                smsModel.CreateTime = DateTime.Now;
                smsModel.EndTime = smsModel.CreateTime.AddMinutes(10);
                smsModel.IsUse = false;
                smsModel.Phone = Phone;
                Data.SMSCodeDB.AddSMSCode(smsModel);
                return true;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Phone"></param>
        /// <returns>0，验证码错误，1验证码过期，2，正常</returns>
        public static int CheckVCode(string Code, string Phone) {
            var q = QueryBuilder.Create<Data.SMSCode>().Equals(x => x.Phone, Phone).Equals(x => x.Code, Code);
            var model = Data.SMSCodeDB.GETSMSCode(q);
            if (model == null)
            {
                return 0;
            }
            else {
                if (model.EndTime < DateTime.Now)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }
        public ActionResult Reg(string Phone = "", string Pwd = "", string Names = "")
        {
            return View();
        }
        public ActionResult SignOut()
        {
            Cookie.DelCookie("UserID");
            return Content("<script>window.location.href='/Home/';</script>");
        }
        [HttpPost]
        public ActionResult Reg(FormCollection fc)
        {
            var model = new Data.Users();
            model.Address = "";
            model.CreateTime = DateTime.Now;
            model.EndLat = 0;
            model.EndLong = 0;
            model.EndTime = DateTime.Now;
            model.Names = fc["Names"];
            model.Password = Encrypt.DES.Des_Encrypt(fc["Password"]);
            model.Phone = fc["Phone"];
            model.StartLat = 0;
            model.StartLong = 0;
            model.StartTime = DateTime.Now;
            model.WXUserID = iRequest.GetQueryInt("WxID", 0);
            model.isFinal = true;
            model.ParentUserID = 0;
            model.Sex = 1;
            model.EndAddress = "";
            model.StateID = 0;
            model.Number = GetNumber();
            var cint = CheckVCode(fc["Vcode"].ToString(), model.Phone);
            if (cint == 0) {
                return Json(new { success = false,message="验证码错误." }, JsonRequestBehavior.AllowGet);
            }
            else if (cint == 1)
            {
                return Json(new { success = false, message = "验证码过期." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var userID = Data.UsersDB.AddUsers(model);
                if (userID > 0)
                {
                    SetLocal(fc, userID);
                    Cookie.WriteCookie("UserID", model.Phone);
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false,message="注册发生异常." }, JsonRequestBehavior.AllowGet);
                }
            }


            
            //return View();
        }
        public ActionResult Register(int WxID=0)
        {
            return View();
        }
        public ActionResult MRegister(int WxID = 0)
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterFast(FormCollection fc)
        {
            var model = new Data.Users();
            model.Address = fc["hidSAddress"];
            model.EndAddress = fc["hidEAddress"];

            model.StartLat = TypeConverter.StrToDouble(fc["hidSLat"], 0);
            model.StartLong = TypeConverter.StrToDouble(fc["hidSLong"], 0);
            model.EndLat = TypeConverter.StrToDouble(fc["hidELat"], 0);
            model.EndLong = TypeConverter.StrToDouble(fc["hidELong"], 0);
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(FormCollection fc)
        {
            //try
            //{
                var model = new Data.Users();
                model.Address = "";
                model.CreateTime = DateTime.Now;
                model.EndLat = TypeConverter.StrToDouble(fc["hidELat"], 0);
                model.EndLong = TypeConverter.StrToDouble(fc["hidELong"], 0);
                model.StartLat = TypeConverter.StrToDouble(fc["hidSLat"], 0);
                model.StartLong = TypeConverter.StrToDouble(fc["hidSLong"], 0); 
                model.Address = fc["beginAddress"];
                model.EndAddress = fc["EndAddress"];

                //model.EndTime = DateTime.Now;
                //model.Names = fc["Names"];
                //model.Password = Encrypt.DES.Des_Encrypt(fc["Password"]);
                //model.Phone = fc["Phone"];
                
                //model.StartTime = DateTime.Now;
                //model.WXUserID = iRequest.GetQueryInt("WxID", 0);
                //model.isFinal = true;
                //model.ParentUserID = 0;
                //model.Sex = 1;
                //model.EndAddress = "";

                model.Sex = TypeConverter.StrToInt(fc["Sex"], 1);
                model.StartTime = TypeConverter.StrToDateTime(fc["StartTime"]);
                model.EndTime = TypeConverter.StrToDateTime(fc["EndTime"]);
                model.Names = fc["Names"];
                model.Password = Encrypt.DES.Des_Encrypt(fc["Password"]);
                model.Phone = fc["Phone"];


                model.WXUserID = iRequest.GetQueryInt("WxID", 0);
                model.isFinal = true;
                model.ParentUserID = 0;


                model.StateID = 0;
                model.Number = GetNumber();
                var q = QueryBuilder.Create<Data.Users>().Equals(x => x.Phone, model.Phone);
                var _m = Data.UsersDB.GETUsers(q);
                if (_m != null)
                {
                    return Json(new { success = false, message = "该手机号已经注册." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (SendCodes(model.Phone))
                    {
                        return Json(new { success = true}, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "发送手机短信验证码失效." }, JsonRequestBehavior.AllowGet);
                    }
                }
                //var userID = Data.UsersDB.AddUsers(model);
                //if (userID > 0)
                //{
                //    Cookie.WriteCookie("UserID", userID.ToString());
                //    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                //}
                //return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ee)
            //{
            //    return Json(new { success = false, message = ee.Message }, JsonRequestBehavior.AllowGet);
            //}
        }

        [HttpPost]
        public ActionResult MRegister(FormCollection fc)
        {
            //try
            //{
            var model = new Data.Users();
            model.Address = "";
            model.CreateTime = DateTime.Now;
            model.EndLat = TypeConverter.StrToDouble(fc["hidELat"], 0);
            model.EndLong = TypeConverter.StrToDouble(fc["hidELong"], 0);
            model.StartLat = TypeConverter.StrToDouble(fc["hidSLat"], 0);
            model.StartLong = TypeConverter.StrToDouble(fc["hidSLong"], 0);
            model.Address = fc["beginAddress"];
            model.EndAddress = fc["EndAddress"];

            model.Sex = TypeConverter.StrToInt(fc["Sex"], 1);
            model.StartTime = TypeConverter.StrToDateTime(fc["StartTime"]);
            model.EndTime = TypeConverter.StrToDateTime(fc["EndTime"]);
            model.Names = fc["Names"];
            model.Password = Encrypt.DES.Des_Encrypt(fc["Password"]);
            model.Phone = fc["Phone"];


            model.WXUserID = iRequest.GetQueryInt("WxID", 0);
            model.isFinal = true;
            model.ParentUserID = 0;


            model.StateID = 0;
            model.Number = GetNumber();
            var q = QueryBuilder.Create<Data.Users>().Equals(x => x.Phone, model.Phone);
            var _m = Data.UsersDB.GETUsers(q);
            if (_m != null)
            {
                return Json(new { success = false, message = "该手机号已经注册." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (SendCodes(model.Phone))
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "发送手机短信验证码失效." }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion

        #region 查询线路
        public ActionResult LineInfo(int ID = 0)
        {
            var model = Data.BusLineDB.GETBusLine(ID);
            return View(model);
        }
        public ActionResult AllLine()
        {
            var q = QueryBuilder.Create<Data.BusLine>().Equals(x => x.TypeID, 1);
            var allBusLine = Data.BusLineDB.BusLineList(q);
            return View(allBusLine);
        }
        public ActionResult AllLine2()
        {
            var q = QueryBuilder.Create<Data.BusLine>().Equals(x => x.TypeID, 2);
            var allBusLine = Data.BusLineDB.BusLineList(q);
            return View(allBusLine);
        }
        //[UserLogin]
        public ActionResult GetBusLine(int TID=1)
        {
            var allq = QueryBuilder.Create<Data.BusLine>().Equals(x => x.TypeID, TID);
            var allBusLine = Data.BusLineDB.BusLineList(allq);
            var lineList = new List<LineInfo>();
            var userInfo = Common.GetCurrentUsers();
            var list2 = new List<LineInfo>();
            foreach (var item in allBusLine)
            {
                //距离 ？？？
                var juli =
                    userInfo != null ?
                    Common.GetShortDistance(userInfo.StartLong, userInfo.StartLat, item.StartLong, item.StartLat) :
                    1001;

                //用户距离小于1000 ？
                if (juli <= 1000)
                {
                    var line = new LineInfo();
                    line.EAddress = item.EndAddress;
                    line.ETime = item.EndTime;
                    line.LineID = item.ID;
                    line.LineName = item.LineName;
                    line.Price = item.Price;
                    line.SAddress = item.StartAddress;
                    line.STime = item.StartTime;
                    line.Lat = item.StartLat;
                    line.Lng = item.StartLong;
                    if (!lineList.Contains(line))
                    {
                        lineList.Add(line);
                    }
                }
                else
                {
                    var line = new LineInfo();
                    line.EAddress = item.EndAddress;
                    line.ETime = item.EndTime;
                    line.LineID = item.ID;
                    line.LineName = item.LineName;
                    line.Price = item.Price;
                    line.SAddress = item.StartAddress;
                    line.STime = item.StartTime; 
                    line.Lat = item.StartLat;
                    line.Lng = item.StartLong;
                    if (!list2.Contains(line))
                    {
                        list2.Add(line);
                    }
                }
            }
            foreach (var item in list2)
            {
                if (!lineList.Contains(item))
                {
                    lineList.Add(item);
                }
            }
            return View(lineList);
        }
        [UserLogin]
        public ActionResult BusLineInfo(int id = 0)
        {
            var model = new Data.BusLine();
            model = Data.BusLineDB.GETBusLine(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
            //return View();
        }
        [HttpPost]
        [UserLogin]
        public ActionResult BusLineInfo(FormCollection fc)
        {
            var model = new Data.Order();
            model.CreateTime = DateTime.Now;
            model.LineID = iRequest.GetQueryInt("ID");
            model.Memo = "";
            model.Months = TypeConverter.StrToInt(fc["Months"]);
            model.Names = fc["Names"];
            model.Phone = fc["Phone"];
            var linemodel = new Data.BusLine();
            linemodel = Data.BusLineDB.GETBusLine(model.LineID);
            model.Price = linemodel.Price;
            model.States = 1;
            model.UserID = umodel.ID;
            model.PayTypeID = TypeConverter.StrToInt(fc["PayTypeID"]);
            var flag = Data.OrderDB.AddOrder(model)>0;
            return Json(new { success = flag }, JsonRequestBehavior.AllowGet);
            
        }
        #endregion

        #region 调查
        public static int GetStartItemID(int QuestionID)
        {
            var q = QueryBuilder.Create<Data.QuestionItem>().Equals(x => x.QuestionID, QuestionID);
            var allItem = Data.QuestionItemDB.QuestionItemList(q);
            return allItem.Count > 0 ? allItem[allItem.Count-1].ID : 0;
        }
        public ActionResult QuestionList()
        {
            var q = QueryBuilder.Create<Data.Question>();
            var list = Data.QuestionDB.List(q,1,10);
            return View(list);
        }
        public ActionResult Question(int ID=0,int ItemID=0)
        {
            //先读取调查
            var qmodel = Data.QuestionDB.GETQuestion(ID);
            //var q=QueryBuilder.Create<Data.QuestionItem>().Equals(x=>x.QuestionID,ID);
            //var allItem = Data.QuestionItemDB.QuestionItemList(q);
            ViewBag.QModel = qmodel;
            var model = Data.QuestionItemDB.GETQuestionItem(ItemID);
            return View(model);
        }
        public static List<Bus.Data.QuestionItem2> GetQuestionItem2List(int ItemID)
        {
            var q = QueryBuilder.Create<Data.QuestionItem2>().Equals(x => x.QuestionItemID, ItemID);
            return Data.QuestionItem2DB.QuestionItem2List(q);
        }
        [HttpPost]
        public ActionResult Question(FormCollection fc)
        {
            var QuestionID = iRequest.GetQueryInt("ID");
            var ItemID = iRequest.GetQueryInt("ItemID");
            int ValueID = TypeConverter.StrToInt(fc["ItemValue"]);
            var uqmodel = new Data.UserQuestion();
            uqmodel.CreateTime = DateTime.Now;
            uqmodel.QuestionID = QuestionID;
            uqmodel.QuestionItem2ID = ValueID;
            uqmodel.QuestionItemID = ItemID;
            uqmodel.UserID = 0;
            var flag = Data.UserQuestionDB.AddUserQuestion(uqmodel) > 0;
            string nexturl = "";
            if (flag)
            {
                var q = QueryBuilder.Create<Data.QuestionItem>().Equals(x => x.QuestionID, QuestionID);
                var allItem = Data.QuestionItemDB.QuestionItemList(q);
                var model = allItem.Where(x => x.ID > ItemID).OrderBy(x=>x.ID).Skip(0).Take(1).ToList();
                if (model.Count>0) {   
                    nexturl = "Question?id="+model[0].QuestionID+"&ItemID="+model[0].ID;
                }
            }
            return Json(new { success = flag, nexturl = nexturl }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult QuestionFinall()
        {
            return View();
        }
        #endregion
        #region 我的预定
        [UserLogin]
        public ActionResult MyBusLine()
        {
            var q = QueryBuilder.Create<Data.Order>().Equals(x => x.UserID, umodel.ID);
            var list = Data.OrderDB.OrderList(q);
            return View(list);
        }
        #endregion


        #region 文章
        public ActionResult NewsList(int ClassID = 0)
        {
            var q = QueryBuilder.Create<Data.News>().Equals(x => x.ClassID, ClassID);
            var list = Data.NewsDB.List(q, 1, 10);
            return View(list);
        }
        public ActionResult News(int ID = 0)
        {
            var model = Data.NewsDB.GETNews(ID);
            return View(model);
        }
        #endregion

        #region 公告
        public ActionResult Notice()
        {
            var q = QueryBuilder.Create<Data.News>().Equals(x => x.ClassID, 1);
            var list = Data.NewsDB.List(q);
            return View(list);
        }
        public ActionResult ViewNews(int ID = 0)
        {
            var model = Data.NewsDB.GETNews(ID);
            return View(model);
        }
        #endregion
        public ActionResult ordersuccess()
        {
            return View();
        }
        #region 我的订单
        public ActionResult delmybus(int id = 0)
        {
            var flag = Data.OrderDB.DeleteOrder(id);
            return Json(new { success=flag},JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditMyBusLine(int id = 0)
        {
            var model = Data.OrderDB.GETOrder(id);
            ViewBag.BusLineModel = Data.BusLineDB.GETBusLine(model.LineID);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditMyBusLine(FormCollection fc)
        {
            var model = new Data.Order();
            model.ID = iRequest.GetQueryInt("ID");
            model.Names = fc["Names"];
            model.Phone = fc["Phone"];
            model.Months = TypeConverter.StrToInt(fc["Months"]);
            model.PayTypeID = TypeConverter.StrToInt(fc["PayTypeID"]);
            var flag = Data.OrderDB.SaveEditOrder(model);
            return Json(new { success = flag }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetPassword
        public ActionResult GetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetPassword(FormCollection fc)
        {
            var flag = false;
            var message = "";
            string Phone = fc["Phone"];
            var q = QueryBuilder.Create<Data.Users>().Equals(x => x.Phone, Phone);
            var model = Data.UsersDB.GETUsers(q);
            if (model == null)
            {
                flag = false;
                message = "不存在的电话号码.";
            }
            else
            {
                string Code = SMS.CreateCode();
                flag = UsersDB.ChangePWD(model.ID, Encrypt.DES.Des_Encrypt(Code));
                if (flag)
                {
                    SMS.Send(Phone, Code);
                    var smsModel = new Data.SMSCode();
                    smsModel.Code = Code;
                    smsModel.CreateTime = DateTime.Now;
                    smsModel.EndTime = smsModel.CreateTime.AddMinutes(10);
                    smsModel.IsUse = false;
                    smsModel.Phone = Phone;
                    Data.SMSCodeDB.AddSMSCode(smsModel);
                }
                message = "新密码已发送到您号码为"+Phone+"的手机上.";
            }
            return Json(new { success = flag, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 跳转地图
       public ActionResult toMap()
       {
           Response.Redirect("http://map.baidu.com/fwmap/zt/traffic/index.html?city=dalian");
           return View();
       }
        #endregion
        #region 线路图
       public ActionResult LineImage(int LineID = 0)
       {
           var q = QueryBuilder.Create<Data.Station>().Equals(x => x.LineID, LineID);
           var list = Data.StationDB.StationList2(q);
           return View(list);
       }
        #endregion
        #region 站点
       public ActionResult LineStation(int LineID = 0)
       {
           var q = QueryBuilder.Create<Data.Station>().Equals(x => x.LineID, LineID);
           var list = Data.StationDB.StationList2(q);
           return View(list);
       }
        #endregion
    }
}

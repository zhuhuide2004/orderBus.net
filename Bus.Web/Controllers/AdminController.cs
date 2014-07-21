using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bus.Core;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Vml;
using TheCMS.Linq;
using TheCMS.Common;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;
using System.Data.OleDb;
using System.IO;


namespace Bus.Web.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        [AdminIsLogin]
        public ActionResult AddressChooser()
        {
            var list = new List<List<Data.Address>>();
            var counrtyQ = QueryBuilder.Create<Data.Address>().Equals(x => x.AddLevel, 1);
            var provinceQ = QueryBuilder.Create<Data.Address>().Equals(x => x.AddLevel, 2);
            var cityQ = QueryBuilder.Create<Data.Address>().Equals(x => x.AddLevel, 3);
            var streetQ = QueryBuilder.Create<Data.Address>().Equals(x => x.AddLevel, 4);
            var countryList = Data.AddressDB.AddressList(counrtyQ);
            var provinceList = Data.AddressDB.AddressList(provinceQ);
            var cityList = Data.AddressDB.AddressList(cityQ);
            var streetList = Data.AddressDB.AddressList(streetQ);

            list.Add(countryList);
            list.Add(provinceList);
            list.Add(cityList);
            list.Add(streetList);

            return View(list);
        }

        #region 地址管理器
        [AdminIsLogin]
        public ActionResult AddressManager(int Address1ID = 0, int Address2ID = 0, int Address3ID = 0)
        {
            var list = new List<List<Data.Address>>();
            var address1List = new List<Data.Address>();
            var address2List = new List<Data.Address>();
            var address3List = new List<Data.Address>();
            var address4List = new List<Data.Address>();

            var q = QueryBuilder.Create<Data.Address>();

            q = q.Equals(x => x.AddLevel, 1);

            address1List = Data.AddressDB.AddressList(q);

            if (Address1ID != 0)
            {
                q = QueryBuilder.Create<Data.Address>();
                q = q.Equals(x => x.ParentID, Address1ID).Equals(x => x.AddLevel, 2);
                address2List = Data.AddressDB.AddressList(q);
            }

            if (Address2ID != 0)
            {
                q = QueryBuilder.Create<Data.Address>();
                q = q.Equals(x => x.ParentID, Address2ID).Equals(x => x.AddLevel, 3);
                address3List = Data.AddressDB.AddressList(q);
            }

            if (Address3ID != 0)
            {
                q = QueryBuilder.Create<Data.Address>();
                q = q.Equals(x => x.ParentID, Address3ID).Equals(x => x.AddLevel, 4);
                address4List = Data.AddressDB.AddressList(q);
            }

            list.Add(address1List);
            list.Add(address2List);
            list.Add(address3List);
            list.Add(address4List);

            return View(list);
        }

        #region 添加和修改地址
        [AdminIsLogin]
        public ActionResult AddOrModifyAddress(string AddName, int ID, int ParentID, int AddLevel, int SortID)
        {
            var model = new Data.Address();

            model.ID = ID;
            model.AddName = AddName;
            model.SortID = SortID;

            AjaxJson aj = new AjaxJson();

            if (model.ID > 0)
            {
                aj.success = Data.AddressDB.SaveEditAddress(model);
            }
            else
            {
                model.ParentID = ParentID;
                model.AddLevel = AddLevel;
                model.CreateTime = DateTime.Now;

                aj.success = Data.AddressDB.AddAddress(model) > 0;
            }

            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 保存排序
        [AdminIsLogin]
        public ActionResult saveSort(string IDs, String SortIDs)
        {
            AjaxJson aj = new AjaxJson();
            string[] ID = IDs.TrimEnd(',').Split(',');
            string[] SortID = SortIDs.TrimEnd(',').Split(',');

            for (var i = 0; i < ID.Length; i++)
            {
                var model = new Data.Address();

                model.ID = TypeConverter.StrToInt(ID[i]);
                model.SortID = TypeConverter.StrToInt(SortID[i]);

                aj.success = Data.AddressDB.SaveAddressSort(model);
            }

            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        [AdminIsLogin]
        public ActionResult Main()
        {
            return View();
        }
        [AdminIsLogin]
        public ActionResult Index()
        {
            return View();
        }
        [AdminIsLogin]
        public ActionResult Menu()
        {
            return View();
        }
        [AdminIsLogin]
        public ActionResult InExcel()
        {
            return View();
        }

        #region 车辆管理
        [AdminIsLogin]
        public ActionResult BusList(int page = 1, string BusNo = "", string DriverName = "", string Phone = "", int SeatCnt = 0, string DelFlag = "N")
        {
            var q = QueryBuilder.Create<Data.BusView>();
            if (BusNo != "")
            {
                q = q.Like(x => x.BusNo, BusNo);
            }
            if (DriverName != "")
            {
                q = q.Like(x => x.DriverName, DriverName);
            }
            if (Phone != "")
            {
                q = q.Like(x => x.Phone, Phone);
            }
            if (SeatCnt != 0)
            {
                q = q.Equals(x => x.SeatCnt, SeatCnt);
            }

            q = q.Equals(x => x.DelFlag, DelFlag);

            var list = Data.BusViewDB.List(q, page, 15);
            return View(list);
        }
        #region 添加车辆
        public ActionResult BusAdd(int ID = 0)
        {

            var Driver = QueryBuilder.Create<Data.Driver>();
            Driver = Driver.Equals(x => x.DelFlag, "N");

            var Driverlist = Data.DriverDB.List(Driver);
            ViewBag.Driverlist = Driverlist;

            if (ID > 0)
            {
                var model = Data.BusViewDB.GETBusView(ID);
                return View(model);
            }

            return View();
        }
        [AdminIsLogin]
        [HttpPost]
        public JsonResult BusAdd(FormCollection fc)
        {
            var model = new Data.Bus();

            //model.ID = iRequest.GetQueryInt("ID");
            model.CreateTime = DateTime.Now;
            model.BusNo = fc["BusNo"];
            model.MotoType = fc["MotoType"];
            model.SeatCnt = TypeConverter.StrToInt(fc["SeatCnt"]);
            model.DriverID = TypeConverter.StrToInt(fc["DriverID"]);
            //model.Phone = fc["Phone"];
            model.Corp = fc["Corp"];
            model.InsuEndDate = TypeConverter.StrToDateTime(fc["InsuEndDate"]);
            model.BuyDate = TypeConverter.StrToDateTime(fc["BuyDate"]);
            model.OwnerName = fc["OwnerName"];
            model.OwnerPhone = fc["OwnerPhone"];
            model.Etc1 = fc["Etc1"];
            model.Etc2 = fc["Etc2"];
            model.Etc3 = fc["Etc3"];
            model.DelFlag = "N";

            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.BusDB.SaveEditBus(model);
            }
            else
            {
                aj.success = Data.BusDB.AddBus(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 车辆修改
        public ActionResult BusUpdate(int ID = 0)
        {
            var model = Data.BusViewDB.GETBusView(ID);

            var Driver = QueryBuilder.Create<Data.Driver>();
            Driver = Driver.Equals(x => x.DelFlag, "N");

            var Driverlist = Data.DriverDB.List(Driver);
            ViewBag.Driverlist = Driverlist;

            return View(model);
        }
        [AdminIsLogin]
        [HttpPost]
        public JsonResult BusUpdate(FormCollection fc)
        {
            var model = new Data.Bus();
            model.ID = iRequest.GetQueryInt("ID");
            model.CreateTime = DateTime.Now;
            model.BusNo = fc["BusNo"];
            model.MotoType = fc["MotoType"];
            model.SeatCnt = TypeConverter.StrToInt(fc["SeatCnt"]);
            model.DriverID = TypeConverter.StrToInt(fc["DriverID"]);
            //model.Phone = fc["Phone"];
            model.Corp = fc["Corp"];
            model.InsuEndDate = TypeConverter.StrToDateTime(fc["InsuEndDate"]);
            model.BuyDate = TypeConverter.StrToDateTime(fc["BuyDate"]);
            model.OwnerName = fc["OwnerName"];
            model.OwnerPhone = fc["OwnerPhone"];
            model.Etc1 = fc["Etc1"];
            model.Etc2 = fc["Etc2"];
            model.Etc3 = fc["Etc3"];
            model.DelFlag = "N";

            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.BusDB.SaveEditBus(model);
            }
            else
            {
                aj.success = Data.BusDB.AddBus(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region 司机管理
        [AdminIsLogin]
        public ActionResult DriverList(int page = 1, string DriverName="", int Sex=0, string Phone="",string DelFlag="N")
        {
            var q = QueryBuilder.Create<Data.Driver>();
            if (DriverName != "")
            {
                q = q.Like(x => x.DriverName, DriverName);
            }
            if (Sex != 0)
            {
                q = q.Equals(x => x.Sex, Sex);
            }
            if (Phone != "")
            {
                q = q.Like(x => x.Phone, Phone);
            }

            q = q.Equals(x => x.DelFlag, DelFlag);

            var list = Data.DriverDB.List(q, page, 15);
            return View(list);
        }
        #region 添加司机
        [AdminIsLogin]
        public ActionResult Driver(int ID = 0)
        {
            if (ID > 0)
            {
                var model = Data.DriverDB.GETDriver(ID);
                return View(model);
            }
            return View();
        }
        [AdminIsLogin]
        [HttpPost]
        public JsonResult Driver(FormCollection fc)
        {
            var model = new Data.Driver();

            //model.ID = iRequest.GetQueryInt("ID");
            model.CreateTime = DateTime.Now;
            model.DriverName = fc["DriverName"];
            model.Sex = TypeConverter.StrToInt(fc["Sex"]);
            model.Phone = fc["Phone"];
            model.IdCard = fc["IdCard"];
            model.BirthDay = TypeConverter.StrToDateTime(fc["BirthDay"]);
            model.DriveStartTime = TypeConverter.StrToDateTime(fc["DriveStartTime"]);
            model.BreakRules = fc["BreakRules"];
            model.Address = fc["Address"];
            model.Etc = fc["Etc"];
            model.DelFlag = "N";
            
            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.DriverDB.SaveEditDriver(model);
            }
            else
            {
                aj.success = Data.DriverDB.AddDriver(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 司机修改
        public ActionResult DriverUpdate(int ID = 0)
        {
            var model = Data.DriverDB.GETDriver(ID);
            return View(model);
        }
        [AdminIsLogin]
        [HttpPost]
        public JsonResult DriverUpdate(FormCollection fc)
        {
            var model = new Data.Driver();
            model.ID = iRequest.GetQueryInt("ID");
            model.CreateTime = DateTime.Now;
            model.DriverName = fc["DriverName"];
            model.Sex = TypeConverter.StrToInt(fc["Sex"]);
            model.Phone = fc["Phone"];
            model.IdCard = fc["IdCard"];
            model.BirthDay = TypeConverter.StrToDateTime(fc["BirthDay"]);
            model.DriveStartTime = TypeConverter.StrToDateTime(fc["DriveStartTime"]);
            model.BreakRules = fc["BreakRules"];
            model.Address = fc["Address"];
            model.Etc = fc["Etc"];

            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.DriverDB.SaveEditDriver(model);
            }
            else
            {
                aj.success = Data.DriverDB.AddDriver(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
                
        #region 线路管理

        #region 线路查询
        [AdminIsLogin]
        public ActionResult BusLineList(int page = 1, string LineName = "", string StartBusNo = "", string EndBusNo = "", string StartAddress = "", string EndAddress = "")
        {
            var q = QueryBuilder.Create<Data.BusLineView>();
            if (LineName != "")
            {
                q = q.Like(x => x.LineName, LineName);
            }
            //问题（或者条件）
            if (StartBusNo != "")
            {
                q = q.Like(x => x.StartBusNo, StartBusNo);
            }
            if (EndBusNo != "")
            {
                q = q.Like(x => x.EndBusNo, EndBusNo);
            }
            if (StartAddress != "")
            {
                q = q.Like(x => x.StartAddress, StartAddress);
            }
            if (EndAddress != "")
            {
                q = q.Like(x => x.EndAddress, EndAddress);
            }
            var list = Data.BusLineViewDB.List(q, page, 15);
            return View(list);
        }
        #endregion
        
        #region 线路添加
        [AdminIsLogin]
        public ActionResult BusLine(int ID = 0)
        {
            var Bus = QueryBuilder.Create<Data.Bus>();
            Bus = Bus.Equals(x => x.DelFlag, "N");

            var Buslist = Data.BusDB.List(Bus);
            ViewBag.Buslist = Buslist;
            
            if (ID > 0)
            {
                var model = Data.BusLineViewDB.GETBusLineView(ID);
                return View(model);
            }
            return View();
        }
        [AdminIsLogin]
        [HttpPost]
        public JsonResult BusLine(FormCollection fc)
        {
            var model = new Data.BusLine();
            //model.ID = iRequest.GetQueryInt("ID");
            model.LineName = fc["LineName"];
            model.StartBusID = TypeConverter.StrToInt(fc["StartBusNo"]);
            model.StartAddress = fc["StartAddress"];
            string _starttime = DateTime.Now.ToString("yyyy-MM-dd");
            _starttime = _starttime + " " + fc["StartTime"] + ":00";
            model.StartTime = TypeConverter.StrToDateTime(_starttime);

            model.StartLong = TypeConverter.StrToDouble(fc["StartLong"], 0);
            model.StartLat = TypeConverter.StrToDouble(fc["StartLat"], 0);

            model.EndBusID = TypeConverter.StrToInt(fc["EndBusNo"]);
            model.EndAddress = fc["EndAddress"];
            string _endtime = DateTime.Now.ToString("yyyy-MM-dd");
            _endtime = _endtime + " "+fc["EndTime"]+":00";
            model.EndTime = TypeConverter.StrToDateTime(_endtime);

            model.EndLong = TypeConverter.StrToDouble(fc["EndLong"], 0);
            model.EndLat = TypeConverter.StrToDouble(fc["EndLat"], 0);
            
            model.Price = TypeConverter.StrToDecimal(fc["Price"],0);
            model.PriceMon = TypeConverter.StrToDecimal(fc["PriceMon"], 0);
            model.PriceNgt = TypeConverter.StrToDecimal(fc["PriceNgt"], 0);

            model.MinNum = TypeConverter.StrToInt(fc["MinNum"]);
            model.TypeID = TypeConverter.StrToInt(fc["TypeID"]);
            model.Number = fc["Number"];
            model.Price2 = TypeConverter.StrToDecimal(fc["Price2"]);
            model.CheX = fc["Chex"];
            model.CheZ = fc["Chez"];
            model.TypeID = TypeConverter.StrToInt(fc["TypeID"]);
            
            model.CreateTime = DateTime.Now;

            model.Etc = fc["Etc"];
            model.DelFlag = "N";

            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.BusLineDB.SaveEditBusLine(model);
            }
            else
            {
                aj.success = Data.BusLineDB.AddBusLine(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 线路修改
        public ActionResult BusLineUpdate(int ID = 0)
        {
            var model = Data.BusLineViewDB.GETBusLineView(ID);

            //var Bus = QueryBuilder.Create<Data.Bus>();
            //Bus = Bus.Equals(x => x.DelFlag, "N");

            //var Buslist = Data.BusDB.List(Bus);
            //ViewBag.Buslist = Buslist;

            return View(model);
        }
        [AdminIsLogin]
        [HttpPost]
        public JsonResult BusLineUpdate(FormCollection fc)
        {
            var model = new Data.BusLine();
            model.ID = iRequest.GetQueryInt("ID");
            model.LineName = fc["LineName"];
            model.StartBusID = TypeConverter.StrToInt(fc["StartBusNo"]);
            model.StartAddress = fc["StartAddress"];
            string _starttime = DateTime.Now.ToString("yyyy-MM-dd");
            _starttime = _starttime + " " + fc["StartTime"] + ":00";
            model.StartTime = TypeConverter.StrToDateTime(_starttime);

            model.StartLong = TypeConverter.StrToDouble(fc["StartLong"], 0);
            model.StartLat = TypeConverter.StrToDouble(fc["StartLat"], 0);

            model.EndBusID = TypeConverter.StrToInt(fc["EndBusNo"]);
            model.EndAddress = fc["EndAddress"];
            string _endtime = DateTime.Now.ToString("yyyy-MM-dd");
            _endtime = _endtime + " " + fc["EndTime"] + ":00";
            model.EndTime = TypeConverter.StrToDateTime(_endtime);

            model.EndLong = TypeConverter.StrToDouble(fc["EndLong"], 0);
            model.EndLat = TypeConverter.StrToDouble(fc["EndLat"], 0);

            model.Price = TypeConverter.StrToDecimal(fc["Price"], 0);
            model.PriceMon = TypeConverter.StrToDecimal(fc["PriceMon"], 0);
            model.PriceNgt = TypeConverter.StrToDecimal(fc["PriceNgt"], 0);

            model.MinNum = TypeConverter.StrToInt(fc["MinNum"]);
            model.TypeID = TypeConverter.StrToInt(fc["TypeID"]);
            model.Number = fc["Number"];
            model.Price2 = TypeConverter.StrToDecimal(fc["Price2"]);
            model.CheX = fc["Chex"];
            model.CheZ = fc["Chez"];
            model.TypeID = TypeConverter.StrToInt(fc["TypeID"]);

            model.CreateTime = DateTime.Now;

            model.Etc = fc["Etc"];
            model.DelFlag = "N";

            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.BusLineDB.SaveEditBusLine(model);
            }
            else
            {
                aj.success = Data.BusLineDB.AddBusLine(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
        
        #region 缴费报表
        private IQueryBuilder<Data.PayView> getQueryBuilderWhere(string LineName="", string PayTime1 = "", string PayTime2 = "",
                                        string LockFlag = "", int MangerID = 0, string PayType = "0",
                                        decimal PayMoney1 = 0, decimal PayMoney2 = 0)
        {

            var q = QueryBuilder.Create<Data.PayView>();
            if (LineName != "")
            {
                q = q.Like(x => x.LineName, LineName);
            }
            if (PayTime1 != "" && PayTime2 != "")
            {
                q = q.Between(x => x.PayTime, Convert.ToDateTime(PayTime1), Convert.ToDateTime(PayTime2));
            }
            else if (PayTime1 != "" && PayTime2 == "")
            {
                q = q.Between(x => x.PayTime, Convert.ToDateTime(PayTime1), System.DateTime.Now);
            }
            else if (PayTime1 == "" && PayTime2 != "")
            {
                q = q.Between(x => x.PayTime, Convert.ToDateTime("1900-01-01"), Convert.ToDateTime(PayTime2));
            }

            if (LockFlag != null && LockFlag != "")
            {
                q = q.Equals(x => x.LockFlag, LockFlag);
            }

            if (MangerID != null && MangerID != 0)
            {
                q = q.Equals(x => x.MangerID, MangerID);
            }

            if (PayType != "0")
            {
                q = q.Equals(x => x.PayType, PayType);
            }

            if (PayMoney1 != 0 && PayMoney2 != 0)
            {
                q = q.Between(x => x.PayMoney, PayMoney1, PayMoney2);
            }
            else if (PayMoney1 != 0 && PayMoney2 == 0)
            {
                q = q.Between(x => x.PayMoney, PayMoney1, 999999999);
            }
            else if (PayMoney1 == 0 && PayMoney2 != 0)
            {
                q = q.Between(x => x.PayMoney, 0, PayMoney2);
            }

            return q;
        }

        [AdminIsLogin]
        public ActionResult PayReport(int page = 1, string LineName="", string PayTime1 = "", string PayTime2 = "",
                                        string LockFlag = "", int MangerID = 0, string PayType = "0",
                                        decimal PayMoney1 = 0, decimal PayMoney2 = 0)
        {
            var q = getQueryBuilderWhere(LineName, PayTime1, PayTime2,
                                        LockFlag, MangerID, PayType,
                                        PayMoney1, PayMoney2);
                      
            var list = Data.PayViewDB.List(q, page, 15);
            return View(list);
        }

		[AdminIsLogin]
        [HttpPost]
        public JsonResult PayLock(FormCollection fc)
        {

            var q = getQueryBuilderWhere(fc["LineName"], fc["PayTime1"], fc["PayTime2"],
                                        fc["LockFlag"], TypeConverter.StrToInt(fc["MangerID"]), fc["PayType"],
                                        TypeConverter.StrToDecimal(fc["PayMoney1"]), TypeConverter.StrToDecimal(fc["PayMoney2"]));

            var list = Data.PayViewDB.List(q);

            var model = new Data.Pay();
            AjaxJson aj = new AjaxJson();

            for (int i = 0; i < list.Count; i++)
            {

                model.ID = list[i].ID;
                model.LockFlag = "01";

                aj.success = Data.PayDB.LockPay(model);
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 车长收费报表
        private IQueryBuilder<Data.PayLmngView> getQBPayLmngReportWhere(string LineName = "", string PayTime = "", string Names = "",
                                        int MangerID = 0, string LockFlag = "")
        {

            var q = QueryBuilder.Create<Data.PayLmngView>();
            if (LineName != "")
            {
                q = q.Like(x => x.LineName, LineName);
            }

            if (PayTime != "")
            {
                q = q.Equals(x => x.PayTime.Year, Convert.ToDateTime(PayTime + ".01").Year);
                q = q.Equals(x => x.PayTime.Month, Convert.ToDateTime(PayTime + ".01").Month);
            }

            if (Names != "")
            {
                q = q.Like(x => x.Names, Names);
            }

            if (MangerID != 0)
            {
                q = q.Equals(x => x.MangerID, MangerID);
            }

            if (LockFlag != null && LockFlag != "")
            {
                q = q.Equals(x => x.LockFlag, LockFlag);
            }
            
            return q;
        }

        [AdminIsLogin]
        public ActionResult PayLmngReport(int page = 1, string LineName = "", string PayTime = "", string Names = "",
                                        int MangerID = 0, string LockFlag = "")
        {
            var q = getQBPayLmngReportWhere(LineName, PayTime, Names,
                                        MangerID, LockFlag);

            var list = Data.PayLmngViewDB.List(q, page, 15);
            return View(list);
        }

        [AdminIsLogin]
        [HttpPost]
        public JsonResult PayLmngLock(FormCollection fc)
        {

            var q = getQBPayLmngReportWhere(fc["LineName"], fc["PayTime"], fc["Names"],
                                        TypeConverter.StrToInt(fc["MangerID"]), fc["LockFlag"]);

            var list = Data.PayLmngViewDB.List(q);

            var model = new Data.PayLmng();
            AjaxJson aj = new AjaxJson();

            for (int i = 0; i < list.Count; i++)
            {

                model.ID = list[i].ID;
                model.LockFlag = "01";

                aj.success = Data.PayLmngDB.SaveEditPayLmng(model);
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region 数据合并工具
        [AdminIsLogin]
        public ActionResult MergeSelectTool(int page = 1, string SelectMode = "", string LN = "")
        {
            var qPhone = QueryBuilder.Create<Data.MergePhoneView>();
            var qName = QueryBuilder.Create<Data.MergeNameView>();
            ViewBag.MergePhonelist = null;

            //电话
            if (SelectMode == "0") {
                qPhone = qPhone.Like(x => x.LineName, LN);
                var list = Data.MergePhoneViewDB.List(qPhone, page, 15);
                ViewBag.MergePhonelist = list;
                return View();
            }
                //姓名
            else if (SelectMode == "1") {
                qName = qName.Like(x => x.LineName, LN);

                var list = Data.MergeNameViewDB.List(qName, page, 15);

                return View(list);
            }

            return View();
        }

        [AdminIsLogin]
        public ActionResult MergeTool(string Mode = "0", string Value = "", string DelFlag = "")
        {
            var q = QueryBuilder.Create<Data.Users>();

            if (Mode == "0")
            {
                q = q.Equals(x => x.Phone, Value);
            }
            else
            {
                q = q.Equals(x => x.Names, Value);
            }

            if (DelFlag == "")
            {
                q = q.Equals(x => x.DelFlag, "N");
            }

            var list = Data.UsersDB.UsersList(q);

            return View(list);
        }


        [AdminIsLogin]
        [HttpPost]
        public ActionResult MergeSave(FormCollection fc)
        {
            int delCnt = 0;
            int updCnt = 0;
             var userIDAll = fc["UserID"];
             if(userIDAll != null && userIDAll != "")
             {
                 var userIDAry = userIDAll.Split(',');
                 for (int i = 0; i < userIDAry.Length; i++)
                 {
                     var userID = userIDAry[i];

                     //用户信息
                     if (fc["DelFlag"] != null && fc["DelFlag"] != "")
                     {
                         var delFlag = "," + fc["DelFlag"] + ",";
                         if (delFlag.Contains(userID))
                         {
                             if (Data.UsersDB.DeleteUsers(TypeConverter.StrToInt(userID))) {
                                 delCnt++;
                             }
                             continue;
                         }
                     }

                     var model = new Bus.Data.Users();
                     model.ID = TypeConverter.StrToInt(userID);
                     model.Names = fc["Names_" + userID];
                     model.Phone = fc["Phone_" + userID];
                     model.Sex = TypeConverter.StrToInt(fc["Sex_" + userID] == "" ? "0" : fc["Sex_" + userID]);
                     model.CompanyName = fc["CompanyName_" + userID];
                     model.AddressSel = fc["AddressSel_" + userID];
                     model.Address = fc["Address_" + userID];
                     model.StartLong = TypeConverter.StrToDouble(fc["StartLong_" + userID]);
                     model.StartLat = TypeConverter.StrToDouble(fc["StartLat_" + userID]);
                     model.EndAddressSel = fc["EndAddressSel_" + userID];
                     model.EndAddress = fc["EndAddress_" + userID];
                     model.EndLong = TypeConverter.StrToDouble(fc["EndLong_" + userID]);
                     model.EndLat = TypeConverter.StrToDouble(fc["EndLat_" + userID]);
                     model.StartTime = TypeConverter.StrToDateTime(fc["StartTime_" + userID]);
                     model.EndTime = TypeConverter.StrToDateTime(fc["EndTime_" + userID]);

                     model.QQ = fc["QQ_" + userID];
                     model.EMail = fc["EMail_" + userID];
                     model.Etc = fc["Etc_" + userID];
                     model.StateID = TypeConverter.StrToInt(fc["StateID_" + userID]);
                     model.UserType = fc["UserType_" + userID];

                     model.UpdateMngID = LoginManger().ID;
                     model.UpdateTime = DateTime.Now;
                     model.DelFlag = "N";

                     if (Data.UsersDB.SaveMergeUsers(model))
                     {
                         updCnt++;
                     }

                     //线路信息
                     if (fc["LineUser_" + userID] != null && fc["LineUser_" + userID] != "")
                     {
                         var LineUserAll = fc["LineUser_" + userID];
                         var LineUserAry = LineUserAll.Split(',');

                         for (var j = 0; j < LineUserAry.Length; j++)
                         {
                             var LineUserID = LineUserAry[j];

                             var modelLU = new Data.LineUser();
                             modelLU.ID = TypeConverter.StrToInt(LineUserID);
                             modelLU.UserID = TypeConverter.StrToInt(userID);

                             Bus.Data.LineUserDB.ChangeLineUser(modelLU);
                         }
                     }
                 }
             }

             var message = "更新["+updCnt+"]条  删除["+delCnt+"]条";

            AjaxJson aj = new AjaxJson();
            return Json(new { message = aj.message }, JsonRequestBehavior.AllowGet);
        }
        


        #endregion
        
        #region 地图
        public ActionResult Map()
        {
            return View();
        }
        public ActionResult Map2()
        {
            return View();
        }
        #endregion

        #region 图文内容
        [AdminIsLogin]
        public ActionResult MyContentList(int page = 1)
        {
            var q = QueryBuilder.Create<Data.MyContent>();
            var list = Data.MyContentDB.List(q, page, 15);
            return View(list);
        }
        [AdminIsLogin]
        public ActionResult MyContent(int ID = 0)
        {
            if (ID > 0)
            {
                var model = Data.MyContentDB.GETMyContent(ID);
                return View(model);
            }
            return View();
        }
        [AdminIsLogin]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MyContent(FormCollection fc)
        {
            var model = new Data.MyContent();
            model.ID = iRequest.GetQueryInt("ID");
            model.action = fc["action"];
            model.Title = fc["Title"];
            model.Thumb = fc["Thumb"];
            model.SubContent = fc["SubContent"];
            model.Contents = fc["Contents"];
            model.CreateTime = DateTime.Now;
            model.LinkUrl = fc["LinkUrl"];
            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.MyContentDB.SaveEditMyContent(model);
            }
            else
            {
                aj.success = Data.MyContentDB.AddMyContent(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Question
        [AdminIsLogin]
        public ActionResult QuestionList(int page = 1)
        {
            var q = QueryBuilder.Create<Data.Question>();
            var list = Data.QuestionDB.List(q, page, 15);
            return View(list);
        }
        [AdminIsLogin]
        public ActionResult Question(int ID = 0)
        {
            if (ID > 0)
            {
                var model = Data.QuestionDB.GETQuestion(ID);
                return View(model);
            }
            return View();
        }
        [AdminIsLogin]
        [HttpPost]
        public ActionResult Question(FormCollection fc)
        {
            var model = new Data.Question();
            model.ID = iRequest.GetQueryInt("ID");
            model.Title = fc["Title"];
            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.QuestionDB.SaveEditQuestion(model);
            }
            else
            {
                aj.success = Data.QuestionDB.AddQuestion(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region QuestionItem
        [AdminIsLogin]
        public ActionResult QuestionItemList(int page = 1,int QuestionID=0)
        {
            var q = QueryBuilder.Create<Data.QuestionItem>().Equals(x=>x.QuestionID,QuestionID);
            var list = Data.QuestionItemDB.List(q, page, 15);
            return View(list);
        }
        [AdminIsLogin]
        public ActionResult QuestionItem(int ID = 0)
        {
            if (ID > 0)
            {
                var model = Data.QuestionItemDB.GETQuestionItem(ID);
                return View(model);
            }
            return View();
        }
        [AdminIsLogin]
        [HttpPost]
        public ActionResult QuestionItem(FormCollection fc)
        {
            var model = new Data.QuestionItem();
            model.ID = iRequest.GetQueryInt("ID");
            model.ItemName = fc["ItemName"];

            model.QuestionID = iRequest.GetQueryInt("QuestionID");
            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.QuestionItemDB.SaveEditQuestionItem(model);
            }
            else
            {
                aj.success = Data.QuestionItemDB.AddQuestionItem(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region QuestionItem2
        [AdminIsLogin]
        public ActionResult QuestionItem2List(int page = 1, int QuestionItemID=0)
        {
            var q = QueryBuilder.Create<Data.QuestionItem2>().Equals(x=>x.QuestionItemID,QuestionItemID);
            var list = Data.QuestionItem2DB.List(q, page, 15);
            return View(list);
        }
        [AdminIsLogin]
        public ActionResult QuestionItem2(int ID = 0)
        {
            if (ID > 0)
            {
                var model = Data.QuestionItem2DB.GETQuestionItem2(ID);
                return View(model);
            }
            return View();
        }
        [AdminIsLogin]
        [HttpPost]
        public ActionResult QuestionItem2(FormCollection fc)
        {
            var model = new Data.QuestionItem2();
            model.ID = iRequest.GetQueryInt("ID");
            model.QuestionItemID = iRequest.GetQueryInt("QuestionItemID"); //TypeConverter.StrToInt(fc["QuestionItemID"]);

            model.Value = fc["Value"];
            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.QuestionItem2DB.SaveEditQuestionItem2(model);
            }
            else
            {
                aj.success = Data.QuestionItem2DB.AddQuestionItem2(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 导出EXCEL
        public ActionResult ToExcel()
        {
            string GUID = Guid.NewGuid().ToString();
            var flag = false; var message = "";
            try
            {
                var fileTemplatePath = Server.MapPath("~/Content/template/Template.xlsx");
                var filePath = Server.MapPath(string.Format("~/Content/XLS/{0}.xlsx", GUID));
                this.ExcelOut(filePath, fileTemplatePath);
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", Server.UrlEncode("用户信息.xlsx")));
                Response.TransmitFile(filePath);
                this.Response.Flush();
                this.Response.End();

            }
            catch (Exception ee)
            {
                message = ee.Message;
            }
            return Json(new { success=flag,message=message},JsonRequestBehavior.AllowGet);
        }


        private void ExcelOut(string filePath, string fileTemplatePath)
        {
            try
            {
                System.IO.File.Copy(fileTemplatePath, filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("复制Excel文件出错" + ex.Message);
            }

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, true))
            {
                var sheetData = document.GetFirstSheetData();
                OpenXmlHelper.CellStyleIndex = 1;

                ////写标题相关信息
                //this.UpdateTitleText(sheetData);

                const string A = "A", B = "B", C = "C", D = "D", E = "E", F = "F", G = "G", H = "H", I = "I", J = "J", K = "K",L="L",M="M";

                var LIST = Bus.Data.UsersDB.UsersList();
                 int StartRowIndex = 4;
                foreach (var item in LIST)
                {
                    var rowIndex = StartRowIndex++;
                    sheetData.SetCellValue(A + rowIndex, item.Number);
                    sheetData.SetCellValue(B + rowIndex, item.Names);
                    sheetData.SetCellValue(C + rowIndex, item.Sex == 1 ? "女" : item.Sex == 2 ? "男" : "");
                    sheetData.SetCellValue(D + rowIndex, item.Phone);
                    sheetData.SetCellValue(E + rowIndex, item.QQ);
                    sheetData.SetCellValue(F + rowIndex, item.EMail);
                    sheetData.SetCellValue(G + rowIndex, item.Address);
                    sheetData.SetCellValue(H + rowIndex, item.EndAddress);
                    //sheetData.SetCellValue(I + rowIndex, item.isFinal?item.StartTime.ToString("HH:mm"):"");
                    //sheetData.SetCellValue(J + rowIndex, item.isFinal?item.EndTime.ToString("HH:mm"):"");
                    sheetData.SetCellValue(K + rowIndex, item.CreateTime.ToString());
                    sheetData.SetCellValue(L + rowIndex, item.StartLat.ToString() + "," + item.StartLong.ToString());
                    sheetData.SetCellValue(M + rowIndex, item.EndLat.ToString()+","+item.EndLong.ToString());
                }




                //const  Len = 10;

                //for (var i = 0; i < Len; i++)
                //{
                //    var rowIndex = StartRowIndex + i;

                //    // 员工信息
                //    sheetData.SetCellValue(ENo + rowIndex, "Eno" + rowIndex);
                //    sheetData.SetCellValue(EB + rowIndex, DateTime.Now.AddYears(-30).AddYears(new Random().Next(1, 30)));
                //    sheetData.SetCellValue(EName + rowIndex, "员工姓名" + rowIndex);
                //    sheetData.SetCellValue(EW + rowIndex, "入职时间为：" + DateTime.Now.AddYears(-3).AddDays(new Random().Next(1, 1000)));

                //    // 部门信息
                //    sheetData.SetCellValue(DNo + rowIndex, "DNo" + rowIndex);
                //    sheetData.SetCellValue(DName + rowIndex, "部门名称" + rowIndex);

                //    // 备注
                //    sheetData.SetCellValue(R + rowIndex, "Remark:" + rowIndex);
                //}

                // var str = OpenXmlHelper.ValidateDocument(document);验证生成的Excel
            }
        }


        #endregion

        #region 导出EXCEL(PayReport)
        public ActionResult ToExcelReport(int page = 1, string LineName="", string PayTime1 = "", string PayTime2 = "",
                                        string LockFlag = "", int MangerID = 0, string PayType = "0",
                                        decimal PayMoney1 = 0, decimal PayMoney2 = 0)
        {
            string GUID = Guid.NewGuid().ToString();
            var flag = false; var message = "";
            try
            {
                var fileTemplatePath = Server.MapPath("~/Content/template/TemplateReport.xlsx");
                var filePath = Server.MapPath(string.Format("~/Content/XLS/{0}.xlsx", GUID));

                var q = getQueryBuilderWhere(LineName, PayTime1, PayTime2,
                                        LockFlag, MangerID, PayType,
                                        PayMoney1, PayMoney2);

                this.ExcelReportOut(filePath, fileTemplatePath, q);
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", Server.UrlEncode("缴费报表.xlsx")));
                Response.TransmitFile(filePath);
                this.Response.Flush();
                this.Response.End();

            }
            catch (Exception ee)
            {
                message = ee.Message;
            }
            return Json(new { success = flag, message = message }, JsonRequestBehavior.AllowGet);
        }

        private void ExcelReportOut(string filePath, string fileTemplatePath, IQueryBuilder<Data.PayView> q)
        {
            try
            {
                System.IO.File.Copy(fileTemplatePath, filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("复制Excel文件出错" + ex.Message);
            }

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, true))
            {
                var sheetData = document.GetFirstSheetData();
                OpenXmlHelper.CellStyleIndex = 1;

                ////写标题相关信息
                //this.UpdateTitleText(sheetData);

                const string A = "A", B = "B", C = "C", D = "D", E = "E", F = "F", G = "G", H = "H", I = "I", J = "J", K = "K", L = "L", M = "M";

                var LIST = Data.PayViewDB.PayViewList(q);

                int StartRowIndex = 4;
                foreach (var item in LIST)
                {
                    var rowIndex = StartRowIndex++;
                    sheetData.SetCellValue(A + rowIndex, item.LineName);
                    sheetData.SetCellValue(B + rowIndex, item.RideType);
                    sheetData.SetCellValue(C + rowIndex, item.Names);
                    sheetData.SetCellValue(D + rowIndex, item.Sex == 1 ? "女" : item.Sex == 2 ? "男" : "");
                    sheetData.SetCellValue(E + rowIndex, item.Phone);
                    sheetData.SetCellValue(F + rowIndex, item.StartDate.ToString("yyyy.MM.dd"));
                    sheetData.SetCellValue(G + rowIndex, item.EndDate.ToString("yyyy.MM.dd"));
                    sheetData.SetCellValue(H + rowIndex, item.PayMoney);
                    sheetData.SetCellValue(I + rowIndex, item.ManageName);
                    sheetData.SetCellValue(J + rowIndex, item.PayName);
                    sheetData.SetCellValue(K + rowIndex, item.Etc);
                    sheetData.SetCellValue(L + rowIndex, item.PayTime.ToString("yyyy.MM.dd"));
                    sheetData.SetCellValue(M + rowIndex, item.LockFlag == "00" ? "未锁定" : item.LockFlag == "01" ? "已锁定" : "");
                }
            }
        }

        #endregion

        #region 导出EXCEL(PayLmngReport)
        public ActionResult ToExcelPayLmngReport(int page = 1, string LineName = "", string PayTime = "", string Names = "",
                                        int MangerID = 0, string LockFlag = "")
        {
            string GUID = Guid.NewGuid().ToString();
            var flag = false; var message = "";
            try
            {
                var fileTemplatePath = Server.MapPath("~/Content/template/TemplatePayLmngReport.xlsx");
                var filePath = Server.MapPath(string.Format("~/Content/XLS/{0}.xlsx", GUID));

                var q = getQBPayLmngReportWhere(LineName, PayTime, Names,
                                        MangerID, LockFlag);

                this.ExcelPayLmngReportOut(filePath, fileTemplatePath, q);
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", Server.UrlEncode("车长收费报表.xlsx")));
                Response.TransmitFile(filePath);
                this.Response.Flush();
                this.Response.End();

            }
            catch (Exception ee)
            {
                message = ee.Message;
            }
            return Json(new { success = flag, message = message }, JsonRequestBehavior.AllowGet);
        }

        private void ExcelPayLmngReportOut(string filePath, string fileTemplatePath, IQueryBuilder<Data.PayLmngView> q)
        {
            try
            {
                System.IO.File.Copy(fileTemplatePath, filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("复制Excel文件出错" + ex.Message);
            }

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, true))
            {
                var sheetData = document.GetFirstSheetData();
                OpenXmlHelper.CellStyleIndex = 1;

                ////写标题相关信息
                //this.UpdateTitleText(sheetData);

                const string A = "A", B = "B", C = "C", D = "D", E = "E", F = "F", G = "G", H = "H", I = "I", J = "J", K = "K", L = "L";

                var LIST = Data.PayLmngViewDB.PayLmngViewList(q);

                int StartRowIndex = 4;
                foreach (var item in LIST)
                {
                    var rowIndex = StartRowIndex++;
                    sheetData.SetCellValue(A + rowIndex, item.LineName);
                    sheetData.SetCellValue(B + rowIndex, item.PayTime.ToString("yyyy年MM月"));
                    sheetData.SetCellValue(C + rowIndex, item.Names);
                    sheetData.SetCellValue(D + rowIndex, item.Sex == 1 ? "女" : item.Sex == 2 ? "男" : "");
                    sheetData.SetCellValue(E + rowIndex, item.Phone);
                    if (item.PayMoneyYS.ToString() == "")
                    {
                        sheetData.SetCellValue(F + rowIndex, ""); 
                    }
                    else {
                        sheetData.SetCellValue(F + rowIndex, item.PayMoneyYS.ToString().Remove(item.PayMoneyYS.ToString().IndexOf(".")));
                    }
                    if (item.PayMoneySS.ToString() == "")
                    {
                        sheetData.SetCellValue(G + rowIndex, "");
                    }
                    else {
                        sheetData.SetCellValue(G + rowIndex, item.PayMoneySS.ToString().Remove(item.PayMoneySS.ToString().IndexOf(".")));
                    }
                    if (item.PayMoneyDC.ToString() == "")
                    {
                        sheetData.SetCellValue(H + rowIndex, "");
                    }
                    else {
                        sheetData.SetCellValue(H + rowIndex, item.PayMoneyDC.ToString().Remove(item.PayMoneyDC.ToString().IndexOf(".")));
                    }
                    sheetData.SetCellValue(I + rowIndex, item.ManagerName);
                    sheetData.SetCellValue(J + rowIndex, item.Ect);
                    sheetData.SetCellValue(K + rowIndex, item.PayTime.ToString("yyyy.MM.dd"));
                    sheetData.SetCellValue(L + rowIndex, item.LockFlag == "00" ? "未锁定" : item.LockFlag == "01" ? "已锁定" : "");
                }
            }
        }

        #endregion

        #region 会员导出EXCEL(UsersList)
        public ActionResult ToExcelUsersList(int page = 1, string Names = "", string Phone = "",
                                                int StateID = -1, string StartLatLong = "", string EndLatLong = "",
                                                string QQ = "", string A1 = "", string A2 = "", string t1 = "", string t2 = "",
                                                string corp = "", string LN = "", string RT = "", string NoLine = "")
        {
            string GUID = Guid.NewGuid().ToString();
            var flag = false; var message = "";
            try
            {
                var fileTemplatePath = Server.MapPath("~/Content/template/TemplateUsersListReport.xlsx");
                var filePath = Server.MapPath(string.Format("~/Content/XLS/{0}.xlsx", GUID));

                var q = getQBUsersListWhere(Names, Phone, StateID, StartLatLong, EndLatLong,
                                                QQ, A1, A2, t1, t2, corp, LN, RT, NoLine);

                this.ExcelUsersListOut(filePath, fileTemplatePath, q);
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", Server.UrlEncode("会员列表.xlsx")));
                Response.TransmitFile(filePath);
                this.Response.Flush();
                this.Response.End();

            }
            catch (Exception ee)
            {
                message = ee.Message;
            }
            return Json(new { success = flag, message = message }, JsonRequestBehavior.AllowGet);
        }

        private void ExcelUsersListOut(string filePath, string fileTemplatePath, IQueryBuilder<Data.UsersView> q)
        {
            try
            {
                System.IO.File.Copy(fileTemplatePath, filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("复制Excel文件出错" + ex.Message);
            }

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, true))
            {
                var sheetData = document.GetFirstSheetData();
                OpenXmlHelper.CellStyleIndex = 1;

                ////写标题相关信息
                //this.UpdateTitleText(sheetData);

                const string A = "A", B = "B", C = "C", D = "D", E = "E", F = "F", G = "G", H = "H", I = "I", J = "J", K = "K", L = "L";

                var LIST = Data.UsersViewDB.UsersViewList(q);

                int StartRowIndex = 4;
                foreach (var item in LIST)
                {
                    var rowIndex = StartRowIndex++;
                    sheetData.SetCellValue(A + rowIndex, item.Names);
                    sheetData.SetCellValue(B + rowIndex, item.Sex == 1 ? "女" : item.Sex == 2 ? "男" : "");
                    sheetData.SetCellValue(C + rowIndex, item.Phone);
                    sheetData.SetCellValue(D + rowIndex, item.QQ);
                    sheetData.SetCellValue(E + rowIndex, item.EMail);
                    sheetData.SetCellValue(F + rowIndex, item.AddressSel + item.Address);
                    sheetData.SetCellValue(G + rowIndex, item.EndAddressSel + item.EndAddress);
                    sheetData.SetCellValue(H + rowIndex, item.CompanyName);
                    sheetData.SetCellValue(I + rowIndex, item.StartTime.ToString("HH:mm"));
                    sheetData.SetCellValue(J + rowIndex, item.EndTime.ToString("HH:mm"));
                    sheetData.SetCellValue(K + rowIndex, item.LineName);
                    sheetData.SetCellValue(L + rowIndex, item.Etc);
                }
            }
        }

        private IQueryBuilder<Data.UsersView> getQBUsersListWhere(string Names = "", string Phone = "",
                                                int StateID = -1, string StartLatLong = "", string EndLatLong = "",
                                                string QQ = "", string A1 = "", string A2 = "", string t1 = "", string t2 = "",
                                                string corp = "", string LN = "", string RT = "", string NoLine = "")
        {

            var q = QueryBuilder.Create<Data.UsersView>();
            if (StateID > -1)
            {
                q = q.Equals(x => x.StateID, StateID);
            }
            if (Names != "")
            {
                q = q.Like(x => x.Names, Names);
            }
            if (Phone != "")
            {
                q = q.Like(x => x.Phone, Phone);
            }
            if (QQ != "")
            {
                q = q.Like(x => x.QQ, QQ);
            }
            if (A1 != "")
            {
                q = q.Like(x => x.Address, A1);
            }
            if (A2 != "")
            {
                q = q.Like(x => x.CompanyName, A2);
            }
            if (t1 != "")
            {
                var t = TypeConverter.StrToDateTime("2010-01-01 " + t1 + ":00");
                q = q.Equals(x => x.StartTime.Hour, t.Hour).Equals(x => x.StartTime.Minute, t.Minute);
            }
            if (t2 != "")
            {
                var t = TypeConverter.StrToDateTime("2010-01-01 " + t2 + ":00");
                q = q.Equals(x => x.EndTime.Hour, t.Hour).Equals(x => x.EndTime.Minute, t.Minute);
            }

            if (corp != "")
            {
                q = q.Like(x => x.CompanyName, corp);
            }

            if (LN != "")
            {
                q = q.Like(x => x.LineName, LN);
            }

            if (RT != "")
            {

            }

            if (NoLine != "")
            {
                q = q.Equals(x => x.LineName, null);
            }

            return q;
        }

        #endregion

        #region 导入EXCEL
        public ActionResult ImportExcel(object obj)
        {
            string error = string.Empty;
            ViewData["ErrorMsg"] = "";

            try
            {
                foreach (string upload in Request.Files)
                {
                    if (upload != null && upload.Trim() != "")
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory + "TempData\\";

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        System.Web.HttpPostedFileBase postedFile = Request.Files[upload];

                        string fileName = System.IO.Path.GetFileName(postedFile.FileName);

                        if (fileName.Length > 4)
                        {
                            string strExName = fileName.Substring(fileName.Length - 4, 4);

                            if (strExName.ToLower() != ".xls")
                            {
                                error = "文件类型不正确，请重新操作";
                                ViewData["ErrorMsg"] = error;
                            }
                            else
                            {
                                string fileNamePath = path + DateTime.Now.Ticks.ToString() + ".xls";

                                postedFile.SaveAs(fileNamePath);

                                string fileExtension;

                                fileExtension = System.IO.Path.GetExtension(fileName);

                                string FileType = postedFile.ContentType.ToString();//获取要上传的文件类型,验证文件头

                                //在上传文件不为空的情况下，验证文件名以及大小是否符合，如果不符合则不允许上传
                                if (postedFile.ContentLength / 1024 <= 5120)
                                { //在这里通过检查文件头与文件名是否匹配 从而限制了文件上传类型  注：可上传的类型有XLS，且大小只能为5M一下

                                    string[] ExcelSheetNames = GetExcelSheetNames(fileNamePath);
                                    List<string[]>[] ExcelSheetError = new List<string[]>[ExcelSheetNames.Length];
                                    string[] ExcelSheetSuccess = new String[ExcelSheetNames.Length];

                                    DataTable dt;
                                    var flag = false;

                                    for (var i = 0; i < ExcelSheetNames.Length; i++)
                                    {
                                        try
                                        {
                                            dt = GetExcelToDataTableBySheetName(fileNamePath, ExcelSheetNames[i]);
                                        }
                                        catch (Exception e)
                                        {
                                            continue;
                                        }
                                        
                                        var errorList = new List<string[]>();

                                        if (dt.Rows.Count > 0)
                                        {
                                            int j = 0;
                                            int success = 0;
                                            var payYearMonth1 = "";
                                            var payYearMonth2 = "";
                                            var payYearMonth3 = "";
                                            var UserID = 0;
                                            var LineUserID = 0;
                                            var errorArray = new string[4];
                                            errorList = new List<string[]>();
                                            Data.Users usersModel;
                                            Data.LineUser lineUserModel;
                                            Data.Pay payModel;

                                            success = 0;
                                            foreach (DataRow item in dt.Rows)
                                            {
                                                var Lineflag = false;

                                                if (j == 3)
                                                {
                                                    payYearMonth1 = item[12].ToString();
                                                    payYearMonth2 = item[13].ToString();
                                                    payYearMonth3 = item[14].ToString();
                                                }

                                                if (j > 3 && item[0].ToString() != "")
                                                {
                                                    //dbo.Users
                                                    usersModel = new Data.Users();

                                                    usersModel.WXUserID = 0;
                                                    usersModel.Names = item[2].ToString();
                                                    usersModel.Password = Encrypt.DES.Des_Encrypt("123456");
                                                    usersModel.Phone = item[4].ToString();
                                                    usersModel.Address = item[7].ToString();
                                                    try
                                                    {
                                                        usersModel.StartTime = TypeConverter.StrToDateTime(item[9].ToString());
                                                        usersModel.EndTime = TypeConverter.StrToDateTime(item[10].ToString());
                                                    }
                                                    catch(Exception e)
                                                    {
                                                        usersModel.StartTime = TypeConverter.StrToDateTime("00:01");
                                                        usersModel.EndTime = TypeConverter.StrToDateTime("00:01");
                                                    }
                                                    usersModel.StartLong = 0;
                                                    usersModel.StartLat = 0;
                                                    usersModel.EndLong = 0;
                                                    usersModel.EndLat = 0;
                                                    usersModel.Sex = item[3].ToString() == "男" ? 1 : item[3].ToString() == "女"? 2 : 0;
                                                    usersModel.EndAddress = item[8].ToString();
                                                    usersModel.ParentUserID = 0;
                                                    usersModel.EMail = item[5].ToString();
                                                    usersModel.QQ = item[6].ToString();
                                                    usersModel.StateID = 0;
                                                    usersModel.UserType = "USER";
                                                    usersModel.Etc = item[11].ToString();
                                                    usersModel.DataFrom = "Excel";
                                                    usersModel.CreateMngID = LoginManger().ID;

                                                    UserID = Data.UsersDB.AddUsers(usersModel);

                                                    flag = UserID > 0;

                                                    if (flag)
                                                    {
                                                        LineUserID = 0;

                                                        //线路信息
                                                        var lineNm = item[0].ToString();
                                                        var lineList = Bus.Data.BusLineDB.BusLineListByName(lineNm);
                                                        if (lineList.Count == 1)
                                                        {
                                                            lineUserModel = new Data.LineUser();
                                                            lineUserModel.LineID = lineList[0].ID;
                                                            lineUserModel.UserID = UserID;
                                                            lineUserModel.RideType = "MA";
                                                            lineUserModel.CreateTime = DateTime.Now;
                                                            lineUserModel.StateID = 0;
                                                            lineUserModel.DelFlag = "N";

                                                            LineUserID = Data.LineUserDB.AddLineUser(lineUserModel);
                                                        }

                                                        Lineflag = LineUserID > 0;

                                                        if (Lineflag)
                                                        {
                                                            //dbo.Pay
                                                            if (payYearMonth1.Length == 6 && payYearMonth1.Substring(0, 4) == "2014")
                                                            {
                                                                payModel = new Data.Pay();

                                                                payModel.UserID = UserID;
                                                                payModel.LineUserID = LineUserID;
                                                                payModel.StartDate = GetFirstDayOfMonth(payYearMonth1);
                                                                payModel.EndDate = GetLastDayOfMonth(payYearMonth1);
                                                                payModel.PayTime = DateTime.Now;
                                                                payModel.PayMoney = TypeConverter.StrToDecimal(item[12].ToString());
                                                                payModel.PayType = "GH";
                                                                payModel.MangerID = LoginManger().ID;
                                                                payModel.UpdateTime = DateTime.Now;
                                                                payModel.CreateTime = DateTime.Now;
                                                                payModel.DelFlag = "N";

                                                                if (payModel.PayMoney > 0)
                                                                {
                                                                    Data.PayDB.AddPay(payModel);
                                                                }

                                                            }

                                                            if (payYearMonth2.Length == 6 && payYearMonth2.Substring(0, 4) == "2014")
                                                            {
                                                                payModel = new Data.Pay();

                                                                payModel.UserID = UserID;
                                                                payModel.LineUserID = LineUserID;
                                                                payModel.StartDate = GetFirstDayOfMonth(payYearMonth2);
                                                                payModel.EndDate = GetLastDayOfMonth(payYearMonth2);
                                                                payModel.PayTime = DateTime.Now;
                                                                payModel.PayMoney = TypeConverter.StrToDecimal(item[13].ToString());
                                                                payModel.PayType = "GH";
                                                                payModel.MangerID = LoginManger().ID;
                                                                payModel.UpdateTime = DateTime.Now;
                                                                payModel.CreateTime = DateTime.Now;
                                                                payModel.DelFlag = "N";

                                                                if (payModel.PayMoney > 0)
                                                                {
                                                                    Data.PayDB.AddPay(payModel);
                                                                }
                                                            }


                                                            if (payYearMonth3.Length == 6 && payYearMonth3.Substring(0, 4) == "2014")
                                                            {
                                                                payModel = new Data.Pay();

                                                                payModel.UserID = UserID;
                                                                payModel.LineUserID = LineUserID;
                                                                payModel.StartDate = GetFirstDayOfMonth(payYearMonth3);
                                                                payModel.EndDate = GetLastDayOfMonth(payYearMonth3);
                                                                payModel.PayTime = DateTime.Now;
                                                                payModel.PayMoney = TypeConverter.StrToDecimal(item[14].ToString());
                                                                payModel.PayType = "GH";
                                                                payModel.MangerID = LoginManger().ID;
                                                                payModel.UpdateTime = DateTime.Now;
                                                                payModel.CreateTime = DateTime.Now;
                                                                payModel.DelFlag = "N";

                                                                if (payModel.PayMoney > 0)
                                                                {
                                                                    Data.PayDB.AddPay(payModel);
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (!flag || !Lineflag)
                                                    {
                                                        errorArray = new string[4];

                                                        errorArray[0] = (j + 1).ToString();
                                                        errorArray[1] = item[2].ToString();

                                                        if (!flag){ errorArray[2] = "0";}
                                                        else { errorArray[2] = "1"; }

                                                        if (!Lineflag) { errorArray[3] = "0"; }
                                                        else { errorArray[3] = "1"; }

                                                        errorList.Add(errorArray);
                                                    }
                                                    else 
                                                    {
                                                        success++;
                                                    }
                                                }

                                                j++;
                                            }

                                            ExcelSheetSuccess[i] =  success.ToString();
                                        }

                                        
                                        ExcelSheetError[i] = errorList;
                                    }

                                    ViewData["ExcelSheetNames"] = ExcelSheetNames;
                                    ViewData["ExcelSheetError"] = ExcelSheetError;
                                    ViewData["ExcelSheetSuccess"] = ExcelSheetSuccess;
                                }
                                else
                                {
                                    error = "数据文件过大，请重新操作";
                                    ViewData["ErrorMsg"] = error;
                                }
                            }
                        }
                        else
                        {
                            error = "请选择需要导入的文件！";
                            ViewData["ErrorMsg"] = error;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMsg"] = ex.Message;
            }

            return View();
        }

        public static DateTime GetFirstDayOfMonth(string YearMonth)
        {
            int Year = TypeConverter.StrToInt(YearMonth.Substring(0, 4));
            int Month = TypeConverter.StrToInt(YearMonth.Substring(4, 2));

            return Convert.ToDateTime(Year.ToString() + "-" + Month.ToString() + "-1");
        }

        public static DateTime GetLastDayOfMonth(string YearMonth)
        {
            int Year = TypeConverter.StrToInt(YearMonth.Substring(0, 4));
            int Month = TypeConverter.StrToInt(YearMonth.Substring(4, 2));
            int Days = DateTime.DaysInMonth(Year, Month);

            return Convert.ToDateTime(Year.ToString() + "-" + Month.ToString() + "-" + Days.ToString());
        }

        //根据Excel物理路径获取Excel文件中所有表名
        public static String[] GetExcelSheetNames(string FileFullPath)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

            try
            {
                string strConn = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + FileFullPath + ";Extended Properties='Excel 8.0; HDR=NO; IMEX=1'"; //此连接只能操作Excel2007之前(.xls)文件
                //string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + FileFullPath + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'"; //此连接可以操作.xls与.xlsx文件

                objConn = new OleDbConnection(strConn);

                objConn.Open();

                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    return null;
                }

                
                int i = 0;

                List<String> list = new List<String>();
                String[] excelSheets;
                foreach (DataRow row in dt.Rows)
                {
                    string sheetName = row["TABLE_NAME"].ToString();
                    if (sheetName != null && sheetName != "" && sheetName.Substring(sheetName.Length - 2) == "$'")
                    { 
                        list.Add(sheetName);
                    }
                    i++;
                }

                excelSheets = list.ToArray();
                return excelSheets;
            }
            catch(Exception e)
            {
                return null;
            }
            finally
            {
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }

        //根据Excel物理路径、表名(Sheet名)获取数据集
        public static DataTable GetExcelToDataTableBySheetName(string FileFullPath, string SheetName)
        {
            //string strConn = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + FileFullPath + ";Extended Properties='Excel 8.0; HDR=NO; IMEX=1'"; //此连接只能操作Excel2007之前(.xls)文件
            string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + FileFullPath + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'"; //此连接可以操作.xls与.xlsx文件

            OleDbConnection conn = new OleDbConnection(strConn);

            conn.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter odda = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", SheetName), conn);                    //("select * from [Sheet1$]", conn);

            odda.Fill(ds, SheetName);
            conn.Close();

            return ds.Tables[0];
        }
        #endregion

        #region 会员
        //状态
        public static string GetStatesName(int ID)
        {
            var model = Data.UserStateDB.GETUserState(ID);
            return model==null?"":model.StateName;
        }
        
        [AdminIsLogin]
        public ActionResult UsersList2(int page = 1, int StateID = 0, string StartLatLong = "", string EndLatLong = "")
        {
            //var q = QueryBuilder.Create<Data.Users>();
            var q = QueryBuilder.Create<Data.UsersView>();
            if (StateID > 0) {
                q = q.Equals(x => x.StateID, StateID);
            }
            var alllist = Data.UsersViewDB.UsersViewList(q);
            var list2 = new List<Data.UsersView>();
            if ((StartLatLong != "" && StartLatLong.IndexOf(',')>0) ||
                (EndLatLong != "" && EndLatLong.IndexOf(',') > 0) )
            {
                double lngS = 0, latS = 0, lngE = 0, latE = 0, juliStart, juliEnd;

                if (StartLatLong != "" && StartLatLong.IndexOf(',') > 0)
                {
                    lngS = TypeConverter.StrToDouble(StartLatLong.Split(',').GetValue(0).ToString());
                    latS = TypeConverter.StrToDouble(StartLatLong.Split(',').GetValue(1).ToString());
                }

                if (EndLatLong != "" && EndLatLong.IndexOf(',') > 0)
                {
                    lngE = TypeConverter.StrToDouble(EndLatLong.Split(',').GetValue(0).ToString());
                    latE = TypeConverter.StrToDouble(EndLatLong.Split(',').GetValue(1).ToString());
                }


                foreach (var item in alllist)
                {
                    juliStart = 999; juliEnd = 999;

                    if (StartLatLong != "" && StartLatLong.IndexOf(',') > 0)
                    {
                        juliStart = Common.GetShortDistance(item.StartLong, item.StartLat, lngS, latS);
                    }

                    if (EndLatLong != "" && EndLatLong.IndexOf(',') > 0)
                    {
                        juliEnd = Common.GetShortDistance(item.EndLong, item.EndLat, lngE, latE);
                    }

                    if (juliStart <= 1000 && juliEnd <= 1000)
                    {
                        if (!list2.Contains(item)) { list2.Add(item); }
                    }
                }
            }

            return View(list2);
        }
        
        [AdminIsLogin]
        public ActionResult UsersList(int page = 1, string Names = "",string Phone="", 
            int StateID = -1, string StartLatLong = "", string EndLatLong = "",
            string QQ="",string A1="",string A2="",string t1="",string t2="",
            string corp="", string LN="",string RT="",string NoLine="")
        {

            var q = getQBUsersListWhere(Names, Phone, StateID, StartLatLong, EndLatLong,
                                                QQ, A1, A2, t1, t2, corp, LN, RT, NoLine);

            //var q = QueryBuilder.Create<Data.UsersView>();
            //if (StateID > -1)
            //{
            //    q = q.Equals(x => x.StateID, StateID);
            //}
            //if (Names != "")
            //{
            //    q = q.Like(x => x.Names, Names);
            //}
            //if (Phone != "")
            //{
            //    q = q.Like(x => x.Phone, Phone);
            //}
            //if (QQ != "")
            //{
            //    q = q.Like(x => x.QQ, QQ);
            //}
            //if (A1 != "")
            //{
            //    q = q.Like(x => x.Address, A1);
            //}
            //if (A2 != "")
            //{
            //    q = q.Like(x => x.CompanyName, A2);
            //}
            //if (t1 != "")
            //{
            //    var t = TypeConverter.StrToDateTime("2010-01-01 " + t1 + ":00");
            //    q = q.Equals(x => x.StartTime.Hour, t.Hour).Equals(x => x.StartTime.Minute, t.Minute);
            //}
            //if (t2 != "")
            //{
            //    var t = TypeConverter.StrToDateTime("2010-01-01 " + t2 + ":00");
            //    q = q.Equals(x => x.EndTime.Hour, t.Hour).Equals(x => x.EndTime.Minute, t.Minute);
            //}

            //if (corp != "")
            //{
            //    q = q.Like(x => x.CompanyName, corp);
            //}

            //if (LN != "")
            //{
            //    q = q.Like(x => x.LineName, LN);
            //}

            //if (RT != "")
            //{

            //}

            //if (NoLine != "")
            //{
                
            //}

            var list = Data.UsersViewDB.List(q, page, 15);
            return View(list);

        }
        [AdminIsLogin]
        public ActionResult Users(int ID = 0)
        {
            if (ID > 0)
            {
                var model = Data.UsersDB.GETUsers(ID);
                if (model != null)
                {
                    try { model.Password = Encrypt.DES.Des_Decrypt(model.Password); }
                    catch (Exception e) { }
                }

                return View(model);
            }
            return View();
        }
        [AdminIsLogin]
        [HttpPost]
        public ActionResult Users(FormCollection fc)
        {
            var model = new Data.Users();
            model.ID = iRequest.GetQueryInt("ID");

            model.Names = fc["Names"];
            model.Phone = fc["Phone"];
            model.Sex = TypeConverter.StrToInt(fc["Sex"]);

            //密码
            string pwd=fc["Password"];
            pwd = Encrypt.DES.Des_Encrypt(pwd);
            model.Password = pwd;

            model.CardNo = fc["CardNo"];

            string _starttime = DateTime.Now.ToString("yyyy-MM-dd");
            _starttime = _starttime + " " + fc["StartTime"] + ":00";
            model.StartTime = TypeConverter.StrToDateTime(_starttime);

            string _endtime = DateTime.Now.ToString("yyyy-MM-dd");
            _endtime = _endtime + " " + fc["EndTime"] + ":00";
            model.EndTime = TypeConverter.StrToDateTime(_endtime);

            model.CompanyName = fc["CompanyName"];

            //地址
            model.AddressSel = fc["AddressSel"];
            model.Address = fc["Address"];
            model.EndAddressSel = fc["EndAddressSel"];
            model.EndAddress = fc["EndAddress"];
            model.StartLat = fc["StartLat"] == "" ? 0 : TypeConverter.StrToDouble(fc["StartLat"]);
            model.StartLong = fc["StartLong"] == "" ? 0 : TypeConverter.StrToDouble(fc["StartLong"]);
            model.EndLat = fc["EndLat"] == "" ? 0 : TypeConverter.StrToDouble(fc["EndLat"]);
            model.EndLong = fc["EndLong"] == "" ? 0 : TypeConverter.StrToDouble(fc["EndLong"]);

            model.QQ = fc["QQ"];
            model.EMail = fc["EMail"];

            model.StateID = TypeConverter.StrToInt(fc["StateID"]);
            model.UserType = fc["UserType"];

            model.UpdateMngID = LoginManger().ID;

            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.UsersDB.SaveEditUsers2(model);
            }
            else
            {
                aj.success = Data.UsersDB.AddUsers(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }

        [AdminIsLogin]
        public ActionResult UsersMain()
        {
            return View();
        }

        [AdminIsLogin]
        public ActionResult UsersPopup(String Names, String Phone)
        {
            var q = QueryBuilder.Create<Data.Users>();
            if (Names != "")
            {
                q = q.Like(x => x.Names, Names);
            }
            if (Phone != "")
            {
                q = q.Like(x => x.Phone, Phone);
            }

            var list = Data.UsersDB.UsersList(q);

            return View(list);
        }

        [AdminIsLogin]
        [HttpPost]
        public JsonResult UserSelect(FormCollection fc)
        {
            var model = new Data.Users();

            var Names = fc["Names"];
            var Phone = fc["Phone"];

            var q = QueryBuilder.Create<Data.Users>();
            if (Names != "")
            {
                q = q.Like(x => x.Names, Names);
            }
            if (Phone != "")
            {
                q = q.Like(x => x.Phone, Phone);
            }

            var list = Data.UsersDB.UsersList(q);

            AjaxJson aj = new AjaxJson();
            if (list == null || list.Count == 0)
            {
                aj.message = "NoBody";
            }
            else if (list.Count == 1)
            {
                aj.message = list[0].ID.ToString();
            }
            else
            {
                aj.message = "MutiBody";
            }
            return Json(new { message = aj.message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 用户线路

        
        [AdminIsLogin]
        public ActionResult LineUserMng()
        {
            return View();
        }

        [AdminIsLogin]
        public ActionResult LineUserList(int UserID = 0 )
        {
            var q = QueryBuilder.Create<Data.LineUser>();
            q = q.Equals(x => x.UserID, UserID);
            q = q.Equals(x => x.DelFlag, "N");

            var list = Data.LineUserDB.LineUserList(q);
            return View(list);
        }

        [AdminIsLogin]
        public ActionResult LineUserList2(string RT = "", int lineID = 0)
        {
            var q = QueryBuilder.Create<Data.LineUserView>();

            q.Equals(x => x.LineID, lineID);

            if (RT != "")
            {
                var rtAry = RT.Split(',');
                q.In(x => x.RideType, rtAry);
            }
            else 
            {
                q.Equals(x => x.RideType, "ALL");
            }

            var list = Data.LineUserDB.LineUserViewList(q);
            return View(list);
        }

        [AdminIsLogin]
        public ActionResult LineCntList1(String LineName = "")
        {
            var q = QueryBuilder.Create<Data.LineCntView>();

            if (LineName != "")
            {
                q = q.Like(x => x.LineName, LineName);
            }
            var list = Data.BusLineDB.LineCntList(q);
            return View(list);
        }

        [AdminIsLogin]
        public ActionResult LineCntList2(String LineName = "", String StartAddress = "", String EndAddress = "")
        {
            var q = QueryBuilder.Create<Data.LineCntView>();

            if (LineName != "")
            {
                q = q.Like(x => x.LineName, LineName);
            }

            if (StartAddress != "")
            {
                q = q.Like(x => x.StartAddress, StartAddress);
            }

            if (EndAddress != "")
            {
                q = q.Like(x => x.EndAddress, EndAddress);
            }

            var list = Data.BusLineDB.LineCntList(q);
            return View(list);
        }

        [AdminIsLogin]
        [HttpPost]
        public JsonResult AddMultiLineUser(FormCollection fc)
        {
            //参数
            var LineId = fc["LineId"];
            var RideType = fc["RideType"];
            var UserIDs = fc["UserIDs"];

            //组合参数
            var modelList = new List< Data.LineUser>();
            
            //多个成员
            var userIdAry = UserIDs.Split(',');
            foreach (var userId in userIdAry){
                var model = new Data.LineUser();

                model.LineID = TypeConverter.StrToInt(LineId);
                model.RideType = RideType;
                model.CreateTime = DateTime.Now;
                model.DelFlag = "N";

                model.UserID = TypeConverter.StrToInt(userId);
                modelList.Add(model);
            }

            var cnt = Data.LineUserDB.AddMultiLineUser(modelList);

            AjaxJson aj = new AjaxJson();
            if (cnt > 0)
            {
                aj.success = true;
            }
            else
            {
                aj.success = false;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 用户状态
        [AdminIsLogin]
        public ActionResult UserStateList(int page = 1)
        {
            var q = QueryBuilder.Create<Data.UserState>();
            var list = Data.UserStateDB.List(q, page, 15);
            return View(list);
        }
        [AdminIsLogin]
        public ActionResult UserState(int ID = 0)
        {
            if (ID > 0)
            {
                var model = Data.UserStateDB.GETUserState(ID);
                return View(model);
            }
            return View();
        }
        [AdminIsLogin]
        [HttpPost]
        public ActionResult UserState(FormCollection fc)
        {
            var model = new Data.UserState();
            model.ID = iRequest.GetQueryInt("ID");
            model.StateName = fc["StateName"];
            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.UserStateDB.SaveEditUserState(model);
            }
            else
            {
                aj.success = Data.UserStateDB.AddUserState(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region News
        [AdminIsLogin]
        public ActionResult NewsList(int page = 1)
        {
            var q = QueryBuilder.Create<Data.News>();
            var list = Data.NewsDB.List(q, page, 15);
            return View(list);
        }
        [AdminIsLogin]
        public ActionResult News(int ID = 0)
        {
            if (ID > 0)
            {
                var model = Data.NewsDB.GETNews(ID);
                return View(model);
            }
            return View();
        }
        [AdminIsLogin]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult News(FormCollection fc)
        {
            var model = new Data.News();
            model.ID = iRequest.GetQueryInt("ID");
            model.Title = fc["Title"];

            model.Thumb = fc["Thumb"];

            model.SubContent = fc["SubContent"];

            model.Contents = fc["Contents"];

            model.CreateTime = TypeConverter.StrToDateTime(fc["CreateTime"]);
            model.ClassID = TypeConverter.StrToInt(fc["ClassID"]);
            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.NewsDB.SaveEditNews(model);
            }
            else
            {
                aj.success = Data.NewsDB.AddNews(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 订单管理
        [AdminIsLogin]
        public ActionResult OrdersList(int page = 1)
        {
            var q = QueryBuilder.Create<Data.Order>();
            var list = Data.OrderDB.List(q,page, 15);
            return View(list);
        }
        public static string GetLineName(int LineID)
        {
            var model = Data.BusLineDB.GETBusLine(LineID);
            return model == null ? "" : model.LineName;
        }
        [AdminIsLogin]
        public ActionResult Orders(int id = 0)
        {
            var model = Data.OrderDB.GETOrder(id);
            return View(model);
        }
        #endregion
        public ActionResult soMap()
        {
            return View();
        }
        #region 删除操作
        public ActionResult DelData(string act = "", int dataid = 0)
        {
            AjaxJson aj = new AjaxJson();
            var flag = false;
            if (act == "delbusline")
            {
                flag = Data.BusLineDB.DeleteBusLine(dataid);
            }
            else if (act == "delmanager")
            {
                flag = Data.ManagerDB.DeleteManager(dataid);
            }
            else if (act == "delstation")
            {
                flag = Data.StationDB.DeleteStation(dataid);
            }
            else if (act == "dellineuser")
            {
                flag = Data.LineUserDB.DeleteLineUser(dataid);
            }
            else if (act == "delpayment")
            {
                flag = Data.PayMentDB.DeletePayMent(dataid);
            }
            else if (act == "delLineUser")
            {
                flag = Data.LineUserDB.DeleteLineUser(dataid);
            }
            else if (act == "deladdress")
            {
                var q = QueryBuilder.Create<Data.Address>();

                q.Equals(x => x.ID, dataid);

                flag = Data.AddressDB.DeleteAddress(q);

                if (!flag)
                {
                    return Json(new { success = flag }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    q = QueryBuilder.Create<Data.Address>();

                    q.Equals(x => x.ParentID, dataid);

                    flag = Data.AddressDB.DeleteAddressByParentID(q);
                }
            }
            else if (act == "deletePay")
            {
                flag = Data.PayDB.DeletePay(dataid);
            }
            else if (act == "deletePayLmng")
            {
                flag = Data.PayLmngDB.DeletePayLmng(dataid);
            }
            //dellineuser
            return Json(new { success = flag }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DelDataNew(string act = "", int dataid = 0, string dataName = "")
        {
            AjaxJson aj = new AjaxJson();
            var flag = false;
            if  (act == "delDriver")
            {
                flag = Data.DriverDB.DeleteDriver(dataid);
            }
            else if (act == "delBus")
            {
                var q1 = QueryBuilder.Create<Data.BusLine>();
                var q2 = QueryBuilder.Create<Data.BusLine>();
                if (dataid > 0)
                {
                    q1 = q1.Equals(x => x.StartBusID, dataid);
                    q2 = q2.Equals(x => x.EndBusID, dataid);
                }
                var list1 = Data.BusLineDB.List(q1);
                var list2 = Data.BusLineDB.List(q2);

                if (list1.Count > 0)
                {
                    string flagTemp;
                    flagTemp = "删除车辆【" + dataName + "】前，请先给线路【" + list1[0].LineName + "】指定其他车辆";

                    return Json(new { success = flagTemp }, JsonRequestBehavior.AllowGet);

                }
                else if (list2.Count > 0)
                {
                    string flagTemp;
                    flagTemp = "删除车辆【" + dataName + "】前，请先给线路【" + list2[0].LineName + "】指定其他车辆";

                    return Json(new { success = flagTemp }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    flag = Data.BusDB.DeleteBus(dataid);
                }
            }
            else if (act == "delbusline")
            {
                var q = QueryBuilder.Create<Data.LineUser>();
                if (dataid > 0)
                {
                    q = q.Equals(x => x.LineID, dataid);
                }
                var list1 = Data.LineUserDB.List(q);

                if (list1.Count > 0)
                {
                    string flagTemp;
                    flagTemp = "删除线路【" + dataName + "】前，请先将该线路中所有会员移出该线路";
                    return Json(new { success = flagTemp }, JsonRequestBehavior.AllowGet);
                }
                else {
                    flag = Data.BusLineDB.DeleteBusLine(dataid);
                }
            }
            //dellineuser
            return Json(new { success = flag }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Manager
        [AdminIsLogin]
        public ActionResult ManagerList(int page = 1)
        {
            var q = QueryBuilder.Create<Data.Manager>();
            var list = Data.ManagerDB.List(q, page, 15);
            return View(list);
        }
        [AdminIsLogin]
        public ActionResult Manager(int ID = 0)
        {
            if (ID > 0)
            {
                var model = Data.ManagerDB.GETManager(ID);
                return View(model);
            }
            return View();
        }
        [AdminIsLogin]
        [HttpPost]
        public ActionResult Manager(FormCollection fc)
        {
            var model = new Data.Manager();
            model.ID = iRequest.GetQueryInt("ID");
            model.UserName = fc["UserName"];
            if (fc["Password"] == "")
            {
                model.Password = fc["hidPassword"];
            }
            else
            {
                string pwd=fc["Password"];
                pwd = Encrypt.DES.Des_Encrypt(pwd);
                model.Password = pwd;
            }
            model.RealName = fc["RealName"];
            model.ManagerType = fc["ManagerType"];
            model.CreateTime = DateTime.Now;
            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.ManagerDB.SaveEditManager(model);
            }
            else
            {
                aj.success = Data.ManagerDB.AddManager(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            string uname = fc["username"];
            string pwd = fc["password"];
            if (uname == "" || pwd == "")
            {
                return Content("<script>alert('用户名或者密码不能为空.');window.location.href='/Admin/Login';</script>");
            }
            else
            {
                pwd = Encrypt.DES.Des_Encrypt(pwd);
                var q = QueryBuilder.Create<Data.Manager>().Equals(x => x.UserName, uname).Equals(x => x.Password, pwd);
                var model = Data.ManagerDB.GETManager(q);
                if (model == null)
                {
                    return Content("<script>alert('用户名或者密码错误.');window.location.href='/Admin/Login';</script>");
                }
                else
                {
                    Cookie.WriteCookie("AdminHash", Encrypt.DES.Des_Encrypt(model.ID.ToString()));
                    Cookie.WriteCookie("AdminName", HttpUtility.UrlEncode(model.RealName));
                    Cookie.WriteCookie("ManagerType", model.ManagerType);

                    return Content("<script>window.location.href='/Admin/';</script>");

                }
            }
        }

        //退出
        public ActionResult SignOut()
        {
            Cookie.DelCookie("AdminHash");
            Cookie.DelCookie("AdminId");
            Cookie.DelCookie("AdminName");
            return Content("<script>window.location.href='/Admin/';</script>");
        }

        //登录的用户信息
        public static Data.Manager LoginManger()
        {
            var manager = new Data.Manager();

            if (Cookie.GetCookie("AdminHash") != null)
            {
                var managerID = Cookie.GetCookie("AdminHash").ToString();
                if (managerID != "")
                {
                    manager.ID = TypeConverter.StrToInt(Encrypt.DES.Des_Decrypt(managerID));
                    manager.RealName = HttpUtility.UrlDecode(Cookie.GetCookie("AdminName").ToString());
                    manager.ManagerType = Cookie.GetCookie("ManagerType").ToString();
                }
            }
            return manager;
        }
        #endregion

        #region 周围用户
        public ActionResult ViewLineUser(int LineID = 0)
        {
            var model = Data.BusLineDB.GETBusLine(LineID);
            var list = Common.GetUserList(model.StartLong, model.StartLat);
            var list1 = new List<Data.Users>(); //已预定
            var list2 = new List<Data.Users>();         //未预定
            var list3 = new List<Data.Users>();
            //检测是否已经预定
            foreach (var item in list)
            {
                var q = QueryBuilder.Create<Data.Order>().Equals(x => x.UserID, item.ID).Equals(x => x.LineID, LineID);
                var _list = Data.OrderDB.OrderList(q);
                if (_list.Count > 0)
                {
                    list1.Add(item);
                }
                else
                {
                    list2.Add(item);
                }
            }
            foreach (var item in list1)
            {
                list3.Add(item);
            }
            foreach (var item in list2)
            {
                list3.Add(item);
            }
            return View(list3);
        }
        #endregion
        
        #region 添加用户
        [AdminIsLogin]
        public ActionResult AddUser(int ID=0)
        {
            if (ID > 0)
            {
                var model = Data.UsersDB.GETUsers(ID);
                return View(model);
            }
            return View();
        }

        [AdminIsLogin]
        [HttpPost]
        public ActionResult AddUser(FormCollection fc)
        {
            var flag = false;
            if (iRequest.GetQueryInt("ID") == 0){
                var P = fc["Phone"];
                var q = QueryBuilder.Create<Data.Users>().Equals(x => x.Phone, P);
                var qlist = Data.UsersDB.UsersList(q);
                if (qlist.Count > 0)
                {
                    return Json(new { success = false, message = "已经存在相同电话号码，不能继续添加。" }, JsonRequestBehavior.AllowGet);
                }
            }
            try
            {
                var model = new Data.Users();
                model.ID = iRequest.GetQueryInt("ID");
                model.CompanyName = fc["CompanyName"];
                model.CreateTime = DateTime.Now;
                model.EMail = fc["EMail"];
                model.Names = fc["Names"];
                if (model.ID > 0)
                {
                    model.Password = fc["hidpwd"];
                }
                else
                {
                    model.Password = Encrypt.DES.Des_Encrypt(fc["Passwprd"]);
                }
                model.Phone = fc["Phone"];
                model.QQ = fc["QQ"];
                model.Sex = TypeConverter.StrToInt(fc["Sex"]);
                model.isFinal = true;
                model.StateID = TypeConverter.StrToInt(fc["StateID"]);
                model.Address = fc["Address"];
                model.Number = GetNumber();
                string _starttime = DateTime.Now.ToString("yyyy-MM-dd");
                _starttime = _starttime + " " + fc["StartTime1"] + ":" + fc["StartTime2"] + ":00";
                model.StartTime = TypeConverter.StrToDateTime(_starttime);

                string _endtime = DateTime.Now.ToString("yyyy-MM-dd");
                _endtime = _endtime + " " + fc["EndTime1"] + ":" + fc["EndTime2"] + ":00";
                model.EndTime = TypeConverter.StrToDateTime(_endtime);
                model.StartLong = TypeConverter.StrToDouble(fc["StartLong"], 0);
                model.StartLat = TypeConverter.StrToDouble(fc["StartLat"], 0);
                model.WXUserID = 0;
                model.EndAddress = fc["EndAddress"];
                if (model.ID > 0)
                {
                    flag = Data.UsersDB._SaveEditUsers(model);
                }
                else
                {
                    flag = Data.UsersDB.AddUsers(model) > 0;
                }
            }
            catch (Exception ee)
            {

                return Json(new { success = false, message = ee.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = flag }, JsonRequestBehavior.AllowGet);
            //return View();
        }

        public ActionResult Map3()
        {
            return View();
        }

        public static string GetNumber()
        {
            var dt = DateTime.Now;
            var q = QueryBuilder.Create<Data.Users>().Equals(x => x.CreateTime.Year, dt.Year).Equals(x => x.CreateTime.Month, dt.Month).Equals(x => x.CreateTime.Day, dt.Day);
            var list = Data.UsersDB.UsersList(q);
            int count = list.Count + 1;
            var strdt = dt.ToString("yyMMdd");
            var allcount = Data.UsersDB.UsersList().Count + 1;
            string result = "";
            if (allcount < 1000)
            {
                result = strdt + count.ToString().PadLeft(3, '0');
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
        #endregion

        #region 站点管理

        [AdminIsLogin]
        public ActionResult StationList(int page = 1,int LineID=0)
        {
            var q = QueryBuilder.Create<Data.Station>().Equals(x => x.LineID, LineID);
            var list = Data.StationDB.List(q, page, 20);
            return View(list);
        }

        [AdminIsLogin]
        public ActionResult Station(int LineID = 0, int ID = 0)
        {
            if (ID > 0)
            {
                var model = Data.StationDB.GETStation(ID);
                return View(model);
            }
            return View();
        }

        [AdminIsLogin]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Station(FormCollection fc)
        {
            var flag = false;
            var message = "";
            try
            {
                var model = new Data.Station();
                model.ID = iRequest.GetQueryInt("ID");
                model.Images = fc["Images"];
                model.LineID = iRequest.GetQueryInt("LineID");
                model.Names = fc["Names"];
                model.SortID = TypeConverter.StrToInt(fc["SortID"]);
                if (model.ID > 0)
                {
                    flag = Data.StationDB.SaveEditStation(model);
                }
                else
                {
                    flag = Data.StationDB.AddStation(model) > 0;
                }
            }
            catch (Exception ee)
            {
                flag = false;
                message = ee.Message;
            }
            return Json(new { success = flag, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 站点用户

        public static string GetUserLineNumber(int UserID)
        {
            var q = QueryBuilder.Create<Data.LineUser>().Equals(x => x.UserID, UserID);
            var list = Data.LineUserDB.LineUserList(q);
            if (list == null || list.Count == 0)
            {
                return "";
            }
            else
            {
                var lineID = 0;
                lineID = list[0].LineID;
                var model = Data.BusLineDB.GETBusLine(lineID);
                return model == null ? "路线不存在" : model.Number;
            }
        }
        public static bool isYD(int UserID)
        {
            var q = QueryBuilder.Create<Data.LineUser>().Equals(x => x.UserID, UserID);
            var model = Data.LineUserDB.GETLineUser(q);
            return model != null;
        }

        public ActionResult MoveTo(string ids = "", string LineID = "")
        {
            var flag = false;
            var _LineID = 0;
            var q = QueryBuilder.Create<Data.BusLine>().Equals(x => x.Number, LineID);
            var _m = Data.BusLineDB.GETBusLine(q);
            if (_m == null)
            {
                return Json(new { success = false,message="没有找到编号为"+LineID+"的线路。" }, JsonRequestBehavior.AllowGet); 
            }
            else
            {
                _LineID = _m.ID;
            }
            try
            {
                var index = 0;
                ids = ids.TrimEnd(',');
                string[] _ids = ids.Split(',');
                foreach (var id in _ids)
                {
                    var model = new Data.LineUser();
                    model.CreateTime = DateTime.Now;
                    model.LineID = _LineID;
                    model.UserID = TypeConverter.StrToInt(id);
                    var um = Data.UsersDB.GETUsers(model.UserID);
                    model.EndAddress = um.EndAddress;
                    model.EndTime = model.CreateTime.AddMonths(1);
                    model.Memo = "";
                    model.Names = um.Names;
                    model.Phone = um.Phone;
                    model.StartAddress = um.Address;
                    model.StateID = 1;
                    model.QQ = um.QQ;
                    model.EMail = um.EMail;

                    var buq = QueryBuilder.Create<Data.LineUser>()
    .Equals(x => x.LineID, model.LineID)
    .Equals(x => x.UserID, model.UserID);
                    var bum = Data.LineUserDB.GETLineUser(buq);
                    if (bum == null)
                    {
                        Data.LineUserDB.AddLineUser(model);
                    }
                    else
                    {
                        index++;

                    }
                }
                var message = "添加成功，其中有"+index+"个用户已存在该线路。";
                return Json(new { success = true,message=(index>0?message:"添加所选用户到线路成功.") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                flag = false;
            }
            return Json(new { success = flag }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BusLineUsersList(int page = 1, int LineID = 0, string Names = "", string Phone = "", string SAddress = "", string EAddress = "", int State=0)
        {
            var q = QueryBuilder.Create<Data.LineUser>().Equals(x => x.LineID, LineID);
            if (Names != "")
            {
                q = q.Like(x => x.Names, Names);
            }
            if (Phone != "")
            {
                q = q.Like(x => x.Phone, Phone);
            }
            if (SAddress != "")
            {
                q = q.Like(x => x.StartAddress, SAddress);
            }
            if (EAddress != "")
            {
                q = q.Like(x => x.EndAddress, EAddress);
            }
            if (State > 0) {
                q = q.Equals(x => x.StateID, State);
            }
            var list = Data.LineUserDB.List(q, page, 20);
            return View(list);
        }
        public static string GetBusUserPay(int LineID, int UserID)
        {
            var q = QueryBuilder.Create<Data.PayMent>().Equals(x => x.LineID, LineID).Equals(x => x.UsersID, UserID);
            var list = Data.PayMentDB.PayMentList(q);
            var result = "";
            if (list.Count > 0)
            {
                var tid = list[0].ZWQ;
                if (tid.HasValue)
                {
                    if (tid.Value == 1) { result = "早单程"; }
                    else if (tid.Value == 2) { result = "晚单程"; }
                    else if (tid.Value == 3) { result = "全程"; }
                }
            }
            return result;
        }
        public ActionResult ChangeLineUserState(int ID = 0, int StateID = 0)
        {
            var flag = false;
            flag = Data.LineUserDB.State(ID, StateID);
            return Json(new { success = flag }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 缴费
        [AdminIsLogin]
        public ActionResult Pay(int ID = 0)
        {
            var model = Data.PayViewDB.GETPayView(ID);
            return View(model);
        }

        [AdminIsLogin]
        [HttpPost]
        public ActionResult PayUpdate(FormCollection fc)
        {
            var model = new Data.Pay();
            model.ID = TypeConverter.StrToInt(fc["ID"]);

            model.UserID = TypeConverter.StrToInt(fc["UserID"]);
            model.LineUserID = TypeConverter.StrToInt(fc["LineUserID"]);
            model.StartDate = TypeConverter.StrToDateTime(fc["StartDate"] + " 12:00:00");
            model.EndDate = TypeConverter.StrToDateTime(fc["EndDate"] + " 12:00:00");
            model.PayTime = TypeConverter.StrToDateTime(fc["PayTime"] + " 12:00:00");
            model.PayMoney = TypeConverter.StrToDecimal(fc["PayMoney"]);
            model.PayType = fc["PayType"];
            model.MangerID = LoginManger().ID;
            model.UpdateTime = DateTime.Now;
            model.CreateTime = DateTime.Now;
            model.Etc = fc["Etc"];

            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.PayDB.SaveEditPay(model);
            }
            else
            {
                aj.success = Data.PayDB.AddPay(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }

        

        public ActionResult PayList(int UserID = 0)
        {
            var q = QueryBuilder.Create<Data.PayView>();
            q = q.Equals(x => x.UserID, UserID);
            q = q.Equals(x => x.DelFlag, "N");

            var list = Data.PayViewDB.PayViewList(q);
            return View(list);
        }

        public ActionResult PaymentList(int page = 1, int LineID = 0, int UserID = 0, int isUse = 0, int ZWQ = 0, string ctime = "", string etime = "",string t1="",string t2="")
        {
            var q = QueryBuilder.Create<Data.PayMent>();//.Equals(x => x.LineID, LineID);
            if (LineID > 0) {
                q = q.Equals(x => x.LineID, LineID);
            }
            if (UserID > 0) {
                q = q.Equals(x => x.UsersID, UserID);
            }
            if (isUse == 1)
            {
                q = q.Equals(x => x.isUse, true);
            }
            if (isUse == 2)
            {
                q = q.Equals(x => x.isUse, false);
            }
            if (ZWQ > 0) {
                q = q.Equals(x => x.ZWQ, ZWQ);
            }
            if (ctime != "")
            {
                var t = TypeConverter.StrToDateTime(ctime + " 00:00:00");
                var _t2 = DateTime.Now;
                if (etime != "")
                {
                    _t2=TypeConverter.StrToDateTime(etime + " 23:59:59");
                }
                q = q.Between(x => x.CreateTime, t, _t2);
            }
            if (t1 != "")
            {
                var _t1 = TypeConverter.StrToDateTime(t1 + " 00:00:00");
                q = q.Between(x => x.StartTime, _t1, DateTime.Now);
            }
            if (t2 != "")
            {
                var _t2 = TypeConverter.StrToDateTime(t2 + " 00:00:00");
                var __t2 = TypeConverter.StrToDateTime(t2 + " 23:59:59");
                q = q.Between(x => x.EndTime, __t2.AddYears(-1), __t2);
            }
            var list = Data.PayMentDB.List(q, page, 10);
            return View(list);
        }

        public static string GetLineNumber(int LineID)
        {
            var model = Data.BusLineDB.GETBusLine(LineID);
            return model == null ? "" : model.Number;
        }
        public static int GetLineIdByNumber(string Number)
        {
            var q = QueryBuilder.Create<Data.BusLine>().Equals(x => x.Number, Number);
            var model = Data.BusLineDB.GETBusLine(q);
            return model == null ? 0 : model.ID;
        }
        [AdminIsLogin]
        public ActionResult PayMent(int ID = 0)
        {
            if (ID > 0)
            {
                var model = Data.PayMentDB.GETPayMent(ID);
                return View(model);
            }
            return View();
        }
        [AdminIsLogin]
        [HttpPost]
        public ActionResult PayMent(FormCollection fc)
        {
            if (GetLineIdByNumber(fc["LineNumber"]) == 0)
            {
                return Json(new { success = false, message = "没有找到编号为" + fc["LineNumber"] + "的线路。" }, JsonRequestBehavior.AllowGet);
            }
            var model = new Data.PayMent();
            model.ID = iRequest.GetQueryInt("ID");
            model.HeXiao = fc["HeXiao"];

            model.HXTime = TypeConverter.StrToDateTime(fc["HXTime"]);
            model.Memo = fc["Memo"];

            model.addUsers = fc["addUsers"];

            model.CreateTime = TypeConverter.StrToDateTime(fc["CreateTime"]);
            model.LineID = GetLineIdByNumber(fc["LineNumber"]);//iRequest.GetQueryInt("LineID");

            model.UsersID = iRequest.GetQueryInt("UserID");

            model.PayNames = fc["PayNames"];

            model.StartTime = TypeConverter.StrToDateTime(fc["StartTime"]);
            model.EndTime = TypeConverter.StrToDateTime(fc["EndTime"]);
            model.Money = TypeConverter.StrToDecimal(fc["Money"], 0);
            model.isUse = TypeConverter.StrToBool(fc["isUse"].ToString(CultureInfo.InvariantCulture),false);
            model.SZ = TypeConverter.StrToInt(fc["SZ"]);
            model.ZWQ = TypeConverter.StrToInt(fc["ZWQ"]);
            model.PayType = fc["PayType"];
            var aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.PayMentDB.SaveEditPayMent(model);
            }
            else
            {
                //var _q=QueryBuilder.Create<Data.LineUser>()
                //.Equals(x => x.LineID, model.LineID)
                //.Equals(x => x.UserID, model.UsersID);
                //var _m = Data.LineUserDB.GETLineUser(_q);
                //if(_m==null){
                aj.success = Data.PayMentDB.AddPayMent(model) > 0;
                //}
                //else
                //{
                //    return Json(new { success = false,message="该线路已经存在此用户的预定,请检查.不要重复添加." }, JsonRequestBehavior.AllowGet);  
                //}
            }
            var buq = QueryBuilder.Create<Data.LineUser>()
                .Equals(x => x.LineID, model.LineID)
                .Equals(x => x.UserID, model.UsersID);
            var bum = Data.LineUserDB.GETLineUser(buq);
            if (bum == null)
            {
                var modelx = new Data.LineUser();
                modelx.CreateTime = DateTime.Now;
                modelx.LineID = model.LineID;
                modelx.UserID = model.UsersID;
                var um = Data.UsersDB.GETUsers(modelx.UserID);
                modelx.EndAddress = um.EndAddress;
                modelx.EndTime = model.CreateTime.AddMonths(1);
                modelx.Memo = "";
                modelx.Names = um.Names;
                modelx.Phone = um.Phone;
                modelx.StartAddress = um.Address;
                modelx.StateID = 1;
                modelx.QQ = um.QQ;
                modelx.EMail = um.EMail;
                Data.LineUserDB.AddLineUser(modelx);
            }


            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 车长收费
        [AdminIsLogin]
        public ActionResult PayLmngList(int UserID = 0)
        {
            var q = QueryBuilder.Create<Data.PayLmngView>();
            q = q.Equals(x => x.UserID, UserID);
            q = q.Equals(x => x.DelFlag, "N");
            var list = Data.PayLmngDB.PayLmngViewList(q);
            return View(list);
        }

        [AdminIsLogin]
        public ActionResult PayLmng(int ID = 0)
        {
            var model = Data.PayLmngViewDB.GETPayLmngView(ID);
            return View(model);
        }

        [AdminIsLogin]
        [HttpPost]
        public ActionResult PayLmngUpdate(FormCollection fc)
        {
            var model = new Data.PayLmng();
            model.ID = TypeConverter.StrToInt(fc["ID"]);
            model.UserID = TypeConverter.StrToInt(fc["UserID"]);

            model.LineID = TypeConverter.StrToInt(fc["LineID"]);
            model.PayTime = TypeConverter.StrToDateTime(fc["PayTime"] + " 12:00:00");

            model.PayMoneyYS = TypeConverter.StrToDecimal(fc["PayMoneyYS"]);
            model.PayMoneySS = TypeConverter.StrToDecimal(fc["PayMoneySS"]);
            model.PayMoneyDC = TypeConverter.StrToDecimal(fc["PayMoneyDC"]);
            model.Ect = fc["Ect"];

            model.MangerID = LoginManger().ID;

            AjaxJson aj = new AjaxJson();
            if (model.ID > 0)
            {
                aj.success = Data.PayLmngDB.SaveEditPayLmng(model);
            }
            else
            {
                aj.success = Data.PayLmngDB.AddPayLmng(model) > 0;
            }
            return Json(new { success = aj.success }, JsonRequestBehavior.AllowGet);
        }
        
        #endregion


        #region PhoneMsg

        public ActionResult PhoneMsg()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PhoneMsg(FormCollection fc)
        {

            try
            {
                string Phone = fc["Phone"];
                var list = Phone.TrimEnd(',').Split(',');
                string Content = fc["Contents"];
                int index = 0;
                foreach (var item in list)
                {
                    SMS.Send2(item, Content);
                    index++;
                    if (index == 4) {
                        index = 0;
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ee)
            {
                return Json(new { success = false,message=ee.Message }, JsonRequestBehavior.AllowGet);  
            }
        }

        #endregion
    }
}

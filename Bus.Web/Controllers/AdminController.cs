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
            var addressList = Data.AddressDB.AddressList();
            var counrtyQ = QueryBuilder.Create<Data.Address>().Equals(x => x.AddLevel, 1);
            var provinceQ = QueryBuilder.Create<Data.Address>().Equals(x => x.AddLevel, 2);
            var cityQ = QueryBuilder.Create<Data.Address>().Equals(x => x.AddLevel, 3);
            var countryList = Data.AddressDB.AddressList(counrtyQ);
            var provinceList = Data.AddressDB.AddressList(provinceQ);
            var cityList = Data.AddressDB.AddressList(cityQ);

            list.Add(addressList);
            list.Add(countryList);
            list.Add(provinceList);
            list.Add(cityList);

            return View(list);
        }
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

            model.ID = iRequest.GetQueryInt("ID");
            model.CreateTime = DateTime.Now;
            model.BusNo = fc["BusNo"];
            model.MotoType = fc["MotoType"];
            model.SeatCnt = TypeConverter.StrToInt(fc["SeatCnt"]);
            model.DriverID = TypeConverter.StrToInt(fc["DriverName"]);
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
            model.DriverID = TypeConverter.StrToInt(fc["DriverName"]);
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
        public ActionResult BusLineList(int page = 1,int TID=0)
        {
            var q = QueryBuilder.Create<Data.BusLineView>();
            if (TID > 0)
            {
                q = q.Equals(x => x.TypeID, TID);
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
            model.LineType = TypeConverter.StrToInt(fc["LineType"]);
            
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

            var Bus = QueryBuilder.Create<Data.Bus>();
            Bus = Bus.Equals(x => x.DelFlag, "N");

            var Buslist = Data.BusDB.List(Bus);
            ViewBag.Buslist = Buslist;

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
            model.LineType = TypeConverter.StrToInt(fc["LineType"]);

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
        [AdminIsLogin]
        public ActionResult PayReport(int page = 1, int TID = 0)
        {
            var q = QueryBuilder.Create<Data.PayView>();
            if (TID > 0)
            {
                q = q.Equals(x => x.ID, TID);
            }
            var list = Data.PayViewDB.List(q, page, 15);
            return View(list);
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
                    sheetData.SetCellValue(I + rowIndex, item.isFinal?item.StartTime.ToString("HH:mm"):"");
                    sheetData.SetCellValue(J + rowIndex, item.isFinal?item.EndTime.ToString("HH:mm"):"");
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

        #region Users
        public static string GetStatesName(int ID)
        {
            var model = Data.UserStateDB.GETUserState(ID);
            return model==null?"":model.StateName;
        }
        [AdminIsLogin]
        public ActionResult UsersList2(int StateID = 0, string StartLatLong = "")
        {
            var q = QueryBuilder.Create<Data.Users>();
            if (StateID > 0) {
                q = q.Equals(x => x.StateID, StateID);
            }
            var alllist = Data.UsersDB.UsersList(q);
            if (StartLatLong != ""&&StartLatLong.IndexOf(',')>0)
            {
                double lng = TypeConverter.StrToDouble(StartLatLong.Split(',').GetValue(0).ToString());
                double lat = TypeConverter.StrToDouble(StartLatLong.Split(',').GetValue(1).ToString());
                var list2 = new List<Data.Users>();
                foreach (var item in alllist)
                {
                    var juli = Common.GetShortDistance(item.StartLong, item.StartLat, lng, lat);
                    if (juli <= 1000)
                    {
                        if (!list2.Contains(item)) { list2.Add(item); }
                    }
                }
                return View(list2);
            }
            return View(alllist);
        }
        [AdminIsLogin]
        public ActionResult UsersList(int page = 1, string Names = "",string Phone="", 
            int StateID = -1, string StartLatLong = "", string EndLatLong = "",
            string QQ="",string A1="",string A2="",string t1="",string t2="",
            string corp="", string LN="",string RT="",string NoLine="")
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
                
            }

            var list = Data.UsersViewDB.List(q, page, 15);
            return View(list);

        }
        [AdminIsLogin]
        public ActionResult Users(int ID = 0)
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
        public ActionResult Users(FormCollection fc)
        {
            var model = new Data.Users();
            model.ID = iRequest.GetQueryInt("ID");
            model.StateID = TypeConverter.StrToInt(fc["StateID"]);
            //model.Sex = TypeConverter.StrToInt(fc["Sex"]);
            model.EndAddress = fc["EndAddress"];
            model.CompanyName = fc["CompanyName"];
            model.QQ = fc["QQ"];
            model.EMail = fc["EMail"];
            model.Names = fc["Names"];
            model.Phone = fc["Phone"];
            model.Address = fc["Address"];
            string _starttime = DateTime.Now.ToString("yyyy-MM-dd");
            _starttime = _starttime + " " + fc["StartTime1"] + ":" + fc["StartTime2"] + ":00";
            model.StartTime = TypeConverter.StrToDateTime(_starttime);

            string _endtime = DateTime.Now.ToString("yyyy-MM-dd");
            _endtime = _endtime + " " + fc["EndTime1"] + ":" + fc["EndTime2"] + ":00";
            model.EndTime = TypeConverter.StrToDateTime(_endtime);

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
        #endregion

        #region 用户线路
        [AdminIsLogin]
        public ActionResult LineUserList(int UserID )
        {
            var q = QueryBuilder.Create<Data.LineUser>();
            q = q.Equals(x => x.UserID, UserID);

            var list = Data.LineUserDB.LineUserList(q);
            return View(list);
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
            else if (act == "delineuser")
            {
                flag = Data.LineUserDB.DeleteLineUser(dataid);
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
                    return Content("<script>window.location.href='/Admin/';</script>");
                }
            }
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

        public ActionResult SignOut()
        {
            Cookie.DelCookie("AdminHash");
            return Content("<script>window.location.href='/Admin/';</script>");
        }
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

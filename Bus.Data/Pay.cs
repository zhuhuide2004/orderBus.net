using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class PayDB
    {
         
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddPay(Pay model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    model.LockFlag = "00";
                    model.DelFlag = "N";

                    entity.AddToPay(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static Pay GETPay(IQueryBuilder<Pay> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Pay.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static Pay GETPay(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.Pay.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeletePay(int ID)
        {
            //using (var entity = new BusEntities())
            //{
            //    var obj = entity.Pay.FirstOrDefault(x => x.ID == ID);
            //    entity.DeleteObject(obj);
            //    return entity.SaveChanges() > 0;
            //}


            using (var entity = new BusEntities())
            {
                var obj = entity.Pay.FirstOrDefault(x => x.ID == ID);
                if (obj != null)
                {
                    obj.DelFlag = "Y";
                    return entity.SaveChanges() > 0;
                }
                return false;
            }

        }


        public static bool DeletePay(IQueryBuilder<Pay> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Pay.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeletePay(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.Pay.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool LockPay(Pay model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Pay.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.ID = model.ID;

                    obj.LockFlag = model.LockFlag;

                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }

        public static bool SaveEditPay(Pay model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Pay.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.UserID = model.UserID;
                    obj.LineUserID = model.LineUserID;
                    obj.StartDate = model.StartDate;
                    obj.EndDate = model.EndDate;
                    obj.PayTime = model.PayTime;
                    obj.PayMoney = model.PayMoney;
                    obj.PayType = model.PayType;
                    obj.MangerID = model.MangerID;
                    obj.UpdateTime = model.UpdateTime;
                    obj.CreateTime = model.CreateTime;
                    obj.Etc = model.Etc;

                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<Pay> PayList(IQueryBuilder<Pay> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Pay.Where(iquery.Expression).OrderByDescending(x => x.UserID).ThenByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<Pay>();
            }
        }
        public static List<Pay> PayList(IQueryBuilder<Pay> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Pay.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<Pay> PayList()
        {
            using (var entity = new BusEntities())
            {
                return entity.Pay.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<Pay> List(IQueryBuilder<Pay> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Pay.Where(iquery.Expression).OrderByDescending(x => x.UserID).ThenByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
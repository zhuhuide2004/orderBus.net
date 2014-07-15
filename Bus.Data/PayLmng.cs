using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class PayLmngDB
    {
         
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddPayLmng(PayLmng model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    model.UpdateTime = DateTime.Now;
                    model.CreateTime = DateTime.Now;
                    model.DelFlag = "N";

                    entity.AddToPayLmng(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion

        #region Get
        public static PayLmng GETPayLmng(IQueryBuilder<PayLmng> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmng.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static PayLmng GETPayLmng(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmng.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion

        #region ViewGet
        public static PayLmngView GETPayLmngView(IQueryBuilder<PayLmngView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmngView.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static PayLmngView GETPayLmngView(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmngView.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion

        #region 删除
        public static bool DeletePayLmng(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.PayLmng.FirstOrDefault(x => x.ID == ID);
                if (obj != null)
                {
                    obj.DelFlag = "Y";
                    return entity.SaveChanges() > 0;
                }
                return false;
            }

        }


        public static bool DeletePayLmng(IQueryBuilder<PayLmng> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.PayLmng.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeletePayLmng(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.PayLmng.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion

        #region 保存修改
        public static bool SaveEditPayLmng(PayLmng model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.PayLmng.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.LineID = model.LineID;
                    obj.PayTime = model.PayTime;
                    obj.PayMoneyYS = model.PayMoneyYS;
                    obj.PayMoneySS = model.PayMoneySS;
                    obj.PayMoneyDC = model.PayMoneyDC;
                    obj.Ect = model.Ect;

                    obj.MangerID = model.MangerID;
                    obj.UpdateTime = DateTime.Now;

                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<PayLmng> PayLmngList(IQueryBuilder<PayLmng> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmng.Where(iquery.Expression).OrderByDescending(x => x.UserID).ThenByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<PayLmng>();
            }
        }
        public static List<PayLmng> PayLmngList(IQueryBuilder<PayLmng> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmng.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<PayLmng> PayLmngList()
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmng.OrderByDescending(x => x.ID).ToList();
            }
        }
        #endregion

        #region ViewList
        public static List<PayLmngView> PayLmngViewList(IQueryBuilder<PayLmngView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmngView.Where(iquery.Expression).OrderByDescending(x => x.PayTime).ThenByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<PayLmngView>();
            }
        }
        public static List<PayLmngView> PayLmngViewList(IQueryBuilder<PayLmngView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmngView.Where(iquery.Expression).OrderByDescending(x => x.PayTime).ToList();
            }
        }
        public static List<PayLmngView> PayLmngViewList()
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmngView.OrderByDescending(x => x.PayTime).ToList();
            }
        }
        #endregion

    }
}
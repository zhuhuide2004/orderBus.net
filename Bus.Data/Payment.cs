using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class PayMentDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddPayMent(PayMent model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToPayMent(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static PayMent GETPayMent(IQueryBuilder<PayMent> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMent.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static PayMent GETPayMent(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMent.FirstOrDefault(x => x.ID == ID);
            }
        }
        #endregion
        #region 删除
        public static bool DeletePayMent(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.PayMent.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeletePayMent(IQueryBuilder<PayMent> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.PayMent.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeletePayMent(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.PayMent.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditPayMent(PayMent model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.PayMent.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.LineID = model.LineID;
                    obj.UsersID = model.UsersID;
                    obj.PayNames = model.PayNames;
                    obj.StartTime = model.StartTime;
                    obj.EndTime = model.EndTime;
                    obj.Money = model.Money;
                    obj.isUse = model.isUse;
                    obj.PayType = model.PayType;
                    obj.HeXiao = model.HeXiao;
                    obj.HXTime = model.HXTime;
                    obj.Memo = model.Memo;
                    obj.addUsers = model.addUsers;
                    obj.ZWQ = model.ZWQ;
                    obj.SZ = model.SZ;
                    obj.CreateTime = model.CreateTime;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<PayMent> PayMentList(IQueryBuilder<PayMent> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMent.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<PayMent>();
            }
        }
        public static List<PayMent> PayMentList(IQueryBuilder<PayMent> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMent.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<PayMent> PayMentList()
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMent.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<PayMent> List(IQueryBuilder<PayMent> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMent.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class OrderDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddOrder(Order model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToOrder(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static Order GETOrder(IQueryBuilder<Order> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Order.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static Order GETOrder(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.Order.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteOrder(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Order.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteOrder(IQueryBuilder<Order> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Order.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteOrder(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.Order.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditOrder(Order model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Order.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    //obj.LineID = model.LineID;
                    obj.Phone = model.Phone;
                    obj.Names = model.Names;
                    obj.Months = model.Months;
                    //obj.Price = model.Price;
                    //obj.States = model.States;
                    //obj.Memo = model.Memo;
                    //obj.UserID = model.UserID;
                    obj.PayTypeID = model.PayTypeID;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<Order> OrderList(IQueryBuilder<Order> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Order.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<Order>();
            }
        }
        public static List<Order> OrderList(IQueryBuilder<Order> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Order.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<Order> OrderList()
        {
            using (var entity = new BusEntities())
            {
                return entity.Order.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<Order> List(IQueryBuilder<Order> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Order.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
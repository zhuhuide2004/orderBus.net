﻿using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class PayViewDB
    {
         
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddPayView(PayView model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToPayView(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static PayView GETPayView(IQueryBuilder<PayView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayView.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static PayView GETPayView(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayView.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeletePayView(int ID)
        {
            //using (var entity = new BusEntities())
            //{
            //    var obj = entity.PayView.FirstOrDefault(x => x.ID == ID);
            //    entity.DeleteObject(obj);
            //    return entity.SaveChanges() > 0;
            //}


            using (var entity = new BusEntities())
            {
                var obj = entity.PayView.FirstOrDefault(x => x.ID == ID);
                if (obj != null)
                {
                    obj.DelFlag = "Y";
                    return entity.SaveChanges() > 0;
                }
                return false;
            }

        }


        public static bool DeletePayView(IQueryBuilder<PayView> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.PayView.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeletePayView(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.PayView.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditPayView(PayView model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.PayView.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    //obj.BusNo = model.BusNo;
                    //obj.DriverName = model.DriverName;
                    //obj.Phone = model.Phone;
                    //obj.CreateTime = model.CreateTime;
                    //obj.MotoType = model.MotoType;
                    //obj.SeatCnt = model.SeatCnt;
                    //obj.Corp = model.Corp;
                    //obj.ID = model.ID;
                    //obj.InsuEndDate = model.InsuEndDate;
                    //obj.BuyDate = model.BuyDate;
                    //obj.OwnerName = model.OwnerName;
                    //obj.OwnerPhone = model.OwnerPhone;
                    //obj.Etc1 = model.Etc1;
                    //obj.Etc2 = model.Etc2;
                    //obj.Etc3 = model.Etc3;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<PayView> PayViewList(IQueryBuilder<PayView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayView.Where(iquery.Expression).OrderByDescending(x =>x.LineName).ThenByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<PayView>();
            }
        }
        public static List<PayView> PayViewList(IQueryBuilder<PayView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayView.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<PayView> PayViewList()
        {
            using (var entity = new BusEntities())
            {
                return entity.PayView.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<PayView> List(IQueryBuilder<PayView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayView.Where(iquery.Expression).OrderByDescending(x => x.LineName).ThenByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
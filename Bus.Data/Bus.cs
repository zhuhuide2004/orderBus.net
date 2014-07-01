using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class BusDB
    {
         
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddBus(Bus model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToBus(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static Bus GETBus(IQueryBuilder<Bus> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Bus.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static Bus GETBus(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.Bus.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteBus(int ID)
        {
            //using (var entity = new BusEntities())
            //{
            //    var obj = entity.Bus.FirstOrDefault(x => x.ID == ID);
            //    entity.DeleteObject(obj);
            //    return entity.SaveChanges() > 0;
            //}


            using (var entity = new BusEntities())
            {
                var obj = entity.Bus.FirstOrDefault(x => x.ID == ID);
                if (obj != null)
                {
                    obj.DelFlag = "Y";
                    return entity.SaveChanges() > 0;
                }
                return false;
            }

        }


        public static bool DeleteBus(IQueryBuilder<Bus> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Bus.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteBus(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.Bus.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditBus(Bus model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Bus.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.DriverID = model.DriverID;
                    obj.BusNo = model.BusNo;
                    obj.MotoType = model.MotoType;
                    obj.CreateTime = model.CreateTime;
                    obj.SeatCnt = model.SeatCnt;
                    obj.Corp = model.Corp;
                    obj.InsuEndDate = model.InsuEndDate;
                    obj.ID = model.ID;
                    obj.BuyDate = model.BuyDate;
                    obj.OwnerName = model.OwnerName;
                    obj.OwnerPhone = model.OwnerPhone;
                    obj.Etc1 = model.Etc1;
                    obj.Etc2 = model.Etc2;
                    obj.Etc3 = model.Etc3;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<Bus> BusList(IQueryBuilder<Bus> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Bus.Where(iquery.Expression).OrderByDescending(x =>x.BusNo).ThenByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<Bus>();
            }
        }
        public static List<Bus> BusList(IQueryBuilder<Bus> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Bus.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<Bus> BusList()
        {
            using (var entity = new BusEntities())
            {
                return entity.Bus.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<Bus> List(IQueryBuilder<Bus> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Bus.Where(iquery.Expression).OrderByDescending(x => x.BusNo).ThenByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
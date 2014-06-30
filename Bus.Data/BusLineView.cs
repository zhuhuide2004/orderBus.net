using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class BusLineViewDB
    {
         
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddBusLineView(BusLineView model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToBusLineView(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static BusLineView GETBusLineView(IQueryBuilder<BusLineView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.BusLineView.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static BusLineView GETBusLineView(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.BusLineView.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteBusLineView(int ID)
        {
            //using (var entity = new BusEntities())
            //{
            //    var obj = entity.BusLineView.FirstOrDefault(x => x.ID == ID);
            //    entity.DeleteObject(obj);
            //    return entity.SaveChanges() > 0;
            //}


            using (var entity = new BusEntities())
            {
                var obj = entity.BusLineView.FirstOrDefault(x => x.ID == ID);
                if (obj != null)
                {
                    obj.DelFlag = "Y";
                    return entity.SaveChanges() > 0;
                }
                return false;
            }

        }


        public static bool DeleteBusLineView(IQueryBuilder<BusLineView> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.BusLineView.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteBusLineView(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.BusLineView.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditBusLineView(BusLineView model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.BusLineView.FirstOrDefault(x => x.ID == model.ID);
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
        public static List<BusLineView> BusLineViewList(IQueryBuilder<BusLineView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.BusLineView.Where(iquery.Expression).OrderByDescending(x =>x.EndBusID).ThenByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<BusLineView>();
            }
        }
        public static List<BusLineView> BusLineViewList(IQueryBuilder<BusLineView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.BusLineView.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<BusLineView> BusLineViewList()
        {
            using (var entity = new BusEntities())
            {
                return entity.BusLineView.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<BusLineView> List(IQueryBuilder<BusLineView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.BusLineView.Where(iquery.Expression).OrderByDescending(x => x.EndBusID).ThenByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
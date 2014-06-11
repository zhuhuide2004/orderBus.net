using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class StationDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddStation(Station model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToStation(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static Station GETStation(IQueryBuilder<Station> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Station.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static Station GETStation(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.Station.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteStation(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Station.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteStation(IQueryBuilder<Station> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Station.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteStation(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.Station.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditStation(Station model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Station.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.Names = model.Names;
                    obj.LineID = model.LineID;
                    obj.Images = model.Images;
                    obj.SortID = model.SortID;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<Station> StationList(IQueryBuilder<Station> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Station.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<Station>();
            }
        }
        public static List<Station> StationList(IQueryBuilder<Station> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Station.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<Station> StationList2(IQueryBuilder<Station> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Station.Where(iquery.Expression).OrderBy(x => x.SortID).ToList();
            }
        }
        public static List<Station> StationList()
        {
            using (var entity = new BusEntities())
            {
                return entity.Station.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<Station> List(IQueryBuilder<Station> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Station.Where(iquery.Expression).OrderByDescending(x => x.SortID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class ManagerDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddManager(Manager model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    model.DelFlag = "N";
                    entity.AddToManager(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static Manager GETManager(IQueryBuilder<Manager> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Manager.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static Manager GETManager(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.Manager.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteManager(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Manager.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteManager(IQueryBuilder<Manager> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Manager.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteManager(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.Manager.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditManager(Manager model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Manager.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.UserName = model.UserName;
                    obj.Password = model.Password;
                    obj.ManagerType = model.ManagerType;
                    obj.RealName = model.RealName;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<Manager> ManagerList(IQueryBuilder<Manager> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Manager.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<Manager>();
            }
        }
        public static List<Manager> ManagerList(IQueryBuilder<Manager> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Manager.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<Manager> ManagerList()
        {
            using (var entity = new BusEntities())
            {
                return entity.Manager.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<Manager> List(IQueryBuilder<Manager> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Manager.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
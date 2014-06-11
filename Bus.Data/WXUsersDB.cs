using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class WXUsersDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddWXUsers(WXUsers model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToWXUsers(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static WXUsers GETWXUsers(IQueryBuilder<WXUsers> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.WXUsers.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static WXUsers GETWXUsers(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.WXUsers.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteWXUsers(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.WXUsers.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteWXUsers(IQueryBuilder<WXUsers> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.WXUsers.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteWXUsers(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.WXUsers.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditWXUsers(WXUsers model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.WXUsers.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.OpenID = model.OpenID;
                    obj.NickName = model.NickName;
                    obj.HeadImg = model.HeadImg;
                    obj.Sex = model.Sex;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<WXUsers> WXUsersList(IQueryBuilder<WXUsers> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.WXUsers.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<WXUsers>();
            }
        }
        public static List<WXUsers> WXUsersList(IQueryBuilder<WXUsers> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.WXUsers.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<WXUsers> WXUsersList()
        {
            using (var entity = new BusEntities())
            {
                return entity.WXUsers.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<WXUsers> List(IQueryBuilder<WXUsers> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.WXUsers.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
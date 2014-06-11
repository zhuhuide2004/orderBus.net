using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class UserStateDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddUserState(UserState model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToUserState(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static UserState GETUserState(IQueryBuilder<UserState> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.UserState.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static UserState GETUserState(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.UserState.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteUserState(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.UserState.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteUserState(IQueryBuilder<UserState> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.UserState.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteUserState(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.UserState.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditUserState(UserState model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.UserState.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.StateName = model.StateName;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<UserState> UserStateList(IQueryBuilder<UserState> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.UserState.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<UserState>();
            }
        }
        public static List<UserState> UserStateList(IQueryBuilder<UserState> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.UserState.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<UserState> UserStateList()
        {
            using (var entity = new BusEntities())
            {
                return entity.UserState.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<UserState> List(IQueryBuilder<UserState> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.UserState.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
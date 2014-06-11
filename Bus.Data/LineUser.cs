using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class LineUserDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddLineUser(LineUser model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToLineUser(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static LineUser GETLineUser(IQueryBuilder<LineUser> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.LineUser.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static LineUser GETLineUser(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.LineUser.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteLineUser(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.LineUser.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteLineUser(IQueryBuilder<LineUser> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.LineUser.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteLineUser(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.LineUser.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditLineUser(LineUser model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.LineUser.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.LineID = model.LineID;
                    obj.UserID = model.UserID;
                    obj.Names = model.Names;
                    obj.Phone = model.Phone;
                    obj.StartAddress = model.StartAddress;
                    obj.EndAddress = model.EndAddress;
                    obj.EndTime = model.EndTime;
                    obj.Memo = model.Memo;
                    obj.EMail = model.EMail;
                    obj.QQ = model.QQ;
                    obj.StateID = model.StateID;

                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        public static bool State(int ID,int StateID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.LineUser.FirstOrDefault(x => x.ID == ID);
                if (obj != null)
                {
                    obj.StateID = StateID;

                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<LineUser> LineUserList(IQueryBuilder<LineUser> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.LineUser.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<LineUser>();
            }
        }
        public static List<LineUser> LineUserList(IQueryBuilder<LineUser> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.LineUser.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<LineUser> LineUserList()
        {
            using (var entity = new BusEntities())
            {
                return entity.LineUser.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<LineUser> List(IQueryBuilder<LineUser> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.LineUser.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
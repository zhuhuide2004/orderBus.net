using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class SysCodeDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddSysCode(SysCode model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToSysCode(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static SysCode GETSysCode(IQueryBuilder<SysCode> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.SysCode.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static SysCode GETSysCode(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.SysCode.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteSysCode(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.SysCode.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteSysCode(IQueryBuilder<SysCode> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.SysCode.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteSysCode(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.SysCode.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditSysCode(SysCode model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.SysCode.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.Types = model.Types;
                    obj.Keys = model.Keys;
                    obj.Names = model.Names;
                    obj.SortNo = model.SortNo;

                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<SysCode> SysCodeList(IQueryBuilder<SysCode> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.SysCode.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<SysCode>();
            }
        }
        public static List<SysCode> SysCodeList(IQueryBuilder<SysCode> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.SysCode.Where(iquery.Expression).OrderBy(x => x.SortNo).ToList();
            }
        }
        public static List<SysCode> SysCodeList()
        {
            using (var entity = new BusEntities())
            {
                return entity.SysCode.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static List<SysCode> SysCodeList(string Types)
        {
            var q = QueryBuilder.Create<Data.SysCode>();
            if (Types != null)
            {
                q = q.Equals(x => x.Types, Types);
            }

            return SysCodeList(q);
        }

        public static PagedList<SysCode> List(IQueryBuilder<SysCode> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.SysCode.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class SMSCodeDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddSMSCode(SMSCode model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToSMSCode(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static SMSCode GETSMSCode(IQueryBuilder<SMSCode> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.SMSCode.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static SMSCode GETSMSCode(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.SMSCode.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteSMSCode(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.SMSCode.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteSMSCode(IQueryBuilder<SMSCode> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.SMSCode.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteSMSCode(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.SMSCode.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditSMSCode(SMSCode model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.SMSCode.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.Phone = model.Phone;
                    obj.Code = model.Code;
                    obj.EndTime = model.EndTime;
                    obj.IsUse = model.IsUse;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        public static bool SaveEditSMSCode_Use(string Phone)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.SMSCode.FirstOrDefault(x => x.Phone == Phone);
                if (obj != null)
                {
                    obj.IsUse = true;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<SMSCode> SMSCodeList(IQueryBuilder<SMSCode> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.SMSCode.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<SMSCode>();
            }
        }
        public static List<SMSCode> SMSCodeList(IQueryBuilder<SMSCode> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.SMSCode.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<SMSCode> SMSCodeList()
        {
            using (var entity = new BusEntities())
            {
                return entity.SMSCode.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<SMSCode> List(IQueryBuilder<SMSCode> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.SMSCode.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
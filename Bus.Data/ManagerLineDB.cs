using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class ManagerLineDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static void AddManagerLine(ManagerLine model)
        {
            using (var entity = new BusEntities())
            {
                try
                {
                    entity.AddToManagerLine(model);
                    entity.SaveChanges();
                }
                catch (Exception e) { }
            }
        }
        #endregion
        #region Get
        public static ManagerLine GetManagerLine(IQueryBuilder<ManagerLine> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.ManagerLine.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static ManagerLine GetManagerLine(int ManagerID, int LineID)
        {
            using (var entity = new BusEntities())
            {
                return entity.ManagerLine.Where(x => x.ManagerID == ManagerID && x.LineID == LineID).FirstOrDefault();
            }
        }
        #endregion
        #region Delete
        public static bool DeleteManagerLine(IQueryBuilder<ManagerLine> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.ManagerLine.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }
        public static bool DeleteManagerLine(int ManagerID, int LineID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.ManagerLine.FirstOrDefault(x => x.ManagerID == ManagerID && x.LineID == LineID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }
        #endregion
        #region List
        public static List<ManagerLine> ListManagerLine(IQueryBuilder<ManagerLine> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.ManagerLine.Where(iquery.Expression).OrderBy(x => x.UpdateTime).ToList();
            }
        }
        public static List<ManagerLine> ListManagerLine(int ManagerID)
        {
            using (var entity = new BusEntities())
            {
                return entity.ManagerLine.Where(x => x.ManagerID == ManagerID).OrderBy(x => x.UpdateTime).ToList();
            }
        }
        #endregion
    }
}
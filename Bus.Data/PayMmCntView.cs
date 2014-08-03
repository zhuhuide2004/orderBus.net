using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class PayMmCntViewDB
    {
        public BusEntities entity = new BusEntities();

        #region Get
        public static PayMmCntView GETPayMmCntView(IQueryBuilder<PayMmCntView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMmCntView.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static PayMmCntView GETPayMmCntView(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMmCntView.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion

        #region List
        public static List<PayMmCntView> PayMmCntViewList(IQueryBuilder<PayMmCntView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMmCntView.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<PayMmCntView>();
            }
        }

        public static List<PayMmCntView> PayMmCntViewList(IQueryBuilder<PayMmCntView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMmCntView.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }

        public static List<PayMmCntView> PayMmCntViewList()
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMmCntView.OrderByDescending(x => x.ID).ToList();
            }
        }
        public static PagedList<PayMmCntView> List(IQueryBuilder<PayMmCntView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMmCntView.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
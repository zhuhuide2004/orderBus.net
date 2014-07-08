using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class PayViewDB
    {
        public BusEntities entity = new BusEntities();

        #region Get
        public static PayView GETPayView(IQueryBuilder<PayView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayView.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static PayView GETPayView(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayView.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion

        #region List
        public static List<PayView> PayViewList(IQueryBuilder<PayView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayView.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<PayView>();
            }
        }

        public static List<PayView> PayViewList(IQueryBuilder<PayView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayView.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }

        public static List<PayView> PayViewList()
        {
            using (var entity = new BusEntities())
            {
                return entity.PayView.OrderByDescending(x => x.ID).ToList();
            }
        }
        public static PagedList<PayView> List(IQueryBuilder<PayView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayView.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
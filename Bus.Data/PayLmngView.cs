using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class PayLmngViewDB
    {
        public BusEntities entity = new BusEntities();

        #region Get
        public static PayLmngView GETPayLmngView(IQueryBuilder<PayLmngView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmngView.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static PayLmngView GETPayLmngView(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmngView.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion

        #region List
        public static List<PayLmngView> PayLmngViewList(IQueryBuilder<PayLmngView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmngView.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<PayLmngView>();
            }
        }

        public static List<PayLmngView> PayLmngViewList(IQueryBuilder<PayLmngView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmngView.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }

        public static List<PayLmngView> PayLmngViewList()
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmngView.OrderByDescending(x => x.ID).ToList();
            }
        }
        public static PagedList<PayLmngView> List(IQueryBuilder<PayLmngView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayLmngView.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
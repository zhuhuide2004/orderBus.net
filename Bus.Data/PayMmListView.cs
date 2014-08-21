using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class PayMmListViewDB
    {
        public BusEntities entity = new BusEntities();

        #region Get
        public static PayMmListView GETPayMmListView(IQueryBuilder<PayMmListView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMmListView.Where(iquery.Expression).FirstOrDefault();
            }
        }
        #endregion

        #region List
        public static List<PayMmListView> PayMmListViewList(IQueryBuilder<PayMmListView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMmListView.Where(iquery.Expression).OrderByDescending(x => x.Names).ToList();
            }
        }

        public static List<PayMmListView> PayMmListViewList()
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMmListView.OrderByDescending(x => x.Names).ToList();
            }
        }
        public static PagedList<PayMmListView> List(IQueryBuilder<PayMmListView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.PayMmListView.Where(iquery.Expression).OrderByDescending(x => x.Names).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
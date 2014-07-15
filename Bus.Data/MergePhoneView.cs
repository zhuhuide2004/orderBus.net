using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class MergePhoneViewDB
    {
        public BusEntities entity = new BusEntities();

        #region Get
        public static MergePhoneView GETMergePhoneView(IQueryBuilder<MergePhoneView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.MergePhoneView.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static MergePhoneView GETMergePhoneView(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.MergePhoneView.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion

        #region List
        public static List<MergePhoneView> MergePhoneViewList(IQueryBuilder<MergePhoneView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.MergePhoneView.Where(iquery.Expression).OrderByDescending(x => x.Phone).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<MergePhoneView>();
            }
        }

        public static List<MergePhoneView> MergePhoneViewList(IQueryBuilder<MergePhoneView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.MergePhoneView.Where(iquery.Expression).OrderByDescending(x => x.Phone).ToList();
            }
        }

        public static List<MergePhoneView> MergePhoneViewList()
        {
            using (var entity = new BusEntities())
            {
                return entity.MergePhoneView.OrderByDescending(x => x.Phone).ToList();
            }
        }
        public static PagedList<MergePhoneView> List(IQueryBuilder<MergePhoneView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.MergePhoneView.Where(iquery.Expression).OrderByDescending(x => x.Phone).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
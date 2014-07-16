using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class MergeNameViewDB
    {
        public BusEntities entity = new BusEntities();

        #region Get
        public static MergeNameView GETMergeNameView(IQueryBuilder<MergeNameView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.MergeNameView.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static MergeNameView GETMergeNameView(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.MergeNameView.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion

        #region List
        public static List<MergeNameView> MergeNameViewList(IQueryBuilder<MergeNameView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.MergeNameView.Where(iquery.Expression).OrderByDescending(x => x.Names).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<MergeNameView>();
            }
        }

        public static List<MergeNameView> MergeNameViewList(IQueryBuilder<MergeNameView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.MergeNameView.Where(iquery.Expression).OrderByDescending(x => x.Names).ToList();
            }
        }

        public static List<MergeNameView> MergeNameViewList()
        {
            using (var entity = new BusEntities())
            {
                return entity.MergeNameView.OrderByDescending(x => x.Names).ToList();
            }
        }
        public static PagedList<MergeNameView> List(IQueryBuilder<MergeNameView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.MergeNameView.Where(iquery.Expression).OrderByDescending(x => x.userCnt).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class UsersViewDB
    {
        public BusEntities entity = new BusEntities();

        #region Get
        public static UsersView GETUsersView(IQueryBuilder<UsersView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.UsersView.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static UsersView GETUsersView(int ID) 
        {
            using (var entity = new BusEntities())
            {
                return entity.UsersView.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion

        #region List
        public static List<UsersView> UsersViewList(IQueryBuilder<UsersView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.UsersView.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<UsersView>();
            }
        }
        public static List<UsersView> UsersViewList(IQueryBuilder<UsersView> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.UsersView.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<UsersView> UsersViewList()
        {
            using (var entity = new BusEntities())
            {
                return entity.UsersView.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<UsersView> List(IQueryBuilder<UsersView> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.UsersView.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
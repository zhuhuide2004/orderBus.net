using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class NewsDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddNews(News model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToNews(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static News GETNews(IQueryBuilder<News> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.News.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static News GETNews(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.News.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteNews(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.News.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteNews(IQueryBuilder<News> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.News.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteNews(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.News.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditNews(News model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.News.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.Title = model.Title;
                    obj.Thumb = model.Thumb;
                    obj.SubContent = model.SubContent;
                    obj.Contents = model.Contents;
                    obj.ClassID = model.ClassID;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<News> NewsList(IQueryBuilder<News> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.News.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<News>();
            }
        }
        public static List<News> NewsList(IQueryBuilder<News> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.News.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<News> NewsList()
        {
            using (var entity = new BusEntities())
            {
                return entity.News.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<News> List(IQueryBuilder<News> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.News.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class MyContentDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddMyContent(MyContent model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToMyContent(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static MyContent GETMyContent(IQueryBuilder<MyContent> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.MyContent.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static MyContent GETMyContent(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.MyContent.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        public static MyContent GETMyContent(string action)
        {
            using (var entity = new BusEntities())
            {
                return entity.MyContent.Where(x => x.action == action).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteMyContent(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.MyContent.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteMyContent(IQueryBuilder<MyContent> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.MyContent.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteMyContent(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.MyContent.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditMyContent(MyContent model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.MyContent.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.action = model.action;
                    obj.Title = model.Title;
                    obj.Thumb = model.Thumb;
                    obj.SubContent = model.SubContent;
                    obj.LinkUrl = model.LinkUrl;
                    obj.Contents = model.Contents;
                    
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<MyContent> MyContentList(IQueryBuilder<MyContent> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.MyContent.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<MyContent>();
            }
        }
        public static List<MyContent> MyContentList(IQueryBuilder<MyContent> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.MyContent.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<MyContent> MyContentList()
        {
            using (var entity = new BusEntities())
            {
                return entity.MyContent.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<MyContent> List(IQueryBuilder<MyContent> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.MyContent.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
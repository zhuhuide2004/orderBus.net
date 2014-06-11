using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class QuestionItemDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddQuestionItem(QuestionItem model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToQuestionItem(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static QuestionItem GETQuestionItem(IQueryBuilder<QuestionItem> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.QuestionItem.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static QuestionItem GETQuestionItem(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.QuestionItem.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteQuestionItem(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.QuestionItem.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteQuestionItem(IQueryBuilder<QuestionItem> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.QuestionItem.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteQuestionItem(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.QuestionItem.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditQuestionItem(QuestionItem model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.QuestionItem.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.ItemName = model.ItemName;
                    obj.QuestionID = model.QuestionID;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<QuestionItem> QuestionItemList(IQueryBuilder<QuestionItem> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.QuestionItem.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<QuestionItem>();
            }
        }
        public static List<QuestionItem> QuestionItemList(IQueryBuilder<QuestionItem> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.QuestionItem.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<QuestionItem> QuestionItemList()
        {
            using (var entity = new BusEntities())
            {
                return entity.QuestionItem.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<QuestionItem> List(IQueryBuilder<QuestionItem> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.QuestionItem.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
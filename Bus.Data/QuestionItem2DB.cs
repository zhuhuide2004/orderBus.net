using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class QuestionItem2DB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddQuestionItem2(QuestionItem2 model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToQuestionItem2(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static QuestionItem2 GETQuestionItem2(IQueryBuilder<QuestionItem2> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.QuestionItem2.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static QuestionItem2 GETQuestionItem2(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.QuestionItem2.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteQuestionItem2(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.QuestionItem2.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteQuestionItem2(IQueryBuilder<QuestionItem2> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.QuestionItem2.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteQuestionItem2(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.QuestionItem2.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditQuestionItem2(QuestionItem2 model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.QuestionItem2.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.QuestionItemID = model.QuestionItemID;
                    obj.Value = model.Value;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<QuestionItem2> QuestionItem2List(IQueryBuilder<QuestionItem2> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.QuestionItem2.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<QuestionItem2>();
            }
        }
        public static List<QuestionItem2> QuestionItem2List(IQueryBuilder<QuestionItem2> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.QuestionItem2.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<QuestionItem2> QuestionItem2List()
        {
            using (var entity = new BusEntities())
            {
                return entity.QuestionItem2.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<QuestionItem2> List(IQueryBuilder<QuestionItem2> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.QuestionItem2.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
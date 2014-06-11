using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class QuestionDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddQuestion(Question model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToQuestion(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static Question GETQuestion(IQueryBuilder<Question> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Question.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static Question GETQuestion(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.Question.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteQuestion(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Question.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteQuestion(IQueryBuilder<Question> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Question.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteQuestion(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.Question.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditQuestion(Question model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Question.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.Title = model.Title;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<Question> QuestionList(IQueryBuilder<Question> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Question.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<Question>();
            }
        }
        public static List<Question> QuestionList(IQueryBuilder<Question> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Question.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<Question> QuestionList()
        {
            using (var entity = new BusEntities())
            {
                return entity.Question.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<Question> List(IQueryBuilder<Question> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Question.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
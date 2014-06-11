using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class UserQuestionDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddUserQuestion(UserQuestion model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToUserQuestion(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static UserQuestion GETUserQuestion(IQueryBuilder<UserQuestion> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.UserQuestion.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static UserQuestion GETUserQuestion(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.UserQuestion.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteUserQuestion(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.UserQuestion.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteUserQuestion(IQueryBuilder<UserQuestion> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.UserQuestion.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteUserQuestion(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.UserQuestion.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditUserQuestion(UserQuestion model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.UserQuestion.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.UserID = model.UserID;
                    obj.QuestionItem2ID = model.QuestionItem2ID;
                    obj.QuestionItemID = model.QuestionItemID;
                    obj.QuestionID = model.QuestionID;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<UserQuestion> UserQuestionList(IQueryBuilder<UserQuestion> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.UserQuestion.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<UserQuestion>();
            }
        }
        public static List<UserQuestion> UserQuestionList(IQueryBuilder<UserQuestion> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.UserQuestion.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<UserQuestion> UserQuestionList()
        {
            using (var entity = new BusEntities())
            {
                return entity.UserQuestion.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<UserQuestion> List(IQueryBuilder<UserQuestion> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.UserQuestion.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
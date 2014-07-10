using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class UsersDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddUsers(Users model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    model.CreateTime = DateTime.Now;
                    model.UpdateTime = DateTime.Now;
                    model.Number = DateTime.Now.ToString("yyyyMMdd");
                    model.DelFlag = "N";
                    entity.AddToUsers(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch (Exception e)
                {

                }
                return id;
            }
        }
        #endregion
        #region Get
        public static Users GETUsers(IQueryBuilder<Users> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Users.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static Users GETUsers(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.Users.Where(x => x.ID == ID).FirstOrDefault();
            }
        }

        //用户类型 车长 or 普通乘员
        public static string GetUserType(int ID)
        {
            var model = GETUsers(ID);
            return model == null ? "" : model.UserType;
        }
        #endregion
        #region 删除
        public static bool DeleteUsers(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Users.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteUsers(IQueryBuilder<Users> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Users.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteUsers(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.Users.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditUsersMyInfo(Users model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Users.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                   
                    obj.Names = model.Names;
                    obj.Phone = model.Phone;
                    obj.Sex = model.Sex;
                    obj.EMail = model.EMail;
                    obj.QQ = model.QQ;
                    obj.CompanyName = model.CompanyName;
                    obj.StartTime = model.StartTime;
                    obj.EndTime = model.EndTime;
                    obj.isFinal = true;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        public static bool SaveEditUsers(Users model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Users.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.Address = model.Address;
                    obj.StartTime = model.StartTime;
                    obj.EndTime = model.EndTime;
                    obj.StartLong = model.StartLong;
                    obj.StartLat = model.StartLat;
                    obj.EndLong = model.EndLong;
                    obj.EndLat = model.EndLat;
                    obj.isFinal = model.isFinal;
                    obj.EndAddress = model.EndAddress;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        public static bool _SaveEditUsers(Users model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Users.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.StateID = model.StateID;
                    obj.Names = model.Names;
                    obj.Phone = model.Phone;
                    obj.Address = model.Address;
                    obj.StartTime = model.StartTime;
                    obj.EndTime = model.EndTime;
                    obj.EMail = model.EMail;
                    obj.QQ = model.QQ;
                    obj.CompanyName = model.CompanyName;
                    obj.EndAddress = model.EndAddress;
                    obj.Sex = model.Sex;
                    obj.Password = model.Password;
                    obj.StartLat = model.StartLat;
                    obj.StartLong = model.StartLong;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        public static bool ChangePWD(int UID, string PWD)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Users.FirstOrDefault(x => x.ID == UID);
                if (obj != null)
                {
                    obj.Password = PWD;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        public static bool SaveEditUsers_NoMap(Users model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Users.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.Address = model.Address;
                    obj.StartTime = model.StartTime;
                    obj.EndTime = model.EndTime;
                    //obj.StartLong = model.StartLong;
                    //obj.StartLat = model.StartLat;
                    //obj.EndLong = model.EndLong;
                    //obj.EndLat = model.EndLat;
                    //obj.isFinal = model.isFinal;
                    obj.EndAddress = model.EndAddress;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        public static bool SaveEditUsers_Map2(Users model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Users.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    //obj.Address = model.Address;
                    //obj.StartTime = model.StartTime;
                    //obj.EndTime = model.EndTime;
                    //obj.StartLong = model.StartLong;
                    //obj.StartLat = model.StartLat;
                    obj.EndLong = model.EndLong;
                    obj.EndLat = model.EndLat;
                    //obj.isFinal = model.isFinal;
                    obj.EndAddress = model.EndAddress;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        public static bool SetUserLocal(Users model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Users.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.Address = model.Address;
                    obj.StartLong = model.StartLong;
                    obj.StartLat = model.StartLat;
                    obj.EndLong = model.EndLong;
                    obj.EndLat = model.EndLat;
                    obj.EndAddress = model.EndAddress;
                    var result = entity.SaveChanges();
                    return result > 0;
                }
                return false;
            }
        }
        public static bool SaveEditUsers_Map(Users model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Users.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.Address = model.Address;
                    //obj.StartTime = model.StartTime;
                    //obj.EndTime = model.EndTime;
                    obj.StartLong = model.StartLong;
                    obj.StartLat = model.StartLat;
                    //obj.EndLong = model.EndLong;
                    //obj.EndLat = model.EndLat;
                    //obj.isFinal = model.isFinal;
                    //obj.EndAddress = model.EndAddress;
                    var result = entity.SaveChanges();
                    return result > 0;
                }
                return false;
            }
        }
        public static bool SaveEditUsers2(Users model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Users.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    
                    obj.Names = model.Names;
                    obj.Phone = model.Phone;
                    obj.Sex = model.Sex;
                    obj.Password = model.Password;

                    obj.StartTime = model.StartTime;
                    obj.EndTime = model.EndTime;

                    obj.CompanyName = model.CompanyName;
                    obj.AddressSel = model.AddressSel;
                    obj.Address = model.Address;
                    obj.EndAddressSel = model.EndAddressSel;
                    obj.EndAddress = model.EndAddress;

                    obj.StartLat = model.StartLat;
                    obj.StartLong = model.StartLong;
                    obj.EndLat = model.EndLat;
                    obj.EndLong = model.EndLong;

                    obj.EMail = model.EMail;
                    obj.QQ = model.QQ;

                    obj.StateID = model.StateID;
                    obj.UserType = model.UserType == null ? "USER" : model.UserType;

                    obj.UpdateTime = DateTime.Now;
                    obj.UpdateMngID = model.UpdateMngID;

                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<Users> UsersList(IQueryBuilder<Users> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Users.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<Users>();
            }
        }
        public static List<Users> UsersList(IQueryBuilder<Users> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Users.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<Users> UsersList()
        {
            using (var entity = new BusEntities())
            {
                return entity.Users.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<Users> List(IQueryBuilder<Users> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Users.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class DriverDB
    {
         
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddDriver(Driver model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToDriver(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static Driver GETDriver(IQueryBuilder<Driver> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Driver.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static Driver GETDriver(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.Driver.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteDriver(int ID)
        {
            //using (var entity = new BusEntities())
            //{
            //    var obj = entity.Driver.FirstOrDefault(x => x.ID == ID);
            //    entity.DeleteObject(obj);
            //    return entity.SaveChanges() > 0;
            //}


            using (var entity = new BusEntities())
            {
                var obj = entity.Driver.FirstOrDefault(x => x.ID == ID);
                if (obj != null)
                {
                    obj.DelFlag = "Y";
                    return entity.SaveChanges() > 0;
                }
                return false;
            }

        }


        public static bool DeleteDriver(IQueryBuilder<Driver> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Driver.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteDriver(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.Driver.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditDriver(Driver model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Driver.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.Address = model.Address;
                    obj.BirthDay = model.BirthDay;
                    obj.BreakRules = model.BreakRules;
                    obj.CreateTime = model.CreateTime;
                    obj.DriverName = model.DriverName;
                    obj.DriveStartTime = model.DriveStartTime;
                    obj.Etc = model.Etc;
                    //obj.ID = model.ID;
                    obj.IdCard = model.IdCard;
                    obj.Phone = model.Phone;
                    obj.Sex = model.Sex;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<Driver> DriverList(IQueryBuilder<Driver> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Driver.Where(iquery.Expression).OrderByDescending(x =>x.DriverName).ThenByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<Driver>();
            }
        }
        public static List<Driver> DriverList(IQueryBuilder<Driver> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Driver.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<Driver> DriverList()
        {
            using (var entity = new BusEntities())
            {
                return entity.Driver.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static List<Driver> List(IQueryBuilder<Driver> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Driver.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<Driver> List(IQueryBuilder<Driver> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Driver.Where(iquery.Expression).OrderByDescending(x => x.DriverName).ThenByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        public static PagedList<Driver> List(IQueryBuilder<Driver> iquery, int Page = 1, int PageSize = 10, string sortCol = "0", string sortType = "0" )
        {
            using (var entity = new BusEntities())
            {
                if (sortType == "0")//升序
                {
                    if (sortCol == "0")
                    {
                        return entity.Driver.Where(iquery.Expression).OrderBy(x => x.ID).ToPagedList(Page, PageSize);
                    }
                    else if (sortCol == "1")
                    {
                        return entity.Driver.Where(iquery.Expression).OrderBy(x => x.DriverName).ToPagedList(Page, PageSize);
                    }
                    else if (sortCol == "2")
                    {
                        return entity.Driver.Where(iquery.Expression).OrderBy(x => x.Sex).ToPagedList(Page, PageSize);
                    }
                    else if (sortCol == "3")
                    {
                        return entity.Driver.Where(iquery.Expression).OrderBy(x => x.Phone).ToPagedList(Page, PageSize);
                    }
                    else if (sortCol == "4")
                    {
                        return entity.Driver.Where(iquery.Expression).OrderBy(x => x.IdCard).ToPagedList(Page, PageSize);
                    }
                    else if (sortCol == "5")
                    {
                        return entity.Driver.Where(iquery.Expression).OrderBy(x => x.BirthDay).ToPagedList(Page, PageSize);
                    }
                    else
                    {
                        return entity.Driver.Where(iquery.Expression).OrderBy(x => x.ID).ToPagedList(Page, PageSize);
                    }
                }
                else if (sortType == "1")//降序
                {
                    if (sortCol == "0")
                    {
                        return entity.Driver.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
                    }
                    else if (sortCol == "1")
                    {
                        return entity.Driver.Where(iquery.Expression).OrderByDescending(x => x.DriverName).ToPagedList(Page, PageSize);
                    }
                    else if (sortCol == "2")
                    {
                        return entity.Driver.Where(iquery.Expression).OrderByDescending(x => x.Sex).ToPagedList(Page, PageSize);
                    }
                    else if (sortCol == "3")
                    {
                        return entity.Driver.Where(iquery.Expression).OrderByDescending(x => x.Phone).ToPagedList(Page, PageSize);
                    }
                    else if (sortCol == "4")
                    {
                        return entity.Driver.Where(iquery.Expression).OrderByDescending(x => x.IdCard).ToPagedList(Page, PageSize);
                    }
                    else if (sortCol == "5")
                    {
                        return entity.Driver.Where(iquery.Expression).OrderByDescending(x => x.BirthDay).ToPagedList(Page, PageSize);
                    }
                    else
                    {
                        return entity.Driver.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
                    }

                }
                return entity.Driver.Where(iquery.Expression).OrderBy(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
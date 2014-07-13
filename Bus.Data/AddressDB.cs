using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class AddressDB
    {
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddAddress(Address model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToAddress(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch (Exception e) { }
                return id;
            }
        }
        #endregion
        #region Get
        public static Address GETAddress(IQueryBuilder<Address> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Address.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static Address GETAddress(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.Address.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteAddress(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Address.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteAddress(IQueryBuilder<Address> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Address.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteAddress(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.Address.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteAddressByParentID(IQueryBuilder<Address> iquery)
        {
            using (var entity = new BusEntities())
            {
                List<Address> addressList = entity.Address.Where(iquery.Expression).ToList();

                if (addressList.Count != 0)
                {
                    foreach (var address in addressList)
                    {
                        entity.DeleteObject(address);
                    }

                    return entity.SaveChanges() > 0;
                }
                else
                {
                    return true;
                }
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditAddress(Address model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Address.FirstOrDefault(x => x.ID == model.ID);

                if (obj != null)
                {
                    obj.AddName = model.AddName;
                    obj.SortID = model.SortID;

                    return entity.SaveChanges() > 0;
                }

                return false;
            }
        }
        #endregion

        #region 保存顺序
        public static bool SaveAddressSort(Address model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.Address.FirstOrDefault(x => x.ID == model.ID);

                if (obj != null)
                {
                    obj.SortID = model.SortID;

                    return entity.SaveChanges() > 0;
                }

                return false;
            }
        }
        #endregion

        #region List
        public static List<Address> AddressList(IQueryBuilder<Address> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Address.Where(iquery.Expression).OrderByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<Address>();
            }
        }
        public static List<Address> AddressList(IQueryBuilder<Address> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.Address.Where(iquery.Expression).OrderBy(x => x.SortID).ToList();
            }
        }
        public static List<Address> AddressList()
        {
            using (var entity = new BusEntities())
            {
                return entity.Address.OrderBy(x => x.ID).ToList();
            }
        }

        public static PagedList<Address> List(IQueryBuilder<Address> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.Address.Where(iquery.Expression).OrderByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
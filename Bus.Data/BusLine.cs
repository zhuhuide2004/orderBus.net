﻿using TheCMS.Linq;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace Bus.Data
{
    public class BusLineDB
    {
         
        public BusEntities entity = new BusEntities();
        #region Add
        public static int AddBusLine(BusLine model)
        {
            using (var entity = new BusEntities())
            {
                var id = 0;
                try
                {
                    entity.AddToBusLine(model);
                    entity.SaveChanges();
                    id = model.ID;
                }
                catch { }
                return id;
            }
        }
        #endregion
        #region Get
        public static BusLine GETBusLine(IQueryBuilder<BusLine> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.BusLine.Where(iquery.Expression).FirstOrDefault();
            }
        }
        public static BusLine GETBusLine(int ID)
        {
            using (var entity = new BusEntities())
            {
                return entity.BusLine.Where(x => x.ID == ID).FirstOrDefault();
            }
        }
        #endregion
        #region 删除
        public static bool DeleteBusLine(int ID)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.BusLine.FirstOrDefault(x => x.ID == ID);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }


        public static bool DeleteBusLine(IQueryBuilder<BusLine> iquery)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.BusLine.FirstOrDefault(iquery.Expression);
                entity.DeleteObject(obj);
                return entity.SaveChanges() > 0;
            }
        }

        public static bool DeleteBusLine(int[] ids)
        {
            using (var entity = new BusEntities())
            {
                foreach (var id in ids)
                {
                    var obj = entity.BusLine.FirstOrDefault(x => x.ID == id);
                    entity.DeleteObject(obj);
                }
                return entity.SaveChanges() > 0;
            }
        }

        #endregion
        #region 保存修改
        public static bool SaveEditBusLine(BusLine model)
        {
            using (var entity = new BusEntities())
            {
                var obj = entity.BusLine.FirstOrDefault(x => x.ID == model.ID);
                if (obj != null)
                {
                    obj.LineName = model.LineName;
                    obj.StartAddress = model.StartAddress;
                    obj.StartTime = model.StartTime;
                    obj.StartLong = model.StartLong;
                    obj.StartLat = model.StartLat;
                    obj.EndAddress = model.EndAddress;
                    obj.EndTime = model.EndTime;
                    obj.EndLong = model.EndLong;
                    obj.EndLat = model.EndLat;
                    obj.Price = model.Price;
                    obj.MinNum = model.MinNum;
                    obj.TypeID = model.TypeID;
                    obj.Number = model.Number;
                    obj.LineType = model.LineType;
                    obj.CheX = model.CheX;
                    obj.CheZ = model.CheZ;
                    obj.Price2 = model.Price2;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }
        #endregion

        #region List
        public static List<BusLine> BusLineList(IQueryBuilder<BusLine> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.BusLine.Where(iquery.Expression).OrderByDescending(x =>x.Number).ThenByDescending(x => x.ID).Skip(((Page == 0 ? 1 : Page) - 1) * PageSize).Take(PageSize).ToList<BusLine>();
            }
        }
        public static List<BusLine> BusLineList(IQueryBuilder<BusLine> iquery)
        {
            using (var entity = new BusEntities())
            {
                return entity.BusLine.Where(iquery.Expression).OrderByDescending(x => x.ID).ToList();
            }
        }
        public static List<BusLine> BusLineList()
        {
            using (var entity = new BusEntities())
            {
                return entity.BusLine.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static PagedList<BusLine> List(IQueryBuilder<BusLine> iquery, int Page = 1, int PageSize = 10)
        {
            using (var entity = new BusEntities())
            {
                return entity.BusLine.Where(iquery.Expression).OrderByDescending(x => x.Number).ThenByDescending(x => x.ID).ToPagedList(Page, PageSize);
            }
        }

        #endregion

    }
}
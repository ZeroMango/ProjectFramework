﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.MapperConfig;
using Framework.Helper;
using Framework.Dapper;
using Framework.DI;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Configuration.Configure();

            //OrderGoodDTO ogd = new OrderGoodDTO
            //{
            //    bIsDeleted = false,
            //    iAmount = 2,
            //    ID = Guid.NewGuid().ToString(),
            //    dTotalPrice = 180,
            //    iGoodsType = 1,
            //    iReturnAmount = 2,
            //    iSinglePrice = 90,
            //    sClientID = "1",
            //    sColorValue = "绿色",
            //    sGoodsName = "胸罩",
            //    sGoodsNo = "00010",
            //    sGoodsPrimaryKey = "1",
            //    sGoodStandID = "b75",
            //    sOrderID = "1",
            //    sOrderNo = "001",
            //    sSizeName = "85"
            //};

            //// Order o = new Order { orderGoods=new List<OrderGood>(new List<Order>)}

            //OrderDTO order = new OrderDTO
            //{
            //    bIsClientDelete = false,
            //    bIsDeleted = false,
            //    dBookTime = DateTime.Now,
            //    dFinishTime = DateTime.Now,
            //    dPayTime = DateTime.Now,
            //    dRoyaltyOutput = 80,
            //    dSendTime = DateTime.Now,
            //    iBaggingPrice = 15,
            //    iClientType = 1
            //};

            //Order or = MapperHelpper.Map<Order>(order);

            //OrderGood o = MapperHelpper.Map<OrderGood>(ogd);

            //or.orderGoods = new List<OrderGood>();
            //or.orderGoods.Add(o);

            //GuidHelper.CreateRpcrt4Guid();

            //QueryHelper qb = new DapperDBHelper();
            //var list = qb.QueryList<Object>("select * from CICUser where [Username] = @name", new { name = "yangyukun" });

            var list = Framework.web.config.WebConfig.ParseDictionary("unity");


            QueryHelper helper = DIEntity.GetInstance().GetImpl<QueryHelper>();

            ////Dictionary<string,object>

            //var ret = helper.PaginationQuery<EHECD_Orders>("select ID,iTotalPrice from EHECD_Orders where iTotalPrice > @condi", new PageInfo
            //{
            //    OrderBy = "iTotalPrice",
            //    orderType = OrderType.DESC,
            //    PageIndex = 1,
            //    PageSize = 10
            //}, new { condi = 100 });


            var ret2 = helper.QueryList<Dictionary<string, object>>("select ID,iTotalPrice from EHECD_Orders where iTotalPrice > @condi", new { condi = 100 });

            var ret3 = helper.SingleQuery<Dictionary<string, object>>("select ID,iTotalPrice from EHECD_Orders where iTotalPrice > @condi", new { condi = 100 });
        }
    }
}
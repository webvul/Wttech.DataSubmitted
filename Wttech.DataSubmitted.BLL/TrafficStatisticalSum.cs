/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/18 13:11:46
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.DAL;

#endregion

namespace Wttech.DataSubmitted.BLL
{
    class TrafficStatisticalSum
    {
        #region 1 Const
        #endregion

        #region 2 Static Fields
        #endregion

        #region 3 Fields
        #endregion

        #region 4 Properties
        #endregion

        #region 5 Constructors
        #endregion

        #region 6 Delegate
        #endregion

        #region 7 Define Event
        #endregion

        #region 8 Event Methods
        #endregion

        #region 9 Public Methods
        /// <summary>
        /// 创建或修改15，16合计
        /// </summary>
        /// <param name="para"></param>
        /// <param name="stationtype">收费站类型</param>
        public void CreateOrUpdateSum(QueryParameters para, int stationtype)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                #region 合计
                //查询日期当天除合计外的全部数据
                IEnumerable<RP_NatSta> naturalall = db.RP_NatSta.Where(s => s.CalcuTime == para.StartTime && s.StaType == stationtype && s.HourPer != 24);

                IEnumerable<RP_NatSta> naturallistsum = db.RP_NatSta.Where(s => s.CalcuTime == para.StartTime && s.HourPer == 24 && s.StaType == stationtype);
                RP_NatSta natural = new RP_NatSta();
                //如果有数据则进行合计
                if (naturalall.Count() > 0)
                {
                    if (naturallistsum.Count() > 0)
                    {
                        natural = naturallistsum.First();
                    }

                    natural.Sum = naturalall.Sum(s => s.Sum);
                    natural.ExNum = naturalall.Sum(s => s.ExNum);
                    natural.EnNum = naturalall.Sum(s => s.EnNum);
                    natural.State = "1";
                    natural.UpdDate = DateTime.Now;
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        if (naturallistsum.Count() <= 0)
                        {
                            natural.RunStae = "正常";
                            natural.HourPer = 24;//24代表合计
                            natural.Id = Guid.NewGuid();
                            natural.StaType = stationtype;
                            natural.CalcuTime = (DateTime)para.StartTime;
                            natural.CrtDate = DateTime.Now;
                            db.RP_NatSta.Add(natural);
                        }
                        db.SaveChanges();
                        //提交事务
                        transaction.Complete();
                    }
                #endregion
                }
            }
        #endregion

            #region 10 Static Methods
            #endregion

            #region 11 Private Methods
            #endregion

            #region 12 Protected Methods
            #endregion
        }
    }
}

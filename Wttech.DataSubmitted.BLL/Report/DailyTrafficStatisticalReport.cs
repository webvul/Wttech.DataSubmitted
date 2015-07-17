/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/7 11:15:03
 */
#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.DAL;
using System.Transactions;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.Resources;
using Wttech.DataSubmitted.IBLL.IReport;
using Wttech.DataSubmitted.BLL.Tool;
using System.Threading;
#endregion
namespace Wttech.DataSubmitted.BLL.Report
{
    /// <summary>
    /// 日报表统计类
    /// </summary>
    public class DailyTrafficStatisticalReport : IObserver
    {
        public string ReportName { get; set; }

        public DailyTrafficStatisticalReport(string name)
        {
            this.ReportName = name;
        }
        /// <summary>
        ///更新每日报送数据15.16.17
        /// </summary>
        public void Update(DateTime dt, int HourPer)
        {
            string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SystemLog.GetInstance().Log.Info(string.Format("{0}：开始获取15,16,17报表数据", startTime));
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                DateTime pdt = DateTime.Parse(dt.ToShortDateString());
                //北京段相同数据是否存在
                List<RP_NatSta> pBJlist = db.RP_NatSta.Where(p =>
                    p.CalcuTime == pdt
                    && p.HourPer == (byte?)HourPer
                    && p.StaType == (int)StationConfiguration.StationType.BeiJingDuan).ToList();
                bool BJHasTime = pBJlist.Count > 0;
                //大羊坊相同数据是否存在
                List<RP_NatSta> pDYFlist = db.RP_NatSta.Where(p =>
                    p.CalcuTime == pdt
                    && p.HourPer == (byte?)HourPer
                    && p.StaType == (int)StationConfiguration.StationID.DYF).ToList();
                bool DYFHasTime = pDYFlist.Count > 0;
                IList<RP_NatSta> pNaturalEntities = new List<RP_NatSta>();

                List<int> BJStation = StationConfiguration.GetBJStaion();
                List<int> outBJStation = StationConfiguration.GetOutBJStaion();
                //p.VehType != 0 表示不包括合计数据，
                IEnumerable<DS_DataSource> pStagingCollection = db.DS_DataSource.Where(p =>
                    p.CalcuTime == dt
                    && p.VehType != 0
                    && p.HourPer == HourPer
                    && BJStation.Contains(p.StaID.Value));


                RP_NatSta pNaturalTrafficBJ = null;//北京段
                RP_NatSta pNaturalTraffic = null;//大羊坊
                //北京段
                //如果相同日期，相同时间段，相同统计站类型数据已存在则进行更新
                if (!BJHasTime)
                {
                    pNaturalTrafficBJ = new RP_NatSta();//北京段
                    pNaturalTrafficBJ.Id = Guid.NewGuid();
                }
                else
                {
                    pNaturalTrafficBJ = pBJlist[0];
                }
                pNaturalTrafficBJ.EnNum = pStagingCollection.Sum(s => s.OutNum);
                pNaturalTrafficBJ.ExNum = pStagingCollection.Where(s => outBJStation.Contains(s.StaID.Value)).Sum(s => s.InNum);
                pNaturalTrafficBJ.Sum = pNaturalTrafficBJ.EnNum + pNaturalTrafficBJ.ExNum;
                pNaturalTrafficBJ.StaType = (int)StationConfiguration.StationType.BeiJingDuan;
                pNaturalTrafficBJ.CalcuTime = DateTime.Parse(dt.ToShortDateString());
                pNaturalTrafficBJ.HourPer = (byte)HourPer;
                pNaturalTrafficBJ.CrtDate = DateTime.Now;
                pNaturalTrafficBJ.RunStae = SystemConst.RunningStatus;
                pNaturalTrafficBJ.State = "0";
                //大羊坊
                //如果相同日期，相同时间段，相同统计站类型数据已存在则进行更新
                if (!DYFHasTime)
                {
                    pNaturalTraffic = new RP_NatSta();//大羊坊
                    pNaturalTraffic.Id = Guid.NewGuid();
                }
                else
                {
                    pNaturalTraffic = pDYFlist[0];
                }
                pNaturalTraffic.EnNum = pStagingCollection.Where(s => s.StaID == (int)StationConfiguration.StationID.DYF).Sum(s => s.OutNum);
                pNaturalTraffic.ExNum = pStagingCollection.Where(s => s.StaID == (int)StationConfiguration.StationID.DYF).Sum(s => s.InNum);
                pNaturalTraffic.Sum = pNaturalTraffic.EnNum + pNaturalTraffic.ExNum;
                pNaturalTraffic.StaType = (int)StationConfiguration.StationID.DYF;
                pNaturalTraffic.CrtDate = DateTime.Now;
                pNaturalTraffic.HourPer = (byte)HourPer;
                pNaturalTraffic.CalcuTime = DateTime.Parse(dt.ToShortDateString());
                pNaturalTraffic.RunStae = SystemConst.RunningStatus;
                pNaturalTraffic.State = "0";

                pNaturalEntities.Add(pNaturalTrafficBJ);
                pNaturalEntities.Add(pNaturalTraffic);
                using (TransactionScope transaction = new TransactionScope())
                {
                    //如果都不存在，则添加
                    if (!DYFHasTime && !BJHasTime)
                        db.RP_NatSta.AddRange(pNaturalEntities);
                    else if (!DYFHasTime && BJHasTime)
                        db.RP_NatSta.Add(pNaturalTraffic);
                    else if (DYFHasTime && !BJHasTime)
                        db.RP_NatSta.Add(pNaturalTrafficBJ);

                    db.SaveChanges();
                    //提交事务
                    transaction.Complete();
                }
                string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：结束获取15.16.17报表数据", endTime));
                SystemLog.GetInstance().Log.Info(string.Format("统计15,16,17报表数据耗时{0}秒", (DateTime.Parse(endTime) - DateTime.Parse(startTime)).TotalSeconds));
                //显示执行该方法的线程ID
                //SystemLog.GetInstance().Log.Info(string.Format("调用15,16,17Update的线程ID为：{0}", Thread.CurrentThread.ManagedThreadId));
                Thread.Sleep(1000);
            }
        }
    }
}

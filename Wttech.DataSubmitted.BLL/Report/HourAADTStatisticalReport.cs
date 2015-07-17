/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/7 15:43:12
 */
#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Wttech.DataSubmitted.BLL.Tool;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL.IReport;

#endregion

namespace Wttech.DataSubmitted.BLL.Report
{
    /// <summary>
    /// 报表13,14源数据获取类
    /// </summary>
    public class HourAADTStatisticalReport : IObserver
    {

        #region 4 Properties
        public string ReportName { get; set; }
        #endregion

        #region 5 Constructors
        public HourAADTStatisticalReport(string name)
        {
            this.ReportName = name;
        }
        #endregion

        #region 9 Public Methods
        public void Update(DateTime dt, int HourPer)
        {
            try
            {
                string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：开始获取13,14报表数据", startTime));
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    DateTime pDt = DateTime.Parse(dt.ToShortDateString());
                    //报表13,14收费站
                    List<int> BJStation = StationConfiguration.GetBJStaion();
                    //获取今年当日数据//p.VehType == 0 表示合计数据，
                    List<DS_DataSource> pCollection = db.DS_DataSource.Where(p => p.CalcuTime == dt && p.VehType == 0 && p.HourPer == HourPer && BJStation.Contains(p.StaID.Value)).ToList();

                    bool pIsHas = false;
                    //北京段数据
                    RP_HourAADT pHoursTraffic = null;
                    List<RP_HourAADT> pBJList = db.RP_HourAADT.Where(p => p.CalcuTime == pDt && p.HourPer == (byte)HourPer).ToList();
                    if (pBJList.Count > 0)
                    {
                        pHoursTraffic = pBJList.FirstOrDefault();
                        //每种状态需要重新赋值，防止公用同一个变量，值不明确
                        pIsHas = true;
                    }
                    else
                    {
                        pHoursTraffic = new RP_HourAADT();
                        pIsHas = false;
                    }
                    //更新实体
                    UpdateInfo(pCollection, pHoursTraffic, HourPer, pDt, pIsHas);

                    using (TransactionScope transaction = new TransactionScope())
                    {
                        //更新或添加
                        if (!pIsHas)
                            db.RP_HourAADT.Add(pHoursTraffic);
                        db.SaveChanges();
                        //提交事务
                        transaction.Complete();
                    }
                }
                string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：结束获取,13,14报表数据", endTime));
                SystemLog.GetInstance().Log.Info(string.Format("统计13,14报表数据耗时{0}秒", (DateTime.Parse(endTime) - DateTime.Parse(startTime)).TotalSeconds));
                //显示执行该方法的线程ID
                //SystemLog.GetInstance().Log.Info(string.Format("调用13,14Update的线程ID为：{0}", Thread.CurrentThread.ManagedThreadId));
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 11 Private Methods
        /// <summary>
        /// 更新实体信息
        /// </summary>
        /// <param name="sourcelist"></param>
        /// <param name="info"></param>
        /// <param name="statype"></param>
        /// <param name="dt"></param>
        /// <param name="ishas"></param>
        private void UpdateInfo(List<DS_DataSource> sourcelist, RP_HourAADT info, int hourper, DateTime dt, bool ishas)
        {
            if (ishas)//已存在，则更新
            {
                info.UpdDate = DateTime.Now;
                info.State = "1";
            }
            else//不存在，添加
            {
                info.Id = Guid.NewGuid();
                info.CrtDate = DateTime.Now;
                info.State = "0";
            }
            //大羊坊
            info.Dyf_ExIn = sourcelist.Where(s => s.StaID == (int)StationConfiguration.StationID.DYF).Sum(s => s.InNum);
            info.Dyf_EnOut = sourcelist.Where(s => s.StaID == (int)StationConfiguration.StationID.DYF).Sum(s => s.OutNum);
            //马驹桥东
            info.Mjqd_EnIn = sourcelist.Where(s => s.StaID == (int)StationConfiguration.StationID.MJQD).Sum(s => s.InNum);
            info.Mjqd_EnOut = sourcelist.Where(s => s.StaID == (int)StationConfiguration.StationID.MJQD).Sum(s => s.OutNum);
            //马驹桥西
            info.Mjqx_ExIn = sourcelist.Where(s => s.StaID == (int)StationConfiguration.StationID.MJQ).Sum(s => s.InNum);
            info.Mjqx_EnOut = sourcelist.Where(s => s.StaID == (int)StationConfiguration.StationID.MJQ).Sum(s => s.OutNum);
            //采育
            info.Cy_ExIn = sourcelist.Where(s => s.StaID == (int)StationConfiguration.StationID.CY).Sum(s => s.InNum);
            info.Cy_EnOut = sourcelist.Where(s => s.StaID == (int)StationConfiguration.StationID.CY).Sum(s => s.OutNum);

            info.HourPer = (byte)hourper;
            info.CalcuTime = DateTime.Parse(dt.ToShortDateString());

        }
        #endregion

    }
}

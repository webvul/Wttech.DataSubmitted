/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/15 14:41:53
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// 报表11,12源数据获取类
    /// </summary>
    public class HDayStatisticalReport : IObserver
    {
        #region 4 Properties
        /// <summary>
        /// 报表类型名称
        /// </summary>
        public string ReportName { get; set; }
        #endregion

        #region 5 Constructors
        public HDayStatisticalReport(string name)
        {
            this.ReportName = name;
        }
        #endregion

        #region 9 Public Methods
        /// <summary>
        /// 获取或更新报表11,12源数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="HourPer"></param>
        public void Update(DateTime dt, int HourPer)
        {
            try
            {
                string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：开始获取11,12报表数据", startTime));
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    //获取报表12配置的假期信息
                    List<OT_HDayConfig> pdayconfigs = db.OT_HDayConfig.Where(s => s.Id == 12).ToList();
                    OT_HDayConfig pdayconfig = new OT_HDayConfig();
                    if (pdayconfigs.Count > 0)
                    {
                        pdayconfig = pdayconfigs.First();
                    }
                    DateTime pDt = DateTime.Parse(dt.ToShortDateString());
                    //报表11,12收费站
                    List<int> TJStation = StationConfiguration.GetStaionList();
                    //获取今年当日数据//p.VehType == 0 表示合计数据，
                    IEnumerable<DS_DataSource> pCollection = db.DS_DataSource.Where(p =>
                        DbFunctions.TruncateTime(p.CalcuTime) == DbFunctions.TruncateTime(dt)
                        && p.VehType == 0 && TJStation.Contains(p.StaID.Value));

                    //今年数据
                    IEnumerable<DS_DataSource> pHdayCollection = db.DS_DataSource.Where(p =>
                        (DbFunctions.TruncateTime(p.CalcuTime) <= pdayconfig.HDayEnd.Value
                        && DbFunctions.TruncateTime(p.CalcuTime) >= pdayconfig.HDayStart.Value)
                        && p.VehType == 0 && TJStation.Contains(p.StaID.Value));

                    //每日数据
                    List<RP_HDayAADT> pList = db.RP_HDayAADT.Where(p => p.CalcuTime == pDt).ToList();
                    RP_HDayAADT pAADTInfo = null;
                    bool pIsHas = false;
                    if (pList.Count > 0)
                    {
                        pAADTInfo = pList.FirstOrDefault();
                        pIsHas = true;
                    }
                    else
                    {
                        pAADTInfo = new RP_HDayAADT();
                    }
                    //更新实体
                    UpdateInfo(pCollection, pAADTInfo, pDt, pIsHas, pHdayCollection);
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        //如果不存在，则添加，否则则更新
                        if (!pIsHas)
                            db.RP_HDayAADT.Add(pAADTInfo);
                        db.SaveChanges();
                        transaction.Complete();
                    }
                }
                string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：结束获取11,12报表数据", endTime));
                SystemLog.GetInstance().Log.Info(string.Format("统计11,12报表数据耗时{0}秒", (DateTime.Parse(endTime) - DateTime.Parse(startTime)).TotalSeconds));
                //显示执行该方法的线程ID
                //SystemLog.GetInstance().Log.Info(string.Format("调用11,12Update的线程ID为：{0}", Thread.CurrentThread.ManagedThreadId));
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
        /// <param name="sourcelist">数据源列表</param>
        /// <param name="info">实体信息</param>
        /// <param name="oldinfo">去年同期</param>
        /// <param name="dt">数据日期</param>
        /// <param name="ishas">记录是否已存在</param>
        /// <param name="hdaylist">今年数据</param>
        private void UpdateInfo(IEnumerable<DS_DataSource> sourcelist, RP_HDayAADT info, DateTime dt, bool ishas, IEnumerable<DS_DataSource> hdaylist)
        {
            if (ishas)//已存在
            {
                info.UpdDate = DateTime.Now;
                info.State = "1";
            }
            else//不存在
            {
                info.Id = Guid.NewGuid();
                info.CrtDate = DateTime.Now;
                info.State = "0";
            }
            info.CalcuTime = dt;
            //杨村站流量
            info.YC = sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.YC).Sum(s => s.OutNum.Value)
                + sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.YC).Sum(s => s.InNum.Value);
            //宜兴埠东站流量
            info.YXBD = sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.YXBD).Sum(s => s.OutNum.Value)
                + sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.YXBD).Sum(s => s.InNum.Value);
            //宜兴埠西站流量
            info.YXBX = sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.YXBX).Sum(s => s.OutNum.Value)
                + sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.YXBX).Sum(s => s.InNum.Value);
            //金钟路站流量
            info.JZL = sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.JZL).Sum(s => s.OutNum.Value)
                + sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.JZL).Sum(s => s.InNum.Value);
            //机场站流量
            info.JC = sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.JC).Sum(s => s.OutNum.Value)
                + sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.JC).Sum(s => s.InNum.Value);
            //空港经济区站流量
            info.KG = sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.KG).Sum(s => s.OutNum.Value)
                + sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.KG).Sum(s => s.InNum.Value);
            //塘沽西站流量
            info.TGX = sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.TGX).Sum(s => s.OutNum.Value)
                + sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.TGX).Sum(s => s.InNum.Value);
            //塘沽西分站流量
            info.TGXF = sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.TGXF).Sum(s => s.OutNum.Value)
                + sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.TGXF).Sum(s => s.InNum.Value);
            //塘沽北站流量
            info.TGB = sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.TG).Sum(s => s.OutNum.Value)
                + sourcelist.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.TG).Sum(s => s.InNum.Value);

            //本年数据
            double pTodayTemp = hdaylist.Sum(s => s.OutNum.Value);
            //表12出口流量
            info.Out = sourcelist.Sum(s => s.OutNum.Value);
            //去年同期
            info.SameSum = GetSameSum(dt);
            //    //合计
            //    info.Sum = pTodayTemp;
            //    //同比增幅    
            //    if (info.SameSum.HasValue && info.SameSum.Value != 0 && info.SameSum != 0.0)
            //        info.CompGrow = Math.Round((pTodayTemp - info.SameSum.Value) / info.SameSum.Value, 2);
        }
        /// <summary>
        /// 获取同期流量
        /// </summary>
        /// <param name="dt">数据日期</param>
        /// <returns></returns>
        public double? GetSameSum(DateTime dt)
        {
            double? pSameSum = null;
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                DateTime plasttime = new DateTime();
                //获取报表12配置的假期同期
                List<OT_HDayConfig> pdayconfigs = db.OT_HDayConfig.Where(s => s.Id == 12).ToList();
                OT_HDayConfig pdayconfig = new OT_HDayConfig();
                if (pdayconfigs.Count > 0)
                {
                    pdayconfig = pdayconfigs.First();
                    if (pdayconfig.HDayStart.HasValue && pdayconfig.HDayEnd.HasValue)
                    {
                        int ptemp = 0;
                        //判断当天是否在假期配置时间范围内
                        DateTime ptempdt = DateTime.Parse(dt.ToShortDateString());
                        if (ptempdt >= pdayconfig.HDayStart.Value && ptempdt <= pdayconfig.HDayEnd)
                        {
                            //间隔的天数
                            ptemp = (ptempdt - pdayconfig.HDayStart.Value).Days;
                        }
                        if (pdayconfig.CompStart.HasValue)
                        {
                            plasttime = pdayconfig.CompStart.Value.AddDays(ptemp);
                        }
                    }
                }
                List<RP_HDayAADT> pOlds = db.RP_HDayAADT.Where(s => s.CalcuTime <= plasttime && s.CalcuTime >= pdayconfig.CompStart.Value).ToList();
                pSameSum = pOlds.Sum(s => s.Sum.Value);
            }
            return pSameSum;
        }
        #endregion

    }
}

/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/3 13:57:04
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
    /// 报表8,9,10源数据获取类
    /// </summary>
    public class AADTStatisticalReport : IObserver
    {
        #region 4 Properties
        /// <summary>
        /// 报表类型名称
        /// </summary>
        public string ReportName { get; set; }
        #endregion

        #region 5 Constructors
        public AADTStatisticalReport(string name)
        {
            this.ReportName = name;
        }
        #endregion

        #region 9 Public Methods
        /// <summary>
        /// 8.9.10数据源更新方法
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="HourPer"></param>
        public void Update(DateTime dt, int HourPer)
        {
            try
            {
                string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：开始获取8,9,10报表数据", startTime));
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    DateTime plasttime = new DateTime();
                    List<OT_HDayConfig> pdayconfigs = db.OT_HDayConfig.Where(s => s.Id == 9).ToList();
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

                    List<RP_AADTSta> pOlds = db.RP_AADTSta.Where(s => s.CalculTime == plasttime).ToList();
                    //判断去年同期是否存在
                    RP_AADTSta oldinfo = new RP_AADTSta();
                    if (pOlds.Count > 0)
                    {
                        oldinfo = pOlds.First();
                    }
                    DateTime pdt = DateTime.Parse(dt.ToShortDateString());
                    //北京段包含出入口收费站
                    List<int> BJStation = StationConfiguration.GetBJStaion();
                    //北京段只包含出口收费站
                    List<int> outBJStation = StationConfiguration.GetOutBJStaion();
                    //获取北京段数据//p.VehType == 0 表示合计数据，
                    IEnumerable<DS_DataSource> pBJCollection = db.DS_DataSource.Where(p =>
                        DbFunctions.TruncateTime(p.CalcuTime) == DbFunctions.TruncateTime(dt)
                        && p.VehType == 0
                        && BJStation.Contains(p.StaID.Value));
                    //判断数据是否已存在
                    RP_AADTSta pAADTStaInfo = null;
                    List<RP_AADTSta> pList = db.RP_AADTSta.Where(p => p.CalculTime == pdt).ToList();
                    if (pList.Count > 0)//已存在，则更新
                    {
                        pAADTStaInfo = pList.FirstOrDefault();
                        pAADTStaInfo.UpdDate = DateTime.Now;
                        pAADTStaInfo.State = "1";
                    }
                    else//不存在，添加
                    {
                        pAADTStaInfo = new RP_AADTSta();
                        pAADTStaInfo.Id = Guid.NewGuid();
                        pAADTStaInfo.CrtDate = DateTime.Now;
                        pAADTStaInfo.State = "0";
                    }
                    //涉及到金额的单位全部转为万元，保留两位小数
                    //出京路线总交通量,不包括绿通
                    pAADTStaInfo.LineExSum = pBJCollection.Where(s => outBJStation.Contains(s.StaID.Value) && s.CalcuType != 3).Sum(s => s.InNum);
                    //入京路线总交通量，不包括绿通
                    pAADTStaInfo.LineEnSum = pBJCollection.Where(s => BJStation.Contains(s.StaID.Value) && s.CalcuType != 3).Sum(s => s.OutNum);
                    pAADTStaInfo.LineSum = pAADTStaInfo.LineEnSum + pAADTStaInfo.LineExSum;
                    //总交通量同比增幅
                    if (oldinfo.LineSum.HasValue)
                    {
                        pAADTStaInfo.SumGrow = double.Parse(string.Format("{0:0.00}", (pAADTStaInfo.LineSum - oldinfo.LineSum) / oldinfo.LineSum));
                    }
                    //出进京比
                    pAADTStaInfo.ExEnPer = double.Parse(string.Format("{0:0.00}", (pAADTStaInfo.LineExSum / pAADTStaInfo.LineEnSum)));
                    //收费/免征总金额
                    pAADTStaInfo.FeeSum = Math.Round(pBJCollection.Sum(s => s.RecMoney.Value) / 10000, 2);
                    //出京小型客车免费通行交通量
                    pAADTStaInfo.ExSmaCarFee = pBJCollection.Where(s => s.CalcuType == 0 && outBJStation.Contains(s.StaID.Value)).Sum(s => s.InNum);
                    //进京小型客车免费通行交通量
                    pAADTStaInfo.EnSmaCarFee = pBJCollection.Where(s => s.CalcuType == 0).Sum(s => s.OutNum);
                    //小型客车免费通行交通量（合计）
                    pAADTStaInfo.SmaCarFeeNum = pAADTStaInfo.ExSmaCarFee + pAADTStaInfo.EnSmaCarFee;
                    //小型客车交通量同比增幅
                    if (oldinfo.SmaCarFeeNum.HasValue)
                    {
                        pAADTStaInfo.SmaCarCompGrow = double.Parse(string.Format("{0:0.00}", (pAADTStaInfo.SmaCarFeeNum - oldinfo.SmaCarFeeNum) / oldinfo.SmaCarFeeNum));

                    }//小型客车免费金额
                    pAADTStaInfo.SmaCarFee = Math.Round(pBJCollection.Where(s => s.CalcuType.Value == 0).Sum(s => s.RecMoney.Value) / 10000, 2);
                    //出京收费车辆
                    pAADTStaInfo.ExChagNum = pBJCollection.Where(s => outBJStation.Contains(s.StaID.Value) && (s.CalcuType == 1 || s.CalcuType == 2)).Sum(s => s.InNum);
                    //进京收费车辆
                    pAADTStaInfo.EnChagNum = pBJCollection.Where(s => s.CalcuType == 1 || s.CalcuType == 2).Sum(s => s.OutNum);
                    //收费车辆合计
                    pAADTStaInfo.ChagSumNum = pAADTStaInfo.ExChagNum + pAADTStaInfo.EnChagNum;
                    //收费额度
                    pAADTStaInfo.ChagAmount = Math.Round(pBJCollection.Where(s => s.CalcuType == 1 || s.CalcuType == 2).Sum(s => s.RecMoney.Value) / 10000, 2);
                    //绿色通道车辆数
                    pAADTStaInfo.GreNum = pBJCollection.Where(s => s.CalcuType == 3).Sum(s => s.OutNum);
                    //绿色通道免收费金额
                    pAADTStaInfo.GreFee = Math.Round(pBJCollection.Where(s => s.CalcuType == 3).Sum(s => s.RecMoney.Value) / 10000, 2);
                    //出京总交通量（站）
                    pAADTStaInfo.StaExSum = pBJCollection.Where(s => s.StaID == (int)StationConfiguration.StationID.DYF).Sum(s => s.InNum);
                    //进京总交通量（站）
                    pAADTStaInfo.StaEnSum = pBJCollection.Where(s => s.StaID == (int)StationConfiguration.StationID.DYF).Sum(s => s.OutNum);
                    pAADTStaInfo.CalculTime = pdt;

                    using (TransactionScope trans = new TransactionScope())
                    {
                        if (pList.Count <= 0)
                            db.RP_AADTSta.Add(pAADTStaInfo);
                        db.SaveChanges();
                        trans.Complete();
                    }
                }
                string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：结束获取8,9,10报表数据", endTime));
                SystemLog.GetInstance().Log.Info(string.Format("统计8,9,10报表数据耗时{0}秒", (DateTime.Parse(endTime) - DateTime.Parse(startTime)).TotalSeconds));
                //显示执行该方法的线程ID
                //SystemLog.GetInstance().Log.Info(string.Format("调用8,9,10Update的线程ID为：{0}", Thread.CurrentThread.ManagedThreadId));
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}

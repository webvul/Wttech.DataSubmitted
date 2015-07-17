/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/3 17:10:28
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
    /// 报表18数据源获取类
    /// </summary>
    public class HDayAADTStatisticalReport : IObserver
    {
        #region 4 Properties
        /// <summary>
        /// 报表类型名称
        /// </summary>
        public string ReportName { get; set; }
        #endregion

        #region 5 Constructors
        public HDayAADTStatisticalReport(string name)
        {
            this.ReportName = name;
        }
        #endregion

        #region 9 Public Methods
        /// <summary>
        /// 报表18获取数据源方法
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="HourPer"></param>
        public void Update(DateTime dt, int HourPer)
        {
            try
            {
                string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：开始获取18报表数据", startTime));
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    //北京段包含出入口收费站
                    List<int> BJStation = StationConfiguration.GetBJStaion();
                    DateTime pdt = DateTime.Parse(dt.ToShortDateString());
                    //获取当日北京段数据源，不包括合计数据
                    List<DS_DataSource> pBJCollection = db.DS_DataSource.Where(s => DbFunctions.TruncateTime(s.CalcuTime) == pdt && s.VehType != 0 && BJStation.Contains(s.StaID.Value)).ToList();

                    //判断数据是否已存在
                    RP_HDayAADTSta pHDayStaInfo = null;
                    //5种路线类型
                    for (int i = 1; i < 7; i++)
                    {
                        List<RP_HDayAADTSta> pList = db.RP_HDayAADTSta.Where(p => p.CalcuTime == pdt && p.LineType == i).ToList();
                        if (pList.Count > 0)//已存在，则更新
                        {
                            pHDayStaInfo = pList.FirstOrDefault();
                            pHDayStaInfo.UpdDate = DateTime.Now;
                            pHDayStaInfo.State = "1";
                        }
                        else//不存在，添加
                        {
                            pHDayStaInfo = new RP_HDayAADTSta();
                            pHDayStaInfo.Id = Guid.NewGuid();
                            pHDayStaInfo.CrtDate = DateTime.Now;
                            pHDayStaInfo.State = "0";
                        }
                        pHDayStaInfo.CalcuTime = pdt;
                        switch (i)
                        {
                            //观测点1
                            case 1:
                                {
                                    this.SetInfo(pBJCollection, pHDayStaInfo, StationConfiguration.GetEnObs1(), StationConfiguration.GetExObs1(), 50000, i);
                                    break;
                                }
                            //观测点2
                            case 2:
                                {
                                    this.SetInfo(pBJCollection, pHDayStaInfo, StationConfiguration.GetEnObs2(), StationConfiguration.GetExObs2(), 50000, i);
                                    break;
                                }
                            //观测点3
                            case 3:
                                {
                                    this.SetInfo(pBJCollection, pHDayStaInfo, StationConfiguration.GetEnObs3(), StationConfiguration.GetExObs3(), 50000, i);
                                    break;
                                }
                            //收费站马驹桥
                            case 4:
                                {
                                    this.SetInfo(pBJCollection, pHDayStaInfo, new List<int> { (int)StationConfiguration.StationID.MJQ, (int)StationConfiguration.StationID.MJQD }, new List<int> { (int)StationConfiguration.StationID.MJQ }, 35000, i);
                                    break;
                                }
                            //收费站大羊坊
                            case 5:
                                {
                                    this.SetInfo(pBJCollection, pHDayStaInfo, new List<int> { (int)StationConfiguration.StationID.DYF }, new List<int> { (int)StationConfiguration.StationID.DYF }, 60000, i);
                                    break;
                                }
                            case 6://空数据
                                {
                                    pHDayStaInfo.LineType = i;
                                    break;
                                }
                        }
                        if (pList.Count <= 0)
                        {
                            db.RP_HDayAADTSta.Add(pHDayStaInfo);
                        }
                    }
                    using (TransactionScope tran = new TransactionScope())
                    {
                        db.SaveChanges();
                        tran.Complete();
                    }
                }
                string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：结束获取18报表数据", endTime));
                SystemLog.GetInstance().Log.Info(string.Format("统计18报表数据耗时{0}秒", (DateTime.Parse(endTime) - DateTime.Parse(startTime)).TotalSeconds));
                //显示执行该方法的线程ID
                //SystemLog.GetInstance().Log.Info(string.Format("调用18Update的线程ID为：{0}", Thread.CurrentThread.ManagedThreadId));
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
        /// 初始化实体数据
        /// </summary>
        /// <param name="pBJCollection">数据源集合</param>
        /// <param name="pHDayStaInfo">RP_HDayAADTSta实体</param>
        /// <param name="enstations">进京收费站列表</param>
        /// <param name="exstations">出京收费站列表</param>
        /// <param name="equval">设计交通量</param>
        /// <param name="type">路线名称类型</param>
        /// <returns>RP_HDayAADTSta实体</returns>
        private void SetInfo(List<DS_DataSource> collection, RP_HDayAADTSta info, List<int> enstations, List<int> exstations, int equval, int type)
        {
            //货车车型集合
            int[] truks = StationConfiguration.GetTruks();
            //大货车以上车型
            int[] overtruks = StationConfiguration.GetOverTruks();
            //出京自然交通辆
            info.ExNat = collection.Where(s => exstations.Contains(s.StaID.Value)).Sum(s => s.InNum);
            //进京自然交通辆
            info.EnNat = collection.Where(s => enstations.Contains(s.StaID.Value)).Sum(s => s.OutNum);
            //出京当量交通辆
            info.ExEqu = this.GetEqu(collection, exstations, 0);
            //进京当量交通辆
            info.EnEqu = this.GetEqu(collection, enstations, 1);
            info.CrowDeg = double.Parse(string.Format("{0:0.0000}", (info.ExEqu + info.EnEqu) / equval));//拥挤度
            //小型车出京
            info.SmaEx = collection.Where(s => exstations.Contains(s.StaID.Value) && (s.VehType == 1 || s.VehType == 11)).Sum(s => s.InNum);
            //小型车进京
            info.SmaEn = collection.Where(s => enstations.Contains(s.StaID.Value) && (s.VehType == 1 || s.VehType == 11)).Sum(s => s.OutNum);
            //中型车出京
            info.MedEx = collection.Where(s => exstations.Contains(s.StaID.Value) && (s.VehType == 2 || s.VehType == 12)).Sum(s => s.InNum);
            //中型车进京
            info.MedEn = collection.Where(s => enstations.Contains(s.StaID.Value) && (s.VehType == 2 || s.VehType == 12)).Sum(s => s.OutNum);
            //大型车出京
            info.LarEx = collection.Where(s => exstations.Contains(s.StaID.Value) && (s.VehType == 3 || s.VehType == 13)).Sum(s => s.InNum);
            //大型车进京
            info.LarEn = collection.Where(s => enstations.Contains(s.StaID.Value) && (s.VehType == 3 || s.VehType == 13)).Sum(s => s.OutNum);
            //重型车出京
            info.HeaEx = collection.Where(s => exstations.Contains(s.StaID.Value) && (s.VehType == 4 || s.VehType == 14)).Sum(s => s.InNum);
            //重型车进京
            info.HeaEn = collection.Where(s => enstations.Contains(s.StaID.Value) && (s.VehType == 4 || s.VehType == 14)).Sum(s => s.OutNum);
            //超大型车出京
            info.SupEx = collection.Where(s => exstations.Contains(s.StaID.Value) && s.VehType == 15).Sum(s => s.InNum);
            //超大型车进京
            info.SupEn = collection.Where(s => enstations.Contains(s.StaID.Value) && s.VehType == 15).Sum(s => s.OutNum);
            //进出京货车数量
            info.EnExTrukNum = collection.Where(s => enstations.Contains(s.StaID.Value) && truks.Contains(s.VehType.Value)).Sum(s => s.OutNum)
                + collection.Where(s => exstations.Contains(s.StaID.Value) && truks.Contains(s.VehType.Value)).Sum(s => s.InNum);
            //客车货车比例
            //int ptemp = int.Parse(pHDayStaInfo.EnExTrukNum.ToString());
            info.CarTrukPer = double.Parse(string.Format("{0:0.0000}", (info.ExNat + info.EnNat - info.EnExTrukNum) / info.EnExTrukNum));
            //进出京大货车以上车型的数量
            info.SupTruNum = collection.Where(s => enstations.Contains(s.StaID.Value) && overtruks.Contains(s.VehType.Value)).Sum(s => s.OutNum)
                + collection.Where(s => exstations.Contains(s.StaID.Value) && overtruks.Contains(s.VehType.Value)).Sum(s => s.InNum);
            //大货车以上占货车交通量比例
            info.SupTruPer = double.Parse(string.Format("{0:0.0000}", info.SupTruNum / info.EnExTrukNum));
            //路线名称类型
            info.LineType = type;
        }
        /// <summary>
        /// 获取当量交通量
        /// </summary>
        /// <param name="pBJCollection"></param>
        /// <param name="stations"></param>
        /// <param name="exen">0表示出京，1表示入京</param>
        /// <returns></returns>
        private double GetEqu(List<DS_DataSource> pBJCollection, List<int> stations, int exen)
        {
            double ptemp1 = 0;
            //出京当量
            if (exen == 0)
            {
                //客一，客二，货一
                ptemp1 += pBJCollection.Where(s => stations.Contains(s.StaID.Value) && (s.VehType == 1 || s.VehType == 2 || s.VehType == 11)).Sum(s => s.InNum.Value);
                //客三，客四，货二
                ptemp1 += (pBJCollection.Where(s => stations.Contains(s.StaID.Value) && (s.VehType == 3 || s.VehType == 4 || s.VehType == 12)).Sum(s => s.InNum.Value)) * 1.5;
                //货三，货四
                ptemp1 += (pBJCollection.Where(s => stations.Contains(s.StaID.Value) && (s.VehType == 13 || s.VehType == 14)).Sum(s => s.InNum.Value)) * 3;
                //货五
                ptemp1 += (pBJCollection.Where(s => stations.Contains(s.StaID.Value) && s.VehType == 15).Sum(s => s.InNum.Value)) * 4;
            }
            //进京当量
            else
            {
                //客一，客二，货一
                ptemp1 += pBJCollection.Where(s => stations.Contains(s.StaID.Value) && (s.VehType == 1 || s.VehType == 2 || s.VehType == 11)).Sum(s => s.OutNum.Value);
                //客三，客四，货二
                ptemp1 += (pBJCollection.Where(s => stations.Contains(s.StaID.Value) && (s.VehType == 3 || s.VehType == 4 || s.VehType == 12)).Sum(s => s.OutNum.Value)) * 1.5;
                //货三，货四
                ptemp1 += (pBJCollection.Where(s => stations.Contains(s.StaID.Value) && (s.VehType == 13 || s.VehType == 14)).Sum(s => s.OutNum.Value)) * 3;
                //货五
                ptemp1 += (pBJCollection.Where(s => stations.Contains(s.StaID.Value) && s.VehType == 15).Sum(s => s.OutNum.Value)) * 4;
            }
            return Math.Round(ptemp1, 0);

        }
        #endregion
    }
}

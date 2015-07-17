/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/26 10:17:41
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
    /// 报表1.2.3.4获取源数据
    /// </summary>
    public class DataDailyStatisticalReport : IObserver
    {
        #region Properties
        /// <summary>
        /// 报表类型名称
        /// </summary>
        public string ReportName { get; set; }

        public DataDailyStatisticalReport(string name)
        {
            this.ReportName = name;
        }
        #endregion

        #region 9 Public Methods
        /// <summary>
        /// 1.2.3.4获取更新方法
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="HourPer"></param>
        public void Update(DateTime dt, int HourPer)
        {
            try
            {
                string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：开始获取1,2,3,4报表数据", startTime));
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    DateTime pdt = DateTime.Parse(dt.ToShortDateString());

                    //获取北京段收费站列表
                    List<int> BJStation = StationConfiguration.GetBJStaion();
                    //获取天津段收费站列表
                    List<int> TJStation = StationConfiguration.GetStaionList2();

                    //获取北京段各车型合计数据VehType == 0表示合计//数据库中只存放每种车型的合计，有则更新，无则添加
                    IEnumerable<DS_DataSource> pBJDataSource = db.DS_DataSource.Where(p =>
                        DbFunctions.TruncateTime(p.CalcuTime) == DbFunctions.TruncateTime(dt)
                        && BJStation.Contains(p.StaID.Value)
                        && p.VehType == 0);
                    //获取天津段各车型合计数据
                    IEnumerable<DS_DataSource> pTJDataSource = db.DS_DataSource.Where(p =>
                       DbFunctions.TruncateTime(p.CalcuTime) == DbFunctions.TruncateTime(dt)
                       && TJStation.Contains(p.StaID.Value)
                       && p.VehType == 0);

                    //定义实体列表
                    List<RP_Daily> plist = new List<RP_Daily>();

                    //统计北京段
                    plist.AddRange(this.CalcuRP(1, pBJDataSource, pdt, BJStation));
                    //统计天津段
                    plist.AddRange(this.CalcuRP(3, pTJDataSource, pdt, TJStation));
                    //统计大羊坊
                    IEnumerable<DS_DataSource> pDYF = pBJDataSource.Where(s => s.StaID == (int)StationConfiguration.StationID.DYF);
                    plist.AddRange(this.CalcuRP(15, pDYF, pdt, BJStation.Where(s => s.Equals(15)).ToList()));
                    //统计泗村店
                    IEnumerable<DS_DataSource> pSCD = pTJDataSource.Where(s => s.StaID == (int)StationConfiguration.StationID.SCD);
                    plist.AddRange(this.CalcuRP(33, pSCD, pdt, TJStation.Where(s => s.Equals(33)).ToList()));
                    //添加
                    if (plist.Count > 0)
                    {
                        using (TransactionScope transac = new TransactionScope())
                        {
                            db.RP_Daily.AddRange(plist);
                            db.SaveChanges();
                            transac.Complete();
                        }
                    }
                }
                string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：结束获取1,2,3,4报表数据", endTime));
                SystemLog.GetInstance().Log.Info(string.Format("统计1,2,3,4报表数据耗时{0}秒", (DateTime.Parse(endTime) - DateTime.Parse(startTime)).TotalSeconds));
                //显示执行该方法的线程ID
                //SystemLog.GetInstance().Log.Info(string.Format("调用1,2,3,4Update的线程ID为：{0}", Thread.CurrentThread.ManagedThreadId));
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
        /// 统计类型
        /// </summary>
        /// <param name="stationtype">统计段类型</param>
        /// <param name="datasource">统计段数据源</param>
        /// <param name="dt">时间</param>
        /// <returns></returns>
        private List<RP_Daily> CalcuRP(int stationtype, IEnumerable<DS_DataSource> datasource, DateTime dt, List<int> stationlist)
        {
            List<RP_Daily> plist = new List<RP_Daily>();
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                for (int i = 0; i < 4; i++)
                {
                    //0小型客车，1其他客车，2货车（不包含绿通），3绿通
                    //每个站只有四条合计数据，每天数据表里一共只有16条
                    //判断各站各车型数据是有已存在，存在进行更新，不存在进行添加
                    List<RP_Daily> pDailyList = db.RP_Daily.Where(s => s.CalcuTime == dt && s.VehType == i && s.StaType == stationtype).ToList();
                    RP_Daily pDaily = new RP_Daily();
                    if (pDailyList.Count > 0)
                    {
                        pDaily = pDailyList.First();
                        pDaily.State = 1;
                        pDaily.UpdDate = DateTime.Now;
                    }
                    else
                    {
                        pDaily.Id = Guid.NewGuid();
                        pDaily.CrtDate = DateTime.Now;
                        pDaily.VehType = i;//车辆类型
                        pDaily.StaType = stationtype;
                        pDaily.State = 0;
                    }
                    byte ptemp = (byte)i;
                    pDaily.OutNum = datasource.Where(p => p.CalcuType == ptemp).Sum(p => p.OutNum);
                    pDaily.InNum = datasource.Where(p => p.CalcuType == ptemp).Sum(p => p.InNum);
                    if (i == 0)
                    {
                        pDaily.ChagFee = this.GetCharge(stationlist, datasource);
                    }
                    if (i == 1)
                    {
                        pDaily.ChagFee = datasource.Where(s => s.CalcuType == 1).Sum(s => s.RecMoney);
                    }
                    if (i == 2)
                    {
                        pDaily.ChagFee = datasource.Where(s => s.CalcuType == 2).Sum(s => s.RecMoney);
                    }
                    if (i == 3)
                    {
                        pDaily.InNum = 0;//入口绿通为0
                        pDaily.ChagFee = datasource.Where(s => s.CalcuType == 3).Sum(s => s.RecMoney);
                    }
                    pDaily.CalcuTime = DateTime.Parse(dt.ToShortDateString());

                    if (pDailyList.Count > 0)//进行更新
                    {
                        using (TransactionScope transac = new TransactionScope())
                        {
                            db.SaveChanges();
                            transac.Complete();
                        }
                    }
                    else
                    {
                        //将实体加入集合
                        plist.Add(pDaily);
                    }
                }
            }
            return plist;
        }
        /// <summary>
        /// 计算小型车收费免征金额
        /// </summary>
        /// <param name="stattionid"></param>
        /// <returns></returns>
        private decimal GetCharge(List<int> stationlist, IEnumerable<DS_DataSource> datasource)
        {
            //小型车辆数
            int smlvehnum;
            //单车费额
            decimal vehcharge;
            //收费免征金额
            decimal sumcharge = 0;
            try
            {
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    for (int i = 0; i < stationlist.Count; i++)
                    {
                        smlvehnum = datasource.Where(s => s.StaID == stationlist[i] && s.CalcuType == 0).Sum(s => s.OutNum.Value);
                        string ptemp = stationlist[i].ToString();
                        vehcharge = (decimal)db.OT_Station.Where(s => s.Num == ptemp).Select(s => s.Charge).Sum();
                        sumcharge += smlvehnum * vehcharge;
                    }
                }
            }
            catch (Exception e)
            {
                SystemLog.GetInstance().Info(e.Message, e);
            }
            return sumcharge;
        }
        #endregion
    }
}


/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/15 9:14:10
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
    /// 报表5，6源数据获取类
    /// </summary>
    public class EnExStatisticalReport : IObserver
    {
        #region 4 Properties
        /// <summary>
        /// 报表类型名称
        /// </summary>
        public string ReportName { get; set; }
        #endregion

        #region 5 Constructors
        public EnExStatisticalReport(string name)
        {
            this.ReportName = name;
        }
        #endregion

        #region 9 Public Methods
        /// <summary>
        /// 更新或获取报表5,6数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="HourPer"></param>
        public void Update(DateTime dt, int HourPer)
        {
            try
            {
                string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：开始获取5,6报表数据", startTime));
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    DateTime pDt = DateTime.Parse(dt.ToShortDateString());
                    //北京段包含出入口收费站
                    List<int> BJStation = StationConfiguration.GetBJStaion();

                    //获取数据//p.VehType == 0 表示合计数据，
                    IEnumerable<DS_DataSource> pCollection = db.DS_DataSource.Where(p =>
                        DbFunctions.TruncateTime(p.CalcuTime) == DbFunctions.TruncateTime(dt)
                        && p.VehType == 0);
                    //判断北京段数据是否已存在，StaType：1表示北京段，33表示泗村店
                    RP_EnEx pBJEnExInfo = null;
                    RP_EnEx pSCDEnExInfo = null;
                    bool pIsHas = false;
                    //北京段数据
                    List<RP_EnEx> pBJList = db.RP_EnEx.Where(p => p.CalcuTime == pDt && p.StaType == (int)StationConfiguration.StationType.BeiJingDuan).ToList();
                    if (pBJList.Count > 0)
                    {
                        pBJEnExInfo = pBJList.FirstOrDefault();
                        //每种状态需要重新赋值，防止公用同一个变量，值不明确
                        pIsHas = true;
                    }
                    else
                    {
                        pBJEnExInfo = new RP_EnEx();
                        pIsHas = false;
                    }
                    //更新北京段实体信息
                    UpdateInfo(pCollection.Where(s => BJStation.Contains(s.StaID.Value)), pBJEnExInfo, (int)StationConfiguration.StationType.BeiJingDuan, pDt, pIsHas);

                    //泗村店数据
                    List<RP_EnEx> pSCDList = db.RP_EnEx.Where(p => p.CalcuTime == pDt && p.StaType == (int)StationConfiguration.StationID.SCD).ToList();
                    if (pSCDList.Count > 0)
                    {
                        pSCDEnExInfo = pSCDList.FirstOrDefault();
                        pIsHas = true;
                    }
                    else
                    {
                        pSCDEnExInfo = new RP_EnEx();
                        pIsHas = false;
                    }
                    //更新泗村店实体信息
                    UpdateInfo(pCollection.Where(s => s.StaID.Value == (int)StationConfiguration.StationID.SCD), pSCDEnExInfo, (int)StationConfiguration.StationID.SCD, pDt, pIsHas);

                    //更新或添加到数据库
                    using (TransactionScope trans = new TransactionScope())
                    {
                        //如果不存在，则添加，否则则更新
                        if (pBJList.Count <= 0)
                            db.RP_EnEx.Add(pBJEnExInfo);
                        if (pSCDList.Count <= 0)
                            db.RP_EnEx.Add(pSCDEnExInfo);
                        db.SaveChanges();
                        trans.Complete();
                    }
                }
                string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：结束获取5,6报表数据", endTime));
                SystemLog.GetInstance().Log.Info(string.Format("统计5,6报表数据耗时{0}秒", (DateTime.Parse(endTime) - DateTime.Parse(startTime)).TotalSeconds));
                //显示执行该方法的线程ID
                //SystemLog.GetInstance().Log.Info(string.Format("调用5,6Update的线程ID为：{0}", Thread.CurrentThread.ManagedThreadId));
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
        /// <param name="statype">统计类型</param>
        /// <param name="dt">数据日期</param>
        /// <param name="ishas">记录是否已存在</param>
        private void UpdateInfo(IEnumerable<DS_DataSource> sourcelist, RP_EnEx info, int statype, DateTime dt, bool ishas)
        {
            //北京段只包含出口收费站
            List<int> outBJStation = StationConfiguration.GetOutBJStaion();
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
            //给实体字段赋值
            //0:小型客车，1：其他客车，2：货车，3：绿通
            info.EnSmaCar = sourcelist.Where(s => s.CalcuType == 0).Sum(s => s.OutNum.Value);
            info.EnOthCar = sourcelist.Where(s => s.CalcuType == 1).Sum(s => s.OutNum.Value);
            info.EnTruk = sourcelist.Where(s => s.CalcuType == 2).Sum(s => s.OutNum.Value);
            //泗村店，入境“绿色通道”数据项填写“0”
            info.EnGre = 0;
            info.StaType = statype;
            info.CalcuTime = dt;
            //泗村店不统计出镜，，北京段出境绿通为0,不包含马驹桥东入口
            if (statype == (int)StationConfiguration.StationType.BeiJingDuan)
            {
                info.ExSmaCar = sourcelist.Where(s => outBJStation.Contains(s.StaID.Value) && s.CalcuType == 0).Sum(s => s.InNum.Value);
                info.ExOthCar = sourcelist.Where(s => outBJStation.Contains(s.StaID.Value) && s.CalcuType == 1).Sum(s => s.InNum.Value);
                info.ExTruk = sourcelist.Where(s => outBJStation.Contains(s.StaID.Value) && s.CalcuType == 2).Sum(s => s.InNum.Value);
                info.ExGre = 0;
                info.EnGre = sourcelist.Where(s => s.CalcuType == 3).Sum(s => s.OutNum.Value);
            }
        }
        #endregion

    }
}

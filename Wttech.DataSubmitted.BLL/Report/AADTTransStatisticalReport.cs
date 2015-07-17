/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/15 13:15:38
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
    ///更新或获取报表7数据
    /// </summary>
    public class AADTTransStatisticalReport : IObserver
    {
        #region 4 Properties
        /// <summary>
        /// 报表类型名称
        /// </summary>
        public string ReportName { get; set; }
        #endregion

        #region 5 Constructors
        public AADTTransStatisticalReport(string name)
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
                SystemLog.GetInstance().Log.Info(string.Format("{0}：开始获取7报表数据", startTime));
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    DateTime pDt = DateTime.Parse(dt.ToShortDateString());

                    //获取数据//p.VehType == 0 表示合计数据，
                    IEnumerable<DS_DataSource> pCollection = db.DS_DataSource.Where(p =>
                        DbFunctions.TruncateTime(p.CalcuTime) == DbFunctions.TruncateTime(dt)
                        && p.VehType == 0 && p.StaID == (int)StationConfiguration.StationID.DYF);
                    //大羊坊单日数据
                    List<RP_AADTAndTransCalcu> pList = db.RP_AADTAndTransCalcu.Where(p => p.CalcuTime == pDt).ToList();
                    RP_AADTAndTransCalcu pAADTInfo = null;
                    bool pIsHas = false;
                    if (pList.Count > 0)
                    {
                        pAADTInfo = pList.FirstOrDefault();
                        pIsHas = true;
                    }
                    else
                    {
                        pAADTInfo = new RP_AADTAndTransCalcu();
                    }
                    //更新实体
                    UpdateInfo(pCollection, pAADTInfo, pDt, pIsHas);
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        //如果不存在，则添加，否则则更新
                        if (pList.Count <= 0)
                            db.RP_AADTAndTransCalcu.Add(pAADTInfo);
                        db.SaveChanges();
                        transaction.Complete();
                    }

                }
                string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SystemLog.GetInstance().Log.Info(string.Format("{0}：结束获取7报表数据", endTime));
                SystemLog.GetInstance().Log.Info(string.Format("统计7报表数据耗时{0}秒", (DateTime.Parse(endTime) - DateTime.Parse(startTime)).TotalSeconds));
                //显示执行该方法的线程ID
                //SystemLog.GetInstance().Log.Info(string.Format("调用7Update的线程ID为：{0}", Thread.CurrentThread.ManagedThreadId));
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
        /// <param name="dt">数据日期</param>
        /// <param name="ishas">记录是否已存在</param>
        private void UpdateInfo(IEnumerable<DS_DataSource> sourcelist, RP_AADTAndTransCalcu info, DateTime dt, bool ishas)
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
            //赋值
            //进京方向交通量
            info.EnTra = sourcelist.Sum(s => s.OutNum);
            //进京方向客车数
            info.EnCar = sourcelist.Where(s => s.CalcuType == 0 || s.CalcuType == 1).Sum(s => s.OutNum);
            //进京方向旅客量--万人次
            info.EnTrav = Math.Round(info.EnCar.Value * 5.84 / 10000, 2);
            //出京方向交通量
            info.ExTra = sourcelist.Sum(s => s.InNum);
            //出京方向客车数
            info.ExCar = sourcelist.Where(s => s.CalcuType == 0 || s.CalcuType == 1).Sum(s => s.InNum);
            //出京方向旅客量
            info.ExTrav = Math.Round(info.ExCar.Value * 5.84 / 10000, 2);
            //数据日期
            info.CalcuTime = dt;

        }
        #endregion
    }
}

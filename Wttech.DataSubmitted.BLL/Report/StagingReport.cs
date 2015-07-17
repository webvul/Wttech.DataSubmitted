/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/7 11:23:38
 */
#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.DAL;
using System.Threading;
using Wttech.DataSubmitted.IBLL.IReport;

#endregion
namespace Wttech.DataSubmitted.BLL.Report
{
    /// <summary>
    /// 中间表数据统计类
    /// </summary>
    public class StagingReport : IStagingReport, ISubject
    {
        //事件处理程序的委托
        private delegate void UpdateEventHandler(DateTime dt, int HourPer);
        //声明一事件Update，类型为委托EventHandler
        private event UpdateEventHandler Update;

        private int action;

        public StagingReport()
        {
            try
            {
                if (this.Update != null)
                {
                    Delegate[] dels = this.Update.GetInvocationList();
                    for (int i = 0; i < dels.Count(); i++)
                    {
                        this.Update -= (UpdateEventHandler)dels[i];
                    }
                }
                //报表1,2,3,4
                this.Update += new UpdateEventHandler(new DataDailyStatisticalReport("DataDailyStatisticalReport").Update);
                //报表5,6
                this.Update += new UpdateEventHandler(new EnExStatisticalReport("EnExStatisticalReport").Update);
                //报表7
                this.Update += new UpdateEventHandler(new AADTTransStatisticalReport("AADTTransStatisticalReport").Update);
                //报表8,9,10
                this.Update += new UpdateEventHandler(new AADTStatisticalReport("AADTStatisticalReport").Update);
                //报表11,12
                this.Update += new UpdateEventHandler(new HDayStatisticalReport("HDayStatisticalReport").Update);
                //报表13,14
                this.Update += new UpdateEventHandler(new HourAADTStatisticalReport("HourAADTStatisticalReport").Update);
                //报表15,16,17
                this.Update += new UpdateEventHandler(new DailyTrafficStatisticalReport("DailyTrafficStatisticalReport").Update);
                //报表18
                this.Update += new UpdateEventHandler(new HDayAADTStatisticalReport("HDayAADTStatisticalReport").Update);
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
        }
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="HourPer"></param>
        public void Notify(object prame)
        {
            ThreadParameter pThreadparameter = prame as ThreadParameter;
            //检查委托是否为空
            UpdateEventHandler pEventHandler = Update;
            if (pEventHandler != null)
            {
                foreach (UpdateEventHandler handler in pEventHandler.GetInvocationList())
                {
                    try
                    {
                        //异步调用委托
                        handler.BeginInvoke(pThreadparameter.Datetime, pThreadparameter.HourPer, null, null);
                        //循环调用每一个事件，防止其中一个报错，使得下面的事件不被执行
                        //handler(pThreadparameter.Datetime, pThreadparameter.HourPer);
                    }
                    catch (Exception ex)
                    {
                        SystemLog.GetInstance().Info(ex.Message);
                    }
                }
            }

        }

        public int State
        {
            get { return action; }
            set { action = value; }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        public void UpdateData(DateTime pDt, int pHourPer)
        {
            try
            {
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    SystemLog.GetInstance().Log.Info(string.Format("{0}：开始获取中间表数据", startTime));
                    db.SP_StaDataSource(pHourPer, pDt);
                    string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    SystemLog.GetInstance().Log.Info(string.Format("{0}：结束获取中间表数据", endTime));
                    SystemLog.GetInstance().Log.Info(string.Format("统计中间表数据耗时{0}秒", (DateTime.Parse(endTime) - DateTime.Parse(startTime)).TotalSeconds));
                    this.State = 1;
                    if (State == 1)
                    {
                        int timeHour;
                        DateTime dt = pDt.AddHours(-1);
                        if (pHourPer == 0)
                        {

                            timeHour = 23;
                        }
                        else
                        {
                            if (pHourPer == 1)
                            {
                                timeHour = 0;
                            }
                            else
                            {
                                timeHour = pHourPer - 1;
                            }
                        }
                        this.Notify(new ThreadParameter(dt, timeHour));
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Log.Info(ex.Message);
            }
        }
    }
    /// <summary>
    /// 参数类
    /// </summary>
    class ThreadParameter
    {
        private DateTime datetime;
        private int hourper;

        public DateTime Datetime
        {
            get { return datetime; }
            set { datetime = value; }
        }

        public int HourPer
        {
            get { return hourper; }
            set { hourper = value; }
        }

        public ThreadParameter(DateTime dt, int timeperiod)
        {
            this.Datetime = dt;
            this.HourPer = timeperiod;
        }

    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wttech.DataSubmitted.BLL.Report;
using Wttech.DataSubmitted.Common;

namespace Wttech.DataSubmitted.Tests
{
    /// <summary>
    /// 1234测试类
    /// </summary>
    [TestClass]
    public class DataDailyTest
    {
        /// <summary>
        /// 获取源数据测试方法
        /// </summary>
        [TestMethod]
        public void DataDailyGetDataTestMethod()
        {
            DataDailyStatisticalReport datadaily = new DataDailyStatisticalReport("");
            for (int i = 0; i < 24; i++)
            {
                int pTimePeriodHour = i;
                DateTime pDt = DateTime.Parse("2014-10-1").AddHours(i);
                datadaily.Update(pDt, pTimePeriodHour);
            }
           
        }
        /// <summary>
        /// 查询测试方法
        /// </summary>
        [TestMethod]
        public void GetListByPraTestMethod()
        {
            //naturalManege.GetList();
            DataDailyTrafficStatistical da = new DataDailyTrafficStatistical();
            da.GetListByPra(new QueryParameters { StartTime = DateTime.Parse("2014-10-1"), StationType = 3, ReportType = 2 });
        }
    }
}

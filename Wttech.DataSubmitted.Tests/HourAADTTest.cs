using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wttech.DataSubmitted.BLL.Report;
using Wttech.DataSubmitted.Common;

namespace Wttech.DataSubmitted.Tests
{
    /// <summary>
    /// 报表13,14测试类
    /// </summary>
    [TestClass]
    public class HourAADTTest
    {
        /// <summary>
        /// 数据源获取测试方法
        /// </summary>
        [TestMethod]
        public void HourAADTStatisticalReportUpdateTest()
        {
            HourAADTStatisticalReport houtaadt = new HourAADTStatisticalReport("");
            for (int i = 0; i < 24; i++)
            {
                int pTimePeriodHour = i;
                DateTime pDt = DateTime.Parse("2014-10-1").AddHours(i);
                houtaadt.Update(pDt, pTimePeriodHour);
            }
           
        }
        /// <summary>
        /// 查询测试方法
        /// </summary>
        [TestMethod]
        public void HourAADTStatisticalQuery()
        {
            HourAADTStatistical houtaadt = new HourAADTStatistical();
            houtaadt.GetListByPra(new QueryParameters { StartTime = DateTime.Parse("2014-10-1"), ReportType = 13, StationType = 1 });
        }

    }
}

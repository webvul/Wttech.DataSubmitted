using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wttech.DataSubmitted.BLL.Report;
using Wttech.DataSubmitted.Common;

namespace Wttech.DataSubmitted.Tests
{
    /// <summary>
    /// 报表11,12测试类
    /// </summary>
    [TestClass]
    public class HDayTest
    {
        /// <summary>
        /// 数据源更新获取测试方法
        /// </summary>
        [TestMethod]
        public void HDayUpdateTest()
        {
            HDayStatisticalReport hday = new HDayStatisticalReport("");
            for (int i = 0; i < 24; i++)
            {
                int pTimePeriodHour = i;
                DateTime pDt = DateTime.Parse("2014-10-1").AddHours(i);
                hday.Update(pDt, pTimePeriodHour);
            }    
        }
        /// <summary>
        /// 数据源查询测试方法
        /// </summary>
        [TestMethod]
        public void HDayQueryTest()
        {
            HDayExStatistical hdayex = new HDayExStatistical();
            hdayex.GetListByPra(new QueryParameters() { StartTime = DateTime.Parse("2014-10-1"), EndTime = DateTime.Parse("2014-10-1"), LastYearStart = DateTime.Parse("2014-10-1"), LastYearEnd = DateTime.Parse("2014-10-1"), ReportType = 12, StationType = 3 });
        }
        /// <summary>
        /// 导出测试方法
        /// </summary>
        [TestMethod]
        public void HDayExportTest()
        {
            HDayExStatistical hdayex = new HDayExStatistical();
            hdayex.ExportReport(new QueryParameters() { StartTime = DateTime.Parse("2014-10-1"), EndTime = DateTime.Parse("2014-10-4"), LastYearStart = DateTime.Parse("2014-10-1"), LastYearEnd = DateTime.Parse("2014-10-1"), ReportType = 12, StationType = 3 });
        }
    }
}

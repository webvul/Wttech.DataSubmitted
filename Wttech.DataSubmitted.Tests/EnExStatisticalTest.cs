using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wttech.DataSubmitted.BLL.Report;

namespace Wttech.DataSubmitted.Tests
{
    /// <summary>
    /// 报表5,6单元测试类
    /// </summary>
    [TestClass]
    public class EnExStatisticalTest
    {
        /// <summary>
        /// 数据源更新或获取测试方法
        /// </summary>
        [TestMethod]
        public void EnExReportUpdateTest()
        {
            EnExStatisticalReport enex = new EnExStatisticalReport("");
            for (int i = 0; i < 24; i++)
            {
                int pTimePeriodHour = i;
                DateTime pDt = DateTime.Parse("2014-10-1").AddHours(i);
                enex.Update(pDt, pTimePeriodHour);
            }
        }
    }
}

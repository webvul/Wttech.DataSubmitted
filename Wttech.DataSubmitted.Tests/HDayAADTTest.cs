using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wttech.DataSubmitted.BLL.Report;
using Wttech.DataSubmitted.Common;

namespace Wttech.DataSubmitted.Tests
{
    /// <summary>
    /// 18测试类
    /// </summary>
    [TestClass]
    public class HDayAADTTest
    {
        /// <summary>
        /// 数据源获取测试方法
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            HDayAADTStatisticalReport hday = new HDayAADTStatisticalReport("");
            for (int i = 0; i < 24; i++)
            {
                int pTimePeriodHour = i;
                DateTime pDt = DateTime.Parse("2014-10-1").AddHours(i);
                hday.Update(pDt, pTimePeriodHour);
            }         
        }
        /// <summary>
        /// 查询测试方法
        /// </summary>
        [TestMethod]
        public void TestGetListByPra()
        {
            HDayAADTStatistical hday = new HDayAADTStatistical();
            hday.GetListByPra(new QueryParameters() { StartTime = DateTime.Parse("2014-10-1"), StationType = 1 });
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wttech.DataSubmitted.BLL.Report;

namespace Wttech.DataSubmitted.Tests
{
    /// <summary>
    /// 报表7单元测试类
    /// </summary>
    [TestClass]
    public class AADTTransStatisticalTest
    {
        /// <summary>
        /// 数据源更新或添加测试方法
        /// </summary>
        [TestMethod]
        public void AADTTransUpdateTest()
        {
            AADTTransStatisticalReport aadt = new AADTTransStatisticalReport("");
            for (int i = 0; i < 24; i++)
            {
                int pTimePeriodHour = i;
                DateTime pDt = DateTime.Parse("2014-10-1").AddHours(i);
                aadt.Update(pDt, pTimePeriodHour);
            }
        }
    }
}

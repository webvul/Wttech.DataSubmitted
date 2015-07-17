using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wttech.DataSubmitted.BLL.Report;

namespace Wttech.DataSubmitted.Tests
{
    /// <summary>
    /// 8,9,10
    /// </summary>
    [TestClass]
    public class AADTStatisticalTest
    {
        [TestMethod]
        public void TestUpdate()
        {
            AADTStatisticalReport aadt = new AADTStatisticalReport("");
            for (int i = 0; i < 24; i++)
            {
                int pTimePeriodHour = i;
                DateTime pDt = DateTime.Parse("2014-10-1").AddHours(i);
                aadt.Update(pDt, pTimePeriodHour);
            }
        }
    }
}

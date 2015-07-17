using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wttech.DataSubmitted.BLL.Report;
using Wttech.DataSubmitted.Common;

namespace Wttech.DataSubmitted.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string temp = DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.Hour;
            HDayTraStatistical aa = new HDayTraStatistical();
            aa.GetListByPra(new QueryParameters { StartTime = DateTime.Parse("2014-11-5"), StationType = 1 });
            //AADTStatisticalReport aadt = new AADTStatisticalReport("");
            //aadt.Update(DateTime.Parse("2014-10-1"), -1);
        }
    }
}

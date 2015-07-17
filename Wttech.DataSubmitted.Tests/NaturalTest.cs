using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wttech.DataSubmitted.BLL;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Web.Controllers;
using System.Web;
using System.Runtime.CompilerServices;
using Wttech.DataSubmitted.IBLL;
using System.Web.Mvc;
using Wttech.DataSubmitted.Web;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using Wttech.DataSubmitted.IBLL.IReport;
using Wttech.DataSubmitted.BLL.Report;
namespace Wttech.DataSubmitted.Tests
{
    [TestClass]
    public class NaturalTest
    {

        //public NaturalTest()
        //{
        //    IUnityContainer container = new UnityContainer();
        //    UnityConfigurationSection configuration = ConfigurationManager.GetSection(UnityConfigurationSection.SectionName)
        //    as UnityConfigurationSection;
        //    configuration.Configure(container, "defaultContainer");
        //    naturalManege = container.Resolve<INaturalTrafficStatistical>();
        //    //new NaturalTrafficStatistical();
        //}
        //[Dependency]
        public INaturalTrafficStatistical naturalManege { get; set; }
        [TestMethod]
        public void TestMethod1()
        {
            //naturalManege.GetList();
            NaturalTrafficStatistical na = new NaturalTrafficStatistical();
            na.GetListByPra(new QueryParameters { StartTime = DateTime.Parse("2014-10-1"), ReportType = 15 });
        }
        [TestMethod]
        public void TestMethod2()
        {
        }
        [TestMethod]
        public void TestExportExcelMethod()
        {
            NaturalTrafficStatistical na = new NaturalTrafficStatistical();
            na.ExportReport(new QueryParameters { StartTime = DateTime.Parse("2014-10-1"), HolidayId = 1, ReportType = 15 });
        }
        [TestMethod]
        public void TestCalibrationData()
        {
            NaturalTrafficStatistical na = new NaturalTrafficStatistical();
            na.CalibrationData(new QueryParameters { StartTime = DateTime.Parse("2014-11-20"), ReportType = 15, StationType = 1, LastYearStart = DateTime.Parse("2014-10-1"), FloatingRange = 5 });
        }
        [TestMethod]
        public void TestGetNoDataList()
        {
            NaturalTrafficStatistical na = new NaturalTrafficStatistical();
            na.GetNoDataList(new QueryParameters { StartTime = DateTime.Parse("2014-12-6"), ReportType = 15, StationType = 1 });

        }
        [TestMethod]
        public void UpdateTest()
        {
            DailyTrafficStatisticalReport na = new DailyTrafficStatisticalReport("");
            for (int i = 0; i < 24; i++)
            {
                int pTimePeriodHour = i;
                DateTime pDt = DateTime.Parse("2014-10-1").AddHours(i);
                na.Update(pDt, pTimePeriodHour);
            }


        }
    }
}

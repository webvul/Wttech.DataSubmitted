using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wttech.DataSubmitted.Services;
using Wttech.DataSubmitted.BLL.Report;
using Microsoft.Practices.Unity;
using System.Configuration;
using Microsoft.Practices.Unity.Configuration;

namespace Wttech.DataSubmitted.Tests
{
    [TestClass]
    public class ServicesStartTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //DataServicesCore.GetInstance().Start();
        }
        [TestMethod]
        public void TestMethod2()
        {
            int pTimePeriodHour = DateTime.Now.Hour;
            int pTimePeriodMinute = DateTime.Now.Minute;
            if (pTimePeriodMinute == 2 || pTimePeriodMinute == 3)
            {
                DateTime pDt = DateTime.Today.AddHours(pTimePeriodHour);
                new StagingReport().UpdateData(pDt, pTimePeriodHour);
            }

            //int pTimePeriodHour = 1;
            //DateTime pDt = DateTime.Parse("2014-10-10").AddHours(1);
            //new StagingReport().UpdateData(pDt, pTimePeriodHour);

        }
        [TestMethod]
        public void TestMethodRun()
        {
            int pTimePeriodHour = 3;
            DateTime pDt = DateTime.Parse("2014-10-1").AddHours(3);
            IUnityContainer container = new UnityContainer();
            UnityConfigurationSection configuration = ConfigurationManager.GetSection(UnityConfigurationSection.SectionName)
            as UnityConfigurationSection;
            configuration.Configure(container, "serviceContainer");
            IBLL.ServicesFactory.Instance = container.Resolve(typeof(IBLL.ServicesFactory)) as IBLL.ServicesFactory;
            IBLL.ServicesFactory.Instance.stagingReport.UpdateData(pDt, pTimePeriodHour);


        }       
    }
}

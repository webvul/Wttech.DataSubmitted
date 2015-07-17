using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.IBLL;

namespace Wttech.DataSubmitted.Web.Controllers
{
    public class TJReportController : Controller
    {
        //
        // GET: /TJReport/
        /// <summary>
        /// 报表2：收费公路数据汇总日报
        /// </summary>
        /// <returns></returns>
        public ActionResult TollRoadDaily()
        {
            try
            {
                ForecastViewModel model = ReportFactory.Instance.report1.GetForecastWhere(new QueryParameters() { ReportType = 2 });
                ViewBag.StartTime = model.ForecastDate.Value.ToString("yyyy年M月d日");
                HolidayConfigViewModel pHdayConfig = ReportFactory.Instance.GetHolidayConfigById(2);
                ViewBag.HolidayStartTime = pHdayConfig.HolidayStartTime.Value.ToString("yyyy年M月d日");
                ViewBag.HolidayEndTime = pHdayConfig.HolidayEndTime.Value.ToString("yyyy年M月d日");
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
            return View();
        }
        /// <summary>
        /// 报表2修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateTollRoadDaily(UpdateDataDailyViewModel args)
        {
            args.StationType = 3;
            try
            {
                ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(2), Utility.GetFormatDate(args.DataDate, null));

                return Json(ReportFactory.Instance.report1.Update(args));
            }
            catch (Exception ex)
            {
                CustomResult ret = new CustomResult();
                ret.ResultKey = 0;
                ret.ResultValue = ex.Message;
                return Json(ret);
            }
        }
        /// <summary>
        /// 报表4：重要运输通道主线收费站数据日报表
        /// </summary>
        /// <returns></returns>
        public ActionResult ImpStaDaily()
        {
            try
            {
                ForecastViewModel model = ReportFactory.Instance.report1.GetForecastWhere(new QueryParameters() { ReportType = 4 });
                ViewBag.StartTime = model.ForecastDate.Value.ToString("yyyy年M月d日");
                HolidayConfigViewModel pHdayConfig = ReportFactory.Instance.GetHolidayConfigById(4);
                ViewBag.HolidayStartTime = pHdayConfig.HolidayStartTime.Value.ToString("yyyy年M月d日");
                ViewBag.HolidayEndTime = pHdayConfig.HolidayEndTime.Value.ToString("yyyy年M月d日");
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
            return View();
        }
        /// <summary>
        /// 报表4修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateImpStaDaily(UpdateDataDailyViewModel args)
        {
            args.StationType = 33;
            try
            {
                ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(4), Utility.GetFormatDate(args.DataDate, null));

                return Json(ReportFactory.Instance.report1.Update(args));
            }
            catch (Exception ex)
            {
                CustomResult ret = new CustomResult();
                ret.ResultKey = 0;
                ret.ResultValue = ex.Message;
                return Json(ret);
            }
        }
        /// <summary>
        /// 报表11：天津市高速公路支队****年**假期重点收费站流量表（出口+入口）
        /// </summary>
        /// <returns></returns>
        public ActionResult HDayStaExEn()
        {
            WhereHDayStaExEnViewModel pTemp = ReportFactory.Instance.report11.GetHdayExEnWhere();
            IEnumerable<SelectListItem> pHolName = ReportFactory.Instance.DicGetListbyId((int)EDicParentId.Holiday).Select(i =>
                new SelectListItem()
                {
                    Value = i.DictId.ToString(),
                    Text = i.Name,
                });
            ViewBag.StartTime = pTemp.StartTime.ToString("yyyy年M月d日");
            ViewBag.EndTime = pTemp.EndTime.ToString("yyyy年M月d日");
            ViewBag.HolName = pHolName;
            ViewBag.HolidayId = pTemp.HolidayId.ToString();
            return View();
        }
        /// <summary>
        /// 报表11：天津市高速公路支队****年**假期重点收费站流量表（出口+入口）-修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateHDayStaExEn(UpdateHDayStaExEnViewModel args)
        {
            try
            {
                ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(11), Utility.GetFormatDate(args.StartTime, null));

                return Json(ReportFactory.Instance.report11.Update(args));
            }
            catch (Exception ex)
            {
                CustomResult ret = new CustomResult();
                ret.ResultKey = 0;
                ret.ResultValue = ex.Message;
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
        }
        /// <summary>
        /// 报表12：天津市高速公路支队****年**假期流量表（出口）
        /// </summary>
        /// <returns></returns>
        public ActionResult HDayEx()
        {
            try
            {
                HolidayConfigViewModel pHdayConfig = ReportFactory.Instance.GetHolidayConfigById(12);
                ViewBag.StartTime = pHdayConfig.HolidayStartTime.Value.ToString("yyyy年M月d日");
                ViewBag.EndTime = pHdayConfig.HolidayEndTime.Value.ToString("yyyy年M月d日");
                //去年同期
                ViewBag.LastSameStart = pHdayConfig.ComparedStartTime.Value.ToString("yyyy年M月d日");
                ViewBag.LastSameEnd = pHdayConfig.ComparedEndTime.Value.ToString("yyyy年M月d日");
                //配置天数
                ViewBag.CountDay = (pHdayConfig.HolidayEndTime.Value - pHdayConfig.HolidayStartTime.Value).Days;
                IEnumerable<SelectListItem> pHolName = ReportFactory.Instance.DicGetListbyId((int)EDicParentId.Holiday).Select(i =>
                    new SelectListItem()
                    {
                        Value = i.DictId.ToString(),
                        Text = i.Name,
                    });
                ViewBag.HolName = pHolName;
                ViewBag.HolidayId = pHdayConfig.HolidayId.ToString();
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
            return View();
        }
        /// <summary>
        /// 报表12：天津市高速公路支队假期流量表（出口）-修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateHDayEx(UpdateHdayExViewModel args)
        {
            try
            {
                ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(12), Utility.GetFormatDate(args.StartTime, args.EndTime));

                return Json(ReportFactory.Instance.report12.Update(args));
            }
            catch (Exception ex)
            {
                CustomResult ret = new CustomResult();
                ret.ResultKey = 0;
                ret.ResultValue = ex.Message;
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
        }
        /// <summary>
        /// 报表6：重点城市出入境车辆数日报表
        /// </summary>
        /// <returns></returns>
        public ActionResult CityDailyEnEx()
        {
            try
            {
                HolidayConfigViewModel pHdayConfig = ReportFactory.Instance.GetHolidayConfigById(6);
                ViewBag.StartTime = pHdayConfig.ForecastDate.Value.ToString("yyyy年M月d日");
                ViewBag.HolidayStartTime = pHdayConfig.HolidayStartTime.Value.ToString("yyyy年M月d日");
                ViewBag.HolidayEndTime = pHdayConfig.HolidayEndTime.Value.ToString("yyyy年M月d日");
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
            return View();
        }

        /// <summary>
        /// 报表6：重点城市出入境车辆数日报表-修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateCityDailyEnEx(UpdateEnExViewModel args)
        {
            args.StationType = 33;
            try
            {
                ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(6), Utility.GetFormatDate(args.DataDate, null));

                return Json(ReportFactory.Instance.report6.Update(args));
            }
            catch (Exception ex)
            {
                CustomResult ret = new CustomResult();
                ret.ResultKey = 0;
                ret.ResultValue = ex.Message;
                return Json(ret);
            }
        }
        /// <summary>
        /// 无数据列表
        /// </summary>
        /// <returns></returns>
        public ActionResult NoDataList()
        {
            return View();
        }
    }
}
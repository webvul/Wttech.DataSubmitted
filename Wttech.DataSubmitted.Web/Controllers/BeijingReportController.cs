/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个用户权限管理模块接口控制器文件
* 创建标识：ta0383王建20141114
*/
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
    /// <summary>
    /// 北京段数据报表控制器
    /// </summary>
    public class BeijingReportController : Controller
    {
        #region 9 Public Methods
        //
        // GET: /BeijingReport/
        /// <summary>
        /// 报表1：收费公路数据汇总表
        /// </summary>
        /// <returns></returns>
        public ActionResult TollRoadSum()
        {
            try
            {
                ForecastViewModel model = ReportFactory.Instance.report1.GetForecastWhere(new QueryParameters() { ReportType = 1 });
                ViewBag.StartTime = model.ForecastDate.Value.ToString("yyyy年M月d日");
                HolidayConfigViewModel pHdayConfig = ReportFactory.Instance.GetHolidayConfigById(1);
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
        /// 报表1修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateTollRoadSum(UpdateDataDailyViewModel args)
        {
            args.StationType = 1;
            try
            {
                ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(1), Utility.GetFormatDate(args.DataDate, null));

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
        /// 报表3：京沪大羊坊
        /// </summary>
        /// <returns></returns>
        public ActionResult DYFSum()
        {
            try
            {
                ForecastViewModel model = ReportFactory.Instance.report1.GetForecastWhere(new QueryParameters() { ReportType = 3 });
                ViewBag.StartTime = model.ForecastDate.Value.ToString("yyyy年M月d日");
                HolidayConfigViewModel pHdayConfig = ReportFactory.Instance.GetHolidayConfigById(3);
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
        /// 报表3修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateDYFtollstation(UpdateDataDailyViewModel args)
        {
            args.StationType = 15;
            try
            {
                ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(3), Utility.GetFormatDate(args.DataDate, null));

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
        ///报表5：重点城市出入境车辆数日报表（北京段）
        /// </summary>
        /// <returns></returns>
        public ActionResult CityEnEx()
        {
            try
            {
                HolidayConfigViewModel pHdayConfig = ReportFactory.Instance.GetHolidayConfigById(5);
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
        ///报表5：重点城市出入境车辆数日报表（北京段）-修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateCityEnEx(UpdateEnExViewModel args)
        {
            args.StationType = 1;
            try
            {
                ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(5), Utility.GetFormatDate(args.DataDate, null));

                return Json(ReportFactory.Instance.report5.Update(args));
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
        /// 报表7校正页面
        /// </summary>
        /// <returns></returns>
        public ActionResult checkReport7()
        {
            return View();
        }

        /// <summary>
        ///报表7：黄金周京津塘高速公路交通量统计表
        /// </summary>
        /// <returns></returns>
        public ActionResult JJTHighWay()
        {
            try
            {
                AADTAndTransCalcuWViewModel pTemp = ReportFactory.Instance.report7.GetHdayExEnWhere();
                IEnumerable<SelectListItem> pHolName =ReportFactory.Instance.DicGetListbyId((int)EDicParentId.Holiday).Select(i =>
                    new SelectListItem()
                    {
                        Value = i.DictId.ToString(),
                        Text = i.Name,
                    });
                ViewBag.StartTime = pTemp.StartTime.ToString("yyyy年M月d日");
                ViewBag.EndTime = pTemp.EndTime.ToString("yyyy年M月d日");
                ViewBag.HolName = pHolName;
                ViewBag.HolidayId = pTemp.HolidayId.ToString();
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
            return View();
        }

        /// <summary>
        ///报表7：黄金周京津塘高速公路交通量统计表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateJJTHighWay(AADTAndTransCalcuUViewModel args)
        {
            try
            {
                ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(7), Utility.GetFormatDate(args.DataDate, null));

                return Json(ReportFactory.Instance.report7.Update(args));
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
        ///报表8：假期交通量统计表
        /// </summary>
        /// <returns></returns>
        public ActionResult HDaySta()
        {
            try
            {
                ForecastViewModel model = ReportFactory.Instance.report8.GetForecastWhere();
                ViewBag.StartTime = model.ForecastDate.Value.ToString("yyyy年M月d日");
                HolidayConfigViewModel pHdayConfig = ReportFactory.Instance.GetHolidayConfigById(8);
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
        /// 报表8修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateHDaySta(UpdateHDayTraInfoViewModel args)
        {
            args.StationType = 1;
            try
            {
                ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(8), Utility.GetFormatDate(args.DataDate, null));

                return Json(ReportFactory.Instance.report8.Update(args));
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
        ///报表9：收费公路运行情况
        /// </summary>
        /// <returns></returns>
        public ActionResult RoadRunSit()
        {
            try
            {
                ConfigTimeViewModel model = ReportFactory.Instance.report9.GetRoadRunSitWhere();
                ViewBag.HolidayStartTime = model.HolidayStartTime.Value.ToString("yyyy年M月d日");
                ViewBag.HolidayEndTime = model.HolidayEndTime.Value.ToString("yyyy年M月dd日");
                ViewBag.ComparedStartTime = model.ComparedStartTime.Value.ToString("yyyy年M月d日");
                ViewBag.ComparedEndTime = model.ComparedEndTime.Value.ToString("yyyy年M月d日");
                ForecastViewModel modelForecast = ReportFactory.Instance.report9.GetForecastWhere();
                ViewBag.ForecastDate = modelForecast.ForecastDate.Value.ToString("yyyy年M月d日");
                ViewBag.ForecastFloat = modelForecast.ForecastFloat;
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
            return View();
        }

        /// <summary>
        /// 加载报表9 的查询条件项初始值
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RoadRunSitWhere()
        {
            return Json(ReportFactory.Instance.report9.GetRoadRunSitWhere(), JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 加载报表9 的查询条件项初始值
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ForecastWhere()
        {
            return Json(ReportFactory.Instance.report9.GetForecastWhere(), JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 报表10：假期进出京交通流量表（北京段）
        /// </summary>
        /// <returns></returns>
        public ActionResult HdayExEn()
        {
            try
            {
                HdayExEnWhereViewModel pTemp = ReportFactory.Instance.report10.GetHdayExEnWhere();
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
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
            return View();
        }
        /// <summary>
        /// 报表13：0-24时各高速公路各收费站分方向出入口小时交通量（*月**日）
        /// </summary>
        /// <returns></returns>
        public ActionResult RoadStaExEnHour()
        {
            try
            {
                HolidayConfigViewModel pHdayConfig = ReportFactory.Instance.GetHolidayConfigById(13);
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
        /// 报表13修改
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateRoadStaExEnHour(UpdateHourAADTViewModel args)
        {
            ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(13), Utility.GetFormatDate(args.DataDate, null));
            return Json(ReportFactory.Instance.report13.Update(args));
        }
        /// <summary>
        /// 报表14：各高速公路各收费站出口小时交通量
        /// </summary>
        /// <returns></returns>
        public ActionResult RoadStaExHour()
        {
            try
            {
                HolidayConfigViewModel pHdayConfig = ReportFactory.Instance.GetHolidayConfigById(13);
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
        /// 报表18：假期高速公路交通流量统计表
        /// </summary>
        /// <returns></returns>
        public ActionResult HDayRoadSta()
        {
            try
            {
                HDayAADTViewModelParaViewModel pTemp = ReportFactory.Instance.report18.GetHDayAADTPara();
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
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
            return View();
        }
        /// <summary>
        /// 报表18：假期高速公路交通流量统计表——修改功能
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateHDayRoadSta(UHDayRoadStaViewModel args)
        {
            ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(18), Utility.GetFormatDate(args.DataDate, null));

            return Json(ReportFactory.Instance.report18.Update(args));
        }
        /// <summary>
        /// 校正窗口
        /// </summary>
        /// <returns></returns>
        public ActionResult BJCheckWindow()
        {
            return View();
        }
        /// <summary>
        /// 校正窗口
        /// </summary>
        /// <returns></returns>
        public ActionResult forecastWindow()
        {
            return View();
        }
        #endregion
    }
}
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.IBLL;

namespace Wttech.DataSubmitted.Web.Controllers
{
    public class ReportController : Controller
    {
        /// <summary>
        /// 查询数据表
        /// </summary>
        /// <param name="args">参数类（查询条件），提供报表类型</param>
        /// <returns></returns>
        public JsonResult GetInfo(QueryParameters args)
        {
            var result = ReportFactory.Instance.GetListByPra(args);
            return Json(result, "text/html", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 导出报表
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public JsonResult ExportReport(QueryParameters args)
        {
            string result = ReportFactory.Instance.Export(args);
            return Json(new { path = result }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 校正数据
        /// </summary>
        /// <returns></returns>
        public JsonResult CalibrationData(QueryParameters args)
        {
            var result = ReportFactory.Instance.CalibrationData(args);
            return Json(new { ResultKey = result.ResultKey, ResultValue = result.ResultValue }, "text/html", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 无收费站数据列表
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public JsonResult NoDataList(QueryParameters args)
        {
            var result = ReportFactory.Instance.NoDataList(args);
            return Json(result, "text/html", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 预测数据
        /// </summary>
        /// <returns></returns>
        public JsonResult ForecastData(QueryParameters args)
        {
            var result = ReportFactory.Instance.ForecastData(args);
            return Json(new { ResultKey = result.ResultKey, ResultValue = result.ResultValue }, JsonRequestBehavior.AllowGet);
        }
    }
}
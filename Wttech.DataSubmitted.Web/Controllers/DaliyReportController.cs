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
    /// 日报表控制器
    /// </summary>
    public class DaliyReportController : Controller
    {
        #region 9 Public Methods
        //
        // GET: /DaliyReport/
        public ActionResult NaturalHour()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateNaturalHour(UpdateNaturalInfoViewModel args)
        {
            try
            {
                ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(15), Utility.GetFormatDate(args.DataDate, null)); 
                return Json(ReportFactory.Instance.report15.Update(args));
            }
            catch (Exception ex)
            {
                CustomResult ret = new CustomResult();
                ret.ResultKey = 0;
                ret.ResultValue = ex.Message;
                return Json(ret);
            }
        }

        [HttpPost]
        public ActionResult UpdateDYFNaturalHour(UpdateNaturalInfoViewModel args)
        {
            //站类型设为大羊坊的站编码 15
            args.StationType = 15;
            try
            {
                ReportFactory.Instance.log.WriteLog(OperationType.Update, Utility.GetReportNameByType(16), Utility.GetFormatDate(args.DataDate, null));

                return Json(ReportFactory.Instance.report15.Update(args));
            }
            catch (Exception ex)
            {
                CustomResult ret = new CustomResult();
                ret.ResultKey = 0;
                ret.ResultValue = ex.Message;
                return Json(ret);
            }
        }
        public ActionResult HourCheckRight()
        {
            return View();
        }
        public ActionResult HourNoList()
        {
            return View();
        }
        public ActionResult DYFNaturalHour()
        {
            return View();
        }
        public ActionResult HourDataReport()
        {
            return View();
        }
        #endregion
    }
}
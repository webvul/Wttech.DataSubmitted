/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/5 10:29:02
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.IBLL.Common;

#endregion

namespace Wttech.DataSubmitted.IBLL.IReport
{
    /// <summary>
    /// 报表8接口
    /// </summary>
    public interface IHDayTraStatistical : IReportQuery, IGenerateSheet, IExport, ICheck, IForecast, IUpdate<UpdateHDayTraInfoViewModel>
    {
        /// <summary>
        /// 获取预测条件默认值
        /// </summary>
        /// <returns></returns>
        ForecastViewModel GetForecastWhere();
    }
}

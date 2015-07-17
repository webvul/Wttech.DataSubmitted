/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个报表6接口文件
* 创建标识：ta0395侯兴鼎20141215
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.IBLL.Common;

namespace Wttech.DataSubmitted.IBLL.IReport
{
    /// <summary>
    /// 报表6接口
    /// </summary>
    public interface ICityDailyEnExStatistical : IReportQuery, IUpdate<UpdateEnExViewModel>, IGenerateSheet, IExport, ICheck, IForecast
    {
    }
}

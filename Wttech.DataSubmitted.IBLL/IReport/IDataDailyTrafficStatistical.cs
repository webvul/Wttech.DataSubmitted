using NPOI.SS.UserModel;
/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/26 10:14:51
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.IBLL.Common;

namespace Wttech.DataSubmitted.IBLL.IReport
{
    /// <summary>
    /// 数据报表1,2,3,4接口
    /// </summary>
    public interface IDataDailyTrafficStatistical : IReportQuery, IUpdate<UpdateDataDailyViewModel>, IGenerateSheet, IExport, ICheck, IForecast
    {
        ForecastViewModel GetForecastWhere(QueryParameters para);
    }
}

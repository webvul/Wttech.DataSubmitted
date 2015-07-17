/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/18 13:45:23
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.IBLL.Common;

#endregion

namespace Wttech.DataSubmitted.IBLL.IReport
{
    /// <summary>
    /// 报表12业务层接口
    /// </summary>
    public interface IHDayExStatistical : IReportQuery, IExport, IGenerateSheet, ICheck, IUpdate<UpdateHdayExViewModel>
    {

    }
}

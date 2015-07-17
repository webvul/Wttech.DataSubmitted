/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/16 14:11:12
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
    /// 报表13,14后台功能接口
    /// </summary>
    public interface IHourAADTStatistical : IReportQuery, IGenerateSheet, IExport, ICheck,IUpdate<UpdateHourAADTViewModel>
    { }
}

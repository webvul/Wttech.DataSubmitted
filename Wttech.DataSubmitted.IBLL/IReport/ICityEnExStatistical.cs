/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个报表5接口文件
* 创建标识：ta0395侯兴鼎20141216
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
    /// 报表5接口
    /// </summary>
    public interface ICityEnExStatistical : IReportQuery, IUpdate<UpdateEnExViewModel>, IGenerateSheet, IExport, ICheck, IForecast
    {
    }
}

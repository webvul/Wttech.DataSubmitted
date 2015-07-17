/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：报表10：假期进出京交通流量表（北京段）接口
 * 创建标识：ta0395侯兴鼎20141208
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
    /// 报表10：假期进出京交通流量表（北京段）接口
    /// </summary>
    public interface IHdayExEnTrafficStatistical : IReportQuery, IExport, IGenerateSheet
    {
        /// <summary>
        /// 获取报表10 查询条件配置默认值
        /// </summary>
        /// <returns></returns>
        HdayExEnWhereViewModel GetHdayExEnWhere();
    }
}

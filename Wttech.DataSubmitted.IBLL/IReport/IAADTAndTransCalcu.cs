/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个报表7接口文件
* 创建标识：ta0395侯兴鼎20141216
*/
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.IBLL.Common;

namespace Wttech.DataSubmitted.IBLL.IReport
{
   public interface IAADTAndTransCalcu : IReportQuery, IUpdate<AADTAndTransCalcuUViewModel>, IGenerateSheet, IExport, ICheck
    {
        /// <summary>
        /// 获取报表7查询条件配置默认值
        /// </summary>
        /// <returns></returns>
        AADTAndTransCalcuWViewModel GetHdayExEnWhere();
    }
}

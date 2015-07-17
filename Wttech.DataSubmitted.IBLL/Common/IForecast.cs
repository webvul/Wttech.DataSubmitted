/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/5 10:46:30
 */

#region 引用
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;

#endregion

namespace Wttech.DataSubmitted.IBLL.Common
{
    /// <summary>
    /// 预测接口
    /// </summary>
    public interface IForecast
    {
        #region  Methods
        /// <summary>
        /// 报表预测
        /// </summary>
        /// <param name="para">参考日期，校正日期，浮动百分比</param>
        /// <returns></returns>
        CustomResult ForecastData(QueryParameters para);
        /// <summary>
        ///  预测导出--如果需要将入出口数据进行区分，则分别放在两个list的集合中，若不需区分，则将数据放入list1中，list2为空即可
        /// </summary>
        /// <param name="readworkbook">待修改工作簿</param>
        /// <param name="para">参数类</param>
        /// <param name="list1">数据集合1</param>
        /// <param name="list2">数据集合2（可空）</param>
        /// <returns></returns>
        IWorkbook GenerateSheet(IWorkbook readworkbook, QueryParameters para, List<IReportViewModel> list1, List<IReportViewModel> list2);
        #endregion

    }
}

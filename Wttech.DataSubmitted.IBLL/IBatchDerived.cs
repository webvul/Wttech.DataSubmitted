/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个批量报表导出接口文件
* 创建标识：ta0395侯兴鼎20141103
*/
using System;
using System.Collections.Generic;
using Wttech.DataSubmitted.DAL;

namespace Wttech.DataSubmitted.IBLL
{
    /// <summary>
    /// 批量报表导出接口
    /// </summary>
    public interface IBatchDerived : ICreate<DAL.OT_ExportHis>
    {
        #region Methods

        /// <summary>
        /// 获取导出记录列表
        /// </summary>
        /// <param name="listName">假期名称集合</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">截止时间</param>
        /// <returns>导出记录列表</returns>
        List<OT_ExportHis> GetList(List<int> listName, DateTime start, DateTime end);

        /// <summary>
        /// 获取导出记录列表
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">截止时间</param>
        /// <returns>导出记录列表</returns>
        List<OT_ExportHis> GetList(DateTime start, DateTime end);

        #endregion
    }
}

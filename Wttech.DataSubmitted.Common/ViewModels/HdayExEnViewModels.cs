/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个报表10：假期进出京交通流量表（北京段）实体集合文件
* 创建标识：ta0395侯兴鼎20141208
*/
using System;
using System.Collections.Generic;

namespace Wttech.DataSubmitted.Common.ViewModels
{
    /// <summary>
    /// 报表10：假期进出京交通流量表（北京段）单条数据实体
    /// </summary>
    public class HdayExEnViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 数据日期
        /// </summary>
        public string DataDate { get; set; }

        /// <summary>
        /// 出京流量
        /// </summary>
        public Nullable<double> LineExSum { get; set; }

        /// <summary>
        /// 进京流量
        /// </summary>
        public Nullable<double> LineEnSum { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public double Total { get; set; } 

        #endregion
    }

    /// <summary>
    /// 报表10查询条件实体
    /// </summary>
    public class HdayExEnWhereViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 节假日编号
        /// </summary>
        public int HolidayId { get; set; } 

        #endregion
    }

    /// <summary>
    /// 报表10：假期进出京交通流量表（北京段）数据集合实体
    /// </summary>
    public class HdayExEnListViewModel : IReportViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 报表10数据实体集合
        /// </summary>
        public List<HdayExEnViewModel> HdayExEn { get; set; } 

        #endregion
    }
}

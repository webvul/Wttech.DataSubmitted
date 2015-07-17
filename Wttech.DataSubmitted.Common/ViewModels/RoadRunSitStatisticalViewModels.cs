/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：报表9：收费公路运行情况统计实体类文件
 * 创建标识：ta0395侯兴鼎20141209
 */
using System;
using System.Collections.Generic;

namespace Wttech.DataSubmitted.Common.ViewModels
{
    /// <summary>
    /// 报表9：收费公路运行情况统计实体类
    /// </summary>
    public class RoadRunSitViewModel : IReportViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 数据统计时间
        /// </summary>
        public string CalculTime { get; set; }

        /// <summary>
        /// 总交通量
        /// </summary>
        public double LineSum { get; set; }

        /// <summary>
        /// 总交通量同比增幅
        /// </summary>
        public string SumGrow { get; set; }

        /// <summary>
        /// 出京交通量
        /// </summary>
        public double LineExSum { get; set; }

        /// <summary>
        /// 进京交通量
        /// </summary>
        public double LineEnSum { get; set; }

        /// <summary>
        /// 出进京比
        /// </summary>
        public double ExEnPer { get; set; }

        /// <summary>
        /// 小型客车交通量
        /// </summary>
        public double SmaCarFeeNum { get; set; }

        /// <summary>
        /// 小型客车交通量同比增幅
        /// </summary>
        public string SmaCarCompGrow { get; set; }

        /// <summary>
        /// 小型客车免收通行费
        /// </summary>
        public decimal SmaCarFee { get; set; }

        /// <summary>
        /// 收费车辆
        /// </summary>
        public double ChagSumNum { get; set; } 

        #endregion
    }

    /// <summary>
    /// 报表9查询条件实体
    /// </summary>
    public class RoadRunSitWhereViewModel
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
        /// 去年同期开始日期
        /// </summary>
        public DateTime LastYearStart { get; set; }

        /// <summary>
        /// 去年同期结束日期
        /// </summary>
        public DateTime LastYearEnd { get; set; } 

        #endregion
    }

    /// <summary>
    /// 报表9数据集合实体
    /// </summary>
    public class RoadRunSitListViewModel : IReportViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 报表9：收费公路运行情况统计数据集合
        /// </summary>
        public List<RoadRunSitViewModel> RoadRunSit { get; set; } 

        #endregion
    }
}

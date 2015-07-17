/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个报表11实体类文件
* 创建标识：ta0395侯兴鼎20141219
*/
using System;
using System.Collections.Generic;

namespace Wttech.DataSubmitted.Common.ViewModels
{
    /// <summary>
    /// 报表11数据实体类
    /// </summary>
    public class HDayStaExEnViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 序号
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// 收费站名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属高速
        /// </summary>
        public string Belong { get; set; }

        /// <summary>
        /// 第1天数据
        /// </summary>
        public string Date1 { get; set; }

        /// <summary>
        /// 第2天数据
        /// </summary>
        public string Date2 { get; set; }

        /// <summary>
        /// 第3天数据
        /// </summary>
        public string Date3 { get; set; }

        /// <summary>
        /// 第4天数据
        /// </summary>
        public string Date4 { get; set; }

        /// <summary>
        /// 第5天数据
        /// </summary>
        public string Date5 { get; set; }

        /// <summary>
        /// 第6天数据
        /// </summary>
        public string Date6 { get; set; }

        /// <summary>
        /// 第7天数据
        /// </summary>
        public string Date7 { get; set; }

        /// <summary>
        /// 第8天数据
        /// </summary>
        public string Date8 { get; set; }

        /// <summary>
        /// 第9天数据
        /// </summary>
        public string Date9 { get; set; }

        /// <summary>
        /// 第10天数据
        /// </summary>
        public string Date10 { get; set; }

        /// <summary>
        /// 第11天数据
        /// </summary>
        public string Date11 { get; set; }

        /// <summary>
        /// 第12天数据
        /// </summary>
        public string Date12 { get; set; }

        /// <summary>
        /// 第13天数据
        /// </summary>
        public string Date13 { get; set; }

        /// <summary>
        /// 第14天数据
        /// </summary>
        public string Date14 { get; set; }

        /// <summary>
        /// 第15天数据
        /// </summary>
        public string Date15 { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public string Total { get; set; }

        #endregion
    }

    /// <summary>
    /// 报表11修改实体
    /// </summary>
    public class UpdateHDayStaExEnViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 报表类型
        /// </summary>
        public int ReportType { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 数据信息集合
        /// </summary>
        public List<HDayStaExEnViewModel> DataInfo { get; set; }

        #endregion
    }

    /// <summary>
    /// 报表11查询结果实体
    /// </summary>
    public class QueryHDayStaExEnViewModel : IReportViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 无数据信息列表
        /// </summary>
        public byte IsFull { get; set; }

        /// <summary>
        /// 数据总天数
        /// </summary>
        public int DateTotal { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public List<HDayStaExEnViewModel> TitleList { get; set; }

        /// <summary>
        /// 报表11数据集合
        /// </summary>
        public List<HDayStaExEnViewModel> ReportData { get; set; }

        #endregion
    }

    /// <summary>
    /// 报表11-查询条件实体
    /// </summary>
    public class WhereHDayStaExEnViewModel
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

        /// <summary>
        /// 报表类型
        /// </summary>
        public int ReportType { get; set; }

        #endregion
    }
}

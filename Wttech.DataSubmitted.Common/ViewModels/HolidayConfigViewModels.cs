/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个假期配置报送计划管理模块实体类集合类文件
* 创建标识：ta0395侯兴鼎20141125
*/
using System;
using System.Collections.Generic;

namespace Wttech.DataSubmitted.Common.ViewModels
{
    /// <summary>
    /// 假期配置实体
    /// </summary>
    public class HolidayConfigViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 假期配置编号
        /// </summary>
        public int HolidayConfigId { get; set; }

        /// <summary>
        /// 报表名称
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// 假期名称编号
        /// </summary>
        public Nullable<int> HolidayId { get; set; }

        /// <summary>
        /// 假期名称配置
        /// </summary>
        public string HolidayName { get; set; }

        /// <summary>
        /// 假期开始时间配置
        /// </summary>
        public Nullable<DateTime> HolidayStartTime { get; set; }

        /// <summary>
        /// 假期截止时间配置
        /// </summary>
        public Nullable<DateTime> HolidayEndTime { get; set; }

        /// <summary>
        /// 同比开始时间配置
        /// </summary>
        public Nullable<DateTime> ComparedStartTime { get; set; }

        /// <summary>
        /// 同比截止时间配置
        /// </summary>
        public Nullable<DateTime> ComparedEndTime { get; set; }

        /// <summary>
        /// 预测日期配置
        /// </summary>
        public Nullable<DateTime> ForecastDate { get; set; }

        /// <summary>
        /// 预测浮动百分比
        /// </summary>
        public Nullable<int> ForecastFloat { get; set; }

        /// <summary>
        /// 报表备注配置
        /// </summary>
        public string ReportRemark { get; set; }

        /// <summary>
        /// 校正浮动百分比
        /// </summary>
        public Nullable<int> CheckFloat { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 可配置项
        /// </summary>
        public string ConfigItem { get; set; } 

        #endregion
    }

    /// <summary>
    /// 假期配置实体类
    /// </summary>
    public class ConfigViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 假期配置信息集合
        /// </summary>
        public List<HolidayConfigViewModel> listConfig { get; set; } 

        #endregion
    }

    /// <summary>
    /// 假期配置日期和同比日期实体
    /// </summary>
    public class ConfigTimeViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 假期开始时间配置
        /// </summary>
        public Nullable<DateTime> HolidayStartTime { get; set; }

        /// <summary>
        /// 假期截止时间配置
        /// </summary>
        public Nullable<DateTime> HolidayEndTime { get; set; }

        /// <summary>
        /// 同比开始时间配置
        /// </summary>
        public Nullable<DateTime> ComparedStartTime { get; set; }

        /// <summary>
        /// 同比截止时间配置
        /// </summary>
        public Nullable<DateTime> ComparedEndTime { get; set; } 

        #endregion
    }

    /// <summary>
    /// 预测信息实体类
    /// </summary>
    public class ForecastViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 预测日期配置
        /// </summary>
        public Nullable<DateTime> ForecastDate { get; set; }

        /// <summary>
        /// 预测浮动百分比
        /// </summary>
        public Nullable<int> ForecastFloat { get; set; } 

        #endregion
    }

    /// <summary>
    /// 假期时间实体
    /// </summary>
    public class HolidayTimeViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 假期开始时间配置
        /// </summary>
        public string HolidayStartTime { get; set; }

        /// <summary>
        /// 假期截止时间配置
        /// </summary>
        public string HolidayEndTime { get; set; } 

        #endregion
    }
}
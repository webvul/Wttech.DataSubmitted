/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/18 16:00:43
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Wttech.DataSubmitted.Common.ViewModels
{
    /// <summary>
    /// 报表12实体
    /// </summary>
    public class HDayExViewModel
    {
        #region 3 Fields
        #endregion

        #region 4 Properties
        /// <summary>
        /// 序号
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 高速名称
        /// </summary>
        public string RoadName { get; set; }
        /// <summary>
        /// 第一天数据
        /// </summary>
        public double? Tra1 { get; set; }
        /// <summary>
        /// 第二天数据
        /// </summary>
        public double? Tra2 { get; set; }
        /// <summary>
        /// 第三天数据
        /// </summary>
        public double? Tra3 { get; set; }
        /// <summary>
        /// 第四天数据
        /// </summary>
        public double? Tra4 { get; set; }
        /// <summary>
        /// 第五天数据
        /// </summary>
        public double? Tra5 { get; set; }
        /// <summary>
        /// 第六天数据
        /// </summary>
        public double? Tra6 { get; set; }
        /// <summary>
        /// 第七天数据
        /// </summary>
        public double? Tra7 { get; set; }
        /// <summary>
        /// 第八天数据
        /// </summary>
        public double? Tra8 { get; set; }
        /// <summary>
        /// 第九天数据
        /// </summary>
        public double? Tra9 { get; set; }
        /// <summary>
        /// 第十天数据
        /// </summary>
        public double? Tra10 { get; set; }
        /// <summary>
        /// 第十一天数据
        /// </summary>
        public double? Tra11 { get; set; }
        /// <summary>
        /// 第十二天数据
        /// </summary>
        public double? Tra12 { get; set; }
        /// <summary>
        /// 第十三天数据
        /// </summary>
        public double? Tra13 { get; set; }
        /// <summary>
        /// 第十四天数据
        /// </summary>
        public double? Tra14 { get; set; }
        /// <summary>
        /// 第十五天数据
        /// </summary>
        public double? Tra15 { get; set; }
        /// <summary>
        /// 合计流量
        /// </summary>
        public double? Sum { get; set; }
        /// <summary>
        /// 去年同期总流量
        /// </summary>
        public double? LastSum { get; set; }
        /// <summary>
        /// 同比增幅
        /// </summary>
        public double? Growth { get; set; }
        #endregion
    }
    /// <summary>
    /// 报表12查询返回实体
    /// </summary>
    public class QueryHDayExViewModel : IReportViewModel
    {
        /// <summary>
        /// 天数合计
        /// </summary>
        public int CountDay { get; set; }
        /// <summary>
        /// 实体集合
        /// </summary>
        public List<HDayExViewModel> ReportData;
        /// <summary>
        /// 列名称集合
        /// </summary>
        public List<string> TitleList;
        /// <summary>
        /// 数据是否完整
        /// </summary>
        public byte IsFull;
    }
    //public class UpdateHdayExInfo
    //{
    //    /// <summary>
    //    /// 日期
    //    /// </summary>
    //    public DateTime DateTime { get; set; }
    //    /// <summary>
    //    /// 出口流量
    //    /// </summary>
    //    public double? OutTra { get; set; }
    //}
    /// <summary>
    /// 报表12修改数据实体
    /// </summary>
    public class UpdateHdayExViewModel
    {
        /// <summary>
        /// 数据开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 数据结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 修改数据集合
        /// </summary>
        public List<HDayExViewModel> DataInfo { get; set; }
    }
}

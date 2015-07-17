/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/1 16:21:28
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common.Resources;

#endregion

namespace Wttech.DataSubmitted.Common.ViewModels
{
    /// <summary>
    /// 数据日报表实体
    /// </summary>
    public class DataDailyInfoViewModel : IReportViewModel
    {
        #region 4 Properties
        /// <summary>
        /// 出口或入口
        /// </summary>
        public string ExEn
        {
            get;
            set;
        }
        /// <summary>
        /// 收费/免征金额
        /// </summary>
        public decimal? CarChag
        {
            get;
            set;
        }
        /// <summary>
        /// 车辆数
        /// </summary>
        public float? VehNum
        {
            get;
            set;
        }
        /// <summary>
        /// 车辆类型
        /// </summary>
        public string VehType
        {
            get;
            set;
        }
        /// <summary>
        ///  去年同期
        /// </summary>
        public int lastSame
        {
            get { return 0; }
            set { ;}
        }
        /// <summary>
        /// 普通公路车辆数
        /// </summary>
        public int OrdRoad
        {
            get { return 0; }
            set { ;}
        }
        /// <summary>
        /// 普通公路收费/免征金额
        /// </summary>
        public int? OrdCarChag
        {
            get { return 0; }
            set { ;}
        }
        #endregion
    }
    public class QueryDataDailyInfoViewModel : IReportViewModel
    {
        /// <summary>
        /// 实体集合
        /// </summary>
        public List<DataDailyInfoViewModel> ReportData;
        /// <summary>
        /// 数据是否完整
        /// </summary>
        public byte IsFull;
    }
    /// <summary>
    /// 修改视图模型
    /// </summary>
    public class UpdateDataDailyInfo
    {
        /// <summary>
        ///出口或入口
        /// </summary>
        public string ExEn { get; set; }
        /// <summary>
        /// 收费/免征金额
        /// </summary>
        public decimal? CarChag { get; set; }
        /// <summary>
        /// 车辆数
        /// </summary>
        public float? VehNum { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        public string VehType { get; set; }
    }
    /// <summary>
    /// 修改数据模型
    /// </summary>
    public class UpdateDataDailyViewModel
    {
        /// <summary>
        /// 报表类型
        /// </summary>
        public int ReportType { get; set; }
        /// <summary>
        /// 数据实体
        /// </summary>
        public List<UpdateDataDailyInfo> DataInfo { get; set; }
        /// <summary>
        /// 统计站类型
        /// </summary>
        public int? StationType { get; set; }
        /// <summary>
        /// 数据日期
        /// </summary>
        public DateTime DataDate { get; set; }
    }
}

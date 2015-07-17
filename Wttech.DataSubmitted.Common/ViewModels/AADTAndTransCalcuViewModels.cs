/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个报表7：黄金周京津塘高速公路交通量及客运情况统计表实体集合类文件
* 创建标识：ta0395侯兴鼎20141217
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wttech.DataSubmitted.Common.ViewModels
{
    /// <summary>
    /// 报表7：黄金周京津塘高速公路交通量及客运情况统计表实体类
    /// </summary>
    public class AADTAndTransCalcuViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 进京方向交通量
        /// </summary>
        public Nullable<double> EnTra { get; set; }

        /// <summary>
        /// 进京方向客车数
        /// </summary>
        public Nullable<double> EnCar { get; set; }

        /// <summary>
        /// 进京方向旅客量
        /// </summary>
        public Nullable<double> EnTrav { get; set; }

        /// <summary>
        /// 出京方向交通量
        /// </summary>
        public Nullable<double> ExTra { get; set; }

        /// <summary>
        /// 出京方向客车数
        /// </summary>
        public Nullable<double> ExCar { get; set; }

        /// <summary>
        /// 出京方向旅客量
        /// </summary>
        public Nullable<double> ExTrav { get; set; }

        /// <summary>
        /// 统计时间,显示用
        /// </summary>
        public string CalcuTime { get; set; }

        /// <summary>
        /// 统计时间，修改用
        /// </summary>
        public string CalcuTimeUpdate { get; set; }

        #endregion
    }

    /// <summary>
    /// 报表7查询条件实体
    /// </summary>
    public class AADTAndTransCalcuWViewModel
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
    /// 报表7修改实体
    /// </summary>
    public class AADTAndTransCalcuUViewModel
    {
        /// <summary>
        /// 报表类型
        /// </summary>
        public int ReportType { get; set; }

        /// <summary>
        /// 数据日期
        /// </summary>
        public DateTime DataDate { get; set; }

        /// <summary>
        /// 报表7数据集合
        /// </summary>
       public List<AADTAndTransCalcuViewModel> DataInfo { get; set; }
    }

    /// <summary>
    /// 报表7查询结果实体
    /// </summary>
    public class AADTAndTransCalcuQViewModel : IReportViewModel
    {
        /// <summary>
        /// 数据完整标识
        /// </summary>
        public byte IsFull { get; set; }

        /// <summary>
        /// 报表7数据集合
        /// </summary>
        public List<AADTAndTransCalcuViewModel> ReportData { get; set; }

        /// <summary>
        /// 统计人
        /// </summary>
        public string CrtBy { get; set; }
    }
}
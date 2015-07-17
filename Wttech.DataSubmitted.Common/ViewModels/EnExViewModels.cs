/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个报表5、6实体集合文件
* 创建标识：ta0395侯兴鼎20141215
*/
using System;
using System.Collections.Generic;

namespace Wttech.DataSubmitted.Common.ViewModels
{
    /// <summary>
    /// 报表5、6实体
    /// </summary>
    public class EnExViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 标识当前行数据是今年的还是去年的，今年用 “日车辆数(辆)”，去年的用“去年同期”
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 高速入境小型客车数
        /// </summary>
        public Nullable<double> EnSmaCar { get; set; }
        /// <summary>
        /// 高速入境其他客车数
        /// </summary>
        public Nullable<double> EnOthCar { get; set; }
        /// <summary>
        /// 高速入境货车数
        /// </summary>
        public Nullable<double> EnTruk { get; set; }
        /// <summary>
        /// 高速入境绿色通道数
        /// </summary>
        public Nullable<double> EnGre { get; set; }
        /// <summary>
        /// 高速出境小型客车数
        /// </summary>
        public Nullable<double> ExSmaCar { get; set; }
        /// <summary>
        /// 高速出境其他客车数
        /// </summary>
        public Nullable<double> ExOthCar { get; set; }
        /// <summary>
        /// 高速出境货车数
        /// </summary>
        public Nullable<double> ExTruk { get; set; }
        /// <summary>
        /// 高速出境绿色通道数
        /// </summary>
        public Nullable<double> ExGre { get; set; }


        /// <summary>
        /// 普通入境小型客车数
        /// </summary>
        public Nullable<double> PEnSmaCar { get; set; }
        /// <summary>
        /// 普通入境其他客车数
        /// </summary>
        public Nullable<double> PEnOthCar { get; set; }
        /// <summary>
        /// 普通入境货车数
        /// </summary>
        public Nullable<double> PEnTruk { get; set; }
        /// <summary>
        /// 普通入境绿色通道数
        /// </summary>
        public Nullable<double> PEnGre { get; set; }
        /// <summary>
        /// 普通出境小型客车数
        /// </summary>
        public Nullable<double> PExSmaCar { get; set; }
        /// <summary>
        /// 普通出境其他客车数
        /// </summary>
        public Nullable<double> PExOthCar { get; set; }
        /// <summary>
        /// 普通出境货车数
        /// </summary>
        public Nullable<double> PExTruk { get; set; }
        /// <summary>
        /// 普通出境绿色通道数
        /// </summary>
        public Nullable<double> PExGre { get; set; }


        /// <summary>
        /// 统计站类型：1：(北京段)大羊坊，马驹桥东，马驹桥西，采育33：泗村店
        /// </summary>
        public Nullable<int> StaType { get; set; }
        /// <summary>
        /// 统计时间
        /// </summary>
        public string CalcuTime { get; set; }
        ///// <summary>
        ///// 创建人
        ///// </summary>
        //public string CrtBy { get; set; }
        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CrtDate { get; set; }
        ///// <summary>
        ///// 修改人
        ///// </summary>
        //public string UpdBy { get; set; }
        ///// <summary>
        ///// 修改时间
        ///// </summary>
        //public DateTime UpdDate { get; set; }
        ///// <summary>
        ///// 数据状态，1为已修改，0为未修改
        ///// </summary>
        //public string State { get; set; } 

        #endregion
    }

    /// <summary>
    /// 报表5、6查询结果实体
    /// </summary>
    public class QueryEnExViewModel : IReportViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 数据是否完整
        /// </summary>
        public byte IsFull { get; set; }

        /// <summary>
        /// 报表5、6数据
        /// </summary>
        public List<EnExViewModel> ReportData { get; set; } 

        #endregion
    }

    /// <summary>
    /// 报表5、6修改数据模型
    /// </summary>
    public class UpdateEnExViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 报表类型
        /// </summary>
        public int ReportType { get; set; }
        /// <summary>
        /// 数据实体
        /// </summary>
        public List<EnExViewModel> DataInfo { get; set; }
        /// <summary>
        /// 统计站类型
        /// </summary>
        public int StationType { get; set; }
        /// <summary>
        /// 数据日期
        /// </summary>
        public DateTime DataDate { get; set; } 

        #endregion
    }

    /// <summary>
    /// 报表5、6预测实体
    /// </summary>
    public class ForecastEnExViewModel:IReportViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 高速入境小型客车数
        /// </summary>
        public Nullable<double> EnSmaCar { get; set; }
        /// <summary>
        /// 高速入境其他客车数
        /// </summary>
        public Nullable<double> EnOthCar { get; set; }
        /// <summary>
        /// 高速入境货车数
        /// </summary>
        public Nullable<double> EnTruk { get; set; }
        /// <summary>
        /// 高速入境绿色通道数
        /// </summary>
        public Nullable<double> EnGre { get; set; }

        /// <summary>
        /// 高速出境小型客车数
        /// </summary>
        public Nullable<double> ExSmaCar { get; set; }
        /// <summary>
        /// 高速出境其他客车数
        /// </summary>
        public Nullable<double> ExOthCar { get; set; }
        /// <summary>
        /// 高速出境货车数
        /// </summary>
        public Nullable<double> ExTruk { get; set; }
        /// <summary>
        /// 高速出境绿色通道数
        /// </summary>
        public Nullable<double> ExGre { get; set; } 

        #endregion
    }
}

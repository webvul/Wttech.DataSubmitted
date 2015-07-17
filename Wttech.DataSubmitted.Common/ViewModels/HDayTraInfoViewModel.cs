/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/5 11:13:02
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
    /// 报表8数据实体类
    /// </summary>
    public class HDayTraInfoViewModel : IReportViewModel
    {
        #region 4 Properties
        /// <summary>
        /// 序号
        /// </summary>
        public int? Num { get; set; }
        /// <summary>
        /// 路线名称
        /// </summary>
        public string RoadName { get; set; }
        /// <summary>
        /// 总结通量合计（路线）
        /// </summary>
        public double? LineSum { get; set; }
        /// <summary>
        /// 总结通量出京（路线）
        /// </summary>
        public double? LineExSum { get; set; }
        /// <summary>
        /// 总结通量进京（路线）
        /// </summary>
        public double? LineEnSum { get; set; }
        /// <summary>
        /// 免、收费总金额
        /// </summary>
        public decimal? FeeSum { get; set; }
        /// <summary>
        /// 小型客车免费通行交通量（合计）
        /// </summary>
        public double? SmaCarFeeNum { get; set; }
        /// <summary>
        /// 出京小型客车免费通行交通量
        /// </summary>
        public double? ExSmaCarFee { get; set; }
        /// <summary>
        /// 进京小型客车免费通行交通量
        /// </summary>
        public double? EnSmaCarFee { get; set; }
        /// <summary>
        /// 小型客车免费金额
        /// </summary>
        public decimal? SmaCarFee { get; set; }
        /// <summary>
        /// 收费车辆（合计）
        /// </summary>
        public double? ChagSumNum { get; set; }
        /// <summary>
        /// 出京收费车辆
        /// </summary>
        public double? ExChagNum { get; set; }
        /// <summary>
        /// 进京收费车辆
        /// </summary>
        public double? EnChagNum { get; set; }
        /// <summary>
        /// 收费额度
        /// </summary>
        public decimal? ChagAmount { get; set; }
        /// <summary>
        /// 绿色通道车辆数
        /// </summary>
        public double? GreNum { get; set; }
        /// <summary>
        /// 绿色通道免收费金额
        /// </summary>
        public decimal? GreFee { get; set; }
        /// <summary>
        /// 主收费站名称
        /// </summary>
        public string TollStaName { get; set; }
        /// <summary>
        /// 总交通量（站）合计,数据库中没有此字段，该字段等于：出京总交通量（站）+进京总交通量（站）
        /// </summary>
        public double? StaSum { get; set; }
        /// <summary>
        /// 出京总交通量（站）
        /// </summary>
        public double? StaExSum { get; set; }
        /// <summary>
        /// 进京总交通量（站）
        /// </summary>
        public double? StaEnSum { get; set; }
        /// <summary>
        /// 累计加班人次
        /// </summary>
        public int? WorkPeoNum { get; set; }
        /// <summary>
        /// 发布交通服务信息数量
        /// </summary>
        public string InfoNum { get; set; }
        /// <summary>
        /// 公路阻断和处置情况
        /// </summary>
        public string SitState { get; set; }
        #endregion
    }
    /// <summary>
    /// 查询返回实体
    /// </summary>
    public class QueryHDayTraInfoViewModel : IReportViewModel
    {
        /// <summary>
        /// 数据实体集合
        /// </summary>
        public List<HDayTraInfoViewModel> ReportData;
        /// <summary>
        /// 数据是否完整
        /// </summary>
        public byte IsFull;
    }
    /// <summary>
    /// 修改视图模型
    /// </summary>
    public class UpdateHDayTraInfoViewModel
    {
        /// <summary>
        /// 数据日期
        /// </summary>
        public DateTime DataDate { get; set; }
        /// <summary>
        /// 修改数据
        /// </summary>
        public List<UpdateHDayTraInfo> DataInfo { get; set; }
        /// <summary>
        /// 报表类型
        /// </summary>
        public int ReportType { get; set; }
        /// <summary>
        /// 统计站类型
        /// </summary>
        public int? StationType { get; set; }

    }
    /// <summary>
    /// 修改实体
    /// </summary>
    public class UpdateHDayTraInfo
    {
        #region 4 Properties
        /// <summary>
        /// 免、收费总金额
        /// </summary>
        public decimal? FeeSum { get; set; }
        /// <summary>
        /// 出京小型客车免费通行交通量
        /// </summary>
        public double? ExSmaCarFee { get; set; }
        /// <summary>
        /// 进京小型客车免费通行交通量
        /// </summary>
        public double? EnSmaCarFee { get; set; }
        /// <summary>
        /// 小型客车免费金额
        /// </summary>
        public decimal? SmaCarFee { get; set; }
        /// <summary>
        /// 出京收费车辆
        /// </summary>
        public double? ExChagNum { get; set; }
        /// <summary>
        /// 进京收费车辆
        /// </summary>
        public double? EnChagNum { get; set; }
        /// <summary>
        /// 收费额度
        /// </summary>
        public decimal? ChagAmount { get; set; }
        /// <summary>
        /// 绿色通道车辆数
        /// </summary>
        public double? GreNum { get; set; }
        /// <summary>
        /// 绿色通道免收费金额
        /// </summary>
        public decimal? GreFee { get; set; }
        /// <summary>
        /// 出京总交通量（站）
        /// </summary>
        public double? StaExSum { get; set; }
        /// <summary>
        /// 进京总交通量（站）
        /// </summary>
        public double? StaEnSum { get; set; }
        /// <summary>
        /// 累计加班人次
        /// </summary>
        public int? WorkPeoNum { get; set; }
        /// <summary>
        /// 发布交通服务信息数量
        /// </summary>
        public string InfoNum { get; set; }
        /// <summary>
        /// 公路阻断和处置情况
        /// </summary>
        public string SitState { get; set; }
        #endregion
    }
}

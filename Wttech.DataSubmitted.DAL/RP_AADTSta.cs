//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wttech.DataSubmitted.DAL
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// 交通流量表(8.9.10)
    /// </summary>
    public partial class RP_AADTSta
    {
    	/// <summary>
    	/// 标识列
    	/// </summary>
        public System.Guid Id { get; set; }
    	/// <summary>
    	/// 总交通量（路线合计）：等于报表9总交通量
    	/// </summary>
        public Nullable<double> LineSum { get; set; }
    	/// <summary>
    	/// 总交通量同比增幅
    	/// </summary>
        public Nullable<double> SumGrow { get; set; }
    	/// <summary>
    	/// 出京总交通量（路线）：等于报表9出京交通量， 等于报表10每日出京流量 
    	/// </summary>
        public Nullable<double> LineExSum { get; set; }
    	/// <summary>
    	/// 进京总交通量（路线）：等于报表9进京流量， 等于报表10每日进京流量 
    	/// </summary>
        public Nullable<double> LineEnSum { get; set; }
    	/// <summary>
    	/// 出进京比：出京交通量/进京交通量，保留两位小数
    	/// </summary>
        public Nullable<double> ExEnPer { get; set; }
    	/// <summary>
    	/// 免、收费总金额
    	/// </summary>
        public Nullable<decimal> FeeSum { get; set; }
    	/// <summary>
    	/// 小型客车免费通行交通量（合计）:等于报表9小型客车交通量
    	/// </summary>
        public Nullable<double> SmaCarFeeNum { get; set; }
    	/// <summary>
    	/// 出京小型客车免费通行交通量
    	/// </summary>
        public Nullable<double> ExSmaCarFee { get; set; }
    	/// <summary>
    	/// 进京小型客车免费通行交通量
    	/// </summary>
        public Nullable<double> EnSmaCarFee { get; set; }
    	/// <summary>
    	/// 小型客车交通量同比增幅
    	/// </summary>
        public Nullable<double> SmaCarCompGrow { get; set; }
    	/// <summary>
    	/// 小型客车免费金额：等于报表9小型客车免收通行费
    	/// </summary>
        public Nullable<decimal> SmaCarFee { get; set; }
    	/// <summary>
    	/// 收费车辆（合计）：等于报表9收费车辆
    	/// </summary>
        public Nullable<double> ChagSumNum { get; set; }
    	/// <summary>
    	/// 出京收费车辆
    	/// </summary>
        public Nullable<double> ExChagNum { get; set; }
    	/// <summary>
    	/// 进京收费车辆
    	/// </summary>
        public Nullable<double> EnChagNum { get; set; }
    	/// <summary>
    	/// 收费额度
    	/// </summary>
        public Nullable<decimal> ChagAmount { get; set; }
    	/// <summary>
    	/// 绿色通道车辆数
    	/// </summary>
        public Nullable<double> GreNum { get; set; }
    	/// <summary>
    	/// 绿色通道免收费金额
    	/// </summary>
        public Nullable<decimal> GreFee { get; set; }
    	/// <summary>
    	/// 出京总交通量（站）
    	/// </summary>
        public Nullable<double> StaExSum { get; set; }
    	/// <summary>
    	/// 进京总交通量（站）
    	/// </summary>
        public Nullable<double> StaEnSum { get; set; }
    	/// <summary>
    	/// 累计加班值班（人次）：由中心值班人员按实际发生情况手工填写
    	/// </summary>
        public Nullable<int> WorkPeoNum { get; set; }
    	/// <summary>
    	/// 发布交通服务信息数量：由中心值班人员按实际发生情况手工填写
    	/// </summary>
        public string InfoNum { get; set; }
    	/// <summary>
    	/// 公路阻断和处置情况：由中心值班人员按实际发生情况手工填写
    	/// </summary>
        public string SitState { get; set; }
    	/// <summary>
    	/// 统计时间
    	/// </summary>
        public System.DateTime CalculTime { get; set; }
    	/// <summary>
    	/// 创建人
    	/// </summary>
        public string CrtBy { get; set; }
    	/// <summary>
    	/// 创建日期
    	/// </summary>
        public Nullable<System.DateTime> CrtDate { get; set; }
    	/// <summary>
    	/// 修改人
    	/// </summary>
        public string UpdBy { get; set; }
    	/// <summary>
    	/// 修改日期
    	/// </summary>
        public Nullable<System.DateTime> UpdDate { get; set; }
    	/// <summary>
    	/// 数据状态：1为已修改，0为未修改
    	/// </summary>
        public string State { get; set; }
    }
}

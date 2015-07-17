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
    /// 数据日报表(1.2.3.4)
    /// </summary>
    public partial class RP_Daily
    {
    	/// <summary>
    	/// 标识列
    	/// </summary>
        public System.Guid Id { get; set; }
    	/// <summary>
    	/// 车型
    	/// </summary>
        public Nullable<int> VehType { get; set; }
    	/// <summary>
    	/// 出口车辆数
    	/// </summary>
        public Nullable<double> OutNum { get; set; }
    	/// <summary>
    	/// 入口车辆数
    	/// </summary>
        public Nullable<double> InNum { get; set; }
    	/// <summary>
    	/// 收费/免征金额
    	/// </summary>
        public Nullable<decimal> ChagFee { get; set; }
    	/// <summary>
    	/// 统计报表类型 1：（北京段）大羊坊，马驹桥东，马驹桥西，采育 3：（天津段）泗村店、杨村、宜兴埠、金钟路、机场、空港、塘沽西、塘沽 15：大羊坊 33：泗村店 
    	/// </summary>
        public Nullable<int> StaType { get; set; }
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
        public Nullable<byte> State { get; set; }
    	/// <summary>
    	/// 数据时间
    	/// </summary>
        public System.DateTime CalcuTime { get; set; }
    }
}

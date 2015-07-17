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
    /// 中间表：将需要统计的信息从目标数据库中获取之后存到该中间表，以后以从中间表获取数据的方式进行统计提高系统性能
    /// </summary>
    public partial class DS_DataSource
    {
    	/// <summary>
    	/// 标识列
    	/// </summary>
        public System.Guid Id { get; set; }
    	/// <summary>
    	/// 收费站ID
    	/// </summary>
        public Nullable<int> StaID { get; set; }
    	/// <summary>
    	/// 车型： 0表示对应类型的合计
    	/// </summary>
        public Nullable<int> VehType { get; set; }
    	/// <summary>
    	/// MTC出口车辆数（辆）
    	/// </summary>
        public Nullable<int> OutNumMTC { get; set; }
    	/// <summary>
    	/// MTC入口车辆数（辆）
    	/// </summary>
        public Nullable<int> InNumMTC { get; set; }
    	/// <summary>
    	/// ETC出口车辆数（辆）
    	/// </summary>
        public Nullable<int> OutNumETC { get; set; }
    	/// <summary>
    	/// ETC入口车辆数（辆）
    	/// </summary>
        public Nullable<int> InNumETC { get; set; }
    	/// <summary>
    	/// 出口数量数（辆）
    	/// </summary>
        public Nullable<int> OutNum { get; set; }
    	/// <summary>
    	/// 入口数量数（辆）
    	/// </summary>
        public Nullable<int> InNum { get; set; }
    	/// <summary>
    	/// 应收金额
    	/// </summary>
        public Nullable<decimal> RecMoney { get; set; }
    	/// <summary>
    	/// 实收金额
    	/// </summary>
        public Nullable<decimal> PayMoney { get; set; }
    	/// <summary>
    	/// 时间段（小时）
    	/// </summary>
        public Nullable<byte> HourPer { get; set; }
    	/// <summary>
    	/// 统计日期
    	/// </summary>
        public Nullable<System.DateTime> CalcuTime { get; set; }
    	/// <summary>
    	/// 统计类型：0为小型客车，1为其他客车，2为货车（不包含绿通），3为绿通 ；当VehType=0&&StatisticalType in （0,1,2,3）此数据表示合计数据，当VehType！=0时，表示分车型数据
    	/// </summary>
        public Nullable<byte> CalcuType { get; set; }
    	/// <summary>
    	/// 
    	/// </summary>
        public Nullable<System.DateTime> CrtDate { get; set; }
    }
}

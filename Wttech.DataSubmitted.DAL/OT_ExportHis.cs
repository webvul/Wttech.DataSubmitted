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
    /// 假期导出文件记录表：记录每个类型报表导出的相关信息
    /// </summary>
    public partial class OT_ExportHis
    {
    	/// <summary>
    	/// 标识列
    	/// </summary>
        public System.Guid Id { get; set; }
    	/// <summary>
    	/// 报表类型
    	/// </summary>
        public Nullable<int> TableType { get; set; }
    	/// <summary>
    	/// 假期名称
    	/// </summary>
        public Nullable<int> HDayId { get; set; }
    	/// <summary>
    	/// 文件保存路径
    	/// </summary>
        public string SavePath { get; set; }
    	/// <summary>
    	/// 数据时间段
    	/// </summary>
        public Nullable<byte> HourPer { get; set; }
    	/// <summary>
    	/// 数据日期
    	/// </summary>
        public Nullable<System.DateTime> DataDate { get; set; }
    	/// <summary>
    	/// 统计时间
    	/// </summary>
        public System.DateTime CalcuTime { get; set; }
    
        public virtual OT_Dic OT_Dic { get; set; }
    }
}

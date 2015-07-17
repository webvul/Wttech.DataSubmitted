﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DataSubmittedEntities : DbContext
    {
        public DataSubmittedEntities()
            : base("name=DataSubmittedEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DS_DataSource> DS_DataSource { get; set; }
        public virtual DbSet<OT_Dic> OT_Dic { get; set; }
        public virtual DbSet<OT_ErrorStation> OT_ErrorStation { get; set; }
        public virtual DbSet<OT_ExportHis> OT_ExportHis { get; set; }
        public virtual DbSet<OT_HDayConfig> OT_HDayConfig { get; set; }
        public virtual DbSet<OT_Log> OT_Log { get; set; }
        public virtual DbSet<OT_Role> OT_Role { get; set; }
        public virtual DbSet<OT_Station> OT_Station { get; set; }
        public virtual DbSet<OT_User> OT_User { get; set; }
        public virtual DbSet<OT_UserRole> OT_UserRole { get; set; }
        public virtual DbSet<RP_AADTAndTransCalcu> RP_AADTAndTransCalcu { get; set; }
        public virtual DbSet<RP_AADTSta> RP_AADTSta { get; set; }
        public virtual DbSet<RP_Daily> RP_Daily { get; set; }
        public virtual DbSet<RP_EnEx> RP_EnEx { get; set; }
        public virtual DbSet<RP_HDayAADT> RP_HDayAADT { get; set; }
        public virtual DbSet<RP_HDayAADTSta> RP_HDayAADTSta { get; set; }
        public virtual DbSet<RP_HourAADT> RP_HourAADT { get; set; }
        public virtual DbSet<RP_NatSta> RP_NatSta { get; set; }
    
        public virtual int SP_LinkServer(string serverIp, string userName, string passWord)
        {
            var serverIpParameter = serverIp != null ?
                new ObjectParameter("ServerIp", serverIp) :
                new ObjectParameter("ServerIp", typeof(string));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var passWordParameter = passWord != null ?
                new ObjectParameter("PassWord", passWord) :
                new ObjectParameter("PassWord", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_LinkServer", serverIpParameter, userNameParameter, passWordParameter);
        }
    
        public virtual int SP_StaDataSource(Nullable<int> timePeriodHourPra, Nullable<System.DateTime> staticalTimePra)
        {
            var timePeriodHourPraParameter = timePeriodHourPra.HasValue ?
                new ObjectParameter("timePeriodHourPra", timePeriodHourPra) :
                new ObjectParameter("timePeriodHourPra", typeof(int));
    
            var staticalTimePraParameter = staticalTimePra.HasValue ?
                new ObjectParameter("staticalTimePra", staticalTimePra) :
                new ObjectParameter("staticalTimePra", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_StaDataSource", timePeriodHourPraParameter, staticalTimePraParameter);
        }
    }
}

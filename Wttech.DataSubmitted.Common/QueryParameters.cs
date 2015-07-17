/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/11 13:54:18
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Wttech.DataSubmitted.Common
{
    /// <summary>
    /// 查询条件类
    /// </summary>
    public class QueryParameters
    {
        #region 2 Static Fields
        private static QueryParameters queryParameters = null;
        #endregion

        #region 3 Fields
        private DateTime? starttime;
        private DateTime? endtime;
        private int? holidayid;
        private DateTime? lastyearstart;
        private DateTime? lastyearend;
        private int reporttype;
        private int? stationtype;
        private double floatingrange;
        private int starttimehour;
        private int endtimehour;
        private int? isforecast;
        private int exporttype;
        private int startyear;
        private int endyear;
        #endregion

        #region 4 Properties
        /// <summary>
        /// 批量导出类型，1表示假期报表导出，2表示年度报表导出
        /// </summary>
        public int ExportType
        {
            get { return exporttype; }
            set { exporttype = value; }
        }
        /// <summary>
        /// 批量导出假期集合
        /// </summary>
        public List<int> HDayIdList { get; set; }
        /// <summary>
        /// 开始年份
        /// </summary>
        public int StartYear
        {
            get { return startyear; }
            set { startyear = value; }
        }
        /// <summary>
        /// 结束年份
        /// </summary>
        public int EndYear
        {
            get { return endyear; }
            set { endyear = value; }
        }
        /// <summary>
        /// 是否是预测导出0：不是，1：是
        /// </summary>
        public int? IsForecast
        {
            get { return isforecast; }
            set { isforecast = value; }
        }
        /// <summary>
        /// 开始时间段
        /// </summary>
        public int StartHour
        {
            get { return starttimehour; }
            set { starttimehour = value; }
        }
        /// <summary>
        /// 结束时间段
        /// </summary>
        public int EndHour
        {
            get { return endtimehour; }
            set { endtimehour = value; }
        }
        /// <summary>
        /// 校正浮动范围
        /// </summary>
        public double FloatingRange
        {
            get { return floatingrange; }
            set { floatingrange = value; }
        }
        /// <summary>
        /// 统计站类型
        /// </summary>
        public int? StationType
        {
            get { return stationtype; }
            set { stationtype = value; }
        }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartTime
        {
            get { return starttime; }
            set { starttime = value; }
        }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndTime
        {
            get { return endtime; }
            set { endtime = value; }
        }
        /// <summary>
        /// 假期名称
        /// </summary>
        public int? HolidayId
        {
            get { return holidayid; }
            set { holidayid = value; }
        }
        /// <summary>
        /// 去年同期开始日期
        /// </summary>
        public DateTime? LastYearStart
        {
            get { return lastyearstart; }
            set { lastyearstart = value; }
        }
        /// <summary>
        /// 去年同期结束日期
        /// </summary>
        public DateTime? LastYearEnd
        {
            get { return lastyearend; }
            set { lastyearend = value; }
        }
        /// <summary>
        /// 报表类型
        /// </summary>
        public int ReportType
        {
            get { return reporttype; }
            set { reporttype = value; }
        }
        #endregion

        #region 10 Static Methods
        public static QueryParameters GetInstance()
        {
            if (queryParameters == null)
                queryParameters = new QueryParameters();
            return queryParameters;
        }
        #endregion
    }
}

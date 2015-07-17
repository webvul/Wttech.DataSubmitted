/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个工具类文件
* 创建标识：ta0351王旋20141121
*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wttech.DataSubmitted.Common
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class Utility
    {

        #region 10 Static Methods
        private static string config;
        /// <summary>
        /// 获取下载路径
        /// </summary>
        /// <returns></returns>
        public static string GetDownLoadPath()
        {
            try
            {
                if (string.IsNullOrEmpty(config))
                {
                    config = ConfigurationManager.AppSettings["path"];
                }
                if (string.IsNullOrEmpty(config))
                {
                    config = AppDomain.CurrentDomain.BaseDirectory + "ReportDownLoad";
                }
                return config;
            }
            catch
            {
                return AppDomain.CurrentDomain.BaseDirectory + "ReportDownLoad";
            }
        }
        /// <summary>
        /// 通过报表类型获取报表名称
        /// </summary>
        /// <param name="reportType">报表类型</param>
        /// <returns></returns>
        public static string GetReportNameByType(int reportType)
        {
            try
            {
                return Resources.SystemConst.ResourceManager.GetString("ReportName" + reportType.ToString());
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 通过日期获取格式化后的日期
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns></returns>
        public static string GetFormatDate(DateTime? startTime, DateTime? endTime)
        {
            if (startTime != null)
            {
                return startTime.Value.ToString("yyyy-MM-dd") + (endTime == null ? "" : " - " + endTime.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        /// <summary>
        /// 获取批量导出允许的年份
        /// </summary>
        /// <returns></returns>
        public static List<int> GetExportHasYear()
        {
            List<int> pYearList = new List<int>();
            try
            {
                using (DataSubmitted.DAL.DataSubmittedEntities db = new DAL.DataSubmittedEntities())
                {
                    int pMax = db.OT_ExportHis.Max(s => s.DataDate.Value.Year);
                    int pMin = db.OT_ExportHis.Min(s => s.DataDate.Value.Year);
                    for (int i = pMin; i <= pMax; i++)
                    {
                        pYearList.Add(i);
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
            return pYearList;
        }
        #endregion
    }
}

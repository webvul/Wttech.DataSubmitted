/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个日志实体文件
* 创建标识：ta0395侯兴鼎20141203
*/
using System.Collections.Generic;

namespace Wttech.DataSubmitted.Common.ViewModels
{
    /// <summary>
    /// 日志查询条件实体
    /// </summary>
    public class LogQueryViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 起始时间
        /// </summary>
        public System.DateTime StartDate { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        public System.DateTime EndDate { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 报表名称
        /// </summary>
        public string RptName { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int PageSize { get; set; } 

        #endregion

    }

    /// <summary>
    /// 日志信息实体类
    /// </summary>
    public class LogManageViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public System.DateTime LogDate { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 操作明细
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 报表名称
        /// </summary>
        public string RptName { get; set; }

        /// <summary>
        /// 操作用户
        /// </summary>
        public string UserName { get; set; } 

        #endregion
    }

    public class LogPageViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 日志信息集合
        /// </summary>
        public List<LogManageViewModel> LogList { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int Count { get; set; } 

        #endregion
    }
}

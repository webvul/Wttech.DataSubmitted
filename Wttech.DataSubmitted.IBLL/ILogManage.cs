/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个日志管理接口类文件
* 创建标识：ta0395侯兴鼎20141106
*/
using System;
using System.Collections.Generic;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.DAL;

namespace Wttech.DataSubmitted.IBLL
{
    /// <summary>
    /// 日志管理接口
    /// </summary>
    public interface ILogManage : ICreate<OT_Log>
    {
        #region Methods

        /// <summary>
        /// 日志管理
        /// </summary>
        /// <param name="db">数据库实体</param>
        LogPageViewModel GetList(LogQueryViewModel model);


        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <param name="report">报表名称</param>
        /// <param name="dt">时间(yyyy-MM-dd)或时间范围(yyyy-MM-dd - yyyy-MM-dd)</param>
        /// <returns></returns>
        byte WriteLog(String type, String report, String dt);
              /// <summary>
        /// 写入日志,非报表类操作
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <param name="describe">操作描述</param>
        /// <returns></returns>
        byte WriteLog(String type, string describe);

        #endregion
    }
}
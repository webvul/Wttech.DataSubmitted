/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/12 9:52:15
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Wttech.DataSubmitted.IBLL.IReport
{
    /// <summary>
    /// 观察者接口，每个报表在需要统计数据时，都要继承此接口
    /// </summary>
    public interface IObserver
    {
        #region  Properties
        /// <summary>
        /// 统计报表类型名称
        /// </summary>
        string ReportName { get; set; }
        #endregion

        #region  Methods
        /// <summary>
        /// 更新数据方法
        /// </summary>
        /// <param name="dt">数据日期</param>
        /// <param name="HourPer">数据时间段,小时报数据为时间段，否则为-1</param>
        void Update(DateTime dt, int HourPer);
        #endregion
    }
}

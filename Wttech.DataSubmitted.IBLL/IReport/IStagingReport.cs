/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/25 20:42:02
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
    /// 中间表统计接口
    /// </summary>
    public interface IStagingReport
    {
        #region  Methods
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pDt"></param>
        /// <param name="pHourPer"></param>
        void UpdateData(DateTime pDt, int pHourPer);
        #endregion


    }
}

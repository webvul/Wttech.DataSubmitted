/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/21 9:19:48
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common;

#endregion

namespace Wttech.DataSubmitted.IBLL.Common
{
    /// <summary>
    /// 数据校正接口
    /// </summary>
    public interface ICheck
    {
        #region Methods

        /// <summary>
        /// 校正数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        CustomResult CalibrationData(QueryParameters para);
        #endregion
    }
}

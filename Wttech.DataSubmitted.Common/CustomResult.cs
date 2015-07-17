/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/21 14:56:34
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
    /// 自定义键值类
    /// </summary>
    public class CustomResult
    {

        #region 4 Properties
        /// <summary>
        /// 键
        /// </summary>
        public byte ResultKey { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string ResultValue { get; set; }
        #endregion

    }
}

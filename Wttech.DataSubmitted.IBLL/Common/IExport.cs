/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个导出接口文件
* 创建标识：ta0395侯兴鼎20141030
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common;
namespace Wttech.DataSubmitted.IBLL.Common
{
    /// <summary>
    /// 导出接口
    /// </summary>
    /// <typeparam name="T">数据集合</typeparam>
    public interface IExport
    {
        #region Methods
        /// <summary>
        /// 导出excel表
        /// </summary>
        /// <param name="para">导出条件</param>
        /// <returns></returns>
        string ExportReport(QueryParameters para);
        #endregion
    }
}

/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/12 15:07:10
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using Wttech.DataSubmitted.Common;
using NPOI.SS.UserModel;
#endregion

namespace Wttech.DataSubmitted.IBLL.Common
{
    /// <summary>
    /// 修改数据表内容接口，所有的数据类都必须继承此接口
    /// </summary>
    public interface IGenerateSheet
    {
        #region 9 Public Methods
        /// <summary>
        /// 修改数据表的内容
        /// </summary>
        /// <param name="readworkbook"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        IWorkbook GenerateSheet(IWorkbook readworkbook, QueryParameters para);
        #endregion
    }
}

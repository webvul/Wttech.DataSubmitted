/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/13 9:46:06
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;

#endregion

namespace Wttech.DataSubmitted.IBLL.Common
{
    public interface IReportQuery
    {
        #region  Methods
        /// <summary>
        /// 根据查询条件获取数据
        /// </summary>
        /// <param name="para">查询条件类</param>
        /// <returns></returns>
        IReportViewModel GetListByPra(QueryParameters para);
        #endregion


    }
}

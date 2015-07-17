/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/26 13:34:08
 */

#region 引用
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.IBLL.IReport;

#endregion

namespace Wttech.DataSubmitted.IBLL
{
    public class ServicesFactory
    {

        #region 4 Properties

        [Dependency]
        public IStagingReport stagingReport { get; set; }

        public static ServicesFactory Instance { get; set; }

        #endregion
    }
}

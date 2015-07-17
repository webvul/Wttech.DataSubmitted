/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/18 16:31:32
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.DAL;

#endregion

namespace Wttech.DataSubmitted.IBLL.IReport
{
    /// <summary>
    /// 无收费站列表接口
    /// </summary>
    public interface INoAcceptStationRecord : IQuery<List<OT_ErrorStation>>
    {

    }
}

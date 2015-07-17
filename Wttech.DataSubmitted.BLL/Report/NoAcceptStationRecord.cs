/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/18 16:33:07
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL;
using Wttech.DataSubmitted.IBLL.IReport;

#endregion

namespace Wttech.DataSubmitted.BLL.Report
{
    public class NoAcceptStationRecord : INoAcceptStationRecord
    {
        #region 1 Const
        #endregion

        #region 2 Static Fields
        #endregion

        #region 3 Fields
        #endregion

        #region 4 Properties
        #endregion

        #region 5 Constructors
        #endregion

        #region 6 Delegate
        #endregion

        #region 7 Define Event
        #endregion

        #region 8 Event Methods
        #endregion

        #region 9 Public Methods
        public List<OT_ErrorStation> GetList()
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                return db.OT_ErrorStation.ToList();
            }

        }
        #endregion

        #region 10 Static Methods
        #endregion

        #region 11 Private Methods
        #endregion

        #region 12 Protected Methods
        #endregion
    }
}

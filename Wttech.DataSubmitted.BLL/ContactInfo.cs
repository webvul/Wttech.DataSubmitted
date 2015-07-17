/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/13 13:13:59
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL;
#endregion

namespace Wttech.DataSubmitted.BLL
{
    /// <summary>
    /// 导出文件记录类
    /// </summary>
    class ContactInfo : IContactInfo
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
        /// <summary>
        /// 添加文件导出记录
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public byte Create(OT_ExportHis args)
        {
            byte num;
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    db.OT_ExportHis.Add(args);
                    num = (byte)db.SaveChanges();
                    transaction.Complete();
                }
            }
            return num;
        }
        /// <summary>
        /// 获取文件导出记录
        /// </summary>
        /// <returns></returns>
        public List<OT_ExportHis> GetList()
        {
            throw new ArgumentNullException();
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

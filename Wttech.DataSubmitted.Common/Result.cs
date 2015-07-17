using System;
/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个数据提交结果类文件
* 创建标识：ta0395侯兴鼎20141107
*/
using System.Transactions;
using Wttech.DataSubmitted.Common.Resources;
using Wttech.DataSubmitted.DAL;

namespace Wttech.DataSubmitted.Common
{
    /// <summary>
    /// 数据提交结果
    /// </summary>
    public static class Result
    {
        #region 10 Static Methods

        /// <summary>
        /// 数据提交结果-新增
        /// </summary>
        /// <param name="db">提交数据库对象</param>
        /// <returns>提交结果</returns>
        public static CustomResult SaveChangesResult(DataSubmittedEntities db)
        {
            CustomResult pReturnValue = new CustomResult();
            if (db.SaveChanges() > 0)
            {
                pReturnValue.ResultKey = (byte)EResult.Succeed;
                pReturnValue.ResultValue = Resources.TipInfo.AddSuccess;
            }
            else
            {
                pReturnValue.ResultKey = (byte)EResult.Fail;
                pReturnValue.ResultValue = Resources.TipInfo.AddFaile;
            }
            return pReturnValue;
        }

        /// <summary>
        /// 返回带事物的提交结果-新增
        /// </summary>
        /// <param name="db">提交数据库对象</param>
        /// <param name="transaction">事物</param>
        /// <returns>提交结果</returns>
        public static CustomResult SaveChangesResult(DataSubmittedEntities db, TransactionScope transaction)
        {
            CustomResult pReturnValue = new CustomResult();
            if (db.SaveChanges() > 0)
            {
                transaction.Complete();
                pReturnValue.ResultKey = (byte)EResult.Succeed;
                pReturnValue.ResultValue = Resources.TipInfo.AddSuccess;
            }
            else
            {
                pReturnValue.ResultKey = (byte)EResult.Fail;
                pReturnValue.ResultValue = Resources.TipInfo.AddFaile;
            }
            return pReturnValue;
        }

        /// <summary>
        /// 数据提交结果-删除
        /// </summary>
        /// <param name="db">提交数据库对象</param>
        /// <returns>提交结果</returns>
        public static CustomResult DelChangesResult(DataSubmittedEntities db)
        {
            CustomResult pReturnValue = new CustomResult();
            if (db.SaveChanges() > 0)
            {
                pReturnValue.ResultKey = (byte)EResult.Succeed;
                pReturnValue.ResultValue = Resources.TipInfo.DeleteSuccess;
            }
            else
            {
                pReturnValue.ResultKey = (byte)EResult.Fail;
                pReturnValue.ResultValue = Resources.TipInfo.DeleteFaile;
            }
            return pReturnValue;
        }

        /// <summary>
        /// 返回带事物的提交结果-删除
        /// </summary>
        /// <param name="db">提交数据库对象</param>
        /// <param name="transaction">事物</param>
        /// <returns>提交结果</returns>
        public static CustomResult DelChangesResult(DataSubmittedEntities db, TransactionScope transaction)
        {
            CustomResult pReturnValue = new CustomResult();
            if (db.SaveChanges() > 0)
            {
                transaction.Complete();
                pReturnValue.ResultKey = (byte)EResult.Succeed;
                pReturnValue.ResultValue = Resources.TipInfo.DeleteSuccess;
            }
            else
            {
                pReturnValue.ResultKey = (byte)EResult.Fail;
                pReturnValue.ResultValue = Resources.TipInfo.DeleteFaile;
            }
            return pReturnValue;
        }

        /// <summary>
        /// 数据提交结果-修改使用
        /// </summary>
        /// <param name="db">提交数据库对象</param>
        /// <returns>提交结果</returns>
        public static CustomResult SaveUpdateResult(DataSubmittedEntities db)
        {
            CustomResult pReturnValue = new CustomResult();
            try
            {
                db.SaveChanges();
                pReturnValue.ResultKey = (byte)EResult.Succeed;
                pReturnValue.ResultValue = Resources.TipInfo.UpdateSuccess;
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Error(TipInfo.UpdateFaile, ex);
                pReturnValue.ResultKey = (byte)EResult.Fail;//程序已经使用多处，所有不变动
                pReturnValue.ResultValue = Resources.TipInfo.UpdateFaile;
            }
            return pReturnValue;
        }

        /// <summary>
        /// 返回带事物的提交结果-修改使用
        /// </summary>
        /// <param name="db">提交数据库对象</param>
        /// <param name="transaction">事物</param>
        /// <returns>提交结果</returns>
        public static CustomResult SaveUpdateResult(DataSubmittedEntities db, TransactionScope transaction)
        {
            CustomResult pReturnValue = new CustomResult();
            try
            {
                db.SaveChanges();
                transaction.Complete();
                pReturnValue.ResultKey = (byte)EResult.Succeed;
                pReturnValue.ResultValue = Resources.TipInfo.UpdateSuccess;
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Error(TipInfo.UpdateFaile, ex);
                pReturnValue.ResultKey = (byte)EResult.Fail;//程序已经使用多处，所有不变动
                pReturnValue.ResultValue = Resources.TipInfo.UpdateFaile;
            }
            return pReturnValue;
        }

        #endregion
    }
}

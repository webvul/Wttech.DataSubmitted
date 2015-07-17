/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个日志管理类文件
* 创建标识：ta0395侯兴鼎20141106
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL;

namespace Wttech.DataSubmitted.BLL
{
    /// <summary>
    /// 日志管理类
    /// </summary>
    public class LogManage : ILogManage
    {
        #region 3 Fields

        DataSubmittedEntities db;

        #endregion

        #region 5 Constructors

        /// <summary>
        /// 日志管理
        /// </summary>
        /// <param name="db">数据库实体</param>
        public LogManage(DataSubmittedEntities db)
        {
            this.db = db;
        }

        #endregion

        #region 9 Public Methods

        /// <summary>
        /// 获取日志信息
        /// </summary>
        /// <param name="model">查询条件，日志信息实体，logDate存起始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <returns></returns>
        public LogPageViewModel GetList(LogQueryViewModel model)
        {
            var pLogList = db.OT_Log.Where(a => true);

            //按操作用户查询
            if (!string.IsNullOrEmpty(model.UserName))
                pLogList = pLogList.Where(a => a.UserName.Contains(model.UserName));

            //按操作类型查询,如果类型为所有则不增加改条件
            if (!string.IsNullOrEmpty(model.Type) && model.Type != Common.OperationType.All)
                pLogList = pLogList.Where(a => a.Type == model.Type);

            //按报表名称查询
            if (!string.IsNullOrEmpty(model.RptName))
                pLogList = pLogList.Where(a => a.RptName.Contains(model.RptName));

            //某时间段之间的所有日志
            if (model.EndDate != DateTime.Parse("0001/1/1 0:00:00") && model.StartDate != DateTime.Parse("0001/1/1 0:00:00") && model.EndDate != null && model.StartDate != null && model.StartDate <= model.EndDate)
            {
                DateTime dt=model.EndDate.AddSeconds(1);
                pLogList = pLogList.Where(a => a.LogDate >= model.StartDate & a.LogDate <=dt );

            }
            //大于某时间的所日志
            else if (model.StartDate != null && model.StartDate != DateTime.Parse("0001/1/1 0:00:00"))
            {
                pLogList = pLogList.Where(a => a.LogDate >= model.StartDate);
            }
            //小于某时间的所日志
            else if (model.EndDate != null && model.EndDate != DateTime.Parse("0001/1/1 0:00:00"))
            {
                DateTime dt = model.EndDate.AddSeconds(1);
                pLogList = pLogList.Where(a => a.LogDate <= dt);
            }

            int pageCount, count;//总页数，总行数

            //获取数据总条数
            count = pLogList.Count();

            //获取总页数
            pageCount = count % model.PageSize > 0 ? count / model.PageSize + 1 : count / model.PageSize;

            //如果页数大于总页数，或者小于1，则返回第一页
            if (model.PageIndex > pageCount || model.PageIndex < 1)
                model.PageIndex = 1;

            List<LogManageViewModel> list;
            //获取日志信息
            if (model.PageIndex == 1)
            {
                list = pLogList.OrderByDescending(a => a.LogDate).Take(model.PageSize).Select(a => new LogManageViewModel
                {
                    RoleName = a.RoleName,
                    LogDate = a.LogDate,
                    Type = a.Type,
                    Describe = a.Describe,
                    RptName = a.RptName,
                    UserName = a.UserName
                }).ToList();
            }
            else
            {
                int excludedRows = (model.PageIndex - 1) * model.PageSize;//计算起始索引

                list = pLogList.OrderByDescending(a => a.LogDate).Skip(excludedRows).Take(model.PageSize).Select(a => new LogManageViewModel
                {
                    RoleName = a.RoleName,
                    LogDate = a.LogDate,
                    Type = a.Type,
                    Describe = a.Describe,
                    RptName = a.RptName,
                    UserName = a.UserName
                }).ToList();
            }

            //构建分页日志信息并返回
            LogPageViewModel lpModel = new LogPageViewModel();
            lpModel.Count = count;
            lpModel.PageCount = pageCount;
            lpModel.PageIndex = model.PageIndex;
            lpModel.LogList = list;

            return lpModel;
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="model">日志实体</param>
        /// <returns>添加结果</returns>
        public byte Create(OT_Log model)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                db.OT_Log.Add(model);
                return Result.SaveChangesResult(db, transaction).ResultKey;
            }
        }

        /// <summary>
        /// 写入日志，报表类操作
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <param name="report">报表名称</param>
        /// <param name="dt">时间(yyyy-MM-dd)或时间范围(yyyy-MM-dd - yyyy-MM-dd)</param>
        /// <returns></returns>
        public byte WriteLog(String type, String report,String dt)
        {
            OT_Log pTemp = new OT_Log();
            try
            {
                var pUser = SessionManage.GetLoginUser();
                if (pUser != null)
                {
                    pTemp.Id = Guid.NewGuid();
                    pTemp.LogDate = DateTime.Now;
                    pTemp.Type = type;
                    pTemp.UserName = pUser.UserName;
                    if(type!=OperationType.Login&&type!=OperationType.LoginOut)
                    {
                        pTemp.RptName = report;
                       pTemp.Describe = String.Format("{0}{1}{2}", type, dt, report);
                    }
                    if (pUser.RoleList.Count > 0 && pUser.RoleList[0] != null)
                    {
                        pTemp.RoleName = pUser.RoleList[0].RoleName;
                    }
                    return (Create(pTemp));
                }
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Error("插入操作日志失败!", ex);
            }
            return (byte)EResult.Fail;
        }

        /// <summary>
        /// 写入日志,非报表类操作
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <param name="describe">操作描述</param>
        /// <returns></returns>
        public byte WriteLog(String type, string describe)
        {
            OT_Log pTemp = new OT_Log();
            try
            {
                var pUser = SessionManage.GetLoginUser();
                if (pUser != null)
                {
                    pTemp.Id = Guid.NewGuid();
                    pTemp.LogDate = DateTime.Now;
                    pTemp.Type = type;
                    pTemp.UserName = pUser.UserName;
                    pTemp.RptName = string.Empty;
                    pTemp.Describe = describe;

                    if (pUser.RoleList.Count > 0 && pUser.RoleList[0] != null)
                    {
                        pTemp.RoleName = pUser.RoleList[0].RoleName;
                    }
                    return (Create(pTemp));
                }
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Error("插入操作日志失败!", ex);
            }
            return (byte)EResult.Fail;
        }

        #endregion
    }
}
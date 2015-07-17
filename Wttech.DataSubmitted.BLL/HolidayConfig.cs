/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个假期配置实现类文件
* 创建标识：ta0395侯兴鼎20141203
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.IBLL;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;
using System.Transactions;
using Wttech.DataSubmitted.Common.Resources;

namespace Wttech.DataSubmitted.BLL
{
    /// <summary>
    /// 假期配置实现类文
    /// </summary>
    public class HolidayConfig : IHolidayConfig
    {
        private static HolidayConfig holidayConfig = null;
        public static HolidayConfig GetInstance()
        {
            if (holidayConfig == null)
                holidayConfig = new HolidayConfig();
            return holidayConfig;
        }


        #region 9 Public Methods

        /// <summary>
        /// 修改假期配置
        /// </summary>
        /// <param name="args">配置信息实体</param>
        /// <returns>影响行数</returns>
        public CustomResult Update(OT_HDayConfig model)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    CustomResult pReturnValue = new CustomResult();
                    var list = db.OT_HDayConfig.Where(a => a.Id == model.Id).ToList();
                    if (list != null && list.Count > 0)
                    {
                        if (model.CheckFloat != null && model.CheckFloat > 0)
                            list[0].CheckFloat = model.CheckFloat;

                        if (model.CompEnd != null)
                            list[0].CompEnd = model.CompEnd;

                        if (model.CompStart != null)
                            list[0].CompStart = model.CompStart;

                        if (!string.IsNullOrEmpty(model.ConfigName))
                            list[0].ConfigName = model.ConfigName;

                        if (model.ForeDate != null)
                            list[0].ForeDate = model.ForeDate;

                        if (model.ForeFloat != null && model.ForeFloat > 0)
                            list[0].ForeFloat = model.ForeFloat;

                        if (model.HDayEnd != null)
                            list[0].HDayEnd = model.HDayEnd;

                        if (model.HDayId != null && model.HDayId > 0)
                            list[0].HDayId = model.HDayId;

                        if (model.HDayStart != null)
                            list[0].HDayStart = model.HDayStart;

                        if (!string.IsNullOrEmpty(model.RptRemark))
                            list[0].RptRemark = model.RptRemark;

                        pReturnValue = Result.SaveUpdateResult(db, transaction);
                    }
                    else
                    {
                        pReturnValue.ResultKey = (byte)EResult.IsNull1;
                        pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.Inexist;
                    }
                    return pReturnValue;
                }
            }
        }

        /// <summary>
        /// 修改所有不能设置假期名称的假期名称配置项
        /// </summary>
        /// <param name="holidayId"></param>
        /// <returns></returns>
        public CustomResult Update(int holidayId)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    CustomResult pReturnValue = new CustomResult();
                    var list = db.OT_HDayConfig.ToList();
                    if (list != null && list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            if (item.ConfigItem == null || !item.ConfigItem.Contains("HDayId"))
                                item.HDayId = holidayId;
                        }
                        pReturnValue = Result.SaveUpdateResult(db, transaction);
                    }
                    else
                    {
                        pReturnValue.ResultKey = (byte)EResult.IsNull1;
                        pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.InexistHoliday;
                    }
                    return pReturnValue;
                }
            }
        }

        /// <summary>
        /// 获取假期配置信息列表
        /// </summary>
        /// <returns></returns>
        public List<OT_HDayConfig> GetList()
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                return db.OT_HDayConfig.ToList();
            }
        }

        /// <summary>
        /// 获取假期配置信息列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public List<HolidayConfigViewModel> GetList(int type)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //获取假期配置数据某类型中的所有子类型ID
                var listId = db.OT_Dic.Where(a => a.Type == type & a.Belong == (int)EDicParentId.Config).Select(a =>
                    a.Id
               ).ToList();

                List<HolidayConfigViewModel> list = db.OT_HDayConfig.Where(a => listId.Contains(a.Type)).OrderBy(a => a.Type).Select(a => new HolidayConfigViewModel
                {
                    HolidayConfigId = a.Id,
                    ConfigName = a.ConfigName,
                    HolidayId = a.HDayId,
                    HolidayName = a.OT_Dic.Name,
                    HolidayStartTime = a.HDayStart,
                    HolidayEndTime = a.HDayEnd,
                    ComparedStartTime = a.CompStart,
                    ComparedEndTime = a.CompEnd,
                    ForecastDate = a.ForeDate,
                    ForecastFloat = a.ForeFloat,
                    ReportRemark = a.RptRemark,
                    CheckFloat = a.CheckFloat,
                    ConfigItem = a.ConfigItem,
                    Type = a.OT_Dic1.Name
                }).ToList();
                return list;
            }
        }

        /// <summary>
        /// 根据报表配置ID读取（假期和非假期表都存在HolidayConfig表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OT_HDayConfig GetById(int id)
        {
            OT_HDayConfig pHolidayConfig = null;
            try
            {
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    List<OT_HDayConfig> list = db.OT_HDayConfig.Where(s => s.Id == id).ToList();
                    if (list.Count > 0)
                    {
                        pHolidayConfig = list[0];
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Error(ex);
                pHolidayConfig = null;
            }
            return pHolidayConfig;
        }

        /// <summary>
        /// 根据报表配置ID读取（假期和非假期表都存在HolidayConfig表）
        /// </summary>
        /// <param name="id">报表编号（配置ID）</param>
        /// <returns></returns>
        public HolidayConfigViewModel GetHolidayConfigById(int id)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                List<HolidayConfigViewModel> list = db.OT_HDayConfig.Where(s => s.Id == id).Select(a => new HolidayConfigViewModel
                {
                    HolidayConfigId = a.Id,
                    ConfigName = a.ConfigName,
                    HolidayId = a.HDayId,
                    HolidayName = a.OT_Dic.Name,
                    HolidayStartTime = a.HDayStart,
                    HolidayEndTime = a.HDayEnd,
                    ComparedStartTime = a.CompStart,
                    ComparedEndTime = a.CompEnd,
                    ForecastDate = a.ForeDate,
                    ForecastFloat = a.ForeFloat,
                    ReportRemark = a.RptRemark,
                    CheckFloat = a.CheckFloat,
                    Type = a.OT_Dic1.Name,
                    ConfigItem = a.ConfigItem
                }).ToList();

                if (list != null && list.Count > 0)
                    return list[0];
                else
                    return null;
            }
        }

        /// <summary>
        /// 通过假期名称获取假期时间
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HolidayTimeViewModel GetHolidayTime(string name)
        {
            HolidayTimeViewModel model = new HolidayTimeViewModel();
            model.HolidayStartTime = "2014-10-01";
            model.HolidayEndTime = "2014-10-07";
            return model;
        }

        #endregion
    }
}

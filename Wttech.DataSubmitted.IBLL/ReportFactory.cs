/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/13 16:11:18
 */

#region 引用
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.IBLL.IReport;
using Wttech.DataSubmitted.DAL;

#endregion

namespace Wttech.DataSubmitted.IBLL
{
    public class ReportFactory
    {
        #region 4 Properties
        /// <summary>
        /// 报表1,2,3,4模块
        /// </summary>
        [Dependency]
        public IDataDailyTrafficStatistical report1 { get; set; }

        /// <summary>
        /// 报表5模块
        /// </summary>
        [Dependency]
        public ICityEnExStatistical report5 { get; set; }

        /// <summary>
        /// 报表6模块
        /// </summary>
        [Dependency]
        public ICityDailyEnExStatistical report6 { get; set; }

        /// <summary>
        /// 报表7模块
        /// </summary>
        [Dependency]
        public IAADTAndTransCalcu report7 { get; set; }

        /// <summary>
        /// 报表8模块
        /// </summary>
        [Dependency]
        public IHDayTraStatistical report8 { get; set; }
        /// <summary>
        /// 报表9模块
        /// </summary>
        [Dependency]
        public IRoadRunSitStatistical report9 { get; set; }
        /// <summary>
        /// 报表10模块
        /// </summary>
        [Dependency]
        public IHdayExEnTrafficStatistical report10 { get; set; }
        /// <summary>
        /// 报表11模块
        /// </summary>
        [Dependency]
        public IHDayStaExEnStatistical report11 { get; set; }
        /// <summary>
        /// 报表12模块
        /// </summary>
        [Dependency]
        public IHDayExStatistical report12 { get; set; }
        /// <summary>
        /// 报表13,14模块
        /// </summary>
        [Dependency]
        public IHourAADTStatistical report13 { get; set; }
        /// <summary>
        /// 报表15,16,17模块
        /// </summary>
        [Dependency]
        public INaturalTrafficStatistical report15 { get; set; }
        /// <summary>
        /// 报表18模块
        /// </summary>
        [Dependency]
        public IHDayAADTStatistical report18 { get; set; }


        /// <summary>
        /// 日志管理模块
        /// </summary>
        [Dependency]
        public ILogManage log { get; set; }

        /// <summary>
        /// 用户权限管理模块
        /// </summary>
        [Dependency]
        public IUserManage userManage { get; set; }

        /// <summary>
        /// 数据字典
        /// </summary>
        [Dependency]
        public IDataDictionary dataDictionary { get; set; }

        [Dependency]
        public IReportRelated reportRelatedManage { get; set; }

        /// <summary>
        /// 假期报送配置管理模块
        /// </summary>
        [Dependency]
        public IHolidayConfig holidayConfig { get; set; }

        /// <summary>
        /// 假期报送配置管理模块
        /// </summary>
        [Dependency]
        public IExportAllToZip exportAllToZip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static ReportFactory Instance { get; set; }

        #endregion

        #region 9 Public Methods


        #region 报表部分

        /// <summary>
        /// 查询报表
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public IReportViewModel GetListByPra(QueryParameters args)
        {
            //log.WriteLog(OperationType.Query, Utility.GetReportNameByType(args.ReportType), Utility.GetFormatDate(args.StartTime, args.EndTime));
            switch (args.ReportType)
            {
                //报表1,2,3,4共用一个类
                case 1:
                    {
                        args.StationType = 1;
                        return report1.GetListByPra(args);
                    }
                case 2:
                    {
                        args.StationType = 3;
                        return report1.GetListByPra(args);
                    }
                case 3:
                    {
                        args.StationType = 15;
                        return report1.GetListByPra(args);
                    }
                case 4:
                    {
                        args.StationType = 33;
                        return report1.GetListByPra(args);
                    }
                case 5:
                    {
                        args.StationType = 1;
                        return report5.GetListByPra(args);
                    }
                case 6:
                    {
                        args.StationType = 33;
                        return report6.GetListByPra(args);
                    }
                case 7:
                    {
                        return report7.GetListByPra(args);
                    }
                case 8:
                    {
                        args.StationType = 1;
                        return report8.GetListByPra(args);
                    }
                case 9:
                    {
                        args.StationType = 1;
                        return report9.GetListByPra(args);
                    }
                //报表10
                case 10:
                    {
                        return report10.GetListByPra(args);
                    }
                case 11:
                    {
                        return report11.GetListByPra(args);
                    }
                case 12:
                    {
                        args.StationType = 3;
                        return report12.GetListByPra(args);
                    }
                //13,14公用一个类
                case 13:
                    {
                        args.StationType = 1;
                        return report13.GetListByPra(args);
                    }
                case 14:
                    {
                        args.StationType = 1;
                        return report13.GetListByPra(args);
                    }
                //报表15,16,17共用一个类
                case 15:
                    {
                        args.StationType = 1;
                        return report15.GetListByPra(args);
                    }
                case 16:
                    {
                        args.StationType = 15;
                        return report15.GetListByPra(args);
                    }
                case 17:
                    {
                        args.StationType = 1;
                        return report15.GetListByPra(args);
                    }
                case 18:
                    {
                        args.StationType = 1;//北京段
                        return report18.GetListByPra(args);
                    }
            }
            SystemLog.GetInstance().Info("缺少参数:报表类型!");
            return null;
        }
        /// <summary>
        /// 导出报表
        /// </summary>
        /// <param name="args"></param>
        /// <returns>文件路径</returns>
        public string Export(QueryParameters args)
        {
            log.WriteLog(OperationType.Export, Utility.GetReportNameByType(args.ReportType), Utility.GetFormatDate(args.StartTime, args.EndTime));
            switch (args.ReportType)
            {
                case 1:
                    {
                        args.StationType = 1;
                        return report1.ExportReport(args);
                    }
                case 2:
                    {
                        args.StationType = 3;
                        return report1.ExportReport(args);
                    }
                case 3:
                    {
                        args.StationType = 15;
                        return report1.ExportReport(args);
                    }
                case 4:
                    {
                        args.StationType = 33;
                        return report1.ExportReport(args);
                    }
                case 5:
                    {
                        args.StationType = 1;
                        return report5.ExportReport(args);
                    }
                case 6:
                    {
                        args.StationType = 33;
                        return report6.ExportReport(args);
                    }
                case 7:
                    {
                        return report7.ExportReport(args);
                    }
                case 8:
                    {
                        args.StationType = 1;
                        return report8.ExportReport(args);
                    }
                case 9:
                    {
                        args.StationType = 1;
                        return report9.ExportReport(args);
                    }
                case 10:
                    {
                        return report10.ExportReport(args);
                    }
                case 11:
                    {
                        return report11.ExportReport(args);
                    }
                case 12:
                    {
                        args.StationType = 3;
                        return report12.ExportReport(args);
                    }
                case 13:
                    {
                        return report13.ExportReport(args);
                    }
                case 14:
                    {
                        return report13.ExportReport(args);
                    }
                case 15:
                    {
                        args.StationType = 1;
                        return report15.ExportReport(args);
                    }
                case 16:
                    {
                        args.StationType = 15;
                        return report15.ExportReport(args);
                    }
                case 17:
                    {
                        args.StationType = 1;
                        return report15.ExportReport(args);
                    }
                case 18:
                    {
                        args.StationType = 1;
                        return report18.ExportReport(args);
                    }
                case 0:
                    {
                        return exportAllToZip.ExportReport(args);
                    }

            }
            SystemLog.GetInstance().Info("缺少参数:报表类型!");
            return null;
        }
        /// <summary>
        /// 校正数据
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public CustomResult CalibrationData(QueryParameters para)
        {
            log.WriteLog(OperationType.Calibration, Utility.GetReportNameByType(para.ReportType), Utility.GetFormatDate(para.StartTime, para.EndTime));

            switch (para.ReportType)
            {
                case 1:
                    {
                        para.StationType = 1;
                        return report1.CalibrationData(para);
                    }
                case 2:
                    {
                        para.StationType = 3;
                        return report1.CalibrationData(para);
                    }
                case 3:
                    {
                        para.StationType = 15;
                        return report1.CalibrationData(para);
                    }
                case 4:
                    {
                        para.StationType = 33;
                        return report1.CalibrationData(para);
                    }
                case 5:
                    {
                        para.StationType = 1;
                        return report5.CalibrationData(para);
                    }
                case 6:
                    {
                        para.StationType = 33;
                        return report6.CalibrationData(para);
                    }
                case 7:
                    {
                        return report7.CalibrationData(para);
                    }
                case 8:
                    {
                        para.StationType = 1;
                        return report8.CalibrationData(para);
                    }
                case 11:
                    {
                        return report11.CalibrationData(para);
                    }
                case 12:
                    {
                        return report12.CalibrationData(para);
                    }
                case 13:
                    {
                        para.StationType = 1;
                        return report13.CalibrationData(para);
                    }
                case 15:
                    {
                        para.StationType = 1;
                        return report15.CalibrationData(para);
                    }
                case 16:
                    {
                        para.StationType = 15;
                        return report15.CalibrationData(para);
                    }
                case 18:
                    {
                        para.StationType = 1;
                        return report18.CalibrationData(para);
                    }
            }
            SystemLog.GetInstance().Info("缺少参数:报表类型!");
            return null;
        }
        /// <summary>
        /// 预测数据
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public CustomResult ForecastData(QueryParameters para)
        {
            log.WriteLog(OperationType.Forecast, Utility.GetReportNameByType(para.ReportType), Utility.GetFormatDate(para.LastYearStart, para.LastYearEnd));

            switch (para.ReportType)
            {
                case 1:
                    {
                        para.StationType = 1;
                        return report1.ForecastData(para);
                    }
                case 2:
                    {
                        para.StationType = 3;
                        return report1.ForecastData(para);
                    }
                case 3:
                    {
                        para.StationType = 15;
                        return report1.ForecastData(para);
                    }
                case 4:
                    {
                        para.StationType = 33;
                        return report1.ForecastData(para);
                    }
                case 5:
                    {
                        para.StationType = 1;
                        return report5.ForecastData(para);
                    }
                case 6:
                    {
                        para.StationType = 33;
                        return report6.ForecastData(para);
                    }
                case 8:
                    {
                        para.StationType = 1;
                        return report8.ForecastData(para);
                    }
                case 9:
                    {
                        para.StationType = 1;
                        return report9.ForecastData(para);
                    }
            }
            SystemLog.GetInstance().Info("缺少参数:报表类型!");
            return null;
        }

        #endregion
        /// <summary>
        /// 无收费站信息列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<ReportRelatedViewModels> NoDataList(QueryParameters para)
        {
            switch (para.ReportType)
            {
                case 1:
                    {
                        para.StationType = 1;
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 2:
                    {
                        para.StationType = 3;
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 3:
                    {
                        para.StationType = 15;
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 4:
                    {
                        para.StationType = 33;
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 5:
                    {
                        para.StationType = 1;
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 6:
                    {
                        para.StationType = 33;
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 7:
                    {
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 8:
                    {
                        para.StationType = 1;
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 11:
                    {
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 12:
                    {
                        para.StationType = 3;
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 13:
                    {
                        para.StationType = 1;
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 14:
                    {
                        para.StationType = 1;
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 15:
                    {
                        para.StationType = 1;
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 16:
                    {
                        para.StationType = 15;
                        return reportRelatedManage.GetNoDataList(para);
                    }
                case 18:
                    {
                        para.StationType = 1;
                        return reportRelatedManage.GetNoDataList(para);
                    }
            }
            SystemLog.GetInstance().Info("缺少参数:报表类型!");
            return null;
        }

        #region 数据字典部分

        /// <summary>
        /// 通过父节点编号从数据字典中获取该父节点下的所有子节点
        /// </summary>
        /// <param name="id">父节点编号</param>
        /// <returns></returns>
        public List<DictionaryViewModel> DicGetList(int id)
        {
            return dataDictionary.GetList(id);
        }

        /// <summary>
        /// 通过父节点编号从数据字典中获取该父节点下的所有子节点
        /// </summary>
        /// <param name="id">父节点编号</param>
        /// <returns></returns>
        public List<NodeViewModel> DicGetListbyId(int id)
        {
            return dataDictionary.GetListbyId(id);
        }

        /// <summary>
        /// 增加数据字典节点
        /// </summary>
        /// <param name="id">父节点编号，根节点传0</param>
        /// <param name="name">子节点名称</param>
        /// <returns></returns>
        public CustomResult DicAdd(int id, string name)
        {
            OT_Dic model = new OT_Dic();

            model.Belong = id;
            model.IsDelete = (byte)EDataStatus.Normal;
            model.Name = name;

            return dataDictionary.Add(model, id);
        }

        /// <summary>
        /// 批量删除数据字典
        /// </summary>
        /// <param name="listId">节点编号集合</param>
        /// <returns></returns>
        public CustomResult DicDelete(List<int> listId)
        {
            return dataDictionary.DeleteAndResult(listId);
        }

        /// <summary>
        /// 修改数据字典节点名称
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CustomResult DicUpdate(NodeViewModel model)
        {
            OT_Dic modelDic = new OT_Dic();
            modelDic.Id = model.DictId;
            modelDic.Name = model.Name;
            modelDic.Belong = (byte)EDicParentId.Holiday;
            return dataDictionary.Update(modelDic);
        }

        /// <summary>
        /// 修改已删除数据字典节点为未删除状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CustomResult DicRecover(List<int> listId)
        {
            return dataDictionary.Update(listId);
        }

        #endregion

        #region 假期报送配置部分

        /// <summary>
        /// 获取假期配置信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public List<HolidayConfigViewModel> GetConfig(int type)
        {
            return holidayConfig.GetList(type);
        }
        /// <summary>
        /// 获取假期配置信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public HolidayConfigViewModel GetHolidayConfigById(int type)
        {
            return holidayConfig.GetHolidayConfigById(type);
        }

        /// <summary>
        /// 修改配置信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CustomResult UpdateConfig(HolidayConfigViewModel model)
        {
            OT_HDayConfig modelHC = new OT_HDayConfig();
            modelHC.CheckFloat = model.CheckFloat;
            modelHC.CompEnd = model.ComparedEndTime;
            modelHC.CompStart = model.ComparedStartTime;
            modelHC.ConfigName = model.ConfigName;
            modelHC.ForeDate = model.ForecastDate;
            modelHC.ForeFloat = model.ForecastFloat;
            modelHC.Id = model.HolidayConfigId;
            modelHC.HDayEnd = model.HolidayEndTime;
            modelHC.HDayId = model.HolidayId;
            modelHC.HDayStart = model.HolidayStartTime;
            modelHC.RptRemark = model.ReportRemark;

            return holidayConfig.Update(modelHC);
        }

        /// <summary>
        /// 修改所有不能设置假期名称的假期名称配置项
        /// </summary>
        /// <param name="holidayId"></param>
        /// <returns></returns>
        public CustomResult SetHoliday(int holidayId)
        {
            return holidayConfig.Update(holidayId);
        }

        /// <summary>
        /// 通过假期名称获取假期时间
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HolidayTimeViewModel GetHolidayTime(string name)
        {
            return holidayConfig.GetHolidayTime(name);
        }

        #endregion

        #endregion
    }
}

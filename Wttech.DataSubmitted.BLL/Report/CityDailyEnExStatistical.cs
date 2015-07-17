/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个报表6实现类文件
* 创建标识：ta0395侯兴鼎20141215
*/
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.Resources;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL.IReport;

namespace Wttech.DataSubmitted.BLL.Report
{
    /// <summary>
    /// 报表6实现类
    /// </summary>
    public class CityDailyEnExStatistical : ReportRelated, ICityDailyEnExStatistical
    {
        #region 9 Public Methods

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Common.ViewModels.IReportViewModel GetListByPra(Common.QueryParameters para)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                RepairData(para);//补数据

                var list = db.RP_EnEx.Where(a => a.CalcuTime == para.StartTime & a.StaType == para.StationType).ToList();
                QueryEnExViewModel model = new QueryEnExViewModel();
                if (list != null && list.Count > 0)
                {
                    EnExViewModel cdMole = new EnExViewModel();
                    cdMole.CalcuTime = list[0].CalcuTime.ToString();
                    cdMole.EnGre = 0;//list[0].EnGre==null?0:list[0].EnGre;
                    cdMole.EnOthCar = list[0].EnOthCar;
                    cdMole.EnSmaCar = list[0].EnSmaCar;
                    cdMole.EnTruk = list[0].EnTruk;
                    cdMole.StaType = list[0].StaType;
                    cdMole.Name = "日车辆数(辆)";

                    cdMole.PEnGre = 0;
                    cdMole.PEnOthCar = 0;
                    cdMole.PEnSmaCar = 0;
                    cdMole.PEnTruk = 0;
                    cdMole.PExGre = 0;
                    cdMole.PExOthCar = 0;
                    cdMole.PExSmaCar = 0;
                    cdMole.PExTruk = 0;

                    model.ReportData = new List<EnExViewModel>();
                    model.ReportData.Add(cdMole);

                    EnExViewModel cdMole1 = new EnExViewModel();
                    cdMole1.Name = "去年同期";
                    model.ReportData.Add(cdMole1);
                }
                else
                {
                    EnExViewModel cdMole = new EnExViewModel();
                    model.ReportData = new List<EnExViewModel>();
                    cdMole.Name = "日车辆数(辆)";
                    cdMole.PEnGre = 0;
                    cdMole.PEnOthCar = 0;
                    cdMole.PEnSmaCar = 0;
                    cdMole.PEnTruk = 0;
                    cdMole.PExGre = 0;
                    cdMole.PExOthCar = 0;
                    cdMole.PExSmaCar = 0;
                    cdMole.PExTruk = 0;
                    model.ReportData.Add(cdMole);

                    EnExViewModel cdMole1 = new EnExViewModel();
                    cdMole1.Name = "去年同期";
                    model.ReportData.Add(cdMole1);
                }
                //判断当前统计站类型，数据是否完整
                if (GetNoDataList(para) != null && GetNoDataList(para).Count() > 0)
                    model.IsFull = 0;//不完整
                else
                    model.IsFull = 1;//完整 
                return model;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Common.CustomResult Update(Common.ViewModels.UpdateEnExViewModel args)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    CustomResult pReturnValue = new CustomResult();

                    DateTime dt=new DateTime();
                    if (DateTime.TryParse(args.DataInfo[0].CalcuTime, out dt))
                    {
                        var list = db.RP_EnEx.Where(a => a.CalcuTime == dt & a.StaType == args.StationType).ToList();
                        if (list != null && list.Count > 0&&args.DataInfo!=null&&args.DataInfo.Count>0)
                        {
                            // list[0].CalcuTime = args.DataInfo[0].CalcuTime;
                            //  list[0].EnGre = args.DataInfo[0].EnGre;

                            list[0].EnOthCar = args.DataInfo[0].EnOthCar;
                            list[0].EnSmaCar = args.DataInfo[0].EnSmaCar;
                            list[0].EnTruk = args.DataInfo[0].EnTruk;
                            list[0].State = "1";
                            list[0].UpdDate = DateTime.Now;
                            if (SessionManage.GetLoginUser() != null)
                            {
                                list[0].UpdBy = SessionManage.GetLoginUser().UserName;
                            }
                        }
                        try
                        {
                            db.SaveChanges();
                            transaction.Complete();
                            pReturnValue.ResultKey = (byte)EResult.Succeed;
                            pReturnValue.ResultValue = TipInfo.UpdateSuccess;
                            return pReturnValue;
                        }
                        catch (Exception ex)
                        {
                            Common.SystemLog.GetInstance().Log.Info(TipInfo.UpdateDataRepeat, ex);
                            pReturnValue.ResultKey = (byte)EResult.Fail;
                            pReturnValue.ResultValue = TipInfo.UpdateDataRepeat;
                            return pReturnValue;
                        }
                    }
                    else
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.DateTimeIsNull;
                        return pReturnValue;
                    }
                }
            }
        }

        /// <summary>
        /// 更改Excel工作簿内容
        /// </summary>
        /// <param name="readworkbook"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public NPOI.SS.UserModel.IWorkbook GenerateSheet(NPOI.SS.UserModel.IWorkbook readworkbook, Common.QueryParameters para)
        {
            //获取工作簿
            if (readworkbook != null)
            {
                ISheet sheet = readworkbook.GetSheetAt(0);

                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    //获取导出日期数据
                    List<RP_EnEx> pAADTList = db.RP_EnEx.Where(a => a.CalcuTime == para.StartTime && a.StaType == para.StationType).ToList();

                    if (pAADTList != null && pAADTList.Count > 0)
                    {
                        RP_EnEx pInfo = pAADTList.First();

                        //高速
                        SetValue(sheet, 5, 1, pInfo.EnSmaCar.ToString());//小型客车
                        SetValue(sheet, 5, 2, pInfo.EnOthCar.ToString());//其他客车
                        SetValue(sheet, 5, 3, pInfo.EnTruk.ToString());//高速入境货车数
                        SetValue(sheet, 5, 4, 0);//pInfo.EnGre.ToString());//绿色通道数
                    }
                }
            }
            return readworkbook;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public string ExportReport(Common.QueryParameters para)
        {
            string path = string.Empty;
            string reportpath = string.Empty;
            if (para.ReportType == 6)
            {
                path = string.Format(@"{0}Reporttemplate\编号06--重点城市出入境车辆数日报表（泗村店）.xlsx",AppDomain.CurrentDomain.BaseDirectory);
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
            }
            return reportpath;
        }

        /// <summary>
        /// 校正
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Common.CustomResult CalibrationData(Common.QueryParameters para)
        {
            CustomResult pReturnValue = new CustomResult();
            double pFloating = 1 + para.FloatingRange * 0.01;

            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //判断报表浮动百分比配置是否正确
                OT_HDayConfig pds = HolidayConfig.GetInstance().GetById(para.ReportType);
                if (pds == null)
                {
                    pReturnValue.ResultKey = (byte)EResult.Fail;
                    pReturnValue.ResultValue = TipInfo.CalibrationFaileNoConfig;
                    return pReturnValue;
                }
                if (Math.Abs(para.FloatingRange) > (double)pds.CheckFloat.Value)
                {
                    pReturnValue.ResultKey = (byte)EResult.Fail;
                    pReturnValue.ResultValue = TipInfo.ErrorInfo + "范围应在负" + pds.CheckFloat.Value + "%-正" + pds.CheckFloat.Value + "%之间";
                    return pReturnValue;
                }
                //判断校正数据日期是否合理
                if (para.LastYearStart < para.StartTime && para.StartTime < DateTime.Now.AddDays(1))
                {
                    //获取参考日期符合校正时间段的数据,因为只校正一天的数据，所以只查询开始数据的日期就可以
                    List<RP_EnEx> pRefNaturalList = db.RP_EnEx.Where(s => s.CalcuTime == para.LastYearStart && s.StaType == para.StationType).ToList();
                    //如果参考日期数据为0 则返回失败
                    if (pRefNaturalList == null || pRefNaturalList.Count == 0)
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.CalibrationFaileRefNoData;
                        return pReturnValue;
                    }
                    //需要校正的数据
                    var pCheckNaturalList = db.RP_EnEx.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType).ToList();
                    //如果需要校正的数据为空则返回失败
                    if (pCheckNaturalList == null || pCheckNaturalList.Count == 0)
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.CalibrationFaileNoData;
                        return pReturnValue;
                    }
                    using (TransactionScope tran = new TransactionScope())
                    {
                        //校正数据
                        RP_EnEx pCheckInfo = pCheckNaturalList.First();
                        //参考数据
                        RP_EnEx pRefInfo = pRefNaturalList.First();

                        if (pRefInfo.EnOthCar != null)
                            pCheckInfo.EnOthCar = Math.Round(pRefInfo.EnOthCar.Value * pFloating);
                        if (pCheckInfo.EnSmaCar != null)
                            pCheckInfo.EnSmaCar = Math.Round(pRefInfo.EnSmaCar.Value * pFloating);
                        if (pCheckInfo.EnTruk != null)
                            pCheckInfo.EnTruk = Math.Round(pRefInfo.EnTruk.Value * pFloating);

                        if (SessionManage.GetLoginUser() != null)
                        {
                            pCheckInfo.UpdBy = SessionManage.GetLoginUser().UserName;
                        }
                        pCheckInfo.UpdDate = DateTime.Now;
                        pCheckInfo.State = "1";
                        try
                        {
                            db.SaveChanges();
                            tran.Complete();
                            pReturnValue.ResultKey = (byte)EResult.Succeed;
                            pReturnValue.ResultValue = TipInfo.CalibrationSuccess;
                        }
                        catch (Exception e)
                        {
                            pReturnValue.ResultKey = (byte)EResult.Fail;
                            pReturnValue.ResultValue = TipInfo.CalibrationFaile + e.Message.ToString();
                            SystemLog.GetInstance().Error(TipInfo.CalibrationFaile, e);
                            return pReturnValue;
                        }
                    }
                }
                else
                {
                    pReturnValue.ResultKey = (byte)EResult.Fail;
                    pReturnValue.ResultValue = TipInfo.CalibrationFaileDate;
                }
            }
            return pReturnValue;
        }

        /// <summary>
        /// 预测
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Common.CustomResult ForecastData(Common.QueryParameters para)
        {
            CustomResult pReturnValue = new CustomResult();
            //浮动范围
            double pFloating = 1 + para.FloatingRange * 0.01;
            // List<RP_EnEx> pForeList = new List<RP_EnEx>();
            //预测数据集合
            List<IReportViewModel> pInList = new List<IReportViewModel>();

            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //判断报表浮动百分比配置是否存在
                OT_HDayConfig pds = HolidayConfig.GetInstance().GetById(para.ReportType);
                if (pds == null)
                {
                    pReturnValue.ResultKey = (byte)EResult.Fail;
                    pReturnValue.ResultValue = TipInfo.ForecastFaileNoConfig;
                    return pReturnValue;
                }
                //判断报表预测浮动百分比配置是否正确
                if (Math.Abs(para.FloatingRange) > (double)pds.ForeFloat.Value)
                {
                    pReturnValue.ResultKey = (byte)EResult.Fail;
                    pReturnValue.ResultValue = TipInfo.ErrorInfo + "范围应在负" + pds.ForeFloat.Value + "%-正" + pds.ForeFloat.Value + "%之间";
                    return pReturnValue;
                }
                //获取参考日期符合的数据
                List<RP_EnEx> pRefInfoList = db.RP_EnEx.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType).ToList();
                //如果参考日期数据为0 则返回失败
                if (pRefInfoList == null || pRefInfoList.Count == 0)
                {
                    pReturnValue.ResultKey = (byte)EResult.Fail;
                    pReturnValue.ResultValue = TipInfo.ForecastFaileFaileRefNoData;
                    return pReturnValue;
                }

                //预测数据
                ForecastEnExViewModel pInfo = new ForecastEnExViewModel();
                //预测数据集合
                List<IReportViewModel> plist = new List<IReportViewModel>();
                //参考数据
                RP_EnEx pRefInfo = pRefInfoList.First();

                if (pRefInfo.EnOthCar != null)
                    pInfo.EnOthCar = Math.Round(pRefInfo.EnOthCar.Value * pFloating);
                if (pInfo.EnSmaCar != null)
                    pInfo.EnSmaCar = Math.Round(pRefInfo.EnSmaCar.Value * pFloating);
                if (pInfo.EnTruk != null)
                    pInfo.EnTruk = Math.Round(pRefInfo.EnTruk.Value * pFloating);
                try
                {
                    plist.Add(pInfo);
                    string path = Export(para, plist);
                    pReturnValue.ResultKey = (byte)EResult.Succeed;
                    pReturnValue.ResultValue = path;
                }
                catch (Exception e)
                {
                    pReturnValue.ResultKey = (byte)EResult.Fail;
                    pReturnValue.ResultValue = TipInfo.ForecastFail + e.Message.ToString();
                    SystemLog.GetInstance().Error(TipInfo.ForecastFail, e);
                    return pReturnValue;
                }
            }
            return pReturnValue;
        }

        /// <summary>
        /// 预测导出--如果需要将入出口数据进行区分，则分别放在两个list的集合中，若不需区分，则将数据放入list1中，list2为空即可
        /// </summary>
        /// <param name="readworkbook"></param>
        /// <param name="para"></param>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public NPOI.SS.UserModel.IWorkbook GenerateSheet(NPOI.SS.UserModel.IWorkbook readworkbook, Common.QueryParameters para, List<Common.ViewModels.IReportViewModel> list1, List<Common.ViewModels.IReportViewModel> list2)
        {
            //获取工作簿
            if (readworkbook != null)
            {
                ISheet sheet = readworkbook.GetSheetAt(0);

                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    if (list1 != null && list1.Count > 0)
                    {
                        ForecastEnExViewModel pInfo = list1.First() as ForecastEnExViewModel;

                        //高速入境
                        SetValue(sheet, 5, 1, pInfo.EnSmaCar.ToString());//小型客车
                        SetValue(sheet, 5, 2, pInfo.EnOthCar.ToString());//其他客车
                        SetValue(sheet, 5, 3, pInfo.EnTruk.ToString());//高速入境货车数
                        SetValue(sheet, 5, 4, 0);//pInfo.EnGre.ToString());//绿色通道数
                    }
                }
            }
            return readworkbook;
        } 

        #endregion

        #region 11 Private Methods

        /// <summary>
        /// 补数据
        /// </summary>
        /// <param name="para"></param>
        private void RepairData(QueryParameters para)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    DateTime dtQuery=new DateTime();
                      //判断传入的时间段是否正确
                    if (para.StartTime != null && DateTime.TryParse(para.StartTime.Value.ToString(), out dtQuery))
                    {
                        DateTime dtime = para.StartTime.Value;

                        //时间小于当前时间
                        if (DateTime.Now > dtime)
                        {
                            bool flag = db.RP_EnEx.Where(a => a.CalcuTime == dtime & a.StaType == para.StationType).Select(a => a.CalcuTime).ToList().Contains(dtime);
                            if (!flag)//补数据
                            {
                                RP_EnEx hday = new RP_EnEx();
                                hday.CalcuTime = dtime;

                                if (SessionManage.GetLoginUser() != null)
                                {
                                    hday.CrtBy = SessionManage.GetLoginUser().UserName;
                                }
                                hday.CrtDate = DateTime.Now;
                                hday.Id = Guid.NewGuid();
                                hday.EnGre = 0;
                                hday.EnOthCar = 0;
                                hday.EnSmaCar = 0;
                                hday.EnTruk = 0;
                                hday.State = "0";
                                hday.StaType = para.StationType;
                                db.RP_EnEx.Add(hday);
                            }
                            else//将数据中有空值的改成0
                            {
                                var hday = db.RP_EnEx.Where(a => a.CalcuTime == dtime & a.StaType == para.StationType).ToList()[0];
                                if (hday.EnGre == null)
                                    hday.EnGre = 0;
                                if (hday.EnOthCar == null)
                                    hday.EnOthCar = 0;
                                if (hday.EnSmaCar == null)
                                    hday.EnSmaCar = 0;
                                if (hday.EnTruk == null)
                                    hday.EnTruk = 0;
                            }
                        }
                        try
                        {
                            db.SaveChanges();
                            transaction.Complete();
                        }
                        catch (Exception ex)
                        {
                            SystemLog.GetInstance().Error(TipInfo.AddFaile, ex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 预测导出
        /// </summary>
        /// <param name="para"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private string Export(QueryParameters para, List<IReportViewModel> list)
        {
            string path = string.Empty;
            string reportpath = string.Empty;
            if (para.ReportType == 6)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号06--重点城市出入境车辆数日报表（泗村店）.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this, list, null);
            }
            return reportpath;
        } 

        #endregion
    }
}
/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/26 10:14:51
 */

#region 引用
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Wttech.DataSubmitted.BLL.Tool;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.Resources;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL.IReport;

#endregion

namespace Wttech.DataSubmitted.BLL.Report
{
    /// <summary>
    /// 报表1,2,3,4
    /// </summary>
    public class DataDailyTrafficStatistical : ReportRelated, IDataDailyTrafficStatistical
    {

        #region 9 Public Methods
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public IReportViewModel GetListByPra(QueryParameters para)
        {
            QueryDataDailyInfoViewModel pQueryDataList = new QueryDataDailyInfoViewModel();

            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //出口数据集合
                List<DataDailyInfoViewModel> pOutlist = db.RP_Daily.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType).ToList().Select(s => new DataDailyInfoViewModel()
                {
                    CarChag = s.ChagFee,
                    VehNum = (float)s.OutNum,//出口
                    VehType = s.VehType.ToString()

                }).ToList();
                //入口数据集合
                List<DataDailyInfoViewModel> pInlist = db.RP_Daily.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType).ToList().Select(s => new DataDailyInfoViewModel()
                {
                    CarChag = s.ChagFee,
                    ExEn = "1",
                    VehNum = (float)s.InNum,//入口
                    VehType = s.VehType.ToString()

                }).ToList();
                //插入数据,
                if (db.RP_Daily.Count(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType) < 4)
                {
                    InsertNull(para.StartTime.Value, para.StationType.Value);
                }

                //list.AddRange(pOutlist);
                //list.AddRange(pInlist);
                //遍历出口和入口四种车型
                List<DataDailyInfoViewModel> list = new List<DataDailyInfoViewModel>();
                for (int i = 0; i < 4; i++)
                {
                    string ptemp = i.ToString();
                    //0小型客车，1其他客车，2货车（不包含绿通），3绿通
                    //出口
                    DataDailyInfoViewModel outdailyinfo = new DataDailyInfoViewModel();
                    outdailyinfo.VehType = i.ToString();
                    outdailyinfo.ExEn = "0";
                    outdailyinfo.CarChag = Math.Round(pOutlist.Where(s => s.VehType == ptemp).Sum(s => s.CarChag.Value) / 10000, 2);
                    outdailyinfo.VehNum = pOutlist.Where(s => s.VehType == ptemp).Sum(s => s.VehNum);
                    //入口//入口无收费免征金额
                    DataDailyInfoViewModel indailyinfo = new DataDailyInfoViewModel();
                    indailyinfo.VehType = i.ToString();
                    indailyinfo.ExEn = "1";
                    indailyinfo.VehNum = pInlist.Where(s => s.VehType == ptemp).Sum(s => s.VehNum);
                    //将出口和入口数据加到查询集合中
                    list.Add(outdailyinfo);
                    list.Add(indailyinfo);
                }
                //按出口和车型升序排序，
                pQueryDataList.ReportData = list.OrderBy(s => s.ExEn).ThenBy(s => s.VehType).ToList();
                foreach (DataDailyInfoViewModel info in pQueryDataList.ReportData)
                {
                    if (info.VehType == "0")
                        info.VehType = SystemConst.SmallCar;
                    if (info.VehType == "1")
                        info.VehType = SystemConst.OtherCar;
                    if (info.VehType == "2")
                        info.VehType = SystemConst.Truk;
                    if (info.VehType == "3")
                        info.VehType = SystemConst.Green;
                    if (info.ExEn == "0")
                        info.ExEn = SystemConst.Out;
                    if (info.ExEn == "1")
                        info.ExEn = SystemConst.In;
                }
                //判断当前统计站类型，数据是否完整
                if (GetNoDataList(para).Count() > 0)
                    pQueryDataList.IsFull = 0;//不完整
                else
                    pQueryDataList.IsFull = 1;//完整 
            }
            return pQueryDataList;
        }
        /// <summary>
        /// 校正数据
        /// </summary>
        /// <param name="args">校正日期，参考日期，浮动百分比</param>
        /// <returns></returns>
        public CustomResult CalibrationData(QueryParameters para)
        {
            CustomResult pReturnValue = new CustomResult();
            // QueryNaturalInfoViewModel queryNatural = new QueryNaturalInfoViewModel();

            //校正浮动范围
            double pFloating = para.FloatingRange * 0.01;
            List<RP_Daily> pDailyList = new List<RP_Daily>();
            try
            {
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    //判断报表浮动百分比配置是否存在
                    OT_HDayConfig pds = HolidayConfig.GetInstance().GetById(para.ReportType);
                    if (pds == null)
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.CalibrationFaileNoConfig;
                        return pReturnValue;
                    }
                    //判断报表浮动百分比配置是否正确
                    if (Math.Abs(para.FloatingRange) > (double)pds.CheckFloat.Value)
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.ErrorInfo + "范围应在负%" + pds.CheckFloat.Value + "-正%" + pds.CheckFloat.Value + "之间";
                        return pReturnValue;
                    }
                    //获取参考日期符合校正时间段的数据,因为只校正一天的数据，所以只查询开始数据的日期就可以
                    List<RP_Daily> pRefDataDailyInfo = db.RP_Daily.Where(s => s.CalcuTime == para.LastYearStart && s.StaType == para.StationType).ToList();
                    //如果参考日期数据为0 则返回失败
                    if (pRefDataDailyInfo.Count == 0)
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.CalibrationFaileRefNoData;
                        return pReturnValue;
                    }
                    //判断校正数据日期是否合理
                    if (para.LastYearStart < para.StartTime && para.StartTime < DateTime.Now.AddDays(1))
                    {
                        //需要校正的数据
                        var pCheckList = db.RP_Daily.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType).ToList();
                        //如果校正数据不存在则返回失败
                        if (pCheckList.Count == 0)
                        {
                            pReturnValue.ResultKey = (byte)EResult.Fail;
                            pReturnValue.ResultValue = TipInfo.CalibrationFaileNoData;
                            return pReturnValue;
                        }
                        using (TransactionScope tran = new TransactionScope())
                        {
                            foreach (var item in pCheckList)
                            {
                                IEnumerable<RP_Daily> plist = pRefDataDailyInfo.Where(i => i.VehType == item.VehType);
                                if (plist.Count() > 0)
                                {
                                    RP_Daily pTemp = plist.First();
                                    item.InNum = (int)(pTemp.InNum + pTemp.InNum * pFloating);
                                    item.OutNum = (int)(pTemp.OutNum + pTemp.OutNum * pFloating);
                                    item.ChagFee = pTemp.ChagFee.Value + pTemp.ChagFee.Value * decimal.Parse(pFloating.ToString());
                                    item.CalcuTime = para.StartTime.Value;
                                    item.StaType = para.StationType;
                                    if (SessionManage.GetLoginUser() != null)
                                    {
                                        item.UpdBy = SessionManage.GetLoginUser().UserName;
                                    }
                                    item.UpdDate = DateTime.Now;
                                    item.State = 1;
                                }
                            }
                            db.SaveChanges();
                            tran.Complete();
                            pReturnValue.ResultKey = (byte)EResult.Succeed;
                            pReturnValue.ResultValue = TipInfo.CalibrationSuccess;
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
            catch (Exception e)
            {
                pReturnValue.ResultKey = (byte)EResult.Fail;
                pReturnValue.ResultValue = TipInfo.CalibrationFaile + e.Message.ToString();
                SystemLog.GetInstance().Error(TipInfo.CalibrationFaile, e);
                return pReturnValue;
            }
        }
        /// <summary>
        /// 导出报表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public string ExportReport(QueryParameters para)
        {

            string path = string.Empty;
            string reportpath = string.Empty;
            if (para.ReportType == 1 && para.StationType == 1)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号01--收费公路数据汇总日报.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
            }
            else if (para.ReportType == 2 && para.StationType == 3)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号02--收费公路数据汇总日报.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
            }
            else if (para.ReportType == 3 && para.StationType == 15)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号03--京沪高速大羊坊收费站数据日报表.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
            }
            else if (para.ReportType == 4 && para.StationType == 33)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号04--重要运输通道主线收费站数据日报表（泗村店站）.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
            }
            return reportpath;
        }
        /// <summary>
        /// 修改表内容
        /// </summary>
        /// <param name="readworkbook"></param>
        /// <returns></returns>
        public IWorkbook GenerateSheet(IWorkbook readworkbook, QueryParameters para)
        {
            //数据是从数据库中取的，金额需要将单位转为万元
            //获取工作簿
            ISheet sheet = readworkbook.GetSheetAt(0);
            //设置日期
            if (para.ReportType == 1)
                SetReportDate(sheet, 0, 8, para.StartTime.Value, para.ReportType);
            if (para.ReportType == 3)
                SetReportDate(sheet, 0, 4, para.StartTime.Value, para.ReportType);
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //出口数据集合
                List<DataDailyInfoViewModel> pOutlist = db.RP_Daily.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType).ToList().Select(s => new DataDailyInfoViewModel()
                {
                    CarChag = s.ChagFee,
                    VehNum = (float)s.OutNum,//出口
                    VehType = s.VehType.ToString()

                }).ToList();
                //入口数据集合
                List<DataDailyInfoViewModel> pInlist = db.RP_Daily.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType).ToList().Select(s => new DataDailyInfoViewModel()
                {
                    CarChag = s.ChagFee,
                    VehNum = (float)s.InNum,//入口
                    VehType = s.VehType.ToString()

                }).ToList();
                for (int i = 0; i < 4; i++)
                {
                    string ptemp = i.ToString();
                    int rnum = i;
                    //表样不完全相同，需要更改赋值单元格行数,1和2行数相同,3和4行数相同
                    if (para.ReportType == 3 || para.ReportType == 4)
                    {
                        rnum = i - 1;
                    }
                    //0小型客车，1其他客车，2货车（不包含绿通），3绿通
                    //出口
                    SetValue(sheet, rnum + 4, 2, pOutlist.Where(s => s.VehType == ptemp).Sum(s => s.VehNum).ToString());
                    SetValue(sheet, rnum + 4, 4, Math.Round(pOutlist.Where(s => s.VehType == ptemp).Sum(s => s.CarChag.Value) / 10000, 2).ToString());
                    //入口
                    SetValue(sheet, rnum + 8, 2, pInlist.Where(s => s.VehType == ptemp).Sum(s => s.VehNum).ToString());
                }
            }
            return readworkbook;
        }
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CustomResult Update(UpdateDataDailyViewModel args)
        {
            CustomResult pReturnValue = new CustomResult();
            pReturnValue.ResultKey = (byte)EResult.Fail;
            pReturnValue.ResultValue = TipInfo.UpdateFaile;
            if (args.DataInfo == null)
            {
                pReturnValue.ResultKey = (byte)EResult.Succeed;
                pReturnValue.ResultValue = TipInfo.DataNull;
                return pReturnValue;
            }
            List<UpdateDataDailyInfo> pNewUpdateData = new List<UpdateDataDailyInfo>();

            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                var pReportData = db.RP_Daily.Where(s => s.CalcuTime == args.DataDate && s.StaType == args.StationType).ToList();
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        foreach (var item in args.DataInfo)
                        {
                            //获取查询日期当天的数据
                            if (item.VehType == SystemConst.SmallCar)
                                item.VehType = "0";
                            if (item.VehType == SystemConst.OtherCar)
                                item.VehType = "1";
                            if (item.VehType == SystemConst.Truk)
                                item.VehType = "2";
                            if (item.VehType == SystemConst.Green)
                                item.VehType = "3";
                            int ptemp = int.Parse(item.VehType);
                            RP_Daily pDataTemp = new RP_Daily();
                            if (pReportData.Where(i => i.VehType == ptemp).Count() == 1)
                            {
                                pDataTemp = pReportData.Where(i => i.VehType == ptemp).SingleOrDefault();

                                if (item.ExEn == SystemConst.In)//入口
                                {
                                    pDataTemp.InNum = item.VehNum;//出口
                                }
                                else
                                {
                                    pDataTemp.OutNum = item.VehNum;
                                    pDataTemp.ChagFee = item.CarChag * 10000;//转为元，存到数据库
                                }
                                pDataTemp.UpdDate = DateTime.Now;
                                pDataTemp.State = 1;
                                if (SessionManage.GetLoginUser() != null)
                                {
                                    pDataTemp.UpdBy = SessionManage.GetLoginUser().UserName;
                                }
                            }
                        }
                        db.SaveChanges();
                        transaction.Complete();
                        pReturnValue.ResultKey = (byte)EResult.Succeed;
                        pReturnValue.ResultValue = TipInfo.UpdateSuccess;
                    }
                    catch (Exception ex)
                    {
                        Common.SystemLog.GetInstance().Log.Info(TipInfo.UpdateDataRepeat, ex);
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.UpdateDataRepeat;
                    }
                    return pReturnValue;
                }
            }
        }
        /// <summary>
        /// 预测--修改工作簿内容
        /// </summary>
        /// <param name="readworkbook"></param>
        /// <param name="para"></param>
        /// <param name="outlist">出口数据集合</param>
        /// <param name="inlist">入口数据集合</param>
        /// <returns></returns>
        public IWorkbook GenerateSheet(IWorkbook readworkbook, QueryParameters para, List<IReportViewModel> outlist, List<IReportViewModel> inlist)
        {
            //数据是外界传进来的，不需要载将单位转为万元
            List<DataDailyInfoViewModel> pOutList = new List<DataDailyInfoViewModel>();
            List<DataDailyInfoViewModel> pInList = new List<DataDailyInfoViewModel>();
            foreach (var item in outlist)
            {
                DataDailyInfoViewModel datadaily = item as DataDailyInfoViewModel;
                pOutList.Add(datadaily);
            }
            foreach (var item in inlist)
            {
                DataDailyInfoViewModel datadaily = item as DataDailyInfoViewModel;
                pInList.Add(datadaily);
            }
            //获取工作簿
            ISheet sheet = readworkbook.GetSheetAt(0);
            //设置日期
            SetReportDate(sheet, 0, 8, DateTime.Parse(DateTime.Now.ToShortDateString()), para.ReportType);
            for (int i = 0; i < 4; i++)
            {
                string ptemp = i.ToString();
                int rnum = i;
                //表样不完全相同，需要更改赋值单元格行数,1和2行数相同,3和4行数相同
                if (para.ReportType == 3 || para.ReportType == 4)
                {
                    rnum = i - 1;
                }
                //0小型客车，1其他客车，2货车（不包含绿通），3绿通
                //出口
                SetValue(sheet, rnum + 4, 2, pOutList.Where(s => s.VehType == ptemp).Sum(s => s.VehNum).ToString());
                SetValue(sheet, rnum + 4, 4, pOutList.Where(s => s.VehType == ptemp).Sum(s => s.CarChag).ToString());
                //入口
                SetValue(sheet, rnum + 8, 2, pInList.Where(s => s.VehType == ptemp).Sum(s => s.VehNum).ToString());
            }
            return readworkbook;
        }
        /// <summary>
        /// 预测数据
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public CustomResult ForecastData(QueryParameters para)
        {
            CustomResult pReturnValue = new CustomResult();
            //浮动范围
            double pFloating = para.FloatingRange * 0.01;
            List<RP_Daily> pDailyList = new List<RP_Daily>();
            //预测数据集合
            List<IReportViewModel> pOutList = new List<IReportViewModel>();
            List<IReportViewModel> pInList = new List<IReportViewModel>();
            try
            {
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
                    List<RP_Daily> pRefDataDailyInfo = db.RP_Daily.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType).ToList();
                    //如果参考日期数据为0 则返回失败
                    if (pRefDataDailyInfo.Count == 0)
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.ForecastFaileFaileRefNoData;
                        return pReturnValue;
                    }

                    foreach (RP_Daily info in pRefDataDailyInfo)
                    {
                        DataDailyInfoViewModel pOutInfo = new DataDailyInfoViewModel();
                        DataDailyInfoViewModel pInInfo = new DataDailyInfoViewModel();
                        pInInfo.VehNum = (float)Math.Round((double)(info.InNum + info.InNum * pFloating), 0);
                        pOutInfo.VehNum = (float)Math.Round((double)(info.OutNum + info.OutNum * pFloating), 0);
                        pInInfo.VehType = info.VehType.ToString();
                        pOutInfo.VehType = info.VehType.ToString();
                        pOutInfo.CarChag = Math.Round((decimal)(info.ChagFee + info.ChagFee * (decimal)pFloating) / 10000, 2);
                        pOutList.Add(pOutInfo);
                        pInList.Add(pInInfo);
                    }
                    string path = Export(para, pOutList, pInList);
                    pReturnValue.ResultKey = (byte)EResult.Succeed;
                    pReturnValue.ResultValue = path;
                }

            }
            catch (Exception e)
            {
                pReturnValue.ResultKey = (byte)EResult.Fail;
                pReturnValue.ResultValue = TipInfo.ForecastFail + e.Message.ToString();
                SystemLog.GetInstance().Error(TipInfo.ForecastFail, e);
                return pReturnValue;
            }
            return pReturnValue;
        }
        /// <summary>
        /// 获取配置预测信息
        /// </summary>
        /// <returns></returns>
        public ForecastViewModel GetForecastWhere(QueryParameters para)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                var list = db.OT_HDayConfig.Where(a => a.Id == para.ReportType).Select(a => new ForecastViewModel
                {
                    ForecastDate = a.ForeDate,
                    ForecastFloat = a.ForeFloat
                }).ToList();

                ForecastViewModel model = new ForecastViewModel();
                if (list != null && list.Count > 0)
                {
                    model = list[0];
                }
                else
                {
                    model.ForecastDate = DateTime.Now.AddDays(-1);
                    model.ForecastFloat = 5;
                }
                return model;
            }
        }
        #endregion
        #region 11 Private Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="para"></param>
        /// <param name="pOutList">出口数据集合</param>
        /// <param name="pInList">入口数据集合</param>
        /// <returns></returns>
        private string Export(QueryParameters para, List<IReportViewModel> pOutList, List<IReportViewModel> pInList)
        {
            string path = string.Empty;
            string reportpath = string.Empty;
            if (para.ReportType == 1 && para.StationType == 1)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号01--收费公路数据汇总日报.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this, pOutList, pInList);
            }
            if (para.ReportType == 2 && para.StationType == 3)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号02--收费公路数据汇总日报.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this, pOutList, pInList);
            }
            if (para.ReportType == 3 && para.StationType == 15)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号03--京沪高速大羊坊收费站数据日报表.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this, pOutList, pInList);
            }
            if (para.ReportType == 4 && para.StationType == 33)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号04--重要运输通道主线收费站数据日报表（泗村店站）.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this, pOutList, pInList);
            }
            return reportpath;
        }
        /// <summary>
        /// 补充数据
        /// </summary>
        /// <param name="dt">统计日期</param>
        /// <param name="stationtype">统计类型</param>
        private void InsertNull(DateTime dt, int stationtype)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                List<RP_Daily> pDailyList = new List<RP_Daily>();
                for (int i = 0; i < 4; i++)
                {
                    //0小型客车，1其他客车，2货车（不包含绿通），3绿通
                    //每个站只有四条合计数据，每天数据表里一共只有16条
                    List<RP_Daily> pTemp = db.RP_Daily.Where(s => s.CalcuTime == dt && s.VehType == i && s.StaType == stationtype).ToList();
                    //如果数据不存在，才补充
                    if (pTemp.Count <= 0)
                    {
                        RP_Daily pDaily = new RP_Daily();

                        pDaily.Id = Guid.NewGuid();
                        pDaily.CrtDate = DateTime.Now;
                        pDaily.VehType = i;//车辆类型
                        pDaily.StaType = stationtype;
                        pDaily.State = 0;
                        pDaily.OutNum = 0;
                        pDaily.InNum = 0;
                        pDaily.ChagFee = 0;
                        pDaily.CalcuTime = DateTime.Parse(dt.ToShortDateString());
                        pDailyList.Add(pDaily);
                    }
                }
                using (TransactionScope transac = new TransactionScope())
                {
                    db.RP_Daily.AddRange(pDailyList);
                    if (pDailyList.Count > 0)
                    {
                        db.SaveChanges();
                        transac.Complete();
                    }
                }
            }
        }
        #endregion
    }
}

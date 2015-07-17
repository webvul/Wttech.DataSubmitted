/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/5 10:25:32
 */

#region 引用
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.IBLL.IReport;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.Common.Resources;
using System.Transactions;

#endregion

namespace Wttech.DataSubmitted.BLL.Report
{
    /// <summary>
    ///  报表8：假期交通量统计表
    /// </summary>
    public class HDayTraStatistical : ReportRelated, IHDayTraStatistical
    {
        #region 9 Public Methods
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public IReportViewModel GetListByPra(QueryParameters para)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                if (db.RP_AADTSta.Count(s => s.CalculTime == para.StartTime.Value) <= 0)
                {
                    InsertNull(para.StartTime.Value);
                }

                QueryHDayTraInfoViewModel pReturnData = new QueryHDayTraInfoViewModel();
                List<HDayTraInfoViewModel> pAADTStaList = db.RP_AADTSta.Where(s => s.CalculTime == para.StartTime.Value).ToList().Select(s => new HDayTraInfoViewModel()
                {
                    Num = 14,
                    LineSum = s.LineSum,
                    LineExSum = s.LineExSum,
                    LineEnSum = s.LineEnSum,
                    FeeSum = s.FeeSum,
                    SmaCarFeeNum = s.SmaCarFeeNum,
                    ExSmaCarFee = s.ExSmaCarFee,
                    EnSmaCarFee = s.EnSmaCarFee,
                    SmaCarFee = s.SmaCarFee,
                    ChagSumNum = s.ChagSumNum,
                    ExChagNum = s.ExChagNum,
                    EnChagNum = s.EnChagNum,
                    ChagAmount = s.ChagAmount,
                    TollStaName = SystemConst.TollStaName,
                    GreNum = s.GreNum,
                    GreFee = s.GreFee,
                    StaExSum = s.StaExSum,
                    StaEnSum = s.StaEnSum,
                    StaSum = s.StaEnSum + s.StaExSum,
                    WorkPeoNum = s.WorkPeoNum,
                    InfoNum = s.InfoNum,
                    SitState = s.SitState,
                    RoadName = SystemConst.RoadName2
                }).ToList();
                //添加合计
                List<HDayTraInfoViewModel> pAADTStaListSum = db.RP_AADTSta.Where(s => s.CalculTime == para.StartTime.Value).ToList().Select(s => new HDayTraInfoViewModel()
                {
                    LineSum = s.LineSum,
                    LineExSum = s.LineExSum,
                    LineEnSum = s.LineEnSum,
                    FeeSum = s.FeeSum,
                    SmaCarFeeNum = s.SmaCarFeeNum,
                    ExSmaCarFee = s.ExSmaCarFee,
                    EnSmaCarFee = s.EnSmaCarFee,
                    SmaCarFee = s.SmaCarFee,
                    ChagSumNum = s.ChagSumNum,
                    ExChagNum = s.ExChagNum,
                    EnChagNum = s.EnChagNum,
                    ChagAmount = s.ChagAmount,
                    GreNum = s.GreNum,
                    GreFee = s.GreFee,
                    StaExSum = s.StaExSum,
                    StaEnSum = s.StaEnSum,
                    StaSum = s.StaEnSum + s.StaExSum,
                    RoadName = SystemConst.ReportCount
                }).ToList();
                pAADTStaList.AddRange(pAADTStaListSum);
                if (pAADTStaList.Count == 0)//如果没有数据，进行添加
                {
                    for (int i = 0; i < 2; i++)
                    {
                        HDayTraInfoViewModel info = new HDayTraInfoViewModel();
                        info.RoadName = SystemConst.ReportCount;
                        if (i == 0)
                        {
                            info.RoadName = SystemConst.RoadName2;
                            info.TollStaName = SystemConst.TollStaName;
                            info.Num = 14;
                        }
                        pAADTStaList.Add(info);
                    }
                }
                else if (pAADTStaList.Count > 2)//如果大于两条，则进行删除
                {
                    pAADTStaList.RemoveRange(2, pAADTStaList.Count - 2);
                }
                //判断当前统计站类型，数据是否完整
                if (GetNoDataList(para).Count() > 0)
                    pReturnData.IsFull = 0;//不完整
                else
                    pReturnData.IsFull = 1;//完整 
                pReturnData.ReportData = pAADTStaList.OrderByDescending(s => s.Num).ToList();
                return pReturnData;
            }
        }
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CustomResult Update(UpdateHDayTraInfoViewModel args)
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
                //获取修改日期的数据
                var pReportData = db.RP_AADTSta.Where(s => s.CalculTime == args.DataDate).ToList();
                if (pReportData.Count > 0)
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        try
                        {
                            foreach (var item in args.DataInfo)
                            {
                                var pDataTemp = pReportData.SingleOrDefault();
                                pDataTemp.FeeSum = item.FeeSum;
                                pDataTemp.ExSmaCarFee = item.ExSmaCarFee;
                                pDataTemp.EnSmaCarFee = item.EnSmaCarFee;
                                pDataTemp.SmaCarFee = item.SmaCarFee;
                                pDataTemp.ExChagNum = item.ExChagNum;
                                pDataTemp.EnChagNum = item.EnChagNum;
                                pDataTemp.ChagAmount = item.ChagAmount;
                                pDataTemp.GreNum = item.GreNum;
                                pDataTemp.GreFee = item.GreFee;
                                pDataTemp.StaExSum = item.StaExSum;
                                pDataTemp.StaEnSum = item.StaEnSum;
                                pDataTemp.WorkPeoNum = item.WorkPeoNum;
                                pDataTemp.InfoNum = item.InfoNum;
                                pDataTemp.SitState = item.SitState;
                                pDataTemp.LineExSum = item.ExSmaCarFee + item.ExChagNum;
                                pDataTemp.LineEnSum = item.EnSmaCarFee + item.EnChagNum;
                                pDataTemp.LineSum = pDataTemp.LineExSum + pDataTemp.LineEnSum;
                                pDataTemp.UpdDate = DateTime.Now;
                                pDataTemp.State = "1";
                                if (SessionManage.GetLoginUser() != null)
                                {
                                    pDataTemp.UpdBy = SessionManage.GetLoginUser().UserName;
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
                    }
                }
                else
                {
                    pReturnValue.ResultKey = (byte)EResult.Fail;
                    pReturnValue.ResultValue = TipInfo.DataNull;
                }
                return pReturnValue;
            }
        }
        /// <summary>
        /// 更改Excel工作簿内容
        /// </summary>
        /// <param name="readworkbook"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public IWorkbook GenerateSheet(IWorkbook readworkbook, QueryParameters para)
        {
            //获取工作簿
            ISheet sheet = readworkbook.GetSheetAt(0);
            //设置日期
            SetReportDate(sheet, 1, 9, para.StartTime.Value, para.ReportType);
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //获取导出日期数据
                List<RP_AADTSta> pAADTList = db.RP_AADTSta.Where(s => s.CalculTime == para.StartTime).ToList();
                RP_AADTSta pInfo = null;
                if (pAADTList.Count > 0)
                {
                    pInfo = pAADTList.First();
                }
                if (pInfo != null)
                {
                    for (int i = 0; i < 2; i++)//报表8包括两条，其中一条为合计数据
                    {
                        if (pInfo.LineSum.HasValue)
                            SetValue(sheet, i + 5, 2, pInfo.LineSum.ToString());
                        if (pInfo.LineExSum.HasValue)
                            SetValue(sheet, i + 5, 3, pInfo.LineExSum.ToString());
                        if (pInfo.LineEnSum.HasValue)
                            SetValue(sheet, i + 5, 4, pInfo.LineEnSum.ToString());
                        if (pInfo.FeeSum.HasValue)
                            SetValue(sheet, i + 5, 5, pInfo.FeeSum.ToString());
                        if (pInfo.SmaCarFeeNum.HasValue)
                            SetValue(sheet, i + 5, 6, pInfo.SmaCarFeeNum.ToString());
                        if (pInfo.ExSmaCarFee.HasValue)
                            SetValue(sheet, i + 5, 7, pInfo.ExSmaCarFee.ToString());
                        if (pInfo.EnSmaCarFee.HasValue)
                            SetValue(sheet, i + 5, 8, pInfo.EnSmaCarFee.ToString());
                        if (pInfo.SmaCarFee.HasValue)
                            SetValue(sheet, i + 5, 9, pInfo.SmaCarFee.ToString());
                        if (pInfo.ChagSumNum.HasValue)
                            SetValue(sheet, i + 5, 10, pInfo.ChagSumNum.ToString());
                        if (pInfo.ExChagNum.HasValue)
                            SetValue(sheet, i + 5, 11, pInfo.ExChagNum.ToString());
                        if (pInfo.EnChagNum.HasValue)
                            SetValue(sheet, i + 5, 12, pInfo.EnChagNum.ToString());
                        if (pInfo.ChagAmount.HasValue)
                            SetValue(sheet, i + 5, 13, pInfo.ChagAmount.ToString());
                        if (pInfo.GreNum.HasValue)
                            SetValue(sheet, i + 5, 14, pInfo.GreNum.ToString());
                        if (pInfo.GreFee.HasValue)
                            SetValue(sheet, i + 5, 15, pInfo.GreFee.ToString());
                        //SetValue(sheet, i + 5, 16, pInfo.TollStaName.ToString());//收费站名称
                        if ((pInfo.StaExSum + pInfo.StaEnSum).HasValue)
                            SetValue(sheet, i + 5, 17, (pInfo.StaExSum + pInfo.StaEnSum).ToString());
                        if (pInfo.StaExSum.HasValue)
                            SetValue(sheet, i + 5, 18, pInfo.StaExSum.ToString());
                        if (pInfo.StaEnSum.HasValue)
                            SetValue(sheet, i + 5, 19, pInfo.StaEnSum.ToString());
                        if (i == 0)//合计
                        {
                            if (pInfo.WorkPeoNum.HasValue)
                                SetValue(sheet, i + 5, 20, pInfo.WorkPeoNum.ToString());
                            SetValue(sheet, i + 5, 21, pInfo.InfoNum);
                            SetValue(sheet, i + 5, 22, pInfo.SitState);
                        }
                    }
                }
            }
            return readworkbook;
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public string ExportReport(QueryParameters para)
        {
            string path = string.Empty;
            string reportpath = string.Empty;
            if (para.ReportType == 8)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号08--假期交通量统计表.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
            }
            return reportpath;
        }
        /// <summary>
        /// 校正
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public CustomResult CalibrationData(QueryParameters para)
        {
            CustomResult pReturnValue = new CustomResult();
            double pFloating = 1 + para.FloatingRange * 0.01;
            List<RP_AADTSta> pNaturalTraList = new List<RP_AADTSta>();
            try
            {
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
                        List<RP_AADTSta> pRefNaturalList = db.RP_AADTSta.Where(s => s.CalculTime == para.LastYearStart).ToList();
                        //如果参考日期数据为0 则返回失败
                        if (pRefNaturalList.Count == 0)
                        {
                            pReturnValue.ResultKey = (byte)EResult.Fail;
                            pReturnValue.ResultValue = TipInfo.CalibrationFaileRefNoData;
                            return pReturnValue;
                        }
                        //需要校正的数据
                        var pCheckNaturalList = db.RP_AADTSta.Where(s => s.CalculTime == para.StartTime).ToList();
                        //如果需要校正的数据为空则返回失败
                        if (pCheckNaturalList.Count == 0)
                        {
                            pReturnValue.ResultKey = (byte)EResult.Fail;
                            pReturnValue.ResultValue = TipInfo.CalibrationFaileNoData;
                            return pReturnValue;
                        }
                        using (TransactionScope tran = new TransactionScope())
                        {
                            //校正数据
                            RP_AADTSta pCheckInfo = pCheckNaturalList.First();
                            //参考数据
                            RP_AADTSta pRefInfo = pRefNaturalList.First();

                            //出京总交通量（路线）
                            if (pRefInfo.LineExSum.HasValue)
                                pCheckInfo.LineExSum = Math.Round(pRefInfo.LineExSum.Value * pFloating);
                            //进京总交通量（路线）
                            if (pRefInfo.LineEnSum.HasValue)
                                pCheckInfo.LineEnSum = Math.Round(pRefInfo.LineEnSum.Value * pFloating);
                            //总交通量（路线）
                            pCheckInfo.LineSum = pCheckInfo.LineExSum + pCheckInfo.LineEnSum;
                            //总交通量同比增幅
                            pCheckInfo.SumGrow = double.Parse(string.Format("{0:0.00}", (pCheckInfo.LineSum - pRefInfo.LineSum) / pRefInfo.LineSum));
                            //出进京比
                            pCheckInfo.ExEnPer = double.Parse(string.Format("{0:0.00}", pCheckInfo.LineExSum / pCheckInfo.LineEnSum));
                            //免、收费总金额
                            if (pRefInfo.FeeSum.HasValue)
                                pCheckInfo.FeeSum = Math.Round(pRefInfo.FeeSum.Value * (decimal)pFloating, 2);
                            //出京小型客车免费通行交通量
                            if (pRefInfo.FeeSum.HasValue)
                                pCheckInfo.ExSmaCarFee = Math.Round(pRefInfo.ExSmaCarFee.Value * pFloating);
                            //进京小型客车免费通行交通量
                            if (pRefInfo.FeeSum.HasValue)
                                pCheckInfo.EnSmaCarFee = Math.Round(pRefInfo.EnSmaCarFee.Value * pFloating);
                            //小型客车免费通行交通量（合计）
                            if (pRefInfo.ExSmaCarFee.HasValue)
                                pCheckInfo.SmaCarFeeNum = pRefInfo.ExSmaCarFee.Value + pRefInfo.EnSmaCarFee.Value;
                            //小型客车交通量同比增幅                          
                            pCheckInfo.SmaCarCompGrow = double.Parse(string.Format("{0:0.00}", (pCheckInfo.SmaCarFeeNum - pRefInfo.SmaCarFeeNum) / pRefInfo.SmaCarFeeNum));
                            //小型客车免费金额
                            if (pRefInfo.SmaCarFee.HasValue)
                                pCheckInfo.SmaCarFee = Math.Round(pRefInfo.SmaCarFee.Value * (decimal)pFloating, 2);
                            //收费车辆（合计）
                            if (pRefInfo.ChagSumNum.HasValue)
                                pCheckInfo.ChagSumNum = Math.Round(pRefInfo.ChagSumNum.Value * pFloating);
                            //出京收费车辆
                            if (pRefInfo.ExChagNum.HasValue)
                                pCheckInfo.ExChagNum = Math.Round(pRefInfo.ExChagNum.Value * pFloating);
                            //进京收费车辆
                            if (pRefInfo.EnChagNum.HasValue)
                                pCheckInfo.EnChagNum = Math.Round(pRefInfo.EnChagNum.Value * pFloating);
                            //收费额度
                            if (pRefInfo.ChagAmount.HasValue)
                                pCheckInfo.ChagAmount = Math.Round(pRefInfo.ChagAmount.Value * (decimal)pFloating, 2);
                            //绿色通道车辆数
                            if (pRefInfo.GreNum.HasValue)
                                pCheckInfo.GreNum = Math.Round(pRefInfo.GreNum.Value * pFloating);
                            //绿色通道免收费金额
                            if (pRefInfo.GreFee.HasValue)
                                pCheckInfo.GreFee = Math.Round(pRefInfo.GreFee.Value * (decimal)pFloating, 2);
                            //出京总交通量（站）
                            if (pRefInfo.StaExSum.HasValue)
                                pCheckInfo.StaExSum = Math.Round(pRefInfo.StaExSum.Value * pFloating);
                            //进京总交通量（站）
                            if (pRefInfo.StaEnSum.HasValue)
                                pCheckInfo.StaEnSum = Math.Round(pRefInfo.StaEnSum.Value * pFloating);
                            if (SessionManage.GetLoginUser() != null)
                            {
                                pCheckInfo.UpdBy = SessionManage.GetLoginUser().UserName;
                            }
                            pCheckInfo.UpdDate = DateTime.Now;
                            pCheckInfo.State = "1";

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
        /// 预测
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public CustomResult ForecastData(QueryParameters para)
        {
            CustomResult pReturnValue = new CustomResult();
            //浮动范围
            double pFloating = 1 + para.FloatingRange * 0.01;
            List<RP_AADTSta> pForeList = new List<RP_AADTSta>();
            //预测数据集合
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
                    List<RP_AADTSta> pRefInfoList = db.RP_AADTSta.Where(s => s.CalculTime == para.StartTime).ToList();
                    //如果参考日期数据为0 则返回失败
                    if (pRefInfoList.Count == 0)
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.ForecastFaileFaileRefNoData;
                        return pReturnValue;
                    }

                    //预测数据
                    HDayTraInfoViewModel pInfo = new HDayTraInfoViewModel();
                    //预测数据集合
                    List<IReportViewModel> plist = new List<IReportViewModel>();
                    //参考数据
                    RP_AADTSta pRefInfo = pRefInfoList.First();

                    //出京总交通量（路线）
                    pInfo.LineExSum = Math.Round(pRefInfo.LineExSum.Value * pFloating);
                    //进京总交通量（路线）
                    pInfo.LineEnSum = Math.Round(pRefInfo.LineEnSum.Value * pFloating);
                    //总交通量（路线）
                    pInfo.LineSum = pInfo.LineExSum + pInfo.LineEnSum;
                    //免、收费总金额
                    pInfo.FeeSum = Math.Round(pRefInfo.FeeSum.Value * (decimal)pFloating, 2);
                    //出京小型客车免费通行交通量
                    pInfo.ExSmaCarFee = Math.Round(pRefInfo.ExSmaCarFee.Value * pFloating);
                    //进京小型客车免费通行交通量
                    pInfo.EnSmaCarFee = Math.Round(pRefInfo.EnSmaCarFee.Value * pFloating);
                    //小型客车免费通行交通量（合计）
                    pInfo.SmaCarFeeNum = pRefInfo.ExSmaCarFee.Value + pRefInfo.EnSmaCarFee.Value;
                    //小型客车免费金额
                    pInfo.SmaCarFee = Math.Round(pRefInfo.SmaCarFee.Value * (decimal)pFloating, 2);
                    //收费车辆（合计）
                    pInfo.ChagSumNum = Math.Round(pRefInfo.ChagSumNum.Value * pFloating);
                    //出京收费车辆
                    pInfo.ExChagNum = Math.Round(pRefInfo.ExChagNum.Value * pFloating);
                    //进京收费车辆
                    pInfo.EnChagNum = Math.Round(pRefInfo.EnChagNum.Value * pFloating);
                    //收费额度
                    pInfo.ChagAmount = Math.Round(pRefInfo.ChagAmount.Value * (decimal)pFloating, 2);
                    //绿色通道车辆数
                    pInfo.GreNum = Math.Round(pRefInfo.GreNum.Value * pFloating);
                    //绿色通道免收费金额
                    pInfo.GreFee = Math.Round(pRefInfo.GreFee.Value * (decimal)pFloating, 2);
                    //出京总交通量（站）
                    pInfo.StaExSum = Math.Round(pRefInfo.StaExSum.Value * pFloating);
                    //进京总交通量（站）
                    pInfo.StaEnSum = Math.Round(pRefInfo.StaEnSum.Value * pFloating);
                    plist.Add(pInfo);
                    string path = Export(para, plist);
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
        ///  预测导出--如果需要将入出口数据进行区分，则分别放在两个list的集合中，若不需区分，则将数据放入list1中，list2为空即可
        /// </summary>
        /// <param name="readworkbook">待修改工作簿</param>
        /// <param name="para">参数类</param>
        /// <param name="list1">数据集合1</param>
        /// <param name="list2">数据集合2（可空）</param>
        /// <returns></returns>
        public IWorkbook GenerateSheet(IWorkbook readworkbook, QueryParameters para, List<IReportViewModel> list1, List<IReportViewModel> list2)
        {
            //获取工作簿
            ISheet sheet = readworkbook.GetSheetAt(0);
            //设置日期
            SetReportDate(sheet, 1, 8, DateTime.Parse(DateTime.Now.ToShortDateString()), para.ReportType);
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                HDayTraInfoViewModel pInfo = null;
                if (list1.Count > 0)
                {
                    pInfo = list1.First() as HDayTraInfoViewModel;
                }
                if (pInfo != null)
                {
                    for (int i = 0; i < 2; i++)//报表8包括两条，其中一条为合计数据
                    {
                        if (pInfo.LineSum.HasValue)
                            SetValue(sheet, i + 5, 2, pInfo.LineSum.ToString());
                        if (pInfo.LineExSum.HasValue)
                            SetValue(sheet, i + 5, 3, pInfo.LineExSum.ToString());
                        if (pInfo.LineEnSum.HasValue)
                            SetValue(sheet, i + 5, 4, pInfo.LineEnSum.ToString());
                        if (pInfo.FeeSum.HasValue)
                            SetValue(sheet, i + 5, 5, pInfo.FeeSum.ToString());
                        if (pInfo.SmaCarFeeNum.HasValue)
                            SetValue(sheet, i + 5, 6, pInfo.SmaCarFeeNum.ToString());
                        if (pInfo.ExSmaCarFee.HasValue)
                            SetValue(sheet, i + 5, 7, pInfo.ExSmaCarFee.ToString());
                        if (pInfo.EnSmaCarFee.HasValue)
                            SetValue(sheet, i + 5, 8, pInfo.EnSmaCarFee.ToString());
                        if (pInfo.SmaCarFee.HasValue)
                            SetValue(sheet, i + 5, 9, pInfo.SmaCarFee.ToString());
                        if (pInfo.ChagSumNum.HasValue)
                            SetValue(sheet, i + 5, 10, pInfo.ChagSumNum.ToString());
                        if (pInfo.ExChagNum.HasValue)
                            SetValue(sheet, i + 5, 11, pInfo.ExChagNum.ToString());
                        if (pInfo.EnChagNum.HasValue)
                            SetValue(sheet, i + 5, 12, pInfo.EnChagNum.ToString());
                        if (pInfo.ChagAmount.HasValue)
                            SetValue(sheet, i + 5, 13, pInfo.ChagAmount.ToString());
                        if (pInfo.GreNum.HasValue)
                            SetValue(sheet, i + 5, 14, pInfo.GreNum.ToString());
                        if (pInfo.GreFee.HasValue)
                            SetValue(sheet, i + 5, 15, pInfo.GreFee.ToString());
                        //SetValue(sheet, i + 5, 16, pInfo.TollStaName.ToString());//收费站名称
                        if ((pInfo.StaExSum + pInfo.StaEnSum).HasValue)
                            SetValue(sheet, i + 5, 17, (pInfo.StaExSum + pInfo.StaEnSum).ToString());
                        if (pInfo.StaExSum.HasValue)
                            SetValue(sheet, i + 5, 18, pInfo.StaExSum.ToString());
                        if (pInfo.StaEnSum.HasValue)
                            SetValue(sheet, i + 5, 19, pInfo.StaEnSum.ToString());
                        if (i == 0)//合计
                        {
                            if (pInfo.WorkPeoNum.HasValue)
                                SetValue(sheet, i + 5, 20, pInfo.WorkPeoNum.ToString());
                            SetValue(sheet, i + 5, 21, pInfo.InfoNum);
                            SetValue(sheet, i + 5, 22, pInfo.SitState);
                        }
                    }
                }
            }
            return readworkbook;
        }
        /// <summary>
        /// 获取配置预测信息
        /// </summary>
        /// <returns></returns>
        public ForecastViewModel GetForecastWhere()
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                var list = db.OT_HDayConfig.Where(a => a.Id == 8).Select(a => new ForecastViewModel
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
        private string Export(QueryParameters para, List<IReportViewModel> list)
        {
            string path = string.Empty;
            string reportpath = string.Empty;
            if (para.ReportType == 8)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号08--假期交通量统计表.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this, list, null);
            }
            return reportpath;
        }
        /// <summary>
        /// 补充数据
        /// </summary>
        /// <param name="dt">统计日期</param>
        private void InsertNull(DateTime dt)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                RP_AADTSta info = new RP_AADTSta();
                info.Id = Guid.NewGuid();
                info.CrtDate = DateTime.Now;
                info.State = "0";
                info.CalculTime = dt;
                info.LineSum = 0;
                info.SumGrow = -1;
                info.LineExSum = 0;
                info.LineEnSum = 0;
                info.ExEnPer = 0;
                info.FeeSum = 0;
                info.SmaCarFeeNum = 0;
                info.ExSmaCarFee = 0;
                info.EnSmaCarFee = 0;
                info.SmaCarFee = 0;
                info.SmaCarCompGrow = 0;
                info.ChagSumNum = 0;
                info.ExChagNum = 0;
                info.EnChagNum = 0;
                info.ChagAmount = 0;
                info.GreNum = 0;
                info.GreFee = 0;
                info.StaExSum = 0;
                info.StaEnSum = 0;

                using (TransactionScope transac = new TransactionScope())
                {
                    db.RP_AADTSta.Add(info);
                    db.SaveChanges();
                    transac.Complete();
                }
            }
        }
        #endregion
    }
}

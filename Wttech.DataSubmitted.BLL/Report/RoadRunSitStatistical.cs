/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：报表9：收费公路运行情况统计类文件
 * 创建标识：ta0395侯兴鼎20141209
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
    /// 报表9：收费公路运行情况统计类
    /// </summary>
    public class RoadRunSitStatistical : ReportRelated, IRoadRunSitStatistical
    {
        #region 3 Fields

        /// <summary>
        /// 查询数据缓存，导出时直接使用，而无需再到数据库中查
        /// </summary>
        List<RoadRunSitViewModel> listExport = new List<RoadRunSitViewModel>();

        /// <summary>
        /// 查询结果实体
        /// </summary>
        RoadRunSitListViewModel modelRRS = new RoadRunSitListViewModel(); 

        #endregion

        #region 9 Public Methods

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public IReportViewModel GetListByPra(QueryParameters para)
        {
            modelRRS.RoadRunSit =  GetData(para);
            return modelRRS;
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
            if (para.ReportType == 9 && para.StationType == 1)//表9
            {
                path = string.Format(@"{0}Reporttemplate\编号09--收费公路运行情况统计表.xlsx", AppDomain.CurrentDomain.BaseDirectory);
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
            }
            return reportpath;
        }

        /// <summary>
        /// 预测
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public CustomResult ForecastData(QueryParameters para)
        {
            if (listExport != null)
                listExport.Clear();
            CustomResult pReturnValue = new CustomResult();
            //浮动范围
            double pFloating = 1 + para.FloatingRange * 0.01;
            List<RoadRunSitViewModel> pForeList = new List<RoadRunSitViewModel>();
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
                    if (pRefInfoList==null||pRefInfoList.Count == 0)
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.ForecastFaileFaileRefNoData;
                        return pReturnValue;
                    }

                    //预测数据
                    RoadRunSitViewModel info = new RoadRunSitViewModel();
                    //预测数据集合
                    List<IReportViewModel> plist = new List<IReportViewModel>();

                    //参考数据
                    foreach (RP_AADTSta pRefInfo in pRefInfoList)
                    {
                        //出京总交通量（路线）
                        info.LineExSum = Math.Round(pRefInfo.LineExSum == null ? 0 : pRefInfo.LineExSum.Value * pFloating, 2);
                        //进京总交通量（路线）
                        info.LineEnSum = Math.Round(pRefInfo.LineEnSum == null ? 0 : pRefInfo.LineEnSum.Value * pFloating, 2);
                        //总交通量（路线）
                        info.LineSum = info.LineExSum + info.LineEnSum;
                        //小型客车免费通行交通量（合计）
                        info.SmaCarFeeNum = Math.Round(pRefInfo.ExSmaCarFee == null ? 0 : pRefInfo.ExSmaCarFee.Value + pRefInfo.EnSmaCarFee == null ? 0 : pRefInfo.EnSmaCarFee.Value, 2);
                        //小型客车免费金额
                        info.SmaCarFee = Math.Round(pRefInfo.SmaCarFee == null ? 0 : pRefInfo.SmaCarFee.Value * (decimal)pFloating, 2);
                        //收费车辆（合计）
                        info.ChagSumNum = Math.Round(pRefInfo.ChagSumNum == null ? 0 : pRefInfo.ChagSumNum.Value * pFloating, 2);
                        //数据日期
                        info.CalculTime = (pRefInfo.CalculTime).Month + "月" + (pRefInfo.CalculTime).Day + "日";
                        //出进京比“出进京比”=出京交通量/进京交通量，保留两位小数。
                        if (info.LineEnSum != 0)
                        {
                            info.ExEnPer = Math.Round(info.LineExSum / info.LineEnSum, 2);
                        }
                        //“同比增幅”=（本年数据-去年数据）/去年数据*100%，保留两位小数。

                        //小型客车交通量同比增幅
                        info.SmaCarCompGrow = pRefInfo.SmaCarCompGrow == null ? "0.00" : Math.Round(pRefInfo.SmaCarCompGrow.Value, 2).ToString();
                        //总交通量同比增幅
                        info.SumGrow = pRefInfo.SumGrow == null ? "0.00" : Math.Round(pRefInfo.SumGrow.Value, 2).ToString();

                        plist.Add(info);
                    }
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
        /// 获取预测条件默认值
        /// </summary>
        /// <returns></returns>
        public ForecastViewModel GetForecastWhere()
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                var list = db.OT_HDayConfig.Where(a => a.Id == 9).Select(a => new ForecastViewModel
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

        /// <summary>
        ///  更改Excel工作簿内容-导出按钮导出使用
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
                //设置日期
                SetReportDate(sheet, 1, 6, para.StartTime.Value, para.ReportType);

                if (listExport == null || listExport.Count == 0)
                {
                    GetData(para);
                }

                //获取导出日期数据
                if (listExport != null)
                {
                    for (int i = 0; i < listExport.Count; i++)
                    {
                        SetValue(sheet, i + 3, 0, listExport[i].CalculTime);//数据日期
                        SetValue(sheet, i + 3, 1, listExport[i].LineSum.ToString("F2"));//总交通量（万辆）
                        SetValue(sheet, i + 3, 2, (double.Parse(listExport[i].SumGrow)).ToString("F2"));//同比增幅（%）
                        SetValue(sheet, i + 3, 3, listExport[i].LineEnSum.ToString("F2"));//进京交通量（万辆）
                        SetValue(sheet, i + 3, 4, listExport[i].LineExSum.ToString("F2"));//出京交通量（万辆）
                        SetValue(sheet, i + 3, 5, listExport[i].ExEnPer.ToString("F2")); //出进京比	
                        SetValue(sheet, i + 3, 6, listExport[i].SmaCarFeeNum.ToString("F2"));//小型客车交通量（万辆）	
                        SetValue(sheet, i + 3, 7, (double.Parse(listExport[i].SmaCarCompGrow)).ToString("F2"));//同比增幅（%）	
                        SetValue(sheet, i + 3, 8, listExport[i].SmaCarFee.ToString("F2")); //小型客车免收通行费（万元）	
                        SetValue(sheet, i + 3, 9, listExport[i].ChagSumNum.ToString("F2"));  //收费车辆（万辆）
                    }
                }
            }
            return readworkbook;
        }

        /// <summary>
        /// 获取查询条件默认值
        /// </summary>
        /// <returns></returns>
        public ConfigTimeViewModel GetRoadRunSitWhere()
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                var list = db.OT_HDayConfig.Where(a => a.Id == 9).Select(a => new ConfigTimeViewModel
                {
                    HolidayStartTime = a.HDayStart,
                    HolidayEndTime = a.HDayEnd,
                    ComparedStartTime = a.CompStart,
                    ComparedEndTime = a.CompEnd
                }).ToList();

                ConfigTimeViewModel model = new ConfigTimeViewModel();
                if (list != null && list.Count > 0)
                    model = list[0];
                return model;
            }
        }

        /// <summary>
        /// 预测导出使用
        /// </summary>
        /// <param name="readworkbook"></param>
        /// <param name="para"></param>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public IWorkbook GenerateSheet(IWorkbook readworkbook, QueryParameters para, List<IReportViewModel> list1, List<IReportViewModel> list2)
        {
            //获取工作簿
            if (readworkbook != null)
            {
                ISheet sheet = readworkbook.GetSheetAt(0);
                //设置日期
                SetReportDate(sheet, 1, 6, DateTime.Parse(DateTime.Now.ToShortDateString()), para.ReportType);
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    if (list1 != null && list1.Count > 0)
                    {
                        RoadRunSitViewModel pInfo = list1.First() as RoadRunSitViewModel;
                        for (int i = 0; i < list1.Count; i++)
                        {
                            RoadRunSitViewModel model = (RoadRunSitViewModel)list1[i];
                            SetValue(sheet, i + 3, 0, model.CalculTime);//数据日期
                            SetValue(sheet, i + 3, 1, model.LineSum.ToString("F2"));//总交通量（万辆）
                            SetValue(sheet, i + 3, 2, (double.Parse(model.SumGrow)).ToString("F2"));//同比增幅（%）
                            SetValue(sheet, i + 3, 3, model.LineEnSum.ToString("F2"));//进京交通量（万辆）
                            SetValue(sheet, i + 3, 4, model.LineExSum.ToString("F2"));//出京交通量（万辆）
                            SetValue(sheet, i + 3, 5, model.ExEnPer.ToString("F2")); //出进京比	
                            SetValue(sheet, i + 3, 6, model.SmaCarFeeNum.ToString("F2"));//小型客车交通量（万辆）	
                            SetValue(sheet, i + 3, 7, (double.Parse(model.SmaCarCompGrow)).ToString("F2"));//同比增幅（%）	
                            SetValue(sheet, i + 3, 8, model.SmaCarFee.ToString("F2")); //小型客车免收通行费（万元）	
                            SetValue(sheet, i + 3, 9, model.ChagSumNum.ToString("F2"));  //收费车辆（万辆）
                        }
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
                    DateTime dtime = para.StartTime.Value;

                    //时间小于当前时间
                    if (DateTime.Now > dtime)
                    {
                        bool flag = StrWhere(db, para).Select(a => a.CalculTime).ToList().Contains(dtime);
                        if (!flag)//补数据
                        {
                            RP_AADTSta model = new RP_AADTSta();
                            model.CalculTime = dtime;
                            if (SessionManage.GetLoginUser() != null)
                            {
                                model.CrtBy = SessionManage.GetLoginUser().UserName;
                            }
                            model.CrtDate = DateTime.Now;
                            model.Id = Guid.NewGuid();
                            model.ChagSumNum = 0.00;
                            model.ExEnPer = 0.00;
                            model.LineEnSum = 0.00;
                            model.LineExSum = 0.00;
                            model.LineSum = 0.00;
                            model.SmaCarCompGrow = 0.00;
                            model.SmaCarFee = (decimal)0.00;
                            model.SmaCarFeeNum = 0.00;
                            model.SumGrow = 0.00;
                            model.State = "0";
                            db.RP_AADTSta.Add(model);
                        }
                        else//将数据中有空值的改成0
                        {
                            var model = db.RP_AADTSta.Where(a => a.CalculTime == dtime).ToList()[0];
                            if (model.ChagSumNum == null)
                                model.ChagSumNum = 0.00;
                            if (model.ExEnPer == null)
                                model.ExEnPer = 0.00;
                            if (model.LineEnSum == null)
                                model.LineEnSum = 0.00;
                            if (model.LineExSum == null)
                                model.LineExSum = 0.00;
                            if (model.LineSum == null)
                                model.LineSum = 0.00;
                            if (model.SmaCarCompGrow == null)
                                model.SmaCarCompGrow = 0.00;
                            if (model.SmaCarFee == null)
                                model.SmaCarFee = (decimal)0.00;
                            if (model.SmaCarFeeNum == null)
                                model.SmaCarFeeNum = 0.00;
                            if (model.SumGrow == null)
                                model.SumGrow = 0.00;
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

        /// <summary>
        /// 预测导出功能
        /// </summary>
        /// <param name="para"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private string Export(QueryParameters para, List<IReportViewModel> list)
        {
            string path = string.Empty;
            string reportpath = string.Empty;
            if (para.ReportType == 9)
            {
                path = string.Format(@"{0}Reporttemplate\编号09--收费公路运行情况统计表.xlsx", AppDomain.CurrentDomain.BaseDirectory);
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this, list, null);
            }
            return reportpath;
        }

        /// <summary>
        /// 查询条件组合查询语句
        /// </summary>
        /// <param name="db"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        private IQueryable<RP_AADTSta> StrWhere(DataSubmittedEntities db, Common.QueryParameters para)
        {
            var strWhere = db.RP_AADTSta.Where(a => true);

            if (para.StartTime != DateTime.Parse("0001/1/1 0:00:00") && para.StartTime != null)
            {
                DateTime dtStart = (DateTime)para.StartTime,
                    dtEnd = ((DateTime)para.StartTime).AddDays(1);
                strWhere = strWhere.Where(a => a.CalculTime >= dtStart & a.CalculTime < dtEnd);

            }
            //获取配置中的默认日期的开始日期
            else
            {
                var config = db.OT_HDayConfig.Where(a => a.Id == 9).Select(a => new
                {
                    HDayEnd = (DateTime)a.HDayEnd,
                    HDayStart = (DateTime)a.HDayStart
                }).ToList();
                if (config != null & config.Count > 0)
                {
                    DateTime dtStart = config[0].HDayStart,
                        dtEnd = ((DateTime)config[0].HDayStart).AddDays(1);

                    strWhere = strWhere.Where(a => a.CalculTime >= dtStart & a.CalculTime < dtEnd);
                }
            }
            return strWhere;
        }

        /// <summary>
        /// 获取和构造数据
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private List<RoadRunSitViewModel> GetData(QueryParameters para)
        {
            RepairData(para);//补数据

            List<RoadRunSitViewModel> list;
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                list = StrWhere(db, para).OrderBy(a => a.CalculTime).Select(a => new RoadRunSitViewModel
                {
                    CalculTime = ((DateTime)a.CalculTime).Month + "月" + ((DateTime)a.CalculTime).Day + "日",
                    ChagSumNum = Math.Round((double)a.ChagSumNum / 10000, 2),
                    ExEnPer = Math.Round((double)a.ExEnPer, 2),
                    LineEnSum = Math.Round((double)a.LineEnSum / 10000, 2),
                    LineExSum = Math.Round((double)a.LineExSum / 10000, 2),
                    LineSum = Math.Round((double)a.LineSum / 10000, 2),
                    SmaCarCompGrow = a.SmaCarCompGrow.Value == null ? "0.00" : Math.Round(a.SmaCarCompGrow.Value, 2).ToString(),
                    SmaCarFee = Math.Round((decimal)a.SmaCarFee, 2),
                    SmaCarFeeNum = Math.Round((double)a.SmaCarFeeNum / 10000, 2),
                    SumGrow = a.SumGrow.Value == null ? "0.00" : Math.Round(a.SumGrow.Value, 2).ToString(),
                }).ToList();

                //查询后无数据则按照查询时间构造数据
                if (list == null || list.Count == 0)
                {
                    RoadRunSitViewModel model = new RoadRunSitViewModel();
                    model.CalculTime = ((DateTime)para.StartTime).ToString("M月d日");
                    model.ChagSumNum = 0.00;
                    model.ExEnPer = 0.00;
                    model.LineEnSum = 0.00;
                    model.LineExSum = 0.00;
                    model.LineSum = 0.00;
                    model.SmaCarCompGrow = "0.00";
                    model.SmaCarFee = (decimal)0.00;
                    model.SmaCarFeeNum = 0.00;
                    model.SumGrow = "0.00";
                    list.Add(model);
                }

                listExport.Clear();
                listExport.AddRange(list);
            }
            return list;
        }

        #endregion
    }
}

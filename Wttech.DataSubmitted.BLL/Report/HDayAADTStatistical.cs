/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/9 17:04:56
 */

#region 引用
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.Resources;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL.IReport;

#endregion

namespace Wttech.DataSubmitted.BLL.Report
{
    /// <summary>
    /// 报表18业务类
    /// </summary>
    public class HDayAADTStatistical : ReportRelated, IHDayAADTStatistical
    {

        #region 9 Public Methods
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public IReportViewModel GetListByPra(QueryParameters para)
        {
            QueryHDayAADTViewModel pReturnData = new QueryHDayAADTViewModel();
            try
            {
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    InsertNull(para.StartTime.Value);
                    //获取报表18查询集合
                    List<HDayAADTViewModel> pHdayAADT = db.RP_HDayAADTSta.Where(s => s.CalcuTime == para.StartTime).Select(s => new HDayAADTViewModel()
                    {
                        LineType = s.LineType,
                        ExNat = s.ExNat,
                        EnNat = s.EnNat,
                        NatSum = s.ExNat + s.EnNat,
                        ExEqu = s.ExEqu,
                        EnEqu = s.EnEqu,
                        EquSum = s.ExEqu + s.EnEqu,
                        CrowDeg = s.CrowDeg,
                        SmaEx = s.SmaEx,
                        SmaEn = s.SmaEn,
                        SmaSum = s.SmaEx + s.SmaEn,
                        MedEn = s.MedEn,
                        MedEx = s.MedEx,
                        MedSum = s.MedEx + s.MedEn,
                        LarEx = s.LarEx,
                        LarEn = s.LarEn,
                        LarSum = s.LarEn + s.LarEx,
                        HeaEn = s.HeaEn,
                        HeaEx = s.HeaEx,
                        HeaSum = s.HeaEx + s.HeaEn,
                        SupEn = s.SupEn,
                        SupEx = s.SupEx,
                        SupSum = s.SupEn + s.SupEx,
                        EnExTrukNum = s.EnExTrukNum,
                        CarTrukPer = s.CarTrukPer,
                        SupTruNum = s.SupTruNum,
                        SupTruPer = s.SupTruPer
                    }).ToList();
                    if (pHdayAADT.Count > 0)
                    {
                        pReturnData.IsEdit = 1;
                        pReturnData.ReportData = pHdayAADT;
                    }
                    else
                    {
                        pReturnData.IsEdit = 0;
                        for (int i = 1; i < 7; i++)//添加6条空数据
                        {
                            HDayAADTViewModel pHdayInfo = new HDayAADTViewModel();
                            pHdayInfo.LineType = i;
                            pHdayAADT.Add(pHdayInfo);
                        }
                    }
                    //添加第一条(G2)
                    HDayAADTViewModel pHdayInfofirst = pHdayAADT.Where(s => s.LineType == 3).SingleOrDefault();
                    pHdayAADT.Add(GetFirst(pHdayInfofirst));
                    //升序排序
                    pReturnData.ReportData = pHdayAADT.OrderBy(s => s.Sorting).ToList();
                    //判断当前统计站类型，数据是否完整
                    if (GetNoDataList(para).Count() > 0)
                        pReturnData.IsFull = 0;//不完整
                    else
                        pReturnData.IsFull = 1;//完整 
                    return pReturnData;
                }
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
                return pReturnData;
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public string ExportReport(QueryParameters para)
        {
            string path = string.Empty;
            string reportpath = string.Empty;
            if (para.ReportType == 18)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号18--假期高速公路交通流量统计表.xlsx";
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

            List<RP_HDayAADTSta> pNaturalTraList = new List<RP_HDayAADTSta>();
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
                        List<RP_HDayAADTSta> pRefNaturalList = db.RP_HDayAADTSta.Where(s => s.CalcuTime == para.LastYearStart).ToList();
                        //如果参考日期数据为0 则返回失败
                        if (pRefNaturalList.Count == 0)
                        {
                            pReturnValue.ResultKey = (byte)EResult.Fail;
                            pReturnValue.ResultValue = TipInfo.CalibrationFaileRefNoData;
                            return pReturnValue;
                        }
                        //需要校正的数据
                        var pCheckNaturalList = db.RP_HDayAADTSta.Where(s => s.CalcuTime == para.StartTime).ToList();
                        //如果需要校正的数据为空则返回失败
                        if (pCheckNaturalList.Count == 0)
                        {
                            pReturnValue.ResultKey = (byte)EResult.Fail;
                            pReturnValue.ResultValue = TipInfo.CalibrationFaileNoData;
                            return pReturnValue;
                        }
                        using (TransactionScope tran = new TransactionScope())
                        {

                            ////校正数据
                            //RP_HDayAADTSta pCheckInfo = pCheckNaturalList.First();
                            ////参考数据
                            //RP_HDayAADTSta pRefInfo = pRefNaturalList.First();

                            foreach (RP_HDayAADTSta pCheckInfo in pCheckNaturalList)//校正数据
                            {
                                foreach (RP_HDayAADTSta pRefInfo in pRefNaturalList)//参考数据
                                {
                                    if (pRefInfo.LineType != 0 && pCheckInfo.LineType == pRefInfo.LineType)
                                    {
                                        //出京自然交通辆
                                        if (pRefInfo.ExNat != null)
                                            pCheckInfo.ExNat = Math.Round(pRefInfo.ExNat.Value * pFloating);
                                        //进京自然交通辆
                                        if (pRefInfo.EnNat != null)
                                            pCheckInfo.EnNat = Math.Round(pRefInfo.EnNat.Value * pFloating);
                                        //出京当量交通辆
                                        if (pRefInfo.ExEqu != null)
                                            pCheckInfo.ExEqu = Math.Round(pRefInfo.ExEqu.Value * pFloating);
                                        //进京当量交通辆
                                        if (pRefInfo.EnEqu != null)
                                            pCheckInfo.EnEqu = Math.Round(pRefInfo.EnEqu.Value * pFloating);
                                        //拥挤度-“拥挤度”=交通量（当量交通量）合计/设计交通量，保留四位小数；
                                        if (pCheckInfo.ExEqu != null && pCheckInfo.EnEqu != null)
                                        {
                                            HDayAADTViewModel model = new HDayAADTViewModel();
                                            model.LineType = pRefInfo.LineType;
                                            pCheckInfo.CrowDeg = double.Parse(string.Format("{0:0.0000}", (pCheckInfo.ExEqu + pCheckInfo.EnEqu) / model.DeTra));
                                        }
                                        //小型车出京
                                        if (pRefInfo.SmaEx != null)
                                            pCheckInfo.SmaEx = Math.Round(pRefInfo.SmaEx.Value * pFloating);
                                        //小型车进京
                                        if (pRefInfo.SmaEn != null)
                                            pCheckInfo.SmaEn = Math.Round(pRefInfo.SmaEn.Value * pFloating);
                                        //中型车出京
                                        if (pRefInfo.MedEx != null)
                                            pCheckInfo.MedEx = Math.Round(pRefInfo.MedEx.Value * pFloating);
                                        //中型车进京
                                        if (pRefInfo.MedEn != null)
                                            pCheckInfo.MedEn = Math.Round(pRefInfo.MedEn.Value * pFloating);
                                        //大型车出京
                                        if (pRefInfo.LarEx != null)
                                            pCheckInfo.LarEx = Math.Round(pRefInfo.LarEx.Value * pFloating);
                                        //大型车进京
                                        if (pRefInfo.LarEn != null)
                                            pCheckInfo.LarEn = Math.Round(pRefInfo.LarEn.Value * pFloating);
                                        //重型车出京
                                        if (pRefInfo.HeaEx != null)
                                            pCheckInfo.HeaEx = Math.Round(pRefInfo.HeaEx.Value * pFloating);
                                        //重型车进京
                                        if (pRefInfo.HeaEn != null)
                                            pCheckInfo.HeaEn = Math.Round(pRefInfo.HeaEn.Value * pFloating);
                                        //超大型车出京
                                        if (pRefInfo.SupEx != null)
                                            pCheckInfo.SupEx = Math.Round(pRefInfo.SupEx.Value * pFloating);
                                        //超大型车进京
                                        if (pRefInfo.SupEn != null)
                                            pCheckInfo.SupEn = Math.Round(pRefInfo.SupEn.Value * pFloating);
                                        //进出京大货车以上车型数量-进出京大货车以上车型数量”=大型车（合计）+重型车（合计）+超大型车（合计）。
                                        if (pCheckInfo.LarEx != null && pCheckInfo.LarEn != null && pCheckInfo.HeaEx != null && pCheckInfo.HeaEn != null && pCheckInfo.SupEx != null && pCheckInfo.SupEn != null)
                                            pCheckInfo.SupTruNum = pCheckInfo.LarEx + pCheckInfo.LarEn + pCheckInfo.HeaEx + pCheckInfo.HeaEn + pCheckInfo.SupEx + pCheckInfo.SupEn;
                                        //进出京货车数量
                                        if (pRefInfo.EnExTrukNum != null)
                                            pCheckInfo.EnExTrukNum = Math.Round(pRefInfo.EnExTrukNum.Value * pFloating);
                                        //客车货车比例-客车货车比例=（交通量（自然交通量）合计-进出京货车数量)/进出京货车数量*100%，保留四位小数；
                                        if (pCheckInfo.ExNat != null && pCheckInfo.EnNat != null && pCheckInfo.EnExTrukNum != null && pCheckInfo.EnExTrukNum != null && pCheckInfo.EnExTrukNum != 0)
                                        {
                                            pCheckInfo.CarTrukPer = double.Parse(string.Format("{0:0.0000}", (pCheckInfo.ExNat + pCheckInfo.EnNat - pCheckInfo.EnExTrukNum) / pCheckInfo.EnExTrukNum));
                                        }
                                        //大货车以上占货车交通量比例-大货车以上占货车交通量比例（%）=进出京大货车以上车型的数量/进出京货车数量*100%，保留四位小数。
                                        if (pCheckInfo.SupTruNum != null && pCheckInfo.EnExTrukNum != null && pCheckInfo.EnExTrukNum != 0)
                                        {
                                            pCheckInfo.SupTruPer = double.Parse(string.Format("{0:0.0000}", pCheckInfo.SupTruNum / pCheckInfo.EnExTrukNum));
                                        }
                                        if (SessionManage.GetLoginUser() != null)
                                        {
                                            pCheckInfo.UpdBy = SessionManage.GetLoginUser().UserName;
                                        }
                                        pCheckInfo.UpdDate = DateTime.Now;
                                        pCheckInfo.State = "1";
                                        break;
                                    }
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
        /// 修改工作簿内容
        /// </summary>
        /// <param name="readworkbook"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public IWorkbook GenerateSheet(IWorkbook readworkbook, QueryParameters para)
        {
            //获取工作簿
            ISheet sheet = readworkbook.GetSheetAt(0);
            //设置日期
            if (para.ReportType == 18)
            {
                SetReportDate(sheet, 1, 14, para.StartTime.Value, para.ReportType);
            }
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //设置表头
                var holidayName = db.OT_Dic.Where(a => a.Id == para.HolidayId).Select(a => a.Name).ToList();
                if (!string.IsNullOrEmpty(holidayName[0]))
                    SetValue(sheet, 0, 0, string.Format("{0}假期高速公路交通流量统计表", holidayName[0]));
                //数据集合
                List<HDayAADTViewModel> pHdayAADT = db.RP_HDayAADTSta.Where(s => s.CalcuTime == para.StartTime).Select(s => new HDayAADTViewModel()
                {
                    LineType = s.LineType,
                    ExNat = s.ExNat,
                    EnNat = s.EnNat,
                    NatSum = s.ExNat + s.EnNat,
                    ExEqu = s.ExEqu,
                    EnEqu = s.EnEqu,
                    EquSum = s.ExEqu + s.EnEqu,
                    CrowDeg = s.CrowDeg,
                    SmaEx = s.SmaEx,
                    SmaEn = s.SmaEn,
                    SmaSum = s.SmaEx + s.SmaEn,
                    MedEn = s.MedEn,
                    MedEx = s.MedEx,
                    MedSum = s.MedEx + s.MedEn,
                    LarEx = s.LarEx,
                    LarEn = s.LarEn,
                    LarSum = s.LarEn + s.LarEx,
                    HeaEn = s.HeaEn,
                    HeaEx = s.HeaEx,
                    HeaSum = s.HeaEx + s.HeaEn,
                    SupEn = s.SupEn,
                    SupEx = s.SupEx,
                    SupSum = s.SupEn + s.SupEx,
                    EnExTrukNum = s.EnExTrukNum,
                    CarTrukPer = s.CarTrukPer,
                    SupTruNum = s.SupTruNum,
                    SupTruPer = s.SupTruPer
                }).ToList().OrderBy(s => s.Sorting).ToList();
                if (pHdayAADT.Count > 0)
                {
                    for (int i = 0; i < pHdayAADT.Count; i++)
                    {
                        SetValue(sheet, i + 5, 7, pHdayAADT[i].NatSum.ToString());
                        SetValue(sheet, i + 5, 8, pHdayAADT[i].ExNat.ToString());
                        SetValue(sheet, i + 5, 9, pHdayAADT[i].EnNat.ToString());
                        SetValue(sheet, i + 5, 10, pHdayAADT[i].EquSum.ToString());
                        SetValue(sheet, i + 5, 11, pHdayAADT[i].ExEqu.ToString());
                        SetValue(sheet, i + 5, 12, pHdayAADT[i].EnEqu.ToString());
                        SetValue(sheet, i + 5, 14, pHdayAADT[i].CrowDeg.ToString());
                        SetValue(sheet, i + 5, 15, pHdayAADT[i].SmaSum.ToString());
                        SetValue(sheet, i + 5, 16, pHdayAADT[i].SmaEx.ToString());
                        SetValue(sheet, i + 5, 17, pHdayAADT[i].SmaEn.ToString());
                        SetValue(sheet, i + 5, 18, pHdayAADT[i].MedSum.ToString());
                        SetValue(sheet, i + 5, 19, pHdayAADT[i].MedEx.ToString());
                        SetValue(sheet, i + 5, 20, pHdayAADT[i].MedEn.ToString());
                        SetValue(sheet, i + 5, 21, pHdayAADT[i].LarSum.ToString());
                        SetValue(sheet, i + 5, 22, pHdayAADT[i].LarEx.ToString());
                        SetValue(sheet, i + 5, 23, pHdayAADT[i].LarEn.ToString());
                        SetValue(sheet, i + 5, 24, pHdayAADT[i].HeaSum.ToString());
                        SetValue(sheet, i + 5, 25, pHdayAADT[i].HeaEx.ToString());
                        SetValue(sheet, i + 5, 26, pHdayAADT[i].HeaEn.ToString());
                        SetValue(sheet, i + 5, 27, pHdayAADT[i].SupSum.ToString());
                        SetValue(sheet, i + 5, 28, pHdayAADT[i].SupEx.ToString());
                        SetValue(sheet, i + 5, 29, pHdayAADT[i].SupEn.ToString());
                        SetValue(sheet, i + 5, 30, pHdayAADT[i].EnExTrukNum.ToString());
                        if (i == 5 && string.IsNullOrEmpty(pHdayAADT[i].CarTrukPer.ToString()))
                        {
                            SetValue(sheet, i + 5, 31, 0);
                        }
                        else
                        {
                            SetValue(sheet, i + 5, 31, pHdayAADT[i].CarTrukPer.ToString());
                        }
                        SetValue(sheet, i + 5, 32, pHdayAADT[i].SupTruNum.ToString());
                        if (i == 5 && string.IsNullOrEmpty(pHdayAADT[i].SupTruPer.ToString()))
                        {
                            SetValue(sheet, i + 5, 33, 0);
                        }
                        else
                        {
                            SetValue(sheet, i + 5, 33, pHdayAADT[i].SupTruPer.ToString());
                        }
                    }
                }
                if (pHdayAADT.Count(s => s.LineType == 3) > 0)
                {
                    HDayAADTViewModel pHday = pHdayAADT.Where(s => s.LineType == 3).FirstOrDefault();
                    SetValue(sheet, 4, 7, pHday.NatSum.ToString());
                    SetValue(sheet, 4, 8, pHday.ExNat.ToString());
                    SetValue(sheet, 4, 9, pHday.EnNat.ToString());
                    SetValue(sheet, 4, 10, pHday.EquSum.ToString());
                    SetValue(sheet, 4, 11, pHday.ExEqu.ToString());
                    SetValue(sheet, 4, 12, pHday.EnEqu.ToString());
                    SetValue(sheet, 4, 14, pHday.CrowDeg.ToString());
                    SetValue(sheet, 4, 15, pHday.SmaSum.ToString());
                    SetValue(sheet, 4, 16, pHday.SmaEx.ToString());
                    SetValue(sheet, 4, 17, pHday.SmaEn.ToString());
                    SetValue(sheet, 4, 18, pHday.MedSum.ToString());
                    SetValue(sheet, 4, 19, pHday.MedEx.ToString());
                    SetValue(sheet, 4, 20, pHday.MedEn.ToString());
                    SetValue(sheet, 4, 21, pHday.LarSum.ToString());
                    SetValue(sheet, 4, 22, pHday.LarEx.ToString());
                    SetValue(sheet, 4, 23, pHday.LarEn.ToString());
                    SetValue(sheet, 4, 24, pHday.HeaSum.ToString());
                    SetValue(sheet, 4, 25, pHday.HeaEx.ToString());
                    SetValue(sheet, 4, 26, pHday.HeaEn.ToString());
                    SetValue(sheet, 4, 27, pHday.SupSum.ToString());
                    SetValue(sheet, 4, 28, pHday.SupEx.ToString());
                    SetValue(sheet, 4, 29, pHday.SupEn.ToString());
                    SetValue(sheet, 4, 30, pHday.EnExTrukNum.ToString());
                    SetValue(sheet, 4, 31, pHday.CarTrukPer.ToString());
                    SetValue(sheet, 4, 32, pHday.SupTruNum.ToString());
                    SetValue(sheet, 4, 33, pHday.SupTruPer.ToString());
                }
            }
            return readworkbook;
        }
        public HDayAADTViewModelParaViewModel GetHDayAADTPara()
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                var hday = db.OT_HDayConfig.Where(a => a.Id == 18).ToList();

                HDayAADTViewModelParaViewModel model = new HDayAADTViewModelParaViewModel();
                model.StartTime = hday[0].HDayStart.Value;
                model.EndTime = hday[0].HDayEnd.Value;
                model.HolidayId = (int)hday[0].HDayId;
                return model;
            }
        }

        /// <summary>
        /// 修改功能
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CustomResult Update(UHDayRoadStaViewModel args)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    CustomResult pReturnValue = new CustomResult();
                    List<UpdateHDayRoadStaViewModel> list = args.DataInfo.Where(a => a.LineType != 0).ToList();
                    try
                    {
                        foreach (UpdateHDayRoadStaViewModel model in list)
                        {
                            var listHDayRoadSta = db.RP_HDayAADTSta.Where(a => a.CalcuTime == args.DataDate && a.LineType == model.LineType).ToList();
                            foreach (RP_HDayAADTSta hd in listHDayRoadSta)
                            {
                                //  hd.LineType = model.LineType;
                                hd.ExNat = model.ExNat;
                                hd.EnNat = model.EnNat;
                                hd.ExEqu = model.ExEqu;
                                hd.EnEqu = model.EnEqu;

                                //拥挤度-“拥挤度”=交通量（当量交通量）合计/设计交通量，保留四位小数；
                                if (hd.ExEqu != null && hd.EnEqu != null)
                                {
                                    HDayAADTViewModel model1 = new HDayAADTViewModel();
                                    model1.LineType = model.LineType;
                                    Nullable<double> crowDeg = double.Parse(string.Format("{0:0.0000}", (hd.ExEqu + hd.EnEqu) / model1.DeTra));
                                    hd.CrowDeg = crowDeg;
                                }
                                hd.SmaEx = model.SmaEx;
                                hd.SmaEn = model.SmaEn;
                                hd.MedEx = model.MedEx;
                                hd.MedEn = model.MedEn;
                                hd.LarEx = model.LarEx;
                                hd.LarEn = model.LarEn;
                                hd.HeaEx = model.HeaEx;
                                hd.HeaEn = model.HeaEn;
                                hd.SupEx = model.SupEx;
                                hd.SupEn = model.SupEn;
                                hd.EnExTrukNum = model.EnExTrukNum;

                                //客车货车比例-客车货车比例=（交通量（自然交通量）合计-进出京货车数量)/进出京货车数量*100%，保留四位小数；
                                if (hd.ExNat != null && hd.EnNat != null && hd.EnExTrukNum != null && hd.EnExTrukNum != null)
                                {
                                    if (hd.EnExTrukNum != 0)
                                    {
                                        Nullable<double> carTrukPer = double.Parse(string.Format("{0:0.0000}", (hd.ExNat.Value + hd.EnNat.Value - hd.EnExTrukNum.Value) / hd.EnExTrukNum.Value));
                                        hd.CarTrukPer = carTrukPer;
                                    }
                                    else
                                    {
                                        hd.CarTrukPer = 0;
                                    }
                                }

                                //进出京大货车以上车型数量-进出京大货车以上车型数量”=大型车（合计）+重型车（合计）+超大型车（合计）。
                                if (hd.LarEx != null && hd.LarEn != null && hd.HeaEx != null && hd.HeaEn != null && hd.SupEx != null && hd.SupEn != null)
                                {
                                    Nullable<double> supTruNum = hd.LarEx.Value + hd.LarEn.Value + hd.HeaEx.Value + hd.HeaEn.Value + hd.SupEx.Value + hd.SupEn.Value;
                                    hd.SupTruNum = supTruNum;
                                }
                                //大货车以上占货车交通量比例-大货车以上占货车交通量比例（%）=进出京大货车以上车型的数量/进出京货车数量*100%，保留四位小数。
                                if (hd.SupTruNum != null && hd.EnExTrukNum != null)
                                {
                                    if (hd.EnExTrukNum.Value != 0)
                                    {
                                        Nullable<double> supTruPer = double.Parse(string.Format("{0:0.0000}", hd.SupTruNum.Value / hd.EnExTrukNum.Value));
                                        hd.SupTruPer = supTruPer;
                                    }
                                    else
                                    {
                                        hd.SupTruPer = 0;
                                    }
                                }
                                hd.UpdDate = DateTime.Now;
                                hd.State = "1";
                                if (SessionManage.GetLoginUser() != null)
                                {
                                    hd.UpdBy = SessionManage.GetLoginUser().UserName;
                                }
                            }
                            db.SaveChanges();
                        }

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
            }
        }

        #endregion

        #region 11 Private Methods
        private HDayAADTViewModel GetFirst(HDayAADTViewModel model)
        {
            HDayAADTViewModel HdayInfo = new HDayAADTViewModel();
            HdayInfo.LineType = 0;
            HdayInfo.ExNat = model.ExNat;
            HdayInfo.EnNat = model.EnNat;
            HdayInfo.NatSum = model.NatSum;
            HdayInfo.ExEqu = model.ExEqu;
            HdayInfo.EnEqu = model.EnEqu;
            HdayInfo.EquSum = model.EquSum;
            HdayInfo.CrowDeg = model.CrowDeg;
            HdayInfo.SmaEx = model.SmaEx;
            HdayInfo.SmaEn = model.SmaEn;
            HdayInfo.SmaSum = model.SmaSum;
            HdayInfo.MedEn = model.MedEn;
            HdayInfo.MedEx = model.MedEx;
            HdayInfo.MedSum = model.MedSum;
            HdayInfo.LarEx = model.LarEx;
            HdayInfo.LarEn = model.LarEn;
            HdayInfo.LarSum = model.LarSum;
            HdayInfo.HeaEn = model.HeaEn;
            HdayInfo.HeaEx = model.HeaEx;
            HdayInfo.HeaSum = model.HeaSum;
            HdayInfo.SupEn = model.SupEn;
            HdayInfo.SupEx = model.SupEx;
            HdayInfo.SupSum = model.SupSum;
            HdayInfo.EnExTrukNum = model.EnExTrukNum;
            HdayInfo.CarTrukPer = model.CarTrukPer;
            HdayInfo.SupTruNum = model.SupTruNum;
            HdayInfo.SupTruPer = model.SupTruPer;
            return HdayInfo;
        }
        /// <summary>
        /// 补充数据
        /// </summary>
        /// <param name="dt">统计日期</param>
        private void InsertNull(DateTime dt)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {

                List<RP_HDayAADTSta> pList = new List<RP_HDayAADTSta>();
                for (int i = 1; i < 7; i++)
                {

                    List<RP_HDayAADTSta> pTemp = db.RP_HDayAADTSta.Where(s => s.CalcuTime == dt && s.LineType == i).ToList();
                    //如果数据不存在，才补充
                    if (pTemp.Count <= 0)
                    {
                        RP_HDayAADTSta pHDayAADTSta = new RP_HDayAADTSta();
                        pHDayAADTSta.Id = Guid.NewGuid();
                        pHDayAADTSta.LineType = i;
                        if (i != 6)
                        {
                            pHDayAADTSta.ExNat = 0;
                            pHDayAADTSta.EnNat = 0;
                            pHDayAADTSta.ExEqu = 0;
                            pHDayAADTSta.EnEqu = 0;
                            pHDayAADTSta.CrowDeg = 0;
                            pHDayAADTSta.SmaEx = 0;
                            pHDayAADTSta.SmaEn = 0;
                            pHDayAADTSta.MedEx = 0;
                            pHDayAADTSta.MedEn = 0;
                            pHDayAADTSta.LarEx = 0;
                            pHDayAADTSta.LarEn = 0;
                            pHDayAADTSta.HeaEx = 0;
                            pHDayAADTSta.HeaEn = 0;
                            pHDayAADTSta.SupEx = 0;
                            pHDayAADTSta.SupEn = 0;
                            pHDayAADTSta.EnExTrukNum = 0;
                            pHDayAADTSta.CarTrukPer = 0;
                            pHDayAADTSta.SupTruNum = 0;
                            pHDayAADTSta.SupTruPer = 0;
                        }
                        else
                        {
                            pHDayAADTSta.ExNat = null;
                            pHDayAADTSta.EnNat = null;
                            pHDayAADTSta.ExEqu = null;
                            pHDayAADTSta.EnEqu = null;
                            pHDayAADTSta.CrowDeg = null;
                            pHDayAADTSta.SmaEx = null;
                            pHDayAADTSta.SmaEn = null;
                            pHDayAADTSta.MedEx = null;
                            pHDayAADTSta.MedEn = null;
                            pHDayAADTSta.LarEx = null;
                            pHDayAADTSta.LarEn = null;
                            pHDayAADTSta.HeaEx = null;
                            pHDayAADTSta.HeaEn = null;
                            pHDayAADTSta.SupEx = null;
                            pHDayAADTSta.SupEn = null;
                            pHDayAADTSta.EnExTrukNum = null;
                            pHDayAADTSta.CarTrukPer = null;
                            pHDayAADTSta.SupTruNum = null;
                            pHDayAADTSta.SupTruPer = null;
                        }
                        pHDayAADTSta.CalcuTime = dt;
                        pHDayAADTSta.CrtDate = DateTime.Now;
                        pHDayAADTSta.State = "0";
                        pList.Add(pHDayAADTSta);
                    }
                }
                using (TransactionScope transac = new TransactionScope())
                {
                    db.RP_HDayAADTSta.AddRange(pList);
                    if (pList.Count > 0)
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

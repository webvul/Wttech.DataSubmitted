/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/16 14:10:19
 */

#region 引用
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// 报表13,14后台功能实现类
    /// </summary>
    public class HourAADTStatistical : ReportRelated, IHourAADTStatistical
    {

        #region 9 Public Methods
        /// <summary>
        /// 根据查询条件获取数据
        /// </summary>
        /// <param name="para">查询条件类，站类型也需要</param>
        /// <returns></returns>
        public IReportViewModel GetListByPra(QueryParameters para)
        {
            //查询返回实体
            QueryHourAADTViewModel pReturn = new QueryHourAADTViewModel();
            try
            {
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    //补充空数据
                    InsertNull(para.StartTime.Value);
                    List<int> pStationNames = StationConfiguration.GetBJStaion();
                    List<RP_HourAADT> pHourAADTList = db.RP_HourAADT.Where(s => s.CalcuTime == para.StartTime).ToList();
                    RP_HourAADT pHourAADTInfo = new RP_HourAADT();
                    //创建或更新合计
                    CreateOrUpdateSum(para);
                    //报表数据
                    pReturn.ReportData = GetViewModelInfo(para);
                    //判断当前统计站类型，数据是否完整
                    if (GetNoDataList(para).Count() > 0)
                        pReturn.IsFull = 0;//不完整
                    else
                        pReturn.IsFull = 1;//完整 
                }
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
            return pReturn;
        }
        /// <summary>
        /// 校正数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CustomResult CalibrationData(QueryParameters para)
        {
            CustomResult pReturnValue = new CustomResult();
            //判断选择校正时间段的有效性
            string[] calibrationDataHour = new string[24];
            if (para.StartHour <= para.EndHour)
            {
                for (int i = para.StartHour; i <= para.EndHour; i++)
                {
                    calibrationDataHour[i] = i.ToString();
                }
            }
            else
            {
                pReturnValue.ResultKey = (byte)EResult.Fail;
                pReturnValue.ResultValue = TipInfo.CalibrationFaileHour;
                return pReturnValue;
            }
            double pFloating = 1 + para.FloatingRange * 0.01;
            List<RP_NatSta> pNaturalTraList = new List<RP_NatSta>();
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
                    //获取参考日期符合校正时间段的数据,因为只校正一天的数据，所以只查询开始数据的日期就可以
                    List<RP_HourAADT> pRefHourAADTList = db.RP_HourAADT.Where(s => s.CalcuTime == para.LastYearStart && calibrationDataHour.Contains(s.HourPer.ToString())).ToList();
                    //如果参考日期数据为0 则返回失败
                    if (pRefHourAADTList.Count <= 0)
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.CalibrationFaileRefNoData;
                        return pReturnValue;
                    }
                    //判断时间范围是否相同
                    if ((para.LastYearEnd - para.LastYearStart) == (para.EndTime - para.StartTime))
                    {
                        //需要校正的数据
                        var pCheckHourAADTList = db.RP_HourAADT.Where(s => s.CalcuTime == para.StartTime && calibrationDataHour.Contains(s.HourPer.ToString())).ToList();
                        if (pCheckHourAADTList.Count <= 0)
                        {
                            pReturnValue.ResultKey = (byte)EResult.Fail;
                            pReturnValue.ResultValue = TipInfo.CalibrationFaileNoData;
                            return pReturnValue;
                        }
                        using (TransactionScope tran = new TransactionScope())
                        {
                            foreach (var item in pCheckHourAADTList)
                            {
                                List<RP_HourAADT> plist = pRefHourAADTList.Where(i => i.HourPer == item.HourPer).ToList();
                                if (plist.Count() > 0)
                                {
                                    RP_HourAADT pTemp = plist.First();
                                    item.Dyf_EnOut = Math.Round(pTemp.Dyf_EnOut.Value * pFloating);
                                    item.Dyf_ExIn = Math.Round(pTemp.Dyf_ExIn.Value * pFloating);
                                    item.Mjqd_EnIn = Math.Round(pTemp.Mjqd_EnIn.Value * pFloating);
                                    item.Mjqd_EnOut = Math.Round(pTemp.Mjqd_EnOut.Value * pFloating);
                                    item.Mjqx_ExIn = Math.Round(pTemp.Mjqx_ExIn.Value * pFloating);
                                    item.Mjqx_EnOut = Math.Round(pTemp.Mjqx_EnOut.Value * pFloating);
                                    item.Cy_EnOut = Math.Round(pTemp.Cy_EnOut.Value * pFloating);
                                    item.Cy_ExIn = Math.Round(pTemp.Cy_ExIn.Value * pFloating);
                                    item.CalcuTime = para.StartTime.Value;

                                    if (SessionManage.GetLoginUser() != null)
                                    {
                                        item.UpdBy = SessionManage.GetLoginUser().UserName;
                                    }
                                    item.UpdDate = DateTime.Now;
                                    item.State = "1";
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
                        pReturnValue.ResultValue = TipInfo.CalibrationRangeFaile;
                    }
                }
            }
            catch (Exception e)
            {
                pReturnValue.ResultKey = (byte)EResult.Fail;
                pReturnValue.ResultValue = TipInfo.CalibrationFaile + e.Message.ToString();
                SystemLog.GetInstance().Error(TipInfo.CalibrationFaile, e);
                return pReturnValue;
            }
            return pReturnValue;
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
            if (para.ReportType == 13)//表13
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号13--0-24时各高速公路各收费站分方向出入口小时交通量.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
            }
            if (para.ReportType == 14)// 表14
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号14--各高速公路各收费站出口小时交通量.xlsx";
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
            ISheet sheet = readworkbook.GetSheetAt(1);
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //包括合计
                List<RP_HourAADT> pHourAADTList = db.RP_HourAADT.Where(s => s.CalcuTime == para.StartTime.Value).ToList();
                if (para.ReportType == 13)
                {
                    SetValue(sheet, 0, 0, string.Format("0-24时各高速公路各收费站分方向出入口小时交通量（{0}月{1}日）)", para.StartTime.Value.Month, para.StartTime.Value.Day));
                    SetReportDate(sheet, 1, 13, para.StartTime.Value, 13);
                    if (pHourAADTList.Count > 0)
                    {
                        foreach (RP_HourAADT item in pHourAADTList)
                        {
                            SetValue(sheet, 3, 3 + item.HourPer, item.Dyf_ExIn.Value);
                            SetValue(sheet, 6, 3 + item.HourPer, item.Dyf_EnOut.Value);
                            SetValue(sheet, 9, 3 + item.HourPer, item.Mjqd_EnIn.Value);
                            SetValue(sheet, 10, 3 + item.HourPer, item.Mjqd_EnOut.Value);
                            SetValue(sheet, 11, 3 + item.HourPer, item.Mjqx_ExIn.Value);
                            SetValue(sheet, 14, 3 + item.HourPer, item.Mjqx_EnOut.Value);
                            SetValue(sheet, 15, 3 + item.HourPer, item.Cy_ExIn.Value);
                            SetValue(sheet, 18, 3 + item.HourPer, item.Cy_EnOut.Value);
                        }
                    }
                }
                else if (para.ReportType == 14)
                {
                    SetReportDate(sheet, 1, 11, para.StartTime.Value, 14);
                    if (pHourAADTList.Count > 0)
                    {
                        foreach (RP_HourAADT item in pHourAADTList)
                        {
                            SetValue(sheet, 3, 3 + item.HourPer, item.Dyf_EnOut.Value);
                            SetValue(sheet, 4, 3 + item.HourPer, item.Dyf_EnOut.Value);
                            SetValue(sheet, 6, 3 + item.HourPer, item.Mjqd_EnOut.Value);
                            SetValue(sheet, 7, 3 + item.HourPer, item.Mjqd_EnOut.Value);
                            SetValue(sheet, 9, 3 + item.HourPer, item.Mjqx_EnOut.Value);
                            SetValue(sheet, 10, 3 + item.HourPer, item.Mjqx_EnOut.Value);
                            SetValue(sheet, 12, 3 + item.HourPer, item.Cy_EnOut.Value);
                            SetValue(sheet, 13, 3 + item.HourPer, item.Cy_EnOut.Value);
                        }
                    }
                }
            }
            return readworkbook;
        }
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CustomResult Update(UpdateHourAADTViewModel args)
        {
            CustomResult pReturnValue = new CustomResult();
            pReturnValue.ResultKey = (byte)EResult.Fail;
            pReturnValue.ResultValue = TipInfo.UpdateFaile;
            if (args.UpdateData == null)
            {
                pReturnValue.ResultKey = (byte)EResult.Succeed;
                pReturnValue.ResultValue = TipInfo.DataNull;
                return pReturnValue;
            }
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                var pReportData = db.RP_HourAADT.Where(s => s.CalcuTime == args.DataDate && s.HourPer != 24).ToList();
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        //获取对应的数据                      
                        foreach (UpdateHourAADTInfo item in args.UpdateData)
                        {
                            for (int i = 0; i < 24; i++)
                            {
                                RP_HourAADT pHourAADT = null;
                                List<RP_HourAADT> pHourList = pReportData.Where(s => s.HourPer == i).ToList();
                                if (pHourList.Count > 0)
                                {
                                    pHourAADT = pHourList.SingleOrDefault();
                                }
                                else
                                {
                                    //如果该时段的数据没有找到，则进行下次循环
                                    continue;
                                }
                                Type myType = item.GetType();
                                PropertyInfo pinfo = myType.GetProperty("Count_" + i);
                                if (item.StaName == "大羊坊站" && item.TraName == "出京入")
                                {
                                    pHourAADT.Dyf_ExIn = (double)pinfo.GetValue(item);
                                }
                                if (item.StaName == "马驹桥东站" && item.TraName == "进京入")
                                {
                                    pHourAADT.Mjqd_EnIn = (double)pinfo.GetValue(item);
                                }
                                if (item.StaName == "马驹桥西站" && item.TraName == "出京入")
                                {
                                    pHourAADT.Mjqx_ExIn = (double)pinfo.GetValue(item);
                                }
                                if (item.StaName == "采育站" && item.TraName == "出京入")
                                {
                                    pHourAADT.Cy_ExIn = (double)pinfo.GetValue(item);
                                }
                                if (item.StaName == "大羊坊站" && item.TraName == "进京出")
                                {
                                    pHourAADT.Dyf_EnOut = (double)pinfo.GetValue(item);
                                }
                                if (item.StaName == "马驹桥东站" && item.TraName == "进京出")
                                {
                                    pHourAADT.Mjqd_EnOut = (double)pinfo.GetValue(item);
                                }
                                if (item.StaName == "马驹桥西站" && item.TraName == "进京出")
                                {
                                    pHourAADT.Mjqx_EnOut = (double)pinfo.GetValue(item);
                                }
                                if (item.StaName == "采育站" && item.TraName == "进京出")
                                {
                                    pHourAADT.Cy_EnOut = (double)pinfo.GetValue(item);
                                }
                                pHourAADT.UpdDate = DateTime.Now;
                                pHourAADT.State = "1";
                                if (SessionManage.GetLoginUser() != null)
                                {
                                    pHourAADT.UpdBy = SessionManage.GetLoginUser().UserName;
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
        #endregion

        #region 11 Private Methods
        /// <summary>
        /// 创建报表13,14的空数据
        /// </summary>
        /// <param name="reporttype">报表类型</param>
        /// <returns></returns>
        private List<HourAADTViewModel> GetNullInfo(int reporttype)
        {
            List<HourAADTViewModel> pModelList = new List<HourAADTViewModel>();
            //报表13的字段值
            List<string> pStationList = this.GetStationList();
            //13交通量类型列表
            List<string> pReportTypePro1 = this.GetTraType1();
            //14交通量类型列表
            List<string> pReportTypePro2 = this.GetTraType2();
            for (int i = 0; i < pStationList.Count; i++)
            {
                if (reporttype == 13)
                {
                    for (int n = 0; n < pReportTypePro1.Count; n++)
                    {
                        HourAADTViewModel pModel = new HourAADTViewModel();
                        pModel.StaName = pStationList[i];
                        pModel.TraName = pReportTypePro1[n];
                        pModelList.Add(pModel);
                    }
                }
                if (reporttype == 14)
                {
                    for (int n = 0; n < pReportTypePro2.Count; n++)
                    {
                        HourAADTViewModel pModel = new HourAADTViewModel();
                        pModel.StaName = pStationList[i];
                        pModel.TraName = pReportTypePro2[n];
                        pModelList.Add(pModel);
                    }
                }

            }
            return pModelList.OrderBy(s => s.StaSort.Value).ThenBy(s => s.HourSort.Value).ToList();
        }
        /// <summary>
        /// 补充数据
        /// </summary>
        /// <param name="dt">统计日期</param>
        private void InsertNull(DateTime dt)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                int ptemp = 24;
                if (DateTime.Now.Date == dt)
                {
                    ptemp = DateTime.Now.Hour;
                }
                List<RP_HourAADT> pList = new List<RP_HourAADT>();
                for (int i = 0; i < ptemp; i++)
                {

                    List<RP_HourAADT> pTemp = db.RP_HourAADT.Where(s => s.CalcuTime == dt && s.HourPer == (byte)i).ToList();
                    //如果数据不存在，才补充
                    if (pTemp.Count <= 0)
                    {
                        RP_HourAADT pHour = new RP_HourAADT();
                        pHour.Id = Guid.NewGuid();
                        pHour.Mjqd_EnIn = 0;
                        pHour.Mjqd_EnOut = 0;
                        pHour.Dyf_EnOut = 0;
                        pHour.Dyf_ExIn = 0;
                        pHour.Mjqx_EnOut = 0;
                        pHour.Mjqx_ExIn = 0;
                        pHour.Cy_EnOut = 0;
                        pHour.Cy_ExIn = 0;
                        pHour.State = "0";
                        pHour.CalcuTime = dt;
                        pHour.CrtDate = DateTime.Now;
                        pHour.HourPer = (byte)i;
                        pList.Add(pHour);
                    }
                }
                using (TransactionScope transac = new TransactionScope())
                {
                    db.RP_HourAADT.AddRange(pList);
                    if (pList.Count > 0)
                    {
                        db.SaveChanges();
                        transac.Complete();
                    }
                }
            }
        }
        /// <summary>
        /// 获取查询集合
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private List<HourAADTViewModel> GetViewModelInfo(QueryParameters para)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //获取当天全部时间段的数据
                List<RP_HourAADT> pHourAADTList = db.RP_HourAADT.Where(s => s.CalcuTime == para.StartTime).ToList();

                //获取空数据
                List<HourAADTViewModel> pModelList = GetNullInfo(para.ReportType);

                //遍历修改空数据的值
                foreach (HourAADTViewModel model in pModelList)
                {
                    //遍历25条数据（包括合计）
                    for (int i = 0; i < 25; i++)
                    {
                        RP_HourAADT pHourAADTInfo = null;
                        List<RP_HourAADT> HourList = pHourAADTList.Where(s => s.HourPer == (byte)i).ToList();
                        if (HourList.Count > 0)
                        {
                            pHourAADTInfo = HourList.SingleOrDefault();
                        }
                        if (pHourAADTInfo != null)
                        {
                            Type myType = model.GetType();
                            PropertyInfo pinfo = myType.GetProperty("Count_" + i);
                            if (para.ReportType == 13)
                            {
                                if (model.StaName == "大羊坊站" && model.TraName == "出京入")
                                {
                                    pinfo.SetValue(model, pHourAADTInfo.Dyf_ExIn.Value);
                                }
                                if (model.StaName == "马驹桥东站" && model.TraName == "进京入")
                                {
                                    pinfo.SetValue(model, pHourAADTInfo.Mjqd_EnIn.Value);
                                }
                                if (model.StaName == "马驹桥西站" && model.TraName == "出京入")
                                {
                                    pinfo.SetValue(model, pHourAADTInfo.Mjqx_ExIn.Value);
                                }
                                if (model.StaName == "采育站" && model.TraName == "出京入")
                                {
                                    pinfo.SetValue(model, pHourAADTInfo.Cy_ExIn.Value);
                                }
                                if (model.StaName == "大羊坊站" && model.TraName == "进京出")
                                {
                                    pinfo.SetValue(model, pHourAADTInfo.Dyf_EnOut.Value);
                                }
                                if (model.StaName == "马驹桥东站" && model.TraName == "进京出")
                                {
                                    pinfo.SetValue(model, pHourAADTInfo.Mjqd_EnOut.Value);
                                }
                                if (model.StaName == "马驹桥西站" && model.TraName == "进京出")
                                {
                                    pinfo.SetValue(model, pHourAADTInfo.Mjqx_EnOut.Value);
                                }
                                if (model.StaName == "采育站" && model.TraName == "进京出")
                                {
                                    pinfo.SetValue(model, pHourAADTInfo.Cy_EnOut.Value);
                                }
                            }
                            else if (para.ReportType == 14)
                            {
                                if (model.StaName == "大羊坊站" && (model.TraName == "进出京合计" || model.TraName == "进京"))
                                {
                                    pinfo.SetValue(model, pHourAADTInfo.Dyf_EnOut.Value);
                                }
                                if (model.StaName == "马驹桥东站" && (model.TraName == "进出京合计" || model.TraName == "进京"))
                                {
                                    pinfo.SetValue(model, pHourAADTInfo.Mjqd_EnOut.Value);
                                }
                                if (model.StaName == "马驹桥西站" && (model.TraName == "进出京合计" || model.TraName == "进京"))
                                {
                                    pinfo.SetValue(model, pHourAADTInfo.Mjqx_EnOut.Value);
                                }
                                if (model.StaName == "采育站" && (model.TraName == "进出京合计" || model.TraName == "进京"))
                                {
                                    pinfo.SetValue(model, pHourAADTInfo.Cy_EnOut.Value);
                                }
                            }
                        }
                    }
                }
                return pModelList;
            }
        }
        /// <summary>
        /// 创建或修改13,14合计
        /// </summary>
        /// <param name="para"></param>
        /// <param name="stationtype">收费站类型</param>
        private void CreateOrUpdateSum(QueryParameters para)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //查询日期当天除合计外的全部数据
                IEnumerable<RP_HourAADT> all = db.RP_HourAADT.Where(s => s.CalcuTime == para.StartTime && s.HourPer != 24);

                IEnumerable<RP_HourAADT> listsum = db.RP_HourAADT.Where(s => s.CalcuTime == para.StartTime && s.HourPer == 24);
                RP_HourAADT pHourAADT = new RP_HourAADT();
                //如果有数据则进行合计
                if (all.Count() > 0)
                {
                    if (listsum.Count() > 0)
                    {
                        pHourAADT = listsum.First();
                    }
                    pHourAADT.Dyf_ExIn = all.Sum(s => s.Dyf_ExIn);
                    pHourAADT.Dyf_EnOut = all.Sum(s => s.Dyf_EnOut);
                    pHourAADT.Mjqd_EnIn = all.Sum(s => s.Mjqd_EnIn);
                    pHourAADT.Mjqd_EnOut = all.Sum(s => s.Mjqd_EnOut);
                    pHourAADT.Mjqx_EnOut = all.Sum(s => s.Mjqx_EnOut);
                    pHourAADT.Mjqx_ExIn = all.Sum(s => s.Mjqx_ExIn);
                    pHourAADT.Cy_EnOut = all.Sum(s => s.Cy_EnOut);
                    pHourAADT.Cy_ExIn = all.Sum(s => s.Cy_ExIn);
                    pHourAADT.State = "1";
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        if (listsum.Count() <= 0)
                        {
                            pHourAADT.HourPer = 24;//24代表合计
                            pHourAADT.Id = Guid.NewGuid();
                            pHourAADT.CalcuTime = (DateTime)para.StartTime;
                            pHourAADT.CrtDate = DateTime.Now;
                            pHourAADT.State = "0";
                            db.RP_HourAADT.Add(pHourAADT);
                        }
                        else
                        {
                            pHourAADT.UpdDate = DateTime.Now;
                        }
                        db.SaveChanges();
                        //提交事务
                        transaction.Complete();
                    }
                }
            }
        }
        /// <summary>
        /// 获取收费站列表
        /// </summary>
        /// <returns></returns>
        private List<string> GetStationList()
        {
            return new List<string>() { "大羊坊站", "马驹桥东站", "马驹桥西站", "采育站" };
        }
        /// <summary>
        /// 获取报表13的交通量类型
        /// </summary>
        /// <returns></returns>
        private List<string> GetTraType1()
        {
            return new List<string>() { "出京入", "出京出", "进京入", "进京出" };
        }
        /// <summary>
        /// 获取报表14的交通量类型
        /// </summary>
        /// <returns></returns>
        private List<string> GetTraType2()
        {
            return new List<string>() { "进出京合计", "进京", "出京" };
        }
        #endregion
    }
}

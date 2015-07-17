/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/18 13:54:26
 */

#region 引用
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.Resources;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL;
using Wttech.DataSubmitted.IBLL.IReport;

#endregion

namespace Wttech.DataSubmitted.BLL.Report
{
    /// <summary>
    /// 报表12业务层功能实现
    /// </summary>
    public class HDayExStatistical : ReportRelated, IHDayExStatistical
    {

        #region 9 Public Methods
        /// <summary>
        /// 修改数据信息
        /// </summary>
        /// <typeparam name="T">数据表类型集合</typeparam>
        /// <param name="args">参数</param>
        /// <returns>影响行数</returns>
        public CustomResult Update(UpdateHdayExViewModel args)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    CustomResult pReturnValue = new CustomResult();
                    int count = (args.EndTime.Value - args.StartTime.Value).Days + 1;
                    HDayExViewModel model = null;
                    if (args.DataInfo.Count > 0)
                    {
                        if (args.DataInfo[0].Num == 15)
                            model = args.DataInfo[0];
                    }
                    try
                    {

                        for (int i = 0; i < count; i++)
                        {
                            DateTime pDateTime = args.StartTime.Value.AddDays(i);
                            var listHDa = db.RP_HDayAADT.Where(a => a.CalcuTime == pDateTime).ToList();
                            foreach (RP_HDayAADT hd in listHDa)
                            {
                                Type myType = model.GetType();
                                PropertyInfo pinfo = myType.GetProperty("Tra" + (i + 1));
                                hd.Out = (double)pinfo.GetValue(model);
                                hd.UpdDate = DateTime.Now;
                                hd.State = "1";
                                if (SessionManage.GetLoginUser() != null)
                                {
                                    hd.UpdBy = SessionManage.GetLoginUser().UserName;
                                }
                            }
                        }
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
            }
        }
        /// <summary>
        /// 校正数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CustomResult CalibrationData(QueryParameters para)
        {
            CustomResult pReturnValue = new CustomResult();
            double pFloating = 1 + para.FloatingRange * 0.01;

            List<RP_HDayAADT> pHDayAADT = new List<RP_HDayAADT>();
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
                    //判断时间范围是否相同
                    if ((para.LastYearEnd - para.LastYearStart) == (para.EndTime - para.StartTime))
                    {
                        //获取参考日期符合校正时间段的数据
                        List<RP_HDayAADT> pRefList = db.RP_HDayAADT.Where(s => s.CalcuTime >= para.LastYearStart && s.CalcuTime <= para.LastYearEnd).ToList();
                        //如果参考日期数据为0 则返回失败
                        if (pRefList.Count == 0)
                        {
                            pReturnValue.ResultKey = (byte)EResult.Fail;
                            pReturnValue.ResultValue = TipInfo.CalibrationFaileRefNoData;
                            return pReturnValue;
                        }
                        //需要校正的数据
                        var pCheckList = db.RP_HDayAADT.Where(s => s.CalcuTime >= para.StartTime && s.CalcuTime <= para.EndTime).ToList();
                        //如果需要校正的数据为空则返回失败
                        if (pCheckList.Count == 0)
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

                            foreach (RP_HDayAADT pCheckInfo in pCheckList)//校正数据
                            {
                                foreach (RP_HDayAADT pRefInfo in pRefList)//参考数据
                                {
                                    if (pRefInfo.CalcuTime == pRefInfo.CalcuTime)
                                    {
                                        pCheckInfo.Out = Math.Round(pRefInfo.Out.Value * pFloating);
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
                        pReturnValue.ResultValue = TipInfo.CalibrationRangeFaile;
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
        /// 查询
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public IReportViewModel GetListByPra(QueryParameters para)
        {
            QueryHDayExViewModel pReturnData = new QueryHDayExViewModel();
            List<HDayExViewModel> pTList = new List<HDayExViewModel>();
            int pDataCount = 0;
            if (para.EndTime.Value >= para.StartTime.Value)
            {
                pDataCount = (para.EndTime.Value - para.StartTime.Value).Days + 1;
            }
            //添加标头
            pReturnData.TitleList = new List<string>();
            pReturnData.TitleList.Add("序号");
            pReturnData.TitleList.Add("高速名称");
            //日期天数
            pReturnData.CountDay = pDataCount;
            for (int i = 0; i < 15; i++)
            {
                //日期
                pReturnData.TitleList.Add(para.StartTime.Value.AddDays(i).ToString("M月d日"));
            }
            pReturnData.TitleList.Add("合计");
            pReturnData.TitleList.Add("去年同期总流量");
            pReturnData.TitleList.Add("同比增幅");
            try
            {
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    HDayExViewModel pHdayExInfo = new HDayExViewModel();
                    pHdayExInfo.RoadName = SystemConst.RoadName;

                    for (int n = 0; n < pDataCount; n++)
                    {
                        DateTime? pDt = para.StartTime.Value.AddDays(n);
                        //如果所选日期数据不存在，则进行添加
                        if (db.RP_HDayAADT.Where(s => DbFunctions.TruncateTime(s.CalcuTime) == pDt).ToList().Count <= 0)
                        {
                            if (pDt <= DateTime.Now)
                            {
                                InsertNull(pDt.Value);
                            }
                        }
                        List<RP_HDayAADT> pHourAADTList = db.RP_HDayAADT.Where(s => DbFunctions.TruncateTime(s.CalcuTime) == pDt).ToList();

                        if (pHourAADTList.Count == 1)
                        {
                            //通过反射取字段名
                            Type myType = pHdayExInfo.GetType();
                            PropertyInfo pinfo = myType.GetProperty("Tra" + (n + 1));
                            //给字段赋值
                            double pTemp = pHourAADTList.SingleOrDefault().Out.Value;
                            pinfo.SetValue(pHdayExInfo, pTemp);
                        }
                    }
                    pHdayExInfo.Num = 15;
                    pHdayExInfo.Sum = db.RP_HDayAADT.Where(s => DbFunctions.TruncateTime(s.CalcuTime) >= para.StartTime.Value && DbFunctions.TruncateTime(s.CalcuTime) <= para.EndTime.Value).Sum(s => s.Out);
                    pHdayExInfo.LastSum = db.RP_HDayAADT.Where(s => DbFunctions.TruncateTime(s.CalcuTime) >= para.LastYearStart.Value && DbFunctions.TruncateTime(s.CalcuTime) <= para.LastYearEnd.Value).Sum(s => s.Out);
                    if (pHdayExInfo.Sum.HasValue && pHdayExInfo.LastSum.HasValue && pHdayExInfo.LastSum != 0 && pHdayExInfo.LastSum != 0.0)
                        pHdayExInfo.Growth = Math.Round((pHdayExInfo.Sum.Value - pHdayExInfo.LastSum.Value) / pHdayExInfo.LastSum.Value, 2);
                    pTList.Add(pHdayExInfo);
                    //添加合计
                    pTList.Add(GetSum(pHdayExInfo));
                    pReturnData.ReportData = pTList.OrderBy(s => s.Num).ToList();
                    //报表数据
                    //判断当前统计站类型，数据是否完整
                    if (GetNoDataList(para).Count() > 0)
                        pReturnData.IsFull = 0;//不完整
                    else
                        pReturnData.IsFull = 1;//完整 
                }
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
            return pReturnData;
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
            if (para.ReportType == 12)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号12--假期流量表（出口）.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
            }
            return reportpath;
        }
        /// <summary>
        /// 修改工作簿内容
        /// </summary>
        /// <param name="readworkbook"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public IWorkbook GenerateSheet(IWorkbook readworkbook, QueryParameters para)
        {
            try
            {
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    //日期合计
                    //获取工作簿
                    ISheet sheet = readworkbook.GetSheetAt(0);
                    int pDataCount = 0;
                    if (para.EndTime.Value >= para.StartTime.Value)
                    {
                        pDataCount = (para.EndTime.Value - para.StartTime.Value).Days + 1;
                    }
                    //设置标题
                    var holidayName = db.OT_Dic.Where(a => a.Id == para.HolidayId).Select(a => a.Name).ToList();
                    string title = string.Format("天津市高速公路支队{0}年{1}假期流量表（出口）", ((DateTime)para.EndTime).Year, holidayName[0].ToString());
                    SetValue(sheet, 0, 0, title);
                    //如果多于4天，则手动添加单元格
                    if (pDataCount > 4)
                    {
                        int pTemp = pDataCount - 4;
                        //遍历创建所有单元格
                        //列
                        for (int i = 0; i < pTemp; i++)
                        {
                            //行--该模版有隐藏行
                            for (int n = 0; n < 19; n++)
                            {
                                IRow row = sheet.GetRow(n);
                                ICell cell = row.CreateCell(9 + i);//创建列
                                cell.CellStyle = GetCellStyle(readworkbook, 1);
                            }
                        }

                    }
                    else if (pDataCount < 4)//如果小于4天，则删除列
                    {
                        int pTemp = 4 - pDataCount;
                        //遍历设置所有单元格没有边框
                        //列
                        for (int i = 0; i < pTemp; i++)
                        {

                            for (int n = 0; n < 19; n++)
                            {
                                ICell cell = sheet.GetRow(n).GetCell(i + 5 + pDataCount);
                                cell.CellStyle = GetCellStyle(readworkbook, 0);
                            }
                        }
                    }
                    double? pSum = db.RP_HDayAADT.Where(s => DbFunctions.TruncateTime(s.CalcuTime) >= para.StartTime.Value && DbFunctions.TruncateTime(s.CalcuTime) <= para.EndTime.Value).Sum(s => s.Out);
                    double? pLastSum = db.RP_HDayAADT.Where(s => DbFunctions.TruncateTime(s.CalcuTime) >= para.LastYearStart.Value && DbFunctions.TruncateTime(s.CalcuTime) <= para.LastYearEnd.Value).Sum(s => s.Out);
                    double? Growth = null;
                    if (pLastSum.HasValue && pLastSum != 0 && pLastSum != 0.0 && pSum.HasValue)
                    {
                        Growth = Math.Round((pSum.Value - pLastSum.Value) / pLastSum.Value, 2);
                    }
                    if (pSum.HasValue)
                    {
                        SetValue(sheet, 17, 2 + pDataCount, pSum.ToString());
                        SetValue(sheet, 18, 2 + pDataCount, pSum.ToString());
                    }
                    if (pLastSum.HasValue)
                    {
                        SetValue(sheet, 17, 3 + pDataCount, pLastSum.ToString());
                        SetValue(sheet, 18, 3 + pDataCount, pLastSum.ToString());
                    }

                    if (Growth.HasValue)
                    {
                        SetValue(sheet, 17, 4 + pDataCount, Growth.ToString());
                        SetValue(sheet, 18, 4 + pDataCount, Growth.ToString());
                    }
                    //合并单元格
                    //标题行
                    SetCellRangeAddress(sheet, 0, 0, 0, 4 + pDataCount);
                    //日期行
                    SetCellRangeAddress(sheet, 1, 1, 2, pDataCount + 1);
                    //合并最后三列
                    SetCellRangeAddress(sheet, 1, 2, 2 + pDataCount, 2 + pDataCount);
                    SetCellRangeAddress(sheet, 1, 2, 3 + pDataCount, 3 + pDataCount);
                    SetCellRangeAddress(sheet, 1, 2, 4 + pDataCount, 4 + pDataCount);
                    //
                    SetValue(sheet, 1, 2 + pDataCount, "合计");
                    SetValue(sheet, 1, 3 + pDataCount, "去年同期总流量");
                    SetValue(sheet, 1, 4 + pDataCount, "同比增幅");

                    for (int i = 0; i < pDataCount; i++)
                    {
                        DateTime? pDt = para.StartTime.Value.AddDays(i);
                        List<RP_HDayAADT> pHourAADTList = db.RP_HDayAADT.Where(s => DbFunctions.TruncateTime(s.CalcuTime) == pDt).ToList();

                        if (pHourAADTList.Count == 1)
                        {
                            SetValue(sheet, 17, 2 + i, pHourAADTList.Sum(s => s.Out).ToString());
                            SetValue(sheet, 18, 2 + i, pHourAADTList.Sum(s => s.Out).ToString());
                        }
                        //添加表头
                        SetValue(sheet, 2, 2 + i, para.StartTime.Value.AddDays(i).ToString("M月d日"));
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
            return readworkbook;
        }
        #endregion
        #region 11 Private Methods
        /// <summary>
        /// 添加合计
        /// </summary>
        /// <param name="pHdayExInfo"></param>
        /// <returns></returns>
        private HDayExViewModel GetSum(HDayExViewModel pHdayExInfo)
        {
            //合计
            HDayExViewModel pHdayExSumInfo = new HDayExViewModel();
            pHdayExSumInfo.Num = 17;
            pHdayExSumInfo.Sum = pHdayExInfo.Sum;
            pHdayExSumInfo.LastSum = pHdayExInfo.LastSum;
            pHdayExSumInfo.Growth = pHdayExInfo.Growth;
            pHdayExSumInfo.RoadName = "总计";
            pHdayExSumInfo.Tra1 = pHdayExInfo.Tra1;
            pHdayExSumInfo.Tra2 = pHdayExInfo.Tra2;
            pHdayExSumInfo.Tra3 = pHdayExInfo.Tra3;
            pHdayExSumInfo.Tra4 = pHdayExInfo.Tra4;
            pHdayExSumInfo.Tra5 = pHdayExInfo.Tra5;
            pHdayExSumInfo.Tra6 = pHdayExInfo.Tra6;
            pHdayExSumInfo.Tra7 = pHdayExInfo.Tra7;
            pHdayExSumInfo.Tra8 = pHdayExInfo.Tra8;
            pHdayExSumInfo.Tra9 = pHdayExInfo.Tra9;
            pHdayExSumInfo.Tra10 = pHdayExInfo.Tra10;
            pHdayExSumInfo.Tra11 = pHdayExInfo.Tra11;
            pHdayExSumInfo.Tra12 = pHdayExInfo.Tra12;
            pHdayExSumInfo.Tra13 = pHdayExInfo.Tra13;
            pHdayExSumInfo.Tra14 = pHdayExInfo.Tra14;
            pHdayExSumInfo.Tra15 = pHdayExInfo.Tra15;
            return pHdayExSumInfo;
        }
        /// <summary>
        /// 补充数据
        /// </summary>
        /// <param name="dt">统计日期</param>
        private void InsertNull(DateTime dt)
        {
            HDayStatisticalReport pHday = new HDayStatisticalReport("");
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                RP_HDayAADT info = new RP_HDayAADT();

                info.Id = Guid.NewGuid();
                info.CrtDate = DateTime.Now;
                info.State = "0";
                info.CalcuTime = dt;
                //杨村站流量
                info.YC = 0;
                //宜兴埠东站流量
                info.YXBD = 0;
                //宜兴埠西站流量
                info.YXBX = 0;
                //金钟路站流量
                info.JZL = 0;
                //机场站流量
                info.JC = 0;
                //空港经济区站流量
                info.KG = 0;
                //塘沽西站流量
                info.TGX = 0;
                //塘沽西分站流量
                info.TGXF = 0;
                //塘沽北站流量
                info.TGB = 0;
                //表12出口流量
                info.Out = 0;
                //去年同期
                info.SameSum = pHday.GetSameSum(dt);
                using (TransactionScope transac = new TransactionScope())
                {
                    db.RP_HDayAADT.Add(info);
                    db.SaveChanges();
                    transac.Complete();
                }
            }
        }
        #endregion
    }
}

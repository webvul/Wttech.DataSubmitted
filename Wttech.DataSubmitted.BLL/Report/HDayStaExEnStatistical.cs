/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个报表11实现类文件
* 创建标识：ta0395侯兴鼎20141219
*/
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Transactions;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.Resources;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL.IReport;

namespace Wttech.DataSubmitted.BLL.Report
{
    /// <summary>
    /// 报表11实现类文件
    /// </summary>
    public class HDayStaExEnStatistical : ReportRelated, IHDayStaExEnStatistical
    {
        #region 3 Fields

        /// <summary>
        /// 查询结果
        /// </summary>
        QueryHDayStaExEnViewModel qModel = new QueryHDayStaExEnViewModel();

        /// <summary>
        /// 查询数据缓存，导出时直接使用，而无需再到数据库中查
        /// </summary>
        List<HDayStaExEnViewModel> listExport = new List<HDayStaExEnViewModel>();

        #endregion

        #region 9 Public Methods

        /// <summary>
        /// 初始化查询条件
        /// </summary>
        /// <returns></returns>
        public Common.ViewModels.WhereHDayStaExEnViewModel GetHdayExEnWhere()
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                var hday = db.OT_HDayConfig.Where(a => a.Id == 11).ToList();

                WhereHDayStaExEnViewModel model = new WhereHDayStaExEnViewModel();
                if (hday != null && hday.Count > 0)
                {
                    model.StartTime = (DateTime)hday[0].HDayStart;
                    model.HolidayId = (int)hday[0].HDayId;
                    model.EndTime = (DateTime)hday[0].HDayEnd;
                }
                return model;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Common.ViewModels.IReportViewModel GetListByPra(Common.QueryParameters para)
        {
            List<HDayStaExEnViewModel> list = GetData(para);
            qModel.ReportData = list;
            if (para.EndTime != null && para.StartTime != null)
            {
                qModel.DateTotal = (para.EndTime.Value - para.StartTime.Value).Days + 1;
            }
            //判断当前统计站类型，数据是否完整
            if (GetNoDataList(para) != null && GetNoDataList(para).Count() > 0)
                qModel.IsFull = 0;//不完整
            else
                qModel.IsFull = 1;//完整 

            return qModel;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Common.CustomResult Update(Common.ViewModels.UpdateHDayStaExEnViewModel args)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    CustomResult pReturnValue = new CustomResult();

                    bool flag = false;//标记数据库中是否有数据修改
                    int count = (args.EndTime - args.StartTime).Days + 1;

                    for (int j = 1; j < args.DataInfo.Count; j++)
                    {
                        HDayStaExEnViewModel model = args.DataInfo[j];

                        Type myType0 = qModel.TitleList[0].GetType();

                        for (int i = 1; i < count + 1; i++)
                        {
                            Type myType = model.GetType();
                            PropertyInfo pinfo = myType.GetProperty("Date" + i);
                            PropertyInfo pinfo0 = myType0.GetProperty("Date" + i);
                            if (args.StartTime.AddDays(i - 1).ToString("M月d日").ToString() == pinfo0.GetValue(qModel.TitleList[0]).ToString())
                            {
                                DateTime dt = args.StartTime.AddDays(i - 1);
                                var list = db.RP_HDayAADT.Where(a => a.CalcuTime == dt).ToList();

                                double info = double.Parse(pinfo.GetValue(model).ToString());

                                foreach (var item in list)
                                {
                                    flag = true;
                                    if (model.Num == "43")//杨村站
                                        item.YC = info;
                                    else if (model.Num == "44")//宜兴埠东站
                                        item.YXBD = info;
                                    else if (model.Num == "45")//宜兴埠西站
                                        item.YXBX = info;
                                    else if (model.Num == "46")//金钟路站
                                        item.JZL = info;
                                    else if (model.Num == "47")//机场站
                                        item.JC = info;
                                    else if (model.Num == "48")//空港经济区站
                                        item.KG = info;
                                    else if (model.Num == "49")//塘沽西站
                                        item.TGX = info;
                                    else if (model.Num == "50")//塘沽西分站
                                        item.TGXF = info;
                                    else if (model.Num == "51")//塘沽北站
                                        item.TGB = info;

                                    item.UpdDate = DateTime.Now;
                                    if (SessionManage.GetLoginUser() != null)
                                    {
                                        item.UpdBy = SessionManage.GetLoginUser().UserName;
                                    }
                                    item.State = "1";
                                }
                            }
                        }
                    }
                    if (flag)//有修改成功的数据
                    {
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
                    else//存在数据尚未生成
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.UpdateDataRepeat;
                        return pReturnValue;
                    }
                }
            }
        }

        /// <summary>
        /// 修改excel工作簿
        /// </summary>
        /// <param name="readworkbook"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public NPOI.SS.UserModel.IWorkbook GenerateSheet(NPOI.SS.UserModel.IWorkbook readworkbook, Common.QueryParameters para)
        {
            if (readworkbook != null)
            {
                ISheet sheet = readworkbook.GetSheetAt(0);

                string title = string.Empty;
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    var holidayName = db.OT_Dic.Where(a => a.Id == para.HolidayId).Select(a => a.Name).ToList();
                    if (holidayName != null && holidayName.Count > 0)
                    {
                        title = string.Format("天津市高速公路支队{0}年{1}假期重点收费站流量表（出口+入口）", ((DateTime)para.EndTime).Year, holidayName[0].ToString());
                    }
                    SetValue(sheet, 0, 0, title);
                }
                if (listExport != null)
                {
                    //计算查询天数
                    int count = (para.EndTime.Value - para.StartTime.Value).Days + 1;
                    if ((para.EndTime.Value - para.StartTime.Value).Days + 1 > 15)
                        count = 15;//最多可查询15天

                    for (int i = 0; i < listExport.Count; i++)
                    {
                        if (count > 0)
                        {
                            SetValue(sheet, i + 45, 3, listExport[i].Date1);
                            if (i == 0)
                                SetValue(sheet, 2, 3, para.StartTime.Value.AddDays(0).ToString("M月d日"));
                        }
                        if (count > 1)
                        {
                            SetValue(sheet, i + 45, 4, listExport[i].Date2);
                            if (i == 0)
                                SetValue(sheet, 2, 4, para.StartTime.Value.AddDays(1).ToString("M月d日"));
                        }
                        if (count > 2)
                        {
                            SetValue(sheet, i + 45, 5, listExport[i].Date3);
                            if (i == 0)
                                SetValue(sheet, 2, 5, para.StartTime.Value.AddDays(2).ToString("M月d日"));
                        }
                        if (count > 3)
                        {
                            SetValue(sheet, i + 45, 6, listExport[i].Date4);
                            if (i == 0)
                                SetValue(sheet, 2, 6, para.StartTime.Value.AddDays(3).ToString("M月d日"));
                        }
                        if (count > 4)
                        {
                            SetValue(sheet, i + 45, 7, listExport[i].Date5);
                            if (i == 0)
                                SetValue(sheet, 2, 7, para.StartTime.Value.AddDays(4).ToString("M月d日"));
                        }
                        if (count > 5)
                        {
                            SetValue(sheet, i + 45, 8, listExport[i].Date6);
                            if (i == 0)
                                SetValue(sheet, 2, 8, para.StartTime.Value.AddDays(5).ToString("M月d日"));
                        }
                        if (count > 6)
                        {
                            SetValue(sheet, i + 45, 9, listExport[i].Date7);
                            if (i == 0)
                                SetValue(sheet, 2, 9, para.StartTime.Value.AddDays(6).ToString("M月d日"));
                        }
                        if (count > 7)
                        {
                            SetValue(sheet, i + 45, 10, listExport[i].Date8);
                            if (i == 0)
                                SetValue(sheet, 2, 10, para.StartTime.Value.AddDays(7).ToString("M月d日"));
                        }
                        if (count > 8)
                        {
                            SetValue(sheet, i + 45, 11, listExport[i].Date9);
                            if (i == 0)
                                SetValue(sheet, 2, 11, para.StartTime.Value.AddDays(8).ToString("M月d日"));
                        }
                        if (count > 9)
                        {
                            SetValue(sheet, i + 45, 12, listExport[i].Date10);
                            if (i == 0)
                                SetValue(sheet, 2, 12, para.StartTime.Value.AddDays(9).ToString("M月d日"));
                        }
                        if (count > 10)
                        {
                            SetValue(sheet, i + 45, 13, listExport[i].Date11);
                            if (i == 0)
                                SetValue(sheet, 2, 13, para.StartTime.Value.AddDays(10).ToString("M月d日"));
                        }
                        if (count > 11)
                        {
                            SetValue(sheet, i + 45, 14, listExport[i].Date12);
                            if (i == 0)
                                SetValue(sheet, 2, 14, para.StartTime.Value.AddDays(11).ToString("M月d日"));
                        }
                        if (count > 12)
                        {
                            SetValue(sheet, i + 45, 15, listExport[i].Date13);
                            if (i == 0)
                                SetValue(sheet, 2, 15, para.StartTime.Value.AddDays(12).ToString("M月d日"));
                        }
                        if (count > 13)
                        {
                            SetValue(sheet, i + 45, 16, listExport[i].Date14);
                            if (i == 0)
                                SetValue(sheet, 2, 16, para.StartTime.Value.AddDays(13).ToString("M月d日"));
                        }
                        if (count > 14)
                        {
                            SetValue(sheet, i + 45, 17, listExport[i].Date15);
                            if (i == 0)
                                SetValue(sheet, 2, 17, para.StartTime.Value.AddDays(14).ToString("M月d日"));
                        }

                        SetValue(sheet, i + 45, 3 + count, listExport[i].Total);
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

            if (listExport == null || listExport.Count == 0)
            {
                GetData(para);
            }
            if (para.ReportType == 11)
            {
                if (para.StartTime != null && para.EndTime != null)
                {
                    int count = (para.EndTime.Value - para.StartTime.Value).Days + 1;
                    if ((para.EndTime.Value - para.StartTime.Value).Days + 1 > 15)
                        count = 15;
                    path = string.Format(@"{0}Reporttemplate\DynamicReport\{1}\编号11--假期重点收费站流量表（出口加入口）.xlsx", AppDomain.CurrentDomain.BaseDirectory, count);
                    reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
                }
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
                //判断时间范围是否相同
                if ((para.LastYearEnd - para.LastYearStart) == (para.EndTime - para.StartTime))
                {
                    //获取参考日期符合校正时间段的数据
                    List<RP_HDayAADT> pRefNaturalList = db.RP_HDayAADT.Where(a => a.CalcuTime >= para.LastYearStart & a.CalcuTime <= para.LastYearEnd).OrderBy(a => a.CalcuTime).ToList();
                    //如果参考日期数据为0 则返回失败
                    if (pRefNaturalList == null || pRefNaturalList.Count == 0)
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.CalibrationFaileRefNoData;
                        return pReturnValue;
                    }
                    //需要校正的数据
                    var pCheckNaturalList = db.RP_HDayAADT.Where(a => a.CalcuTime >= para.StartTime & a.CalcuTime <= para.EndTime).OrderBy(a => a.CalcuTime).ToList();
                    //如果需要校正的数据为空则返回失败
                    if (pCheckNaturalList == null || pCheckNaturalList.Count == 0)
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.CalibrationFaileNoData;
                        return pReturnValue;
                    }
                    using (TransactionScope tran = new TransactionScope())
                    {
                        for (int i = 0; i < pCheckNaturalList.Count; i++)
                        {
                            if (pRefNaturalList[i].JC != null)
                                pCheckNaturalList[i].JC = Math.Round(pRefNaturalList[i].JC.Value * pFloating);
                            if (pRefNaturalList[i].JZL != null)
                                pCheckNaturalList[i].JZL = Math.Round(pRefNaturalList[i].JZL.Value * pFloating);
                            if (pRefNaturalList[i].KG != null)
                                pCheckNaturalList[i].KG = Math.Round(pRefNaturalList[i].KG.Value * pFloating);
                            if (pRefNaturalList[i].TGB != null)
                                pCheckNaturalList[i].TGB = Math.Round(pRefNaturalList[i].TGB.Value * pFloating);
                            if (pRefNaturalList[i].TGX != null)
                                pCheckNaturalList[i].TGX = Math.Round(pRefNaturalList[i].TGX.Value * pFloating);
                            if (pRefNaturalList[i].TGXF != null)
                                pCheckNaturalList[i].TGXF = Math.Round(pRefNaturalList[i].TGXF.Value * pFloating);
                            if (pRefNaturalList[i].YC != null)
                                pCheckNaturalList[i].YC = Math.Round(pRefNaturalList[i].YC.Value * pFloating);
                            if (pRefNaturalList[i].YXBD != null)
                                pCheckNaturalList[i].YXBD = Math.Round(pRefNaturalList[i].YXBD.Value * pFloating);
                            if (pRefNaturalList[i].YXBX != null)
                                pCheckNaturalList[i].YXBX = Math.Round(pRefNaturalList[i].YXBX.Value * pFloating);
                            if (SessionManage.GetLoginUser() != null)
                            {
                                pCheckNaturalList[i].UpdBy = SessionManage.GetLoginUser().UserName;
                            }
                            pCheckNaturalList[i].UpdDate = DateTime.Now;
                            pCheckNaturalList[i].State = "1";
                        }
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
                    pReturnValue.ResultValue = TipInfo.CalibrationRangeFaile;
                }
            }
            return pReturnValue;
        }

        #endregion

        #region 11 Private Methods

        /// <summary>
        /// 获取和构造报表11的数据
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private List<HDayStaExEnViewModel> GetData(Common.QueryParameters para)
        {
            //补数据
            RepairData(para);

            List<HDayStaExEnViewModel> list = new List<HDayStaExEnViewModel>();

            #region 构建数据结构

            //构建列名行
            HDayStaExEnViewModel modelTitle = new HDayStaExEnViewModel();
            modelTitle.Belong = "所属高速";
            modelTitle.Total = "合计";
            modelTitle.Num = "序号";
            modelTitle.Name = "收费站名称";
            modelTitle.Date1 = para.StartTime.Value.AddDays(0).ToString("M月d日");
            modelTitle.Date2 = para.StartTime.Value.AddDays(1).ToString("M月d日");
            modelTitle.Date3 = para.StartTime.Value.AddDays(2).ToString("M月d日");
            modelTitle.Date4 = para.StartTime.Value.AddDays(3).ToString("M月d日");
            modelTitle.Date5 = para.StartTime.Value.AddDays(4).ToString("M月d日");
            modelTitle.Date6 = para.StartTime.Value.AddDays(5).ToString("M月d日");
            modelTitle.Date7 = para.StartTime.Value.AddDays(6).ToString("M月d日");
            modelTitle.Date8 = para.StartTime.Value.AddDays(7).ToString("M月d日");
            modelTitle.Date9 = para.StartTime.Value.AddDays(8).ToString("M月d日");
            modelTitle.Date10 = para.StartTime.Value.AddDays(9).ToString("M月d日");
            modelTitle.Date11 = para.StartTime.Value.AddDays(10).ToString("M月d日");
            modelTitle.Date12 = para.StartTime.Value.AddDays(11).ToString("M月d日");
            modelTitle.Date13 = para.StartTime.Value.AddDays(12).ToString("M月d日");
            modelTitle.Date14 = para.StartTime.Value.AddDays(13).ToString("M月d日");
            modelTitle.Date15 = para.StartTime.Value.AddDays(14).ToString("M月d日");
            qModel.TitleList = new List<HDayStaExEnViewModel>();
            qModel.TitleList.Add(modelTitle);

            //构建杨村站流量行
            HDayStaExEnViewModel modelYC = new HDayStaExEnViewModel();
            modelYC.Belong = "京津塘高速";
            modelYC.Num = "43";
            modelYC.Name = "杨村站";
            list.Add(modelYC);

            //构建宜兴埠东站流量行
            HDayStaExEnViewModel modelYXBD = new HDayStaExEnViewModel();
            modelYXBD.Belong = "京津塘高速";
            modelYXBD.Num = "44";
            modelYXBD.Name = "宜兴埠东站";
            list.Add(modelYXBD);

            //构建宜兴埠西站流量行
            HDayStaExEnViewModel modelYXBX = new HDayStaExEnViewModel();
            modelYXBX.Belong = "京津塘高速";
            modelYXBX.Num = "45";
            modelYXBX.Name = "宜兴埠西站";
            list.Add(modelYXBX);

            //构建金钟路站流量行
            HDayStaExEnViewModel modelJZL = new HDayStaExEnViewModel();
            modelJZL.Belong = "京津塘高速";
            modelJZL.Num = "46";
            modelJZL.Name = "金钟路站";
            list.Add(modelJZL);

            //构建机场站流量行
            HDayStaExEnViewModel modelJC = new HDayStaExEnViewModel();
            modelJC.Belong = "京津塘高速";
            modelJC.Num = "47";
            modelJC.Name = "机场站";
            list.Add(modelJC);


            //构建空港经济区站流量行
            HDayStaExEnViewModel modelKG = new HDayStaExEnViewModel();
            modelKG.Belong = "京津塘高速";
            modelKG.Num = "48";
            modelKG.Name = "空港经济区站";
            list.Add(modelKG);

            //构建塘沽西站流量行
            HDayStaExEnViewModel modelTGX = new HDayStaExEnViewModel();
            modelTGX.Belong = "京津塘高速";
            modelTGX.Num = "49";
            modelTGX.Name = "塘沽西站";
            list.Add(modelTGX);

            //构建机场站流量行
            HDayStaExEnViewModel modelTGXF = new HDayStaExEnViewModel();
            modelTGXF.Belong = "京津塘高速";
            modelTGXF.Num = "50";
            modelTGXF.Name = "塘沽西分站";
            list.Add(modelTGXF);


            //构建塘沽北站流量行
            HDayStaExEnViewModel modelTGB = new HDayStaExEnViewModel();
            modelTGB.Belong = "京津塘高速";
            modelTGB.Num = "51";
            modelTGB.Name = "塘沽北站";
            list.Add(modelTGB);

            #endregion

            if (para.StartTime != null)
            {
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    //查询数据
                    var lst = StrWhere(db, para).OrderBy(a => a.CalcuTime).ToList();
                    // list.AddRange(lst);

                    //按条件查询后有数据
                    if (list != null && list.Count > 0)
                    {
                        //计算查询日期之间的差值
                        int countMax = (para.EndTime.Value - para.StartTime.Value).Days + 1;

                        for (int j = 0; j < lst.Count; j++)
                        {
                            #region 将数据库中存放的数据赋值到表中

                            for (int i = 1; i < 16; i++)
                            {
                                Type myType = qModel.TitleList[0].GetType();
                                PropertyInfo pinfo = myType.GetProperty("Date" + i);

                                if (lst[j].CalcuTime.Value.ToString("M月d日").ToString() == pinfo.GetValue(qModel.TitleList[0]).ToString())
                                {
                                    Type type1 = list[0].GetType();
                                    PropertyInfo pinfo1 = type1.GetProperty("Date" + i);
                                    pinfo1.SetValue(list[0], lst[j].YC.ToString());

                                    Type type2 = list[1].GetType();
                                    PropertyInfo pinfo2 = type2.GetProperty("Date" + i);
                                    pinfo2.SetValue(list[1], lst[j].YXBD.ToString());

                                    Type type3 = list[2].GetType();
                                    PropertyInfo pinfo3 = type3.GetProperty("Date" + i);
                                    pinfo3.SetValue(list[2], lst[j].YXBX.ToString());

                                    Type type4 = list[3].GetType();
                                    PropertyInfo pinfo4 = type4.GetProperty("Date" + i);
                                    pinfo4.SetValue(list[3], lst[j].JZL.ToString());

                                    Type type5 = list[4].GetType();
                                    PropertyInfo pinfo5 = type5.GetProperty("Date" + i);
                                    pinfo5.SetValue(list[4], lst[j].JC.ToString());

                                    Type type6 = list[5].GetType();
                                    PropertyInfo pinfo6 = type6.GetProperty("Date" + i);
                                    pinfo6.SetValue(list[5], lst[j].KG.ToString());

                                    Type type7 = list[6].GetType();
                                    PropertyInfo pinfo7 = type7.GetProperty("Date" + i);
                                    pinfo7.SetValue(list[6], lst[j].TGX.ToString());

                                    Type type8 = list[7].GetType();
                                    PropertyInfo pinfo8 = type8.GetProperty("Date" + i);
                                    pinfo8.SetValue(list[7], lst[j].TGXF.ToString());

                                    Type type9 = list[8].GetType();
                                    PropertyInfo pinfo9 = type9.GetProperty("Date" + i);
                                    pinfo9.SetValue(list[8], lst[j].TGB.ToString());

                                    break;
                                }
                            }
                            #endregion
                        }
                    }

                    #region 计算合计行

                    foreach (HDayStaExEnViewModel model in list)
                    {
                        double total = -1;
                        if (!string.IsNullOrEmpty(model.Date1))
                            total = double.Parse(model.Date1);
                        if (!string.IsNullOrEmpty(model.Date2))
                            total = total + double.Parse(model.Date2);
                        if (!string.IsNullOrEmpty(model.Date3))
                            total = total + double.Parse(model.Date3);
                        if (!string.IsNullOrEmpty(model.Date4))
                            total = total + double.Parse(model.Date4);
                        if (!string.IsNullOrEmpty(model.Date5))
                            total = total + double.Parse(model.Date5);
                        if (!string.IsNullOrEmpty(model.Date6))
                            total = total + double.Parse(model.Date6);
                        if (!string.IsNullOrEmpty(model.Date7))
                            total = total + double.Parse(model.Date7);
                        if (!string.IsNullOrEmpty(model.Date8))
                            total = total + double.Parse(model.Date8);
                        if (!string.IsNullOrEmpty(model.Date9))
                            total = total + double.Parse(model.Date9);
                        if (!string.IsNullOrEmpty(model.Date10))
                            total = total + double.Parse(model.Date10);
                        if (!string.IsNullOrEmpty(model.Date11))
                            total = total + double.Parse(model.Date11);
                        if (!string.IsNullOrEmpty(model.Date12))
                            total = total + double.Parse(model.Date12);
                        if (!string.IsNullOrEmpty(model.Date13))
                            total = total + double.Parse(model.Date13);
                        if (!string.IsNullOrEmpty(model.Date14))
                            total = total + double.Parse(model.Date14);
                        if (!string.IsNullOrEmpty(model.Date15))
                            total = total + double.Parse(model.Date15);
                        if (total > -1)
                            model.Total = total.ToString();
                    }

                    #endregion

                    //存储到缓存中
                    listExport.Clear();
                    listExport.AddRange(list);
                }
            }
            return list;
        }

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
                    int day = (para.EndTime.Value - para.StartTime.Value).Days + 1;
                    for (int i = 0; i < day; i++)
                    {
                        DateTime dtime = para.StartTime.Value.AddDays(i);

                        //时间小于当前时间
                        if (DateTime.Now > dtime)
                        {
                            bool flag = StrWhere(db, para).Select(a => a.CalcuTime).ToList().Contains(dtime);
                            if (!flag)//补数据
                            {
                                RP_HDayAADT hday = new RP_HDayAADT();
                                hday.CalcuTime = dtime;
                                hday.CompGrow = 0;
                                if (SessionManage.GetLoginUser() != null)
                                {
                                    hday.CrtBy = SessionManage.GetLoginUser().UserName;
                                }
                                hday.CrtDate = DateTime.Now;
                                hday.Id = Guid.NewGuid();
                                hday.JC = 0;
                                hday.JZL = 0;
                                hday.KG = 0;
                                hday.Out = 0;
                                hday.SameSum = 0;
                                hday.State = "0";
                                hday.Sum = 0;
                                hday.TGB = 0;
                                hday.TGX = 0;
                                hday.TGXF = 0;
                                hday.YC = 0;
                                hday.YXBD = 0;
                                hday.YXBX = 0;
                                db.RP_HDayAADT.Add(hday);
                            }
                            else//将数据中有空值的改成0
                            {
                                var hday = db.RP_HDayAADT.Where(a => a.CalcuTime == dtime).ToList()[0];
                                if (hday.JC == null)
                                    hday.JC = 0;
                                if (hday.JZL == null)
                                    hday.JZL = 0;
                                if (hday.KG == null)
                                    hday.KG = 0;
                                if (hday.Out == null)
                                    hday.Out = 0;
                                if (hday.SameSum == null)
                                    hday.SameSum = 0;
                                if (hday.Sum == null)
                                    hday.Sum = 0;
                                if (hday.TGB == null)
                                    hday.TGB = 0;
                                if (hday.TGX == null)
                                    hday.TGX = 0;
                                if (hday.TGXF == null)
                                    hday.TGXF = 0;
                                if (hday.YC == null)
                                    hday.YC = 0;
                                if (hday.YXBD == null)
                                    hday.YXBD = 0;
                                if (hday.YXBX == null)
                                    hday.YXBX = 0;
                            }
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
        /// 查询条件组合查询语句
        /// </summary>
        /// <param name="db"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        private IQueryable<RP_HDayAADT> StrWhere(DataSubmittedEntities db, Common.QueryParameters para)
        {
            var strWhere = db.RP_HDayAADT.Where(a => true);

            //某时间段之间的所有数据
            if (para.EndTime != DateTime.Parse("0001/1/1 0:00:00") && para.StartTime != DateTime.Parse("0001/1/1 0:00:00") && para.EndTime != null && para.StartTime != null && para.StartTime <= para.EndTime)
            {
                DateTime dt = ((DateTime)para.EndTime).AddDays(1);
                strWhere = strWhere.Where(a => a.CalcuTime >= para.StartTime & a.CalcuTime < dt);

            }
            //大于某时间的所有数据
            else if (para.StartTime != null && para.StartTime != DateTime.Parse("0001/1/1 0:00:00"))
            {
                strWhere = strWhere.Where(a => a.CalcuTime >= para.StartTime);
            }
            //小于某时间的所有数据
            else if (para.EndTime != null && para.EndTime != DateTime.Parse("0001/1/1 0:00:00"))
            {
                DateTime dt = ((DateTime)para.EndTime).AddDays(1);
                strWhere = strWhere.Where(a => a.CalcuTime < dt);
            }
            //获取配置中的默认时间
            else
            {
                var config = db.OT_HDayConfig.Where(a => a.Id == 11).Select(a => new
                {
                    HDayEnd = (DateTime)a.HDayEnd,
                    HDayStart = (DateTime)a.HDayStart
                }).ToList();
                if (config != null & config.Count > 0)
                {
                    DateTime dtEnd = ((DateTime)config[0].HDayEnd).AddDays(1),
                        dtStart = config[0].HDayStart;
                    strWhere = strWhere.Where(a => a.CalcuTime >= dtStart & a.CalcuTime <= dtEnd);
                }
            }
            return strWhere;
        }

        #endregion
    }
}

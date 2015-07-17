/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个报表7实现类文件
* 创建标识：ta0395侯兴鼎20141216
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
    /// 报表7实现类:黄金周京津塘高速公路交通量及客运情况统计表
    /// </summary>
    public class AADTAndTransCalcu : ReportRelated, IAADTAndTransCalcu
    {
        #region 3 Fields

        /// <summary>
        /// 查询结果
        /// </summary>
        AADTAndTransCalcuQViewModel qModel = new AADTAndTransCalcuQViewModel();

        /// <summary>
        /// 查询数据缓存，导出时直接使用，而无需再到数据库中查
        /// </summary>
        List<AADTAndTransCalcuViewModel> listExport = new List<AADTAndTransCalcuViewModel>();

        #endregion

        #region 9 Public Methods

        /// <summary>
        /// 获取查询条件的配置值
        /// </summary>
        /// <returns></returns>
        public Common.ViewModels.AADTAndTransCalcuWViewModel GetHdayExEnWhere()
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                var hday = db.OT_HDayConfig.Where(a => a.Id == 7).ToList();

                AADTAndTransCalcuWViewModel model = new AADTAndTransCalcuWViewModel();
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
            qModel.ReportData = GetData(para); 

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
        public Common.CustomResult Update(Common.ViewModels.AADTAndTransCalcuUViewModel args)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    CustomResult pReturnValue = new CustomResult();

                    bool flag = false;//标记数据库中是否有数据修改
                    foreach (AADTAndTransCalcuViewModel model in args.DataInfo)
                    {
                        if (!string.IsNullOrEmpty(model.CalcuTimeUpdate))
                        {
                            DateTime dt = new DateTime();
                            if (DateTime.TryParse(model.CalcuTimeUpdate, out dt))
                            {
                                var list = db.RP_AADTAndTransCalcu.Where(a => a.CalcuTime == dt).ToList();
                                foreach (var item in list)
                                {
                                    flag = true;

                                    item.EnTra = model.EnTra;
                                    item.EnCar = model.EnCar;
                                    //“旅客量”=“其中客车数”*5.84/10000
                                    item.EnTrav = item.EnCar == null ? 0.00 : Math.Round(item.EnCar.Value * 5.84 / 10000, 2);

                                    item.ExCar = model.ExCar;
                                    item.ExTra = model.ExTra;
                                    //“旅客量”=“其中客车数”*5.84/10000
                                    item.ExTrav = item.ExCar == null ? 0.00 : Math.Round(item.ExCar.Value * 5.84 / 10000, 2);

                                    item.State = "1";
                                    item.UpdDate = DateTime.Now;
                                    if (SessionManage.GetLoginUser() != null)
                                    {
                                        item.UpdBy = SessionManage.GetLoginUser().UserName;
                                    }
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
        ///  更改Excel工作簿内容
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
                        title = string.Format("{0}年“{1}”黄金周京津塘高速公路交通量及客运情况统计表", ((DateTime)para.EndTime).Year, holidayName[0].ToString());
                    SetValue(sheet, 0, 0, title);
                }

                if (listExport != null && listExport.Count > 0)
                {
                    for (int i = 0; i < listExport.Count; i++)
                    {
                        SetValue(sheet, i + 4, 0, listExport[i].CalcuTime);
                        SetValue(sheet, i + 4, 1, listExport[i].EnTra == null ? "" : listExport[i].EnTra.ToString());
                        SetValue(sheet, i + 4, 2, listExport[i].EnCar == null ? "" : listExport[i].EnCar.ToString());
                        SetValue(sheet, i + 4, 3, listExport[i].EnTrav == null ? "0.00" : listExport[i].EnTrav.Value.ToString("F2"));
                        SetValue(sheet, i + 4, 4, listExport[i].ExTra == null ? "" : listExport[i].ExTra.ToString());
                        SetValue(sheet, i + 4, 5, listExport[i].ExCar == null ? "" : listExport[i].ExCar.ToString());
                        SetValue(sheet, i + 4, 6, listExport[i].ExTrav == null ? "0.00" : listExport[i].ExTrav.Value.ToString("F2"));
                    }
                    SetValue(sheet, listExport.Count + 4, 0, string.Format("统计人：{0}", qModel.CrtBy)); ;
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
            if (para.ReportType == 7)
            {
                if (para.EndTime != null && para.StartTime != null)
                {
                    int count = (para.EndTime.Value - para.StartTime.Value).Days + 1;
                    if ((para.EndTime.Value - para.StartTime.Value).Days + 1 > 15)
                        count = 15;
                    path = string.Format(@"{0}Reporttemplate\DynamicReport\{1}\编号07--黄金周京津塘高速公路交通量及客运情况统计表.xlsx", AppDomain.CurrentDomain.BaseDirectory, count);
                    reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
                }
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
                    List<RP_AADTAndTransCalcu> pRefNaturalList = db.RP_AADTAndTransCalcu.Where(a => a.CalcuTime >= para.LastYearStart & a.CalcuTime <= para.LastYearEnd).OrderBy(a => a.CalcuTime).ToList();
                    //如果参考日期数据为0 则返回失败
                    if (pRefNaturalList == null || pRefNaturalList.Count == 0)
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.CalibrationFaileRefNoData;
                        return pReturnValue;
                    }
                    //需要校正的数据
                    var pCheckNaturalList = db.RP_AADTAndTransCalcu.Where(a => a.CalcuTime >= para.StartTime & a.CalcuTime <= para.EndTime).OrderBy(a => a.CalcuTime).ToList();
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
                            if (pRefNaturalList[i].EnTrav != null)
                                pCheckNaturalList[i].EnTrav = Math.Round(pRefNaturalList[i].EnTrav.Value * pFloating);
                            if (pRefNaturalList[i].EnTra != null)
                                pCheckNaturalList[i].EnTra = Math.Round(pRefNaturalList[i].EnTra.Value * pFloating);
                            if (pRefNaturalList[i].EnCar != null)
                                pCheckNaturalList[i].EnCar = Math.Round(pRefNaturalList[i].EnCar.Value * pFloating);

                            if (pRefNaturalList[i].ExCar != null)
                                pCheckNaturalList[i].ExCar = Math.Round(pRefNaturalList[i].ExCar.Value * pFloating);

                            if (pRefNaturalList[i].ExTra != null)
                                pCheckNaturalList[i].ExTra = Math.Round(pRefNaturalList[i].ExTra.Value * pFloating);
                            if (pRefNaturalList[i].ExTrav != null)
                                pCheckNaturalList[i].ExTrav = Math.Round(pRefNaturalList[i].ExTrav.Value * pFloating);

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
        /// 补数据
        /// </summary>
        /// <param name="para"></param>
        private void RepairData(QueryParameters para)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    DateTime dtQuery = new DateTime();
                    //判断传入的时间段是否正确
                    if (para.StartTime != null && para.EndTime != null && DateTime.TryParse(para.StartTime.Value.ToString(), out dtQuery) && DateTime.TryParse(para.EndTime.Value.ToString(), out dtQuery))
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
                                    RP_AADTAndTransCalcu hday = new RP_AADTAndTransCalcu();
                                    hday.CalcuTime = dtime;

                                    if (SessionManage.GetLoginUser() != null)
                                    {
                                        hday.CrtBy = SessionManage.GetLoginUser().UserName;
                                    }
                                    hday.CrtDate = DateTime.Now;
                                    hday.Id = Guid.NewGuid();
                                    hday.EnCar = 0;
                                    hday.EnTra = 0;
                                    hday.EnTrav = 0.00;
                                    hday.ExCar = 0;
                                    hday.ExTra = 0;
                                    hday.State = "0";
                                    hday.ExTrav = 0.00;

                                    db.RP_AADTAndTransCalcu.Add(hday);
                                }
                                else//将数据中有空值的改成0
                                {
                                    var hday = db.RP_AADTAndTransCalcu.Where(a => a.CalcuTime == dtime).ToList()[0];
                                    if (hday.EnCar == null)
                                        hday.EnCar = 0;
                                    if (hday.EnTra == null)
                                        hday.EnTra = 0;
                                    if (hday.EnTrav == null)
                                        hday.EnTrav = 0.00;
                                    if (hday.ExCar == null)
                                        hday.ExCar = 0;
                                    if (hday.ExTra == null)
                                        hday.ExTra = 0;
                                    if (hday.ExTrav == null)
                                        hday.ExTrav = 0.00;
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
        }

        /// <summary>
        /// 获取和构造报表7的数据
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private List<AADTAndTransCalcuViewModel> GetData(Common.QueryParameters para)
        {
            //补数据
            RepairData(para);

            List<AADTAndTransCalcuViewModel> list = new List<AADTAndTransCalcuViewModel>();
            qModel.CrtBy = "无";//获取统计人
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //构造数据最小日期与查询最小日期之间的数据
                if (StrWhere(db, para) != null && StrWhere(db, para).Count() > 0)
                {
                    DateTime dt = StrWhere(db, para).Min(a => a.CalcuTime);
                    int countMin = (dt - (DateTime)para.StartTime.Value).Days;

                    for (int i = 0; i < countMin; i++)
                    {
                        AADTAndTransCalcuViewModel model1 = new AADTAndTransCalcuViewModel();
                        model1.CalcuTime = para.StartTime.Value.AddDays(i).ToString("M月d日");
                        model1.EnTrav = 0.00;
                        model1.ExTrav = 0.00;
                        list.Add(model1);
                    }
                }

                //查询数据
                var lst = StrWhere(db, para).OrderBy(a => a.CalcuTime).Select(a => new AADTAndTransCalcuViewModel
                {
                    EnTrav = a.EnTrav,
                    EnTra = a.EnTra,
                    EnCar = a.EnCar,
                    CalcuTimeUpdate = a.CalcuTime.ToString(),
                    ExCar = a.ExCar,
                    ExTra = a.ExTra,
                    ExTrav = a.ExTrav,
                    CalcuTime = ((DateTime)a.CalcuTime).Month + "月" + ((DateTime)a.CalcuTime).Day + "日",

                }).ToList();
                list.AddRange(lst);

                //按条件查询后有数据
                if (list != null && list.Count > 0)
                {
                    string name = StrWhere(db, para).First(a => true).CrtBy;
                    qModel.CrtBy = (string.IsNullOrEmpty(name) ? "无" : name);//获取统计人

                    //计算数据最大日期与查询最大日期之间的差值
                    int countMax = ((DateTime)para.EndTime - StrWhere(db, para).Max(a => a.CalcuTime)).Days;

                    //构造已有数据的最大日期到最大查询日期之间的数据
                    for (int i = 1; i <= countMax; i++)
                    {
                        AADTAndTransCalcuViewModel model1 = new AADTAndTransCalcuViewModel();
                        model1.CalcuTime = StrWhere(db, para).Max(a => a.CalcuTime).AddDays(i).ToString("M月d日");
                        list.Add(model1);
                    }
                }
                else//查询后无数据则按照查询时间构造数据
                {
                    for (int i = 0; i < ((DateTime)para.EndTime - (DateTime)para.StartTime).Days + 1; i++)
                    {
                        AADTAndTransCalcuViewModel model2 = new AADTAndTransCalcuViewModel();
                        model2.CalcuTime = para.StartTime.Value.AddDays(i).ToString("M月d日");
                        model2.EnTrav = 0.00;
                        model2.ExTrav = 0.00;
                        list.Add(model2);
                    }
                }

                //添加合计行
                AADTAndTransCalcuViewModel modelTotal = new AADTAndTransCalcuViewModel();
                modelTotal.EnCar = list.Sum(a => a.EnCar);
                modelTotal.EnTra = list.Sum(a => a.EnTra);
                modelTotal.EnTrav = Math.Round((double)list.Sum(a => a.EnTrav), 2);
                modelTotal.ExCar = list.Sum(a => a.ExCar);
                modelTotal.ExTra = list.Sum(a => a.ExTra);
                modelTotal.ExTrav = Math.Round((double)list.Sum(a => a.ExTrav), 2);
                modelTotal.CalcuTime = "合计";
                list.Add(modelTotal);

                //存储到缓存中
                listExport.Clear();
                listExport.AddRange(list);
            }
            return list;
        }

        /// <summary>
        /// 查询条件组合查询语句
        /// </summary>
        /// <param name="db"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        private IQueryable<RP_AADTAndTransCalcu> StrWhere(DataSubmittedEntities db, Common.QueryParameters para)
        {
            var strWhere = db.RP_AADTAndTransCalcu.Where(a => true);

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
                var config = db.OT_HDayConfig.Where(a => a.Id == 7).Select(a => new
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
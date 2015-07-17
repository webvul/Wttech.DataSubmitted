/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/11 14:38:54
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.IBLL;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.Common.ViewModels;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Transactions;
using System.IO;
using System.Reflection;
using Wttech.DataSubmitted.Common.Resources;
using System.Linq.Expressions;
using Wttech.DataSubmitted.IBLL.IReport;
using Wttech.DataSubmitted.BLL.Tool;
#endregion

namespace Wttech.DataSubmitted.BLL.Report
{
    /// <summary>
    /// 每日报送15.16.17
    /// </summary>
    public class NaturalTrafficStatistical : ReportRelated, INaturalTrafficStatistical
    {
        #region 9 Public Methods

        /// <summary>
        /// 校正数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CustomResult CalibrationData(QueryParameters para)
        {
            CustomResult pReturnValue = new CustomResult();
            QueryNaturalInfoViewModel queryNatural = new QueryNaturalInfoViewModel();
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
            double pFloating = para.FloatingRange * 0.01;
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
                    List<NaturalInfoViewModel> pRefNaturalList = db.RP_NatSta.Where(s => s.CalcuTime == para.LastYearStart && s.StaType == para.StationType && calibrationDataHour.Contains(s.HourPer.Value.ToString())).ToList().Select(s => new NaturalInfoViewModel
                    {
                        HourPer = (byte)s.HourPer,
                        DayTraffic = (double)s.Sum,
                        InDayTraffic = (double)s.EnNum,
                        OutDayTraffic = (double)s.ExNum,
                        RunningStatus = s.RunStae,
                        Remark = s.Rek
                    }).ToList();
                    //如果参考日期数据为0 则返回失败
                    if (pRefNaturalList.Count == 0)
                    {
                        pReturnValue.ResultKey = (byte)EResult.Fail;
                        pReturnValue.ResultValue = TipInfo.CalibrationFaileRefNoData;
                        return pReturnValue;
                    }
                    //判断校正数据日期是否合理
                    if (para.LastYearStart < para.StartTime && para.StartTime < DateTime.Now.AddDays(1))
                    {
                        //需要校正的数据
                        var pCheckNaturalList = db.RP_NatSta.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType && calibrationDataHour.Contains(s.HourPer.Value.ToString())).ToList();
                        if (pCheckNaturalList.Count == 0)
                        {
                            pReturnValue.ResultKey = (byte)EResult.Fail;
                            pReturnValue.ResultValue = TipInfo.CalibrationFaileNoData;
                            return pReturnValue;
                        }
                        using (TransactionScope tran = new TransactionScope())
                        {
                            foreach (var item in pCheckNaturalList)
                            {
                                IEnumerable<NaturalInfoViewModel> plist = pRefNaturalList.Where(i => i.HourPer == item.HourPer);
                                if (plist.Count() > 0)
                                {
                                    NaturalInfoViewModel pTemp = plist.First();
                                    item.EnNum = (int)(pTemp.InDayTraffic + pTemp.InDayTraffic * pFloating);
                                    item.ExNum = (int)(pTemp.OutDayTraffic + pTemp.OutDayTraffic * pFloating);
                                    item.Sum = item.EnNum + item.ExNum;
                                    item.RunStae = pTemp.RunningStatus;
                                    item.CalcuTime = para.StartTime.Value;
                                    item.StaType = para.StationType;
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
        /// 15,16,17根据查询条件获取数据
        /// </summary>
        /// <param name="para">查询条件类</param>
        /// <returns></returns>
        public IReportViewModel GetListByPra(QueryParameters para)
        {
            QueryNaturalInfoViewModel pReturn = new QueryNaturalInfoViewModel();
            try
            {
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    //添加合计
                    new TrafficStatisticalSum().CreateOrUpdateSum(para, (int)para.StationType);
                    //某类报表所涉及的收费站名称
                    string[] pStationNames = null;

                    if (para.ReportType == 17)
                    {
                        //报表17所包含的站名称
                        pStationNames = db.OT_Station.Where(s => s.Num == para.StationType.Value.ToString()).Select(s => s.Name).ToArray();

                        pReturn.ReportData = db.RP_NatSta.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType && s.HourPer == 24).ToList().Select(s => new NaturalInfoViewModel
                        {
                            HourPer = 0,
                            DayTraffic = double.Parse(string.Format("{0:0.0000}", s.Sum * 0.0001)),
                            InDayTraffic = double.Parse(string.Format("{0:0.0000}", s.EnNum * 0.0001)),
                            OutDayTraffic = double.Parse(string.Format("{0:0.0000}", s.ExNum * 0.0001)),
                            RunningStatus = s.RunStae,
                            Remark = s.Rek
                        }).ToList();
                        if (pReturn.ReportData.Count == 0)
                        {
                            pReturn.ReportData.Add(new NaturalInfoViewModel
                            {
                                HourPer = 0,
                                RoadName = SystemConst.RoadName,
                                DayTraffic = null,
                                InDayTraffic = null,
                                OutDayTraffic = null,
                                RunningStatus = SystemConst.RunningStatus,
                                Remark = ""
                            });
                            pReturn.ReportData.Add(new NaturalInfoViewModel
                            {
                                HourPer = 24,
                                RoadName = SystemConst.RoadName,
                                DayTraffic = null,
                                InDayTraffic = null,
                                OutDayTraffic = null,
                                RunningStatus = "",
                                Remark = ""
                            });
                        }
                        else
                        {
                            pReturn.ReportData.Add(new NaturalInfoViewModel
                            {
                                HourPer = 24,
                                RoadName = SystemConst.RoadName,
                                DayTraffic = double.Parse(string.Format("{0:0.0000}", pReturn.ReportData[0].DayTraffic)),
                                InDayTraffic = double.Parse(string.Format("{0:0.0000}", pReturn.ReportData[0].InDayTraffic)),
                                OutDayTraffic = double.Parse(string.Format("{0:0.0000}", pReturn.ReportData[0].OutDayTraffic)),
                                RunningStatus = pReturn.ReportData[0].RunningStatus,
                                Remark = pReturn.ReportData[0].Remark
                            });
                        }
                    }
                    else
                    {
                        //报表15,16所包含的站名称
                        pStationNames = db.OT_Station.Where(s => s.District == para.StationType.Value).Select(s => s.Name).ToArray();
                        //获取查询日期当天所有已上传的数据，包括合计
                        pReturn.ReportData = db.RP_NatSta.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType).Select(s => new NaturalInfoViewModel
                        {
                            HourPer = (byte)s.HourPer,
                            DayTraffic = (double)s.Sum,
                            InDayTraffic = (double)s.EnNum,
                            OutDayTraffic = (double)s.ExNum,
                            RunningStatus = s.RunStae,
                            Remark = s.Rek
                        }).ToList();

                        //补充数据
                        IEnumerable<NaturalInfoViewModel> pNa = pReturn.ReportData.Where(s => s.HourPer != 24);
                        int pMaxTimeHour = -1;//补充在此之后的数据
                        int pMinTimeHour = -1;//补充在此之前的数据
                        if (pNa.Count() > 0)
                        {
                            pMinTimeHour = (int)pNa.Min(s => s.HourPer);
                            pMaxTimeHour = (int)pNa.Max(s => s.HourPer);
                        }
                        else
                        {
                            pReturn.ReportData.Add(new NaturalInfoViewModel
                            {
                                HourPer = 24,
                                DayTraffic = null,
                                InDayTraffic = null,
                                OutDayTraffic = null,
                                RunningStatus = SystemConst.RunningStatus,
                                Remark = ""
                            });
                        }
                        if (pMinTimeHour != -1 && pMinTimeHour != 0)
                        {
                            for (int i = 0; i < pMinTimeHour; i++)
                            {
                                InsertNull(para.StartTime.Value, (byte)i, para.StationType.Value);
                                pReturn.ReportData.Add(new NaturalInfoViewModel
                                {

                                    HourPer = (byte)i,
                                    DayTraffic = 0,
                                    InDayTraffic = 0,
                                    OutDayTraffic = 0,
                                    RunningStatus = SystemConst.RunningStatus,
                                    Remark = ""
                                });
                            }
                        }
                        //查看统计的最小时间和最大时间直接，数据是否统计上来
                        for (int i = pMinTimeHour + 1; i < pMaxTimeHour; i++)
                        {
                            if (db.RP_NatSta.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType && s.HourPer == (byte)i).Count() <= 0)
                            {
                                InsertNull(para.StartTime.Value, (byte)i, para.StationType.Value);
                                pReturn.ReportData.Add(new NaturalInfoViewModel
                                {

                                    HourPer = (byte)i,
                                    DayTraffic = 0,
                                    InDayTraffic = 0,
                                    OutDayTraffic = 0,
                                    RunningStatus = SystemConst.RunningStatus,
                                    Remark = ""
                                });
                            }
                        }
                        if (pMaxTimeHour < 23)
                        {
                            int pTemp = DateTime.Now.Hour;
                            for (int i = (int)pMaxTimeHour + 1; i < 24; i++)
                            {
                                //如果查询日期小于服务器时间，并且无数据上来，则认为是系统异常没有统计到数据，则将数据补充到数据库。
                                if (para.StartTime < DateTime.Now.Date)
                                {
                                    InsertNull(para.StartTime.Value, (byte)i, para.StationType.Value);
                                    pReturn.ReportData.Add(new NaturalInfoViewModel
                                    {
                                        HourPer = (byte)i,
                                        DayTraffic = 0,
                                        InDayTraffic = 0,
                                        OutDayTraffic = 0,
                                        RunningStatus = SystemConst.RunningStatus,
                                        Remark = ""
                                    });
                                }
                            }
                            if (para.StartTime == DateTime.Now.Date)
                            {
                                for (int n = 0; n < pTemp; n++)
                                {
                                    if (db.RP_NatSta.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType && s.HourPer == (byte)n).Count() <= 0)
                                    {
                                        InsertNull(para.StartTime.Value, (byte)n, para.StationType.Value);
                                        pReturn.ReportData.Add(new NaturalInfoViewModel
                                        {
                                            HourPer = (byte)n,
                                            DayTraffic = 0,
                                            InDayTraffic = 0,
                                            OutDayTraffic = 0,
                                            RunningStatus = SystemConst.RunningStatus,
                                            Remark = ""
                                        });
                                    }
                                }
                                for (int m = pTemp; m < 24; m++)
                                {
                                    pReturn.ReportData.Add(new NaturalInfoViewModel
                                    {
                                        HourPer = (byte)m,
                                        DayTraffic = null,
                                        InDayTraffic = null,
                                        OutDayTraffic = null,
                                        RunningStatus = SystemConst.RunningStatus,
                                        Remark = ""
                                    });
                                }
                            }
                        }

                        //按序号排序
                        pReturn.ReportData = pReturn.ReportData.OrderBy(s => s.HourPer).ToList();

                        //获取当前统计类型不包含的收费站列表
                        IEnumerable<OT_ErrorStation> pNoaccept = db.OT_ErrorStation.Where(i =>
                            pStationNames.Contains(i.StaName)).ToList().Where(s => s.CalcuTime == para.StartTime);

                        if (pNoaccept.Count() > 0)
                            pReturn.IsFull = 0;//不完整
                        else
                            pReturn.IsFull = 1;//完整                      
                    }
                    //获取报表备注
                    OT_HDayConfig holiday = HolidayConfig.GetInstance().GetById(para.ReportType);
                    if (holiday != null)
                    {
                        if (holiday.RptRemark != null)
                            pReturn.ReportRemark = holiday.RptRemark;
                        else
                            pReturn.ReportRemark = "";
                    }
                    else
                    {
                        pReturn.ReportRemark = "";
                    }
                }
            }
            catch (Exception e)
            {
                SystemLog.GetInstance().Info(e);
            }
            return pReturn;
        }
        /// <summary>
        /// 获取全部数据记录
        /// </summary>
        /// <returns></returns>
        public List<NaturalInfoViewModel> GetList()
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                List<NaturalInfoViewModel> naturalInfo = db.RP_NatSta.Where(s => s.StaType == (int)StationConfiguration.StationType.BeiJingDuan).Select(s => new NaturalInfoViewModel
                {
                    HourPer = (byte)s.HourPer,
                    DayTraffic = (double)s.Sum,
                    InDayTraffic = (double)s.EnNum,
                    OutDayTraffic = (double)s.ExNum,
                    RunningStatus = s.RunStae,
                    Remark = s.Rek
                }).ToList();
                return naturalInfo;
            }
        }
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public CustomResult Update(UpdateNaturalInfoViewModel args)
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
            if (args.StationType != 15)
            {
                args.StationType = 1;
            }
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                var pReportData = db.RP_NatSta.Where(s => s.CalcuTime == args.DataDate && s.StaType == args.StationType).ToList();
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        foreach (var item in args.UpdateData)
                        {
                            //获取查询日期当天所有已上传的数据，包括合计

                            var pDataTemp = pReportData.Where(i => i.HourPer == item.Num - 1).SingleOrDefault();
                            pDataTemp.EnNum = item.InDayTraffic;
                            pDataTemp.ExNum = item.OutDayTraffic;
                            pDataTemp.Sum = item.InDayTraffic + item.OutDayTraffic;
                            pDataTemp.Rek = item.Remark;
                            if (!string.IsNullOrEmpty(item.RunningStatus))
                            {
                                pDataTemp.RunStae = item.RunningStatus;
                            }
                            else
                            {
                                pDataTemp.RunStae = SystemConst.RunningStatus;
                            }
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
                    return pReturnValue;
                }
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
            if (para.ReportType == 15 && para.StationType == 1)//表15
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号15--交通自然流量（含免费）统计日报表（小时数据）--北京段.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
            }
            if (para.ReportType == 16 && para.StationType == 15)//大羊坊 表16
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号16--交通自然流量（含免费）统计日报表（小时数据）--断面数据.xlsx";
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
            }
            if (para.ReportType == 17 && para.StationType == 1)
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Reporttemplate\编号17--交通自然流量（含免费）统计日报表.xlsx";
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
            List<NaturalInfoViewModel> naturalInfoList;
            ISheet sheet = readworkbook.GetSheetAt(0);
            if (para.ReportType == 17)
            {
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    SetRemark(sheet, para.ReportType);
                    naturalInfoList = db.RP_NatSta.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType && s.HourPer == 24).Select(s => new NaturalInfoViewModel
                    {
                        HourPer = (byte)s.HourPer,
                        DayTraffic = (double)s.Sum,
                        InDayTraffic = (double)s.EnNum,
                        OutDayTraffic = (double)s.ExNum,
                        RunningStatus = s.RunStae,
                        Remark = s.Rek
                    }).ToList();
                }
                //设置日期
                SetReportDate(sheet, 1, 0, para.StartTime.Value, para.ReportType);
                if (naturalInfoList.Count == 1)
                {
                    SetValue(sheet, 3, 2, string.Format("{0:0.0000}", naturalInfoList[0].DayTraffic * 0.0001));
                    SetValue(sheet, 3, 3, string.Format("{0:0.0000}", naturalInfoList[0].InDayTraffic * 0.0001));
                    SetValue(sheet, 3, 4, string.Format("{0:0.0000}", naturalInfoList[0].OutDayTraffic * 0.0001));
                    SetValue(sheet, 3, 5, naturalInfoList[0].RunningStatus);
                    SetValue(sheet, 3, 6, naturalInfoList[0].Remark);
                    //合计
                    SetValue(sheet, 4, 2, string.Format("{0:0.0000}", naturalInfoList[0].DayTraffic * 0.0001));
                    SetValue(sheet, 4, 3, string.Format("{0:0.0000}", naturalInfoList[0].InDayTraffic * 0.0001));
                    SetValue(sheet, 4, 4, string.Format("{0:0.0000}", naturalInfoList[0].OutDayTraffic * 0.0001));
                }
            }
            else
            {
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    naturalInfoList = db.RP_NatSta.Where(s => s.CalcuTime == para.StartTime && s.StaType == para.StationType).Select(s => new NaturalInfoViewModel
                    {
                        HourPer = (byte)s.HourPer,
                        DayTraffic = (double)s.Sum,
                        InDayTraffic = (double)s.EnNum,
                        OutDayTraffic = (double)s.ExNum,
                        RunningStatus = s.RunStae,
                        Remark = s.Rek
                    }).ToList();
                }
                //添加报表备注
                SetReportRemark(sheet, 28, 1, para.ReportType);
                //添加报表日期
                SetReportDate(sheet, 1, 0, para.StartTime.Value, para.ReportType);

                foreach (NaturalInfoViewModel nainfo in naturalInfoList)
                {
                    if (nainfo.HourPer == 0)
                    {
                        if (nainfo.DayTraffic.HasValue)
                            SetValue(sheet, 3, 3, nainfo.DayTraffic.ToString());
                        if (nainfo.InDayTraffic.HasValue)
                            SetValue(sheet, 3, 4, nainfo.InDayTraffic.ToString());
                        if (nainfo.OutDayTraffic.HasValue)
                            SetValue(sheet, 3, 5, nainfo.OutDayTraffic.ToString());
                        if (nainfo.RunningStatus != null)
                            SetValue(sheet, 3, 6, nainfo.RunningStatus);
                        else
                            SetValue(sheet, 3, 6, SystemConst.RunningStatus);
                        SetValue(sheet, 3, 7, nainfo.Remark);
                    }
                    else if (nainfo.HourPer == 24)//合计
                    {
                        if (nainfo.DayTraffic.HasValue)
                            SetValue(sheet, 27, 3, nainfo.DayTraffic.ToString());
                        if (nainfo.InDayTraffic.HasValue)
                            SetValue(sheet, 27, 4, nainfo.InDayTraffic.ToString());
                        if (nainfo.OutDayTraffic.HasValue)
                            SetValue(sheet, 27, 5, nainfo.OutDayTraffic.ToString());
                        if (nainfo.RunningStatus != null)
                            SetValue(sheet, 27, 6, nainfo.RunningStatus);
                        else
                            SetValue(sheet, 27, 6, SystemConst.RunningStatus);
                        SetValue(sheet, 27, 7, nainfo.Remark);
                    }
                    else
                    {
                        if (nainfo.DayTraffic.HasValue)
                            SetValue(sheet, ((int)nainfo.HourPer + 3), 3, nainfo.DayTraffic.ToString());
                        if (nainfo.InDayTraffic.HasValue)
                            SetValue(sheet, ((int)nainfo.HourPer + 3), 4, nainfo.InDayTraffic.ToString());
                        if (nainfo.OutDayTraffic.HasValue)
                            SetValue(sheet, ((int)nainfo.HourPer + 3), 5, nainfo.OutDayTraffic.ToString());
                        if (nainfo.RunningStatus != null)
                            SetValue(sheet, ((int)nainfo.HourPer + 3), 6, nainfo.RunningStatus);
                        else
                            SetValue(sheet, ((int)nainfo.HourPer + 3), 6, SystemConst.RunningStatus);
                        //if (nainfo.Remark != null)
                        SetValue(sheet, ((int)nainfo.HourPer + 3), 7, nainfo.Remark);

                    }
                }
            }
            return readworkbook;
        }

        #endregion
        #region
        /// <summary>
        /// 补充数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="hour"></param>
        /// <param name="stationtype"></param>
        private void InsertNull(DateTime dt, byte hour, int stationtype)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                if (db.RP_NatSta.Where(s => s.CalcuTime == dt && s.StaType == stationtype && s.HourPer == hour).Count() <= 0)
                {
                    using (TransactionScope trans = new TransactionScope())
                    {

                        db.RP_NatSta.Add(new RP_NatSta()
                        {
                            Id = Guid.NewGuid(),
                            HourPer = hour,
                            Sum = 0,
                            EnNum = 0,
                            ExNum = 0,
                            RunStae = SystemConst.RunningStatus,
                            CalcuTime = dt,
                            StaType = stationtype,
                            State = "0"
                        });
                        db.SaveChanges();
                        trans.Complete();
                    }
                }
            }

        }
        /// <summary>
        /// 报表17备注
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="reporttype"></param>
        private void SetRemark(ISheet sheet, int reporttype)
        {
            try
            {
                //17备注备注规则，二者用分号分隔,符合全为英文下的，如果不包含两个人的信息则不加分号示例：负责人:张三 联系电话:022-6583411 手机:18888888888;填表人:张四 联系电话:022-6583411 手机:18888888888
                OT_HDayConfig holiday = HolidayConfig.GetInstance().GetById(reporttype);
                if (holiday != null)
                {
                    string[] remark = new string[] { };
                    string officer = string.Empty;
                    string preparer = string.Empty;
                    //包含两个人的信息
                    if (holiday.RptRemark.IndexOf(';') != -1)
                    {
                        remark = holiday.RptRemark.Split(';');
                        if (remark[0].Substring(0, 3).ToString() == "负责人")
                        {
                            officer = remark[0];
                            preparer = remark[1];
                        }
                        else
                        {
                            preparer = remark[0];
                            officer = remark[1];
                        }
                    }
                    else
                    {
                        if (holiday.RptRemark.Split(' ')[0].Substring(0, 3).ToString() == "负责人")
                        {
                            officer = holiday.RptRemark;
                        }
                        else
                        {
                            preparer = holiday.RptRemark;
                        }
                    }
                    if (!string.IsNullOrEmpty(officer))
                    {
                        string[] pTempOfficer = officer.Split(' ');
                        for (int i = 0; i < pTempOfficer.Count(); i++)
                        {
                            if (pTempOfficer[i].Substring(0, 3) == "负责人")
                            {
                                //负责人
                                SetValue(sheet, 6, 2, pTempOfficer[i]);
                            }
                            else if (pTempOfficer[i].Substring(0, 4) == "联系电话")
                            {
                                //负责人联系电话
                                SetValue(sheet, 6, 4, pTempOfficer[i].Substring(5));
                            }
                            else if (pTempOfficer[i].Substring(0, 2) == "手机")
                            {
                                //负责人手机
                                SetValue(sheet, 6, 6, pTempOfficer[i].Substring(3));
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(preparer))
                    {
                        string[] pTempPreparer = preparer.Split(' ');
                        for (int i = 0; i < pTempPreparer.Count(); i++)
                        {
                            //if (pTempPreparer[i].Substring(0, 3) == "填表人")
                            //{
                            //    //填表人
                            //    SetValue(sheet, 7, 2, pTempPreparer[i]);
                            //}
                            //else
                            if (pTempPreparer[i].Substring(0, 4) == "联系电话")
                            {
                                //填表人联系电话
                                SetValue(sheet, 7, 4, pTempPreparer[i].Substring(5));
                            }
                            else if (pTempPreparer[i].Substring(0, 2) == "手机")
                            {
                                //填表人手机
                                SetValue(sheet, 7, 6, pTempPreparer[i].Substring(3));
                            }
                        }
                    }
                    if (SessionManage.GetLoginUser() != null)
                    {
                        //填表人
                        SetValue(sheet, 7, 2, "填表人:" + SessionManage.GetLoginUser().UserName);
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Info(ex.Message);
            }
        }
        #endregion
    }
}

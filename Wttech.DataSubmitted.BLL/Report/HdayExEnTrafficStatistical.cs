/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：报表10：假期进出京交通流量表（北京段）实现类
 * 创建标识：ta0395侯兴鼎20141208
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
    /// 报表10：假期进出京交通流量表（北京段）实现类
    /// </summary>
    public class HdayExEnTrafficStatistical : ReportRelated, IHdayExEnTrafficStatistical
    {
        #region 3 Fields

        /// <summary>
        /// 查询数据缓存，导出时直接使用，而无需再到数据库中查
        /// </summary>
        List<HdayExEnViewModel> listExport = new List<HdayExEnViewModel>();

        /// <summary>
        /// 查询结果返回实体
        /// </summary>
        HdayExEnListViewModel modelHdayExEn = new HdayExEnListViewModel(); 

        #endregion

        #region 9 Public Methods

        /// <summary>
        /// 查询假期进出京交通量（北京段）
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Common.ViewModels.IReportViewModel GetListByPra(Common.QueryParameters para)
        {
            List<HdayExEnViewModel> list = GetData(para);
            modelHdayExEn.HdayExEn = list;
            return modelHdayExEn;
        }

        /// <summary>
        /// 导出报表
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
            if (para.ReportType == 10)
            {
                int count = (para.EndTime.Value - para.StartTime.Value).Days + 1;
                if ((para.EndTime.Value - para.StartTime.Value).Days + 1 > 15)
                    count = 15;
                path = string.Format(@"{0}Reporttemplate\DynamicReport\{1}\编号10--假期进出京交通流量表（北京段）.xlsx", AppDomain.CurrentDomain.BaseDirectory, count);
                reportpath = ExportHelper.GetInstance().ExportExcel(path, para, this);
            }
            return reportpath;
        }

        /// <summary>
        /// 获取查询条件的配置值
        /// </summary>
        /// <returns></returns>
        public HdayExEnWhereViewModel GetHdayExEnWhere()
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                var hday = db.OT_HDayConfig.Where(a => a.Id == 10).ToList();

                HdayExEnWhereViewModel model = new HdayExEnWhereViewModel();
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
        ///  更改Excel工作簿内容
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

                string title = string.Empty;
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    var holidayName = db.OT_Dic.Where(a => a.Id == para.HolidayId).Select(a => a.Name).ToList();
                    if (holidayName != null && holidayName.Count > 0)
                    {
                        title = string.Format("{0}年{1}假期进出京交通流量表（北京段）", ((DateTime)para.EndTime).Year, holidayName[0].ToString());
                    }
                    SetValue(sheet, 0, 0, title);
                }
                if (listExport != null)
                {
                    for (int i = 0; i < listExport.Count; i++)
                    {
                        SetValue(sheet, i + 3, 0, listExport[i].DataDate);
                        SetValue(sheet, i + 3, 1, listExport[i].LineEnSum == null ? "" : listExport[i].LineEnSum.ToString());
                        SetValue(sheet, i + 3, 2, listExport[i].LineExSum == null ? "" : listExport[i].LineExSum.ToString());
                        SetValue(sheet, i + 3, 3, listExport[i].Total);
                    }
                }
            }
            return readworkbook;
        } 

        #endregion

        #region 11 Private Methods

        /// <summary>
        /// 查询条件组合查询语句
        /// </summary>
        /// <param name="db"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        private IQueryable<RP_AADTSta> StrWhere(DataSubmittedEntities db, Common.QueryParameters para)
        {
            var strWhere = db.RP_AADTSta.Where(a => true);

            //某时间段之间的所有数据
            if (para.EndTime != DateTime.Parse("0001/1/1 0:00:00") && para.StartTime != DateTime.Parse("0001/1/1 0:00:00") && para.EndTime != null && para.StartTime != null && para.StartTime <= para.EndTime)
            {
                DateTime dt = ((DateTime)para.EndTime).AddDays(1);
                strWhere = strWhere.Where(a => a.CalculTime >= para.StartTime & a.CalculTime < dt);

            }
            //大于某时间的所有数据
            else if (para.StartTime != null && para.StartTime != DateTime.Parse("0001/1/1 0:00:00"))
            {
                strWhere = strWhere.Where(a => a.CalculTime >= para.StartTime);
            }
            //小于某时间的所有数据
            else if (para.EndTime != null && para.EndTime != DateTime.Parse("0001/1/1 0:00:00"))
            {
                DateTime dt = ((DateTime)para.EndTime).AddDays(1);
                strWhere = strWhere.Where(a => a.CalculTime < dt);
            }
            //获取配置中的默认时间
            else
            {
                var config = db.OT_HDayConfig.Where(a => a.Id == 10).Select(a => new
                {
                    HDayEnd = (DateTime)a.HDayEnd,
                    HDayStart = (DateTime)a.HDayStart
                }).ToList();
                if (config != null & config.Count > 0)
                {
                    DateTime dtEnd = ((DateTime)config[0].HDayEnd).AddDays(1),
                        dtStart = config[0].HDayStart;
                    strWhere = strWhere.Where(a => a.CalculTime >= dtStart & a.CalculTime <= dtEnd);
                }
            }
            return strWhere;
        }

        /// <summary>
        /// 获取和构造报表10的数据
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private List<HdayExEnViewModel> GetData(Common.QueryParameters para)
        {
            RepairData(para);//补数据
            List<HdayExEnViewModel> list = new List<HdayExEnViewModel>();
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //构造数据最小日期与查询最小日期之间的数据
                if (StrWhere(db, para) != null && StrWhere(db, para).Count() > 0)
                {

                    DateTime dt = StrWhere(db, para).Min(a => a.CalculTime);
                    int countMin = (dt - (DateTime)para.StartTime.Value).Days;

                    for (int i = 0; i < countMin; i++)
                    {
                        HdayExEnViewModel model1 = new HdayExEnViewModel();
                        model1.LineEnSum = null;
                        model1.LineExSum = null;
                        model1.Total = 0;
                        model1.DataDate = para.StartTime.Value.AddDays(i).ToString("M月d日");
                        list.Add(model1);
                    }
                }

                //查询数据
                var lst = StrWhere(db, para).OrderBy(a => a.CalculTime).Select(a => new HdayExEnViewModel
                {
                    DataDate = ((DateTime)a.CalculTime).Month + "月" + ((DateTime)a.CalculTime).Day + "日",
                    LineEnSum = (double)a.LineEnSum,
                    LineExSum = (double)a.LineExSum,
                    Total = (double)a.LineEnSum + (double)a.LineExSum
                }).ToList();
                list.AddRange(lst);

                //按条件查询后有数据
                if (list != null && list.Count > 0)
                {
                    //计算数据最大日期与查询最大日期之间的差值
                    int countMax = ((DateTime)para.EndTime - StrWhere(db, para).Max(a => a.CalculTime)).Days;
                    //构造已有数据的最大日期到最大查询日期之间的数据
                    for (int i = 1; i <= countMax; i++)
                    {
                        HdayExEnViewModel model1 = new HdayExEnViewModel();
                        model1.LineEnSum = null;
                        model1.LineExSum = null;
                        model1.Total = 0;
                        model1.DataDate = StrWhere(db, para).Max(a => a.CalculTime).AddDays(i).ToString("M月d日");
                        list.Add(model1);
                    }
                }
                else//查询后无数据则按照查询时间构造数据
                {
                    for (int i = 0; i < ((DateTime)para.EndTime - (DateTime)para.StartTime).Days + 1; i++)
                    {
                        HdayExEnViewModel model2 = new HdayExEnViewModel();
                        model2.LineEnSum = null;
                        model2.LineExSum = null;
                        model2.Total = 0;
                        model2.DataDate = ((DateTime)para.StartTime).AddDays(i).ToString("M月d日");
                        list.Add(model2);
                    }
                }

                //添加合计行
                HdayExEnViewModel modelTotal = new HdayExEnViewModel();
                modelTotal.LineEnSum = list.Sum(a => a.LineEnSum);
                modelTotal.LineExSum = list.Sum(a => a.LineExSum);
                modelTotal.Total = list.Sum(a => a.Total);
                modelTotal.DataDate = "合计";
                list.Add(modelTotal);

                //存储到缓存中
                listExport.Clear();
                listExport.AddRange(list);
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
                            bool flag = StrWhere(db, para).Select(a => a.CalculTime).ToList().Contains(dtime);
                            if (!flag)//补数据
                            {
                                RP_AADTSta hday = new RP_AADTSta();
                                hday.CalculTime = dtime;
                                if (SessionManage.GetLoginUser() != null)
                                {
                                    hday.CrtBy = SessionManage.GetLoginUser().UserName;
                                }
                                hday.CrtDate = DateTime.Now;
                                hday.Id = Guid.NewGuid();
                                hday.LineEnSum = 0;
                                hday.LineExSum = 0;
                                db.RP_AADTSta.Add(hday);
                            }
                            else//将数据中有空值的改成0
                            {
                                var hday = db.RP_AADTSta.Where(a => a.CalculTime == dtime).ToList()[0];
                                if (hday.LineEnSum == null)
                                    hday.LineEnSum = 0;
                                if (hday.LineExSum == null)
                                    hday.LineExSum = 0;
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

        #endregion
    }
}
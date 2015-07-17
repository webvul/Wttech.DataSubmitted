/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/20 15:59:15
 */

#region 引用
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.BLL.Tool;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL;

#endregion

namespace Wttech.DataSubmitted.BLL
{
    /// <summary>
    ///报表基类
    /// </summary>
    public class ReportRelated : IReportRelated
    {

        #region 9 Public Methods
        /// <summary>
        /// 获取无收费站数据列表
        /// </summary>
        /// <param name="para">统计站类型，数据日期</param>
        /// <returns></returns>
        public List<ReportRelatedViewModels> GetNoDataList(QueryParameters para)
        {
            List<ReportRelatedViewModels> pList = new List<ReportRelatedViewModels>();
            try
            {
                using (DataSubmittedEntities db = new DataSubmittedEntities())
                {
                    //获取某类报表所涉及的收费站名称
                    string[] pStationNames = null;
                    if (para.StationType == (int)StationConfiguration.StationID.DYF || para.StationType == (int)StationConfiguration.StationID.SCD)//大羊坊或泗村店收费站
                    {
                        pStationNames = db.OT_Station.Where(s => s.Num == para.StationType.Value.ToString()).Select(s => s.Name).ToArray();
                    }
                    else if (para.StationType == (int)StationConfiguration.StationType.BeiJingDuan || para.StationType == (int)StationConfiguration.StationType.HeBei || para.StationType == (int)StationConfiguration.StationType.TianJinDuan)//多个收费站
                    {
                        pStationNames = db.OT_Station.Where(s => s.District == para.StationType.Value).Select(s => s.Name).ToArray();
                    }
                    List<OT_ErrorStation> pNoaccept = new List<OT_ErrorStation>();
                    if (para.StartTime.HasValue && para.EndTime.HasValue)
                    {
                        pNoaccept = db.OT_ErrorStation.Where(i =>
                           pStationNames.Contains(i.StaName)).ToList().Where(s => s.CalcuTime >= para.StartTime && s.CalcuTime <= para.EndTime).ToList();
                    }
                    else
                    {
                        pNoaccept = db.OT_ErrorStation.Where(i =>
                                                 pStationNames.Contains(i.StaName)).ToList().Where(s => s.CalcuTime == para.StartTime).ToList();
                    }
                    var nolist = pNoaccept.GroupBy(s => s.StaID);
                    foreach (var s in nolist)
                    {
                        string pDicNmae = db.OT_Station.Single(m => m.Num == s.Key.ToString() && m.IsDelete == 0).Name;
                        List<byte?> timePeriods = pNoaccept.Where(n => n.StaID == s.Key).Select(n => n.HourPer).OrderBy(n => n.Value).ToList();
                        ReportRelatedViewModels pData = new ReportRelatedViewModels();
                        string value = string.Empty;
                        for (int i = 0; i < timePeriods.Count; i++)
                        {
                            value += timePeriods[i].Value.ToString() + "时、";
                        }
                        pData.StationName = pDicNmae;
                        pData.Time = value.Remove(value.Length - 1);
                        pList.Add(pData);
                    }
                    return pList;
                }
            }
            catch (Exception e)
            {
                SystemLog.GetInstance().Error(e.ToString());
                return null;
            }
        }
        #endregion

        #region 12 Protected Methods
        /// <summary>
        /// 获取单元格样式
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <param name="type">1表示有边框，0表示没有边框</param>
        /// <returns></returns>
        protected ICellStyle GetCellStyle(IWorkbook hssfworkbook, int type)
        {
            ICellStyle cellstyle = hssfworkbook.CreateCellStyle();

            cellstyle.Alignment = HorizontalAlignment.Center;
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            IFont fontLeft = hssfworkbook.CreateFont();
            //字号
            fontLeft.FontHeightInPoints = 16;
            //字体
            fontLeft.FontName = "宋体";
            cellstyle.SetFont(fontLeft);
            if (type == 1)
            {
                //有边框
                cellstyle.BorderBottom = BorderStyle.Thin;
                cellstyle.BorderLeft = BorderStyle.Thin;
                cellstyle.BorderRight = BorderStyle.Thin;
                cellstyle.BorderTop = BorderStyle.Thin;
            }
            else if (type == 0)
            {
                cellstyle.BorderBottom = BorderStyle.None;
                cellstyle.BorderLeft = BorderStyle.None;
                cellstyle.BorderRight = BorderStyle.None;
                cellstyle.BorderTop = BorderStyle.None;
            }
            return cellstyle;
        }
        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sheet"> 要合并单元格所在的sheet </param>
        /// <param name="rowstart"> 开始行的索引 </param>
        /// <param name="rowend"> 结束行的索引 </param>
        /// <param name="colstart"> 开始列的索引 </param>
        /// <param name="colend"> 结束列的索引 </param>
        protected void SetCellRangeAddress(ISheet sheet, int rowstart, int rowend, int colstart, int colend)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);
            sheet.AddMergedRegion(cellRangeAddress);
            // sheet.SetEnclosedBorderOfRegion(cellRangeAddress, NPOI.SS.UserModel.BorderStyle.THIN, NPOI.HSSF.Util.HSSFColor.BLACK.index);
        }

        /// <summary>
        /// 给单元格赋值
        /// </summary>
        /// <param name="sheet">工作簿</param>
        /// <param name="rownum">行数</param>
        /// <param name="cellnum">列数</param>
        /// <param name="value">值</param>
        protected void SetValue(ISheet sheet, int rownum, int cellnum, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                sheet.GetRow(rownum).GetCell(cellnum).SetCellValue(value);
            }
        }
        /// <summary>
        /// 给单元格赋值
        /// </summary>
        /// <param name="sheet">工作簿</param>
        /// <param name="rownum">行数</param>
        /// <param name="cellnum">列数</param>
        /// <param name="value">值</param>
        protected void SetValue(ISheet sheet, int rownum, int cellnum, double value)
        {
            sheet.GetRow(rownum).GetCell(cellnum).SetCellValue(value);
        }
        /// <summary>
        /// 添加报表日期
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="rownum">行数</param>
        /// <param name="cellnum">列数</param>
        /// <param name="dt">日期</param>
        /// <param name="type">报表类型</param>
        protected void SetReportDate(ISheet sheet, int rownum, int cellnum, DateTime dt, int? reportype)
        {
            if (reportype != null)
            {
                string value = string.Empty;
                if (reportype == 1 || reportype == 3 || reportype == 5 || reportype == 8)
                {
                    value = dt.ToString("yyyy.M.d");
                    if (reportype != 8)
                    {
                        value = "数据日期：" + value;
                    }
                    else
                    {
                        value = "时间：" + value;
                    }
                }
                else if (reportype != 18)
                {
                    value = dt.ToString("yyyy年M月d日");
                    value = "数据日期：" + value;
                }
                else
                {
                    value = dt.ToString("yyyy年M月d日");
                }
                SetValue(sheet, rownum, cellnum, value);
            }
        }
        /// <summary>
        /// 添加报表备注
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="rownum">行数</param>
        /// <param name="cellnum">列数</param>
        /// <param name="value">值</param>
        protected void SetReportRemark(ISheet sheet, int rownum, int cellnum, int reporttype)
        {
            OT_HDayConfig holiday = HolidayConfig.GetInstance().GetById(reporttype);
            if (holiday != null)
                SetValue(sheet, rownum, cellnum, holiday.RptRemark);
        }
        #endregion
    }
}

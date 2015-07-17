/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/16 14:21:52
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Wttech.DataSubmitted.Common.ViewModels
{
    /// <summary>
    /// 报表13,14实体类
    /// </summary>
    public class HourAADTViewModel
    {
        #region 3 Fields
        #endregion

        #region 4 Properties
        /// <summary>
        /// 高速公路名称
        /// </summary>
        public string RoadName
        {
            get
            {
                return Common.Resources.SystemConst.RoadName3;
            }
            set { ;}
        }
        /// <summary>
        /// 收费站名称
        /// </summary>
        public string StaName { get; set; }
        /// <summary>
        /// 交通量名称
        /// </summary>
        public string TraName { get; set; }
        /// <summary>
        /// 各时间段数据
        /// </summary>
        public double? Count_0 { get; set; }
        public double? Count_1 { get; set; }
        public double? Count_2 { get; set; }
        public double? Count_3 { get; set; }
        public double? Count_4 { get; set; }
        public double? Count_5 { get; set; }
        public double? Count_6 { get; set; }
        public double? Count_7 { get; set; }
        public double? Count_8 { get; set; }
        public double? Count_9 { get; set; }
        public double? Count_10 { get; set; }
        public double? Count_11 { get; set; }
        public double? Count_12 { get; set; }
        public double? Count_13 { get; set; }
        public double? Count_14 { get; set; }
        public double? Count_15 { get; set; }
        public double? Count_16 { get; set; }
        public double? Count_17 { get; set; }
        public double? Count_18 { get; set; }
        public double? Count_19 { get; set; }
        public double? Count_20 { get; set; }
        public double? Count_21 { get; set; }
        public double? Count_22 { get; set; }
        public double? Count_23 { get; set; }
        /// <summary>
        /// 合计
        /// </summary>
        public double? Count_24 { get; set; }
        /// <summary>
        /// 排序字段1：站名
        /// </summary>
        public int? StaSort
        {
            get
            {
                if (StaName == "大羊坊站")
                    return 1;
                else if (StaName == "马驹桥东站")
                    return 2;
                else if (StaName == "马驹桥西站")
                    return 3;
                else if (StaName == "采育站")
                    return 4;
                else
                    return null;
            }
            set { ;}
        }
        /// <summary>
        /// 排序字段2：小时交通量
        /// </summary>
        public int? HourSort
        {
            get
            {
                if (TraName == "出京入" || TraName == "进出京合计")
                    return 1;
                else if (TraName == "出京出" || TraName == "进京")
                    return 2;
                else if (TraName == "进京入" || TraName == "出京")
                    return 3;
                else if (TraName == "进京出")
                    return 4;
                else
                    return null;
            }
            set { ;}
        }
        /// <summary>
        /// 是否可编辑
        /// </summary>
        public int? IsEdit
        {
            get
            {
                if (StaName == "大羊坊站" && (TraName == "出京入" || TraName == "进京出"))
                    return 1;
                else if (StaName == "马驹桥东站" && (TraName == "进京入" || TraName == "进京出"))
                    return 1;
                else if (StaName == "马驹桥西站" && (TraName == "进京出" || TraName == "出京入"))
                    return 1;
                else if (StaName == "采育站" && (TraName == "进京出" || TraName == "出京入"))
                    return 1;
                else
                    return 0;
            }
            set { ;}
        }
        #endregion
    }
    /// <summary>
    /// 报表13修改实体
    /// </summary>
    public class UpdateHourAADTInfo
    {
        #region 4 Properties
        /// <summary>
        /// 收费站名称
        /// </summary>
        public string StaName { get; set; }
        /// <summary>
        /// 交通量名称
        /// </summary>
        public string TraName { get; set; }
        /// <summary>
        /// 各时间段数据
        /// </summary>
        public double? Count_0 { get; set; }
        public double? Count_1 { get; set; }
        public double? Count_2 { get; set; }
        public double? Count_3 { get; set; }
        public double? Count_4 { get; set; }
        public double? Count_5 { get; set; }
        public double? Count_6 { get; set; }
        public double? Count_7 { get; set; }
        public double? Count_8 { get; set; }
        public double? Count_9 { get; set; }
        public double? Count_10 { get; set; }
        public double? Count_11 { get; set; }
        public double? Count_12 { get; set; }
        public double? Count_13 { get; set; }
        public double? Count_14 { get; set; }
        public double? Count_15 { get; set; }
        public double? Count_16 { get; set; }
        public double? Count_17 { get; set; }
        public double? Count_18 { get; set; }
        public double? Count_19 { get; set; }
        public double? Count_20 { get; set; }
        public double? Count_21 { get; set; }
        public double? Count_22 { get; set; }
        public double? Count_23 { get; set; }
        #endregion
    }

    /// <summary>
    /// 报表13修改视图模型
    /// </summary>
    public class UpdateHourAADTViewModel
    {
        /// <summary>
        /// 数据日期
        /// </summary>
        public DateTime DataDate { get; set; }

        /// <summary>
        /// 修改数据
        /// </summary>
        public List<UpdateHourAADTInfo> UpdateData { get; set; }
    }
    /// <summary>
    /// 报表13,14查询返回实体
    /// </summary>
    public class QueryHourAADTViewModel : IReportViewModel
    {
        #region 4 Properties
        /// <summary>
        /// 报表数据
        /// </summary>
        public List<HourAADTViewModel> ReportData;
        /// <summary>
        /// 数据是否完整
        /// </summary>
        public byte IsFull;
        #endregion
    }
}

/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/9 17:21:36
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
    /// 报表18数据实体类
    /// </summary>
    public class HDayAADTViewModel
    {
        #region 4 Properties
        /// <summary>
        /// 路线类型
        /// </summary>
        public int LineType { get; set; }
        /// <summary>
        /// 排序编号
        /// </summary>
        public int Sorting
        {
            get
            {
                if (LineType == 5)
                    return 1;
                if (LineType == 4)
                    return 2;
                if (LineType == 1)
                    return 3;
                if (LineType == 2)
                    return 4;
                if (LineType == 3)
                    return 5;
                if (LineType == 6)
                    return 6;
                else
                    return 0;
            }
            set { ;}
        }
        /// <summary>
        /// 序号
        /// </summary>
        public string Num
        {
            get
            {
                if (LineType == 4)
                    return "2";
                if (LineType == 5)
                    return "1";
                if (LineType == 0)
                    return "二";
                else
                    return "";
            }
            set { ;}
        }
        /// <summary>
        /// 所在路线编号
        /// </summary>
        public string LineNum
        {
            get
            {
                if (LineType == 0)
                    return "G2";
                else
                    return "";
            }
            set { ;}
        }
        /// <summary>
        /// 所在路线名称
        /// </summary>
        public string LineName
        {
            get
            {
                if (LineType == 4 || LineType == 5)
                    return "收费站";
                if (LineType == 1)
                    return "交调观测点1";
                if (LineType == 2)
                    return "交调观测点2";
                if (LineType == 3)
                    return "交调观测点3";
                if (LineType == 0)
                    return "京沪高速交通量合计";
                else
                    return "...";
            }
            set { ;}
        }
        /// <summary>
        /// 收费站及观测点简称
        /// </summary>
        public string StaName
        {
            get
            {
                if (LineType == 5)
                    return "大羊坊";
                if (LineType == 1 || LineType == 4)
                    return "马驹桥";
                if (LineType == 2)
                    return "采育";
                if (LineType == 3)
                    return "市界";
                else
                    return "";
            }
            set { ;}
        }
        /// <summary>
        /// 收费站及观测点桩号
        /// </summary>
        public string StaNum
        {
            get
            {
                if (LineType == 4)
                    return "12.800";
                if (LineType == 1)
                    return "14.000";
                if (LineType == 2)
                    return "26.500";
                if (LineType == 3)
                    return "35.000";
                if (LineType == 5)
                    return "5.000";
                else
                    return "";
            }
            set { ;}
        }
        /// <summary>
        /// 收费站位置类型
        /// </summary>
        public string StaType
        {
            get
            {
                if (LineType == 4)
                    return "匝道站";
                if (LineType == 5)
                    return "主线站";
                if (LineType == 1 || LineType == 2 || LineType == 3)
                    return "断面线圈";
                else
                    return "";
            }
            set { ;}
        }
        /// <summary>
        /// 车道
        /// </summary>
        public string LaneNum
        {
            get
            {
                if (LineType == 4)
                    return "19";
                if (LineType == 5)
                    return "24";
                if (LineType == 1 || LineType == 2 || LineType == 3)
                    return "4";
                else
                    return "";
            }
            set { ;}
        }
        /// <summary>
        /// 设计交通量
        /// </summary>
        public double? DeTra
        {
            get
            {
                if (LineType == 4)
                    return 35000;
                if (LineType == 5)
                    return 60000;
                if (LineType == 1 || LineType == 2 || LineType == 3 || LineType == 6)
                    return 50000;
                else
                    return null;
            }
            set { ;}
        }
        /// <summary>
        /// 自然交通辆合计
        /// </summary>
        public double? NatSum { get; set; }
        /// <summary>
        /// 出京自然交通辆
        /// </summary>
        public double? ExNat { get; set; }
        /// <summary>
        /// 进京自然交通辆
        /// </summary>
        public double? EnNat { get; set; }
        /// <summary>
        /// 出京当量交通辆（合计）
        /// </summary>
        public double? EquSum { get; set; }
        /// <summary>
        /// 出京当量交通辆
        /// </summary>
        public double? ExEqu { get; set; }
        /// <summary>
        /// 进京当量交通辆
        /// </summary>
        public double? EnEqu { get; set; }
        /// <summary>
        /// 拥挤度
        /// </summary>
        public double? CrowDeg { get; set; }
        /// <summary>
        /// 小型车合计
        /// </summary>
        public double? SmaSum { get; set; }
        /// <summary>
        /// 小型车出京
        /// </summary>
        public double? SmaEx { get; set; }
        /// <summary>
        /// 小型车进京
        /// </summary>
        public double? SmaEn { get; set; }
        /// <summary>
        /// 中型车合计
        /// </summary>
        public double? MedSum { get; set; }
        /// <summary>
        /// 中型车出京
        /// </summary>
        public double? MedEx { get; set; }
        /// <summary>
        /// 中型车进京
        /// </summary>
        public double? MedEn { get; set; }
        /// <summary>
        /// 大型车合计
        /// </summary>
        public double? LarSum { get; set; }
        /// <summary>
        /// 大型车出京
        /// </summary>
        public double? LarEx { get; set; }
        /// <summary>
        /// 大型车进京
        /// </summary>
        public double? LarEn { get; set; }
        /// <summary>
        /// 重型车合计
        /// </summary>
        public double? HeaSum { get; set; }
        /// <summary>
        /// 重型车出京
        /// </summary>
        public double? HeaEx { get; set; }
        /// <summary>
        /// 重型车进京
        /// </summary>
        public double? HeaEn { get; set; }
        /// <summary>
        /// 超大型车合计
        /// </summary>
        public double? SupSum { get; set; }
        /// <summary>
        /// 超大型车出京
        /// </summary>
        public double? SupEx { get; set; }
        /// <summary>
        /// 超大型车进京
        /// </summary>
        public double? SupEn { get; set; }
        /// <summary>
        /// 进出京货车数量
        /// </summary>
        public double? EnExTrukNum { get; set; }
        /// <summary>
        /// 客车货车比例
        /// </summary>
        public double? CarTrukPer { get; set; }
        /// <summary>
        /// 进出京大货车以上车型的数量
        /// </summary>
        public double? SupTruNum { get; set; }
        /// <summary>
        /// 大货车以上占货车交通量比例
        /// </summary>
        public double? SupTruPer { get; set; }
        #endregion
    }
    /// <summary>
    /// 报表18查询条件实体
    /// </summary>
    public class HDayAADTViewModelParaViewModel
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 节假日编号
        /// </summary>
        public int HolidayId { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
    /// <summary>
    /// 查询返回实体
    /// </summary>
    public class QueryHDayAADTViewModel : IReportViewModel
    {
        /// <summary>
        /// 是否可编辑1:可编辑0：不可编辑
        /// </summary>
        public byte IsEdit;
        /// <summary>
        /// 实体集合
        /// </summary>
        public List<HDayAADTViewModel> ReportData;
        /// <summary>
        /// 数据是否完整
        /// </summary>
        public byte IsFull;
    }

    /// <summary>
    /// 修改实体
    /// </summary>
    public class UpdateHDayRoadStaViewModel
    {
        /// <summary>
        /// 路线类型
        /// </summary>
        public int LineType { get; set; }

        /// <summary>
        /// 出京自然交通辆
        /// </summary>
        public double? ExNat { get; set; }

        /// <summary>
        /// 进京自然交通辆
        /// </summary>
        public double? EnNat { get; set; }
      
        /// <summary>
        /// 出京当量交通辆
        /// </summary>
        public double? ExEqu { get; set; }

        /// <summary>
        /// 进京当量交通辆
        /// </summary>
        public double? EnEqu { get; set; }

        /// <summary>
        /// 拥挤度
        /// </summary>
        public double? CrowDeg { get; set; }
     
        /// <summary>
        /// 小型车出京
        /// </summary>
        public double? SmaEx { get; set; }

        /// <summary>
        /// 小型车进京
        /// </summary>
        public double? SmaEn { get; set; }
     
        /// <summary>
        /// 中型车出京
        /// </summary>
        public double? MedEx { get; set; }

        /// <summary>
        /// 中型车进京
        /// </summary>
        public double? MedEn { get; set; }
     
        /// <summary>
        /// 大型车出京
        /// </summary>
        public double? LarEx { get; set; }

        /// <summary>
        /// 大型车进京
        /// </summary>
        public double? LarEn { get; set; }
     
        /// <summary>
        /// 重型车出京
        /// </summary>
        public double? HeaEx { get; set; }

        /// <summary>
        /// 重型车进京
        /// </summary>
        public double? HeaEn { get; set; }
    
        /// <summary>
        /// 超大型车出京
        /// </summary>
        public double? SupEx { get; set; }

        /// <summary>
        /// 超大型车进京
        /// </summary>
        public double? SupEn { get; set; }

        /// <summary>
        /// 进出京货车数量
        /// </summary>
        public double? EnExTrukNum { get; set; }

        /// <summary>
        /// 客车货车比例
        /// </summary>
        public double? CarTrukPer { get; set; }

        /// <summary>
        /// 进出京大货车以上车型的数量
        /// </summary>
        public double? SupTruNum { get; set; }

        /// <summary>
        /// 大货车以上占货车交通量比例
        /// </summary>
        public double? SupTruPer { get; set; }
    }

    /// <summary>
    /// 报表18，接收参数实体
    /// </summary>
    public class UHDayRoadStaViewModel
    {
        /// <summary>
        /// 数据日期
        /// </summary>
        public DateTime DataDate { get; set; }

        /// <summary>
        /// 报表18修改数据实体
        /// </summary>
       public  List<UpdateHDayRoadStaViewModel> DataInfo { get; set; }

        /// <summary>
        /// 报表类型
        /// </summary>
        public int ReportType { get; set; }
    }
}

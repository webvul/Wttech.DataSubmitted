using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wttech.DataSubmitted.Common.ViewModels
{


    public class NaturalInfoViewModel
    {
        public string Num
        {
            get
            {
                if (HourPer != 24)
                {
                    return (HourPer+1).ToString();
                }
                else
                {
                    return Resources.SystemConst.ReportCount;
                }
            }
            set { ;}
        }


        public string TimeScope
        {
            get
            {
                if (HourPer != 24)
                {
                    return String.Format("{0:00}-{1:00}", HourPer, HourPer+1);
                }
                else
                {

                    return "";
                }
            }
            set { ;}
        }
        public string RoadName { get { return Resources.SystemConst.RoadName; } set { ; } }
        public byte? HourPer { get; set; }
        public double? DayTraffic { get; set; }
        public double? InDayTraffic { get; set; }
        public double? OutDayTraffic { get; set; }
        public string RunningStatus { get; set; }
        public string Remark { get; set; }
    }


    /// <summary>
    /// 每日报送报表修改视图模型
    /// </summary>
    public class UpdateNaturalInfoViewModel
    {

        /// <summary>
        /// 站类型
        /// </summary>
        public int StationType { get; set; }
        /// <summary>
        /// 数据日期
        /// </summary>
        public DateTime DataDate { get; set; }

        /// <summary>
        /// 修改数据
        /// </summary>
        public List<UpdateNaturalInfo> UpdateData { get; set; }

    }

    public class UpdateNaturalInfo
    {
        /// <summary>
        /// 时间段
        /// </summary>
        public byte Num { get; set; }
        /// <summary>
        /// 进京方向日交通量
        /// </summary>
        public double InDayTraffic { get; set; }
        /// <summary>
        /// 出京方向日交通量
        /// </summary>
        public double OutDayTraffic { get; set; }
        /// <summary>
        /// 运行状况
        /// </summary>
        public string RunningStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }  
    }


    /// <summary>
    /// 查询返回数据项
    /// </summary>
    public class QueryNaturalInfoViewModel : IReportViewModel
    {
        public List<NaturalInfoViewModel> ReportData;
        public string ReportRemark;
        public byte IsFull;
    }
}

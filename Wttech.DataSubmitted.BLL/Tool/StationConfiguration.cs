/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/10 13:06:05
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.DAL;

#endregion

namespace Wttech.DataSubmitted.BLL.Tool
{
    /// <summary>
    /// 收费站配置
    /// </summary>
    public class StationConfiguration
    {
        private static List<int> outBJStaion = new List<int> { 15, 20, 25 };
        private static List<int> allBJStaion = new List<int> { 15, 20, 25, 120 };
        /// <summary>
        /// 货车车型集合
        /// </summary>
        /// <returns></returns>
        public static int[] GetTruks()
        {
            return new int[] { 11, 12, 13, 14, 15 };
        }
        /// <summary>
        /// 大货车以上车型集合
        /// </summary>
        /// <returns></returns>
        public static int[] GetOverTruks()
        {
            return new int[] { 3, 4, 13, 14, 15 };
        }
        /// <summary>
        /// 获取天津段的收费站
        /// </summary>
        /// <returns></returns>
        public static List<int> GetTJStaion()
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //3表示天津段
                List<string> stationliststr = db.OT_Station.Where(s => s.District == 3 && s.IsDelete == 0).Select(s => s.Num).ToList();
                List<int> stationlist = new List<int>();
                for (int i = 0; i < stationliststr.Count; i++)
                {
                    stationlist.Add(int.Parse(stationliststr[i]));
                }
                return stationlist;
            }
        }
        /// <summary>
        /// 获取报表2的收费站
        /// </summary>
        /// <returns></returns>
        public static List<int> GetStaionList2()
        {
            return new List<int> { 33, 35, 40, 45, 50, 52, 55, 60, 140, 155 };
        }
        /// <summary>
        /// 获取报表11,12的收费站
        /// </summary>
        /// <returns></returns>
        public static List<int> GetStaionList()
        {
            return new List<int> { 35, 40, 45, 50, 52, 55, 60, 140, 155 };
        }
        /// <summary>
        /// 获取北京段的收费站
        /// </summary>
        /// <returns></returns>
        public static List<int> GetBJStaion()
        {
            return allBJStaion;
        }
        /// <summary>
        /// 获取观测点1的出京收费站
        /// </summary>
        /// <returns></returns>
        public static List<int> GetExObs1()
        {
            return new List<int> { 15, 20 };
        }
        /// <summary>
        /// 获取观测点1的进京收费站
        /// </summary>
        /// <returns></returns>
        public static List<int> GetEnObs1()
        {
            return new List<int> { 15, 120 };
        }
        /// <summary>
        ///  获取观测点2的出京收费站
        /// </summary>
        /// <returns></returns>
        public static List<int> GetExObs2()
        {
            return new List<int> { 15, 20, 25 };
        }
        /// <summary>
        /// 获取观测点2的进京收费站
        /// </summary>
        /// <returns></returns>
        public static List<int> GetEnObs2()
        {
            return new List<int> { 15, 20, 120 };
        }
        /// <summary>
        /// 获取观测点3的出京收费站
        /// </summary>
        /// <returns></returns>
        public static List<int> GetExObs3()
        {
            return new List<int> { 15, 20, 25 };
        }
        /// <summary>
        /// 获取观测点3的进京收费站
        /// </summary>
        /// <returns></returns>
        public static List<int> GetEnObs3()
        {
            return allBJStaion;
        }
        /// <summary>
        /// 获取北京出京的收费站
        /// </summary>
        /// <returns></returns>
        public static List<int> GetOutBJStaion()
        {
            return new List<int> { 15, 20, 25 };
        }
        /// <summary>
        /// 收费站
        /// </summary>
        public enum StationID
        {
            DYF = 15,
            MJQ = 20,
            CY = 25,
            SCD = 33,
            YC = 35,
            YXBD = 40,
            JZL = 45,
            JC = 50,
            KG = 52,
            TGX = 55,
            TG = 60,
            MJQD = 120,
            YXBX = 140,
            TGXF = 155

        }
        /// <summary>
        /// 统计站类型
        /// </summary>
        public enum StationType
        {
            /// <summary>
            /// 北京段的StationType为1，大羊坊的stationtype为大羊坊站id
            /// </summary>
            BeiJingDuan = 1,
            /// <summary>
            /// 河北段
            /// </summary>
            HeBei = 2,
            /// <summary>
            /// 天津段
            /// </summary>
            TianJinDuan = 3

        }
    }
}

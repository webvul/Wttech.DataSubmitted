/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个操作类型类文件
* 创建标识：ta0351王旋20141121
*/

using System.Collections.Generic;
namespace Wttech.DataSubmitted.Common
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public class OperationType
    {
        #region 2 Static Fields

        /// <summary>
        /// 所有
        /// </summary>
        public static string All = Resources.SystemConst.All;
        /// <summary>
        /// 登录
        /// </summary>
        public static string Login = Resources.SystemConst.Login;
        /// <summary>
        /// 校正
        /// </summary>
        public static string Calibration = Resources.SystemConst.Calibration;
        /// <summary>
        /// 导出
        /// </summary>
        public static string Export = Resources.SystemConst.Export;
        /// <summary>
        /// 预测
        /// </summary>
        public static string Forecast = Resources.SystemConst.Forecast;
        /// <summary>
        /// 退出
        /// </summary>
        public static string LoginOut = Resources.SystemConst.LoginOut;
        /*
        /// <summary>
        /// 查询
        /// </summary>
        public static string Query = Resources.SystemConst.Query;*/
        /// <summary>
        /// 修改
        /// </summary>
        public static string Update = Resources.SystemConst.Update;

        /// <summary>
        /// 操作集合
        /// </summary>
        public static List<string> OperationTypeList = new List<string> { 
        All,
        Login,
        Update,
        Export,
        Forecast,
        Calibration,
        LoginOut
        };

        #endregion
    }
}

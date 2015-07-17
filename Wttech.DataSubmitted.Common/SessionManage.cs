/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：Session操作类
 * 创建标识：ta0351 王旋2014/11/20 15:38:10
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using Wttech.DataSubmitted.Common.ViewModels;

namespace Wttech.DataSubmitted.Common
{
    /// <summary>
    /// Session操作类
    /// </summary>
    public class SessionManage
    {
        #region 10 Static Methods

        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        /// <returns></returns>
        public static UserInfoViewModel GetLoginUser()
        {

            if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserInfo"] != null)
            {
                return (UserInfoViewModel)HttpContext.Current.Session["UserInfo"];
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 通过key获取Session
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetSession(string key)
        {
            if (HttpContext.Current.Session != null)
            {
                return HttpContext.Current.Session[key];
            }
            return null;
        }

        /// <summary>
        /// 增加设置对应key的session值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        public static void SetSession(string key, object args)
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[key] = args;
            }
        }
        #endregion
    }
}

/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/26 13:30:28
 */

#region 引用
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common;

#endregion

namespace Wttech.DataSubmitted.Services
{
    /// <summary>
    /// IoC初始化类文件
    /// </summary>
    class Bootstrapper
    {
        #region 10 Static Methods

        /// <summary>
        /// 进行IoC的初始化
        /// </summary>
        public static void Run()
        {
            try
            {
                //创建容器  
                IUnityContainer container = new UnityContainer();
                UnityConfigurationSection configuration = ConfigurationManager.GetSection(UnityConfigurationSection.SectionName)
                as UnityConfigurationSection;
                configuration.Configure(container, "serviceContainer");
                IBLL.ServicesFactory.Instance = container.Resolve(typeof(IBLL.ServicesFactory)) as IBLL.ServicesFactory;
            }
            catch (Exception ex)
            {
                SystemLog.GetInstance().Log.Error(ex.Message);
            }

        }

        #endregion
    }
}

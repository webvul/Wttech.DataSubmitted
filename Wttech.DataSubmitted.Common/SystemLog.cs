/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/6 15:32:38
 */

#region 引用
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Wttech.DataSubmitted.Common
{    
    public class SystemLog
    {
        private static SystemLog systemlog = null;
        private ILog log;

        public static SystemLog GetInstance()
        {
            if (systemlog == null)
                systemlog = new SystemLog();
            return systemlog;
        }
        public SystemLog()
        {
            log = log4net.LogManager.GetLogger("DataSubmittedLogger");
        }
        public ILog Log
        {
            get { return log; }
        }
        public void Info(object message)
        {
            Log.Info(message);
        }
        public void Info(object message,Exception e)
        {
            Log.Info(message,e);
        }
        public void Error(object message)
        {
            Log.Error(message);
        }
        public void Error(object message, Exception e)
        {
            Log.Error(message, e);
        }
        public void Debug(object message)
        {
            Log.Debug(message);
        }
        public void Debug(object message, Exception e)
        {
            Log.Debug(message, e);
        }
    }
}

/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/6 14:09:09
 */
#region 引用
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.DAL;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.Unity;
using Wttech.DataSubmitted.IBLL.IReport;
using Wttech.DataSubmitted.BLL.Report;
using Wttech.DataSubmitted.IBLL;
#endregion
namespace Wttech.DataSubmitted.Services
{
    /// <summary>
    /// 数据服务类
    /// </summary>
    public class DataServicesCore
    {
        #region 1 Const
        /// <summary>
        /// 定义时间间隔为2分钟
        /// </summary>
        private const double INTERVALTIME = 2 * 60 * 1000;
        #endregion

        #region 2 Static Fields
        /// <summary>
        /// 数据服务对象
        /// </summary>
        private static DataServicesCore dataservicescore;
        #endregion

        #region 3 Fields
        private DataService dataService;
        /// <summary>
        /// 定时器
        /// </summary>
        private readonly Timer timer;
        /// <summary>
        /// 自定义配置文件信息
        /// </summary>
        private NameValueCollection config;
        #endregion

        #region 5 Constructors
        /// <summary>
        /// 
        /// </summary>
        public DataServicesCore()
        {
            config = (NameValueCollection)ConfigurationManager.GetSection("IpGroup/ServerIp");
            timer = new Timer();
        }
        #endregion
        #region 8 Event Methods
        /// <summary>
        /// 定时执行方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int pHourPer = DateTime.Now.Hour;
            int pTimePeriodMinute = DateTime.Now.Minute;
            if (pTimePeriodMinute == 2 || pTimePeriodMinute == 3)
            {
                try
                {
                    DateTime pDt = DateTime.Today.AddHours(pHourPer);
                    ServicesFactory.Instance.stagingReport.UpdateData(pDt, pHourPer);
                }
                catch (Exception ex)
                {
                    SystemLog.GetInstance().Log.Info(string.Format(ex.Message));
                }
            }
        }
        #endregion
        #region 9 Public Methods
        public void Start(DataService da)
        {
            dataService = da;
            try
            {
                if (RemoteConnection())
                {
                    SystemLog.GetInstance().Log.Info(string.Format("远程服务器连接成功:{0} ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    this.timer.AutoReset = true;
                    this.timer.Enabled = true;
                    this.timer.Elapsed += new ElapsedEventHandler(this.timer_Elapsed);
                    this.timer.Interval = INTERVALTIME;
                    SystemLog.GetInstance().Log.Info(string.Format("服务启动成功,启动时间：{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    SystemLog.GetInstance().Log.Info(string.Format("定时器启动成功,启动时间：{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                }
                else
                {
                    SystemLog.GetInstance().Log.Info(string.Format("{0}：服务启动失败,{1} ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "远程服务器连接失败"));
                }
            }
            catch (Exception e)
            {
                SystemLog.GetInstance().Log.Error(string.Format("远程服务器连接失败:{0} ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), e);
                dataService.Stop();
            }
        }
        public void Stop()
        {
            try
            {
                this.timer.Elapsed -= new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
                this.timer.Enabled = false;
                SystemLog.GetInstance().Log.Info(string.Format("服务已停止:{0} ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + Environment.NewLine);

            }
            catch (Exception e)
            {
                SystemLog.GetInstance().Log.Error(string.Format("{0} 服务异常:{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), e);
            }
        }
        #endregion
        #region 10 Static Methods
        /// <summary>
        /// 获取服务类实例
        /// </summary>
        /// <returns></returns>
        public static DataServicesCore GetInstance()
        {
            if (dataservicescore == null)
                dataservicescore = new DataServicesCore();
            return dataservicescore;
        }
        #endregion

        #region 11 Private Methods
        /// <summary>
        /// 连接远程服务器  注：此方式创建经过测试发现目前无效，目前采用运维人员执行脚本，创建远程连接
        /// </summary>
        /// <returns></returns>
        public bool RemoteConnection()
        {
            bool pIsTrue = false;
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //db.Database.ExecuteSqlCommand("	exec   sp_addlinkedserver     'srv_lnk', '', 'SQLOLEDB', @ServerIp",new SqlParameter("@ServerIp","172.16.2.45"));
                //db.Database.ExecuteSqlCommand("exec   sp_addlinkedsrvlogin   'srv_lnk', 'false',null, @UserName,@PassWord",new SqlParameter[]{new SqlParameter("@UserName","sa"),new SqlParameter("@PassWord","tayh@2013")});
                db.SP_LinkServer(config["IP"], config["UserName"], config["PassWord"]);
                pIsTrue = true;
            }
            return pIsTrue;
        }
        #endregion
    }
}

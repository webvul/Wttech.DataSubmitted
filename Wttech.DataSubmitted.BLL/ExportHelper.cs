/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/11/12 14:35:23
 */

#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Web;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.DAL;
using System.Reflection;
using Wttech.DataSubmitted.IBLL.Common;
using Wttech.DataSubmitted.Common.ViewModels;
using System.Collections.Specialized;
using System.Configuration;
#endregion

namespace Wttech.DataSubmitted.BLL
{
    /// <summary>
    /// 数据报表导出辅助类
    /// </summary>
    public class ExportHelper
    {

        #region 2 Static Fields
        public static ExportHelper exportHelper = null;
        #endregion
        /// <summary>
        /// 保存文件配置变量
        /// </summary>
        private String config;

        #region

        public ExportHelper()
        {
            config = Common.Utility.GetDownLoadPath();
        }
        #endregion

        #region 9 Public Methods
        /// <summary>
        /// 正常导出
        /// </summary>
        /// <param name="path"></param>
        /// <param name="para"></param>
        /// <param name="updatesheet">数据表类对象</param>
        /// <returns></returns>
        public string ExportExcel(string path, QueryParameters para, IGenerateSheet updatesheet)
        {
            string[] num = path.Split('\\');
            string filename = num[num.Length - 1];
            string savepath = string.Empty;
            try
            {
                if (System.IO.File.Exists(path))
                {
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook readworkbook = new XSSFWorkbook(stream);
                        stream.Close();
                        readworkbook = updatesheet.GenerateSheet(readworkbook, para);
                        savepath = SaveFile(readworkbook, filename, para);
                    }
                }
                else
                {
                    SystemLog.GetInstance().Info("导出错误：模板信息不存在！");
                }
            }
            catch (Exception e)
            {
                SystemLog.GetInstance().Error(e);
            }
            return savepath;
        }

        /// <summary>
        /// 预测导出--如果需要将入出口数据进行区分，则分别放在两个list的集合中，若不需区分，则将数据放入list1中，list2为空即可
        /// </summary>
        /// <param name="path"></param>
        /// <param name="para"></param>
        /// <param name="forecase"></param>
        /// <param name="list1">出口</param>
        /// <param name="list2">入口</param>
        /// <returns></returns>
        public string ExportExcel(string path, QueryParameters para, IForecast forecase, List<IReportViewModel> list1, List<IReportViewModel> list2)
        {
            string[] num = path.Split('\\');
            string filename = num[num.Length - 1].Split('.')[0] + "(预测).xlsx";
            string savepath = string.Empty;
            try
            {

                if (System.IO.File.Exists(path))
                {
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {

                        IWorkbook readworkbook = new XSSFWorkbook(stream);
                        stream.Close();
                        readworkbook = forecase.GenerateSheet(readworkbook, para, list1, list2);
                        savepath = SaveFile(readworkbook, filename, para);
                    }
                }
                else
                {
                    SystemLog.GetInstance().Info("导出错误：模板信息不存在！");
                }
            }
            catch (Exception e)
            {
                SystemLog.GetInstance().Error(e);
            }
            return savepath;
        }
        #endregion

        #region 10 Static Methods
        /// <summary>
        /// 获取ExporHelper实例
        /// </summary>
        /// <returns></returns>
        public static ExportHelper GetInstance()
        {
            if (exportHelper == null)
                exportHelper = new ExportHelper();
            return exportHelper;
        }
        #endregion

        #region 11 Private Methods
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <param name="filename"></param>
        /// <returns>返回文件保存路径</returns>
        private string SaveFile(IWorkbook hssfworkbook, string filename, QueryParameters para)
        {
            try
            {
                string pPath = config;
                if (pPath.Substring(pPath.Length - 1) != "\\")
                    pPath = pPath + "\\";
                var tmpPath = pPath + DateTime.Now.ToString("yyyyMMdd") + "\\" + para.ReportType;
                //创建存储目录
                if (!Directory.Exists(tmpPath))
                {
                    Directory.CreateDirectory(tmpPath);
                }
                //创建文件
                string path = tmpPath + "\\" + filename;
                FileStream files = new FileStream(path, FileMode.Create);
                hssfworkbook.Write(files);
                files.Close();
                //添加数据导出记录
                ContactInfo pContactinfo = new ContactInfo();
                OT_ExportHis contactinfo = new OT_ExportHis();
                contactinfo.Id = Guid.NewGuid();
                contactinfo.DataDate = para.StartTime;
                contactinfo.HDayId = para.HolidayId;
                contactinfo.CalcuTime = DateTime.Now;
                contactinfo.SavePath = path;
                contactinfo.TableType = para.ReportType;
                pContactinfo.Create(contactinfo);

                return filename;
            }
            catch (Exception e)
            {
                SystemLog.GetInstance().Error(e);
            }
            return null;

        }
        #endregion

    }
}

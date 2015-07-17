using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wttech.DataSubmitted.Common;

namespace Wttech.DataSubmitted.Web.Controllers
{
    /// <summary>
    /// 下载文件
    /// </summary>
    public class DownFileController : Controller
    {
        //
        // GET: /DownFile/
        public void Downfile()
        {
            try
            {
                string config = Common.Utility.GetDownLoadPath();
                string reallyname = Request.QueryString["name"];
                string reptype = Request.QueryString["reporttype"];
                string filepath = config;
                string FullFileName = string.Empty;
                if (filepath.Substring(filepath.Length - 1) != "\\")
                    filepath = filepath + "\\";
                if (reptype != "all")
                {
                    FullFileName = filepath + DateTime.Now.ToString("yyyyMMdd") + "\\" + reptype + "\\" + reallyname;
                }
                else
                {
                    FullFileName = reallyname;
                    reallyname = reallyname.Substring(reallyname.LastIndexOf("\\") + 1);
                }
                FileInfo info = new FileInfo(FullFileName);
                Response.Clear();
                Response.Charset = "GB2312";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                // 添加头信息，为"文件下载/另存为"对话框指定默认文件名
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(reallyname));
                // 添加头信息，指定文件大小，让浏览器能够显示下载进度
                Response.AddHeader("Content-Length", info.Length.ToString());
                Response.ContentType = "application/octet-stream";
                // 把文件流发送到客户端
                Response.WriteFile(info.FullName);
                // 停止页面的执行
                Response.End();
            }
            catch (Exception e)
            {
                SystemLog.GetInstance().Info(e);
            }
        }
    }
}
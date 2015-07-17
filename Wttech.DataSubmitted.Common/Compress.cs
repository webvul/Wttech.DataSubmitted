
/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个压缩类文件
* 创建标识：ta0395侯兴鼎20141103
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Ionic.Zip;

namespace Wttech.DataSubmitted.Common
{
    /// <summary>
    /// 压缩类
    /// </summary>
    public class Compress
    {
        //#region 9 Public Methods

        ///// <summary>
        ///// 压缩到.zip格式文件中
        ///// </summary>
        ///// <param name="name">压缩文件名称</param>
        ///// <param name="folderPath">需要压缩的文件路径</param>
        //public void CompressAll(string name, string folderPath)
        //{
        //    System.Web.HttpContext.Current.Response.Clear();
        //    System.Web.HttpContext.Current.Response.BufferOutput = false;
        //    string[] files = Directory.GetFiles(folderPath);
        //    //网站文件生成一个readme.txt文件
        //    String readmeText = String.Format("README.TXT" + Environment.NewLine);
        //    System.Web.HttpContext.Current.Response.ContentType = "application/zip";
        //    System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "inline; filename=\"" + String.Format("archive-{0}.zip", DateTime.Now.ToString(name)) + "\"");
        //    //批量压缩操作
        //    using (ZipFile zip = new ZipFile())
        //    {
        //        // the Readme.txt file will not be password-protected.
        //        zip.AddEntry("Readme.txt", readmeText, Encoding.Default);
        //        // zip.Password = "wtkj";
        //        zip.Encryption = EncryptionAlgorithm.WinZipAes256;

        //        // filesToInclude is a string[] or List<String>
        //        zip.AddFiles(files, "files");

        //        zip.Save(System.Web.HttpContext.Current.Response.OutputStream);

        //    }
        //    System.Web.HttpContext.Current.Response.Close();
        //}

        //#endregion
    }
}

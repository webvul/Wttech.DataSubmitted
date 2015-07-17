/**
 * Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
 * 文件功能描述：
 * 创建标识：ta0383王建2014/12/11 9:58:06
 */

#region 引用
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL;

#endregion

namespace Wttech.DataSubmitted.BLL
{
    /// <summary>
    /// 导出全部到压缩文件类
    /// </summary>
    public class ExportAllToZip : IExportAllToZip
    {

        #region 3 Fields
        #endregion

        #region 4 Properties
        #endregion

        #region 9 Public Methods

        public string ExportReport(QueryParameters para)
        {
            return Zip(para);
        }
        #endregion

        #region 11 Private Methods
        /// <summary>
        /// 压缩方法
        /// </summary>
        /// <param name="para">参数类：导出类型,导出假期集合（可空），导出时间范围（可空）</param>
        private string Zip(QueryParameters para)
        {
            Hashtable FilesList = new Hashtable();
            //获取文件
            FilesList = getAllFies(para, FilesList);
            if (FilesList.Count == 0)
            {
                return Common.Resources.TipInfo.BatchExportFail;
            }
            //压缩后的文件名称(包含文件路径)
            string pZipFilePath = Common.Utility.GetDownLoadPath();
            if (pZipFilePath.Substring(pZipFilePath.Length - 1) != "\\")
                pZipFilePath = pZipFilePath + "\\";
            if (!Directory.Exists(pZipFilePath))
            {
                Directory.CreateDirectory(pZipFilePath);
            }
            if (para.ExportType == 1)
            {
                pZipFilePath += "批量导出报表.zip";
            }
            else if (para.ExportType == 2)
            {
                pZipFilePath += string.Format("{0}年度报表.zip", para.StartYear.ToString() + "-" + para.EndYear);
            }
            using (ZipOutputStream zipoutputstream = new ZipOutputStream(File.Create(pZipFilePath)))
            {
                //压缩率0（无压缩）-9（压缩率最高）
                zipoutputstream.SetLevel(6);
                Crc32 crc = new Crc32();
                foreach (DictionaryEntry item in FilesList)
                {
                    if (Path.GetExtension(item.Key.ToString()) != ".zip")
                    {
                        string[] pTemp = item.Key.ToString().Split('\\');
                        string pZipPath = pTemp[pTemp.Length - 3] + "\\" + pTemp[pTemp.Length - 1];
                        //判断文件是否存在
                        if (File.Exists(item.Key.ToString()))
                        {
                            FileStream fs = File.OpenRead(item.Key.ToString());
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            //文件目录
                            ZipEntry entry = new ZipEntry(pZipPath);
                            //文件修改日期
                            entry.DateTime = (DateTime)item.Value;
                            //文件大小
                            entry.Size = fs.Length;
                            fs.Close();
                            crc.Reset();
                            crc.Update(buffer);
                            entry.Crc = crc.Value;
                            zipoutputstream.PutNextEntry(entry);
                            zipoutputstream.Write(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            SystemLog.GetInstance().Log.Info(string.Format("文件不存在，文件路径{0}", item.Key.ToString()));
                        }
                    }
                }
            }
            return pZipFilePath;
        }
        /// <summary>
        /// 获取全部文件
        /// </summary>
        /// <param name="para"></param>
        /// <param name="FilesList"></param>
        /// <returns></returns>
        private Hashtable getAllFies(QueryParameters para, Hashtable FilesList)
        {

            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //年度报表导出
                if (para.ExportType != 1)
                {
                    var pPathList = db.OT_ExportHis.Where(s => (s.CalcuTime.Year >= para.StartYear) && (s.CalcuTime.Year <= para.EndYear)).Select(s => new { s.SavePath, s.CalcuTime });
                    foreach (var temp in pPathList)
                    {
                        if (!FilesList.ContainsKey(temp.SavePath))
                        {
                            FilesList.Add(temp.SavePath, temp.CalcuTime);
                        }
                    }
                }
                else
                {
                    ////导出假期集合
                    //List<int> ptemphasid = new List<int>();
                    ////没有导出假期集合
                    //List<int> ptempnoid = new List<int>();
                    if (para.HDayIdList.Count > 0)//假期报表导出,如果选了假期，则按照假期进行导出
                    {
                        var pPathList = db.OT_ExportHis.Where(s => ((s.CalcuTime.Year == DateTime.Now.Year) && para.HDayIdList.Contains(s.HDayId.Value)) || (s.CalcuTime > para.StartTime && s.CalcuTime < para.EndTime)).Select(s => new { s.SavePath, s.CalcuTime, s.HDayId });
                        foreach (var temp in pPathList)
                        {
                            if (!FilesList.ContainsKey(temp.SavePath))
                            {
                                FilesList.Add(temp.SavePath, temp.CalcuTime);
                                //ptemphasid.Add(temp.HDayId.Value);

                            }
                        }

                        //List<string> HasName = GetHdayId(ptemphasid);
                        ////判断元素集合个数是否相同，减少循环次数
                        //if (ptemphasid.Count != para.HDayIdList.Count)
                        //{
                        //    foreach (int n in para.HDayIdList)
                        //    {
                        //        if (ptemphasid.Exists(s => s == n))
                        //        {
                        //            //没有导出的假期id集合
                        //            ptempnoid.Add(n);
                        //        }
                        //    }
                        //}
                        //List<string> NoHasName = GetHdayId(ptempnoid);
                        ////如果是假期导出则添加导出日志，记录没有导出和导出的假期
                        //int max = HasName.Count;

                        //string nohdaymessage = string.Empty;
                        //string hdaymessage = string.Empty;
                        //if (HasName.Count < NoHasName.Count)
                        //    max = NoHasName.Count;
                        //for (int x = 0; x < max; x++)
                        //{
                        //    if (x < HasName.Count)
                        //        hdaymessage += " " + HasName[x];
                        //    if (x < NoHasName.Count)
                        //        nohdaymessage += " " + NoHasName[x];
                        //    if (NoHasName.Count <= 0)
                        //    {
                        //        nohdaymessage = "空";
                        //    }
                        //}
                        //string message = string.Format("已导出假期：{0},没有导出假期：{1}", hdaymessage, nohdaymessage);
                        //AddLog(FilesList, message);
                    }
                    else//如果没选假期，按照日常报送时间来
                    {
                        var pPathList = db.OT_ExportHis.Where(s => (s.CalcuTime > para.StartTime && s.CalcuTime < para.EndTime)).Select(s => new { s.SavePath, s.CalcuTime });
                        foreach (var temp in pPathList)
                        {
                            if (!FilesList.ContainsKey(temp.SavePath))
                            {
                                FilesList.Add(temp.SavePath, temp.CalcuTime);
                            }
                        }
                    }
                }
                return FilesList;
            }
        }
        /// <summary>
        /// 批量导出日志信息
        /// </summary>
        /// <param name="FilesList"></param>
        private void AddLog(Hashtable FilesList, string message)
        {
            FileStream fs;
            StreamWriter sw;
            //创建导出记录说明文件
            string ptext = AppDomain.CurrentDomain.BaseDirectory + "tmp\\" + "导出日志.txt";
            if (!File.Exists(ptext))
            {
                fs = new FileStream(ptext, FileMode.Create, FileAccess.Write);
            }
            else
            {
                File.Delete(ptext);
                fs = new FileStream(ptext, FileMode.Create, FileAccess.Write);
            }
            sw = new StreamWriter(fs);
            sw.WriteLine(message);//开始写入值
            sw.Close();
            fs.Close();
            FilesList.Add(ptext, DateTime.Now);
        }
        /// <summary>
        /// 将假期ID转换为假期名称
        /// </summary>
        /// <param name="idlist"></param>
        /// <returns></returns>
        private List<string> GetHdayId(List<int> idlist)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                List<string> pHdayNameList = new List<string>();
                foreach (int temp in idlist)
                {
                    pHdayNameList.Add(db.OT_Dic.Where(s => s.IsDelete == 0 && s.Id == temp).Select(s => s.Name).ToList()[0]);
                }
                return pHdayNameList;
            }
        }
        #endregion

        #region 12 Protected Methods
        #endregion
    }
}

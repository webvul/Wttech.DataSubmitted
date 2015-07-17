/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个数据字典模块实现类文件
* 创建标识：ta0395侯兴鼎20141031
*/
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL;

namespace Wttech.DataSubmitted.BLL
{
    /// <summary>
    /// 数据字典模块实现类，目前支持两级
    /// </summary>
    public class DataDictionary : IDataDictionary
    {
        #region 3 Fields

        DataSubmittedEntities db;

        #endregion

        #region 5 Constructors

        /// <summary>
        /// 数据字典构造函数
        /// </summary>
        /// <param name="db">数据库实体</param>
        public DataDictionary(DataSubmittedEntities db)
        {
            this.db = db;
        }

        #endregion

        #region 9 Public Methods

        /// <summary>
        /// 获取某父节点的所有子节点——知道标识
        /// </summary>
        /// <param name="id">节点标识，获取所有根节点时，传入0</param>
        /// <returns>子节点集合</returns>
        public List<DictionaryViewModel> GetList(int id)
        {
            List<DictionaryViewModel> list = db.OT_Dic.Where(a => a.Belong == id ).OrderBy(a=>a.IsDelete).Select(a => new
            DictionaryViewModel
            {
                DictId = a.Id,
                Name = a.Name,
                Type = a.Type,
                Rank = a.Rank,
                Belong = a.Belong,
                Rek = a.Rek,
                IsDelete = a.IsDelete
            }).ToList();
            return list;
        }

        /// <summary>
        ///  获取某父节点的所有子节点的编号和名称——知道标识
        /// </summary>
        /// 标识为删除状态的不获取
        /// <param name="id">节点标识，获取所有根节点时，传入0</param>
        /// <returns></returns>
        public List<NodeViewModel> GetListbyId(int id)
        {
            List<NodeViewModel> list = db.OT_Dic.Where(a => a.Belong == id&a.IsDelete==(byte)EDataStatus.Normal ).Select(a => new NodeViewModel
            {
                DictId = a.Id,
                Name = a.Name,
            }).ToList();
            return list;
        }

        /// <summary>
        /// 获取某根节点的所有子节点——知道名称
        /// </summary>
        /// 查询 数据字典表（OT_Dic）的  
        /// 条件为“Belong=Flag（需要查询的父节点的标识）in 
        /// （select  Flag From  OT_Dic  where  name=“父节点的名称”）”  
        /// 注意：这里我们约定  统一级别的节点的名称 是唯一的
        /// <param name="name">节点名称</param>
        /// <returns>子节点集合</returns>
        public List<DictionaryViewModel> GetList(string name)
        {
            List<DictionaryViewModel> list = db.OT_Dic.Where(a => a.Belong == 0 & a.Name == name ).ToList().Select(a => new DictionaryViewModel
                 {
                     DictId = a.Id,
                     Name = a.Name,
                     Type = a.Type,
                     Rank = a.Rank,
                     Belong = a.Belong,
                     Rek = a.Rek,
                     IsDelete = a.IsDelete
                 }).ToList();
            return list;
        }

        /// <summary>
        /// 通过标识获取节点信息
        /// </summary>
        /// <param name="id">节点标识</param>
        /// <returns>节点信息集合</returns>
        public List<DictionaryViewModel> GetNode(int id)
        {
            List<DictionaryViewModel> list = db.OT_Dic.Where(a => a.Id == id ).ToList().Select(a => new
                 DictionaryViewModel
                 {
                     DictId = a.Id,
                     Name = a.Name,
                     Type = a.Type,
                     Rank = a.Rank,
                     Belong = a.Belong,
                     Rek = a.Rek,
                     IsDelete = a.IsDelete
                 }).ToList();
            return list;
        }

        /// <summary>
        /// 通过标识获取节点信息
        /// </summary>
        /// <param name="belong">所属节点标识</param>
        /// <param name="name">节点名称</param>
        /// <returns>节点信息</returns>
        public List<DictionaryViewModel> GetNode(int belong, string name)
        {
            List<DictionaryViewModel> list = db.OT_Dic.Where(a => a.Belong == belong & a.Name == name).ToList().Select(a => new
                 DictionaryViewModel
            {
                DictId = a.Id,
                Name = a.Name,
                Type = a.Type,
                Rank = a.Rank,
                Belong = a.Belong,
                Rek = a.Rek,
                IsDelete = a.IsDelete
            }).ToList();
            return list;
        }

        /// <summary>
        /// 添加节点，知道节点的父标识，根节点的父标识是0
        /// </summary>
        /// <param name="model">数据字典实体</param>
        /// <returns>是否添加成功</returns>
        public byte Create(DAL.OT_Dic model)
        {
            model.Name = model.Name.Trim();
            using (TransactionScope transaction = new TransactionScope())
            {
                //检查同一级别的节点中是否已经存在了该节点的名称
                var id = db.OT_Dic.Where(a => a.Belong == model.Belong & a.Name == model.Name).ToList().Select(a => new
                {
                    a.Id
                }).ToList();
                if (id != null && id.Count > 0)
                {
                    return (byte)ENodeAddResult.Exist;
                }
                else
                {
                    //添加节点
                    db.OT_Dic.Add(model);
                    return Result.SaveChangesResult(db, transaction).ResultKey;
                }
            }
        }

        /// <summary>
        /// 添加节点，知道父节点的名称
        /// </summary>
        /// <param name="model">节点实体</param>
        /// <param name="name">父节点名称</param>
        /// <returns>添加结果</returns>
        public CustomResult Add(DAL.OT_Dic model, string name)
        {
            model.Name = model.Name.Trim();//去除空格
            using (TransactionScope transaction = new TransactionScope())
            {
                List<DictionaryViewModel> info = GetList(name);//获取根节点信息
                CustomResult pReturnValue = new CustomResult();
                if (info != null && info.Count > 0)
                {
                    //检查同一级别的节点中是否已经存在了该节点的名称
                    var id = db.OT_Dic.Where(a => a.Belong == info[0].DictId & a.Name == model.Name).ToList().Select(a => new
                    {
                        a.Id
                    }).ToList();

                    if (id != null && id.Count > 0)
                    {
                        pReturnValue.ResultKey = (byte)ENodeAddResult.Exist;
                        pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.Exist;
                    }
                    else
                    {
                        //添加节点
                        db.OT_Dic.Add(model);
                        if (db.SaveChanges() > 0)
                        {
                            transaction.Complete();
                            pReturnValue.ResultKey = (byte)ENodeAddResult.Succeed;
                            pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.Success;

                        }
                        else
                        {
                            pReturnValue.ResultKey = (byte)ENodeAddResult.Fail;
                            pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.Failed;
                        }
                    }
                }
                else
                {
                    pReturnValue.ResultKey = (byte)ENodeAddResult.Inexist;
                    pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.Inexist;
                }
                return pReturnValue;
            }
        }

        /// <summary>
        /// 添加节点，知道父节点的编号
        /// </summary>
        /// <param name="model">节点实体</param>
        /// <param name="name">父节点编号</param>
        /// <returns>添加结果</returns>
        public CustomResult Add(DAL.OT_Dic model, int parentId)
        {
            model.Name = model.Name.Trim();//去除空格
            using (TransactionScope transaction = new TransactionScope())
            {
                List<DictionaryViewModel> info = GetNode(parentId);//获取根节点信息
                CustomResult pReturnValue = new CustomResult();
                if (info != null && info.Count > 0)
                {
                    int belong = info[0].DictId;
                    //检查同一级别的节点中是否已经存在了该节点的名称
                    var id = db.OT_Dic.Where(a => a.Belong == belong & a.Name == model.Name).ToList();

                    if (id != null && id.Count > 0)
                    {
                        pReturnValue.ResultKey = (byte)ENodeAddResult.Exist;
                        pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.Exist;
                    }
                    else
                    {
                        model.Id = db.OT_Dic.Max(a => a.Id) + 1;
                        //添加节点
                        db.OT_Dic.Add(model);
                        if (db.SaveChanges() > 0)
                        {
                            transaction.Complete();
                            pReturnValue.ResultKey = (byte)ENodeAddResult.Succeed;
                            pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.AddSuccess;

                        }
                        else
                        {
                            pReturnValue.ResultKey = (byte)ENodeAddResult.Fail;
                            pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.Failed;
                        }
                    }
                }
                else
                {
                    pReturnValue.ResultKey = (byte)ENodeAddResult.Inexist;
                    pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.Inexist;
                }
                return pReturnValue;
            }
        }

        /// <summary>
        /// 修改节点
        /// </summary>
        /// <param name="model">数据字典实体</param>
        /// <returns>影响行数</returns>
        public CustomResult Update(DAL.OT_Dic model)
        {
            model.Name = model.Name.Trim();//去除空格
            using (TransactionScope transaction = new TransactionScope())
            {
                CustomResult pReturnValue = new CustomResult();

                //检查同一级别的节点中是否已经存在了该节点的名称
                var id = db.OT_Dic.Where(a => a.Belong == model.Belong & a.Name == model.Name & a.Id != model.Id).ToList();

                if (id != null && id.Count > 0)
                {
                    pReturnValue.ResultKey = (byte)ENodeAddResult.Exist;
                    pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.Exist;
                    return pReturnValue;
                }
                else
                {
                    var list = db.OT_Dic.Where(a => a.Id == model.Id).ToList();
                    foreach (OT_Dic info in list)
                    {
                        if (!string.IsNullOrEmpty(model.Name))
                            info.Name = model.Name;
                        if (model.Rank != null)
                            info.Rank = model.Rank;
                        if (!string.IsNullOrEmpty(model.Rek))
                            info.Rek = model.Rek;
                        if (model.IsDelete != null)
                            info.IsDelete = model.IsDelete;
                    }
                    return Result.SaveUpdateResult(db, transaction);
                }
            }
        }

        /// <summary>
        /// 将已删除的数据字典节点回复为未删除状态
        /// </summary>
        /// <param name="listId">节点编号集合</param>
        /// <returns></returns>
        public CustomResult Update(List<int> listId)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                foreach (int id in listId)
                {
                    var list = db.OT_Dic.Where(a => a.Id == id).ToList();
                    foreach (OT_Dic info in list)
                    {
                        info.IsDelete = (byte)EDataStatus.Normal;
                    }
                }
                return Result.SaveUpdateResult(db, transaction);
            }
        }

        /// <summary>
        /// 物理删除节点
        /// </summary>
        /// <param name="id">数据字典实体</param>
        /// <returns>影响行数</returns>
        public byte Delete(int id)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                var list = db.OT_Dic.Where(a => a.Id == id).ToList();
                foreach (OT_Dic info in list)
                {
                    db.OT_Dic.Remove(info);
                }
                return Result.DelChangesResult(db, transaction).ResultKey;
            }
        }

        /// <summary>
        /// 物理删除节点
        /// </summary>
        /// <param name="id">数据字典实体</param>
        /// <returns>影响行数</returns>
        public CustomResult DeleteAndResult(int id)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                var list = db.OT_Dic.Where(a => a.Id == id).ToList();
                foreach (OT_Dic info in list)
                {
                    db.OT_Dic.Remove(info);
                }
                return Result.DelChangesResult(db, transaction);
            }
        }

        /// <summary>
        /// 逻辑删除节点
        /// </summary>
        /// <param name="listId">数据字典实体</param>
        /// <returns>影响行数</returns>
        public byte Delete(List<int> listId)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                foreach (int id in listId)
                {
                    var list = db.OT_Dic.Where(a => a.Id == id).ToList();
                    foreach (OT_Dic info in list)
                    {
                        info.IsDelete = (byte)EDataStatus.IsDelete;
                    }
                }
                return Result.DelChangesResult(db, transaction).ResultKey;
            }
        }

        /// <summary>
        /// 逻辑删除节点
        /// </summary>
        /// <param name="listId">数据字典实体</param>
        /// <returns>影响行数</returns>
        public CustomResult DeleteAndResult(List<int> listId)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                foreach (int id in listId)
                {
                    var list = db.OT_Dic.Where(a => a.Id == id).ToList();
                    foreach (OT_Dic info in list)
                    {
                        info.IsDelete = (byte)EDataStatus.IsDelete;
                    }
                }
                return Result.DelChangesResult(db, transaction);
            }
        }

        #endregion
    }
}

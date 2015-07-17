/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个数据字典模块接口文件
* 创建标识：ta0395侯兴鼎20141031
*/
using System.Collections.Generic;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.Common;

namespace Wttech.DataSubmitted.IBLL
{
    /// <summary>
    /// 数据字典模块接口
    /// </summary>
    public interface IDataDictionary : ICreate<OT_Dic>, IUpdate<OT_Dic>, IUpdate<List<int>>, IDelete<int>, IDelete<List<int>>
    {
        #region Methods

        /// <summary>
        /// 获取某父节点的所有子节点——知道标识
        /// </summary>
        /// <param name="id">节点标识，获取所有根节点时，传入0</param>
        /// <returns>子节点集合</returns>
        List<DictionaryViewModel> GetList(int id);

        /// <summary>
        ///  获取某父节点的所有子节点的编号和名称——知道标识
        /// </summary>
        /// <param name="id">节点标识，获取所有根节点时，传入0</param>
        /// <returns></returns>
        List<NodeViewModel> GetListbyId(int id);

        /// <summary>
        /// 获取某根节点的所有子节点——知道名称
        /// </summary>
        /// 查询 数据字典表（OT_Dic）的  
        /// 条件为“Belong=Flag（需要查询的父节点的标识）in 
        /// （select  Flag From  OT_Dic  where  name=“父节点的名称”）”  
        /// 注意：这里我们约定  统一级别的节点的名称 是唯一的
        /// <param name="name">节点名称</param>
        /// <returns>子节点集合</returns>
        List<DictionaryViewModel> GetList(string name);

        /// <summary>
        /// 通过标识获取节点信息
        /// </summary>
        /// <param name="id">节点标识</param>
        /// <returns>节点信息集合</returns>
        List<DictionaryViewModel> GetNode(int id);

        /// <summary>
        /// 通过标识获取节点信息
        /// </summary>
        /// <param name="belong">所属节点标识</param>
        /// <param name="name">节点名称</param>
        /// <returns>节点信息</returns>
        List<DictionaryViewModel> GetNode(int belong, string name);

        /// <summary>
        /// 添加节点，知道父节点的名称
        /// </summary>
        /// <param name="model">节点实体</param>
        /// <param name="name">父节点名称</param>
        /// <returns>添加结果</returns>
        CustomResult Add(DAL.OT_Dic model, string name);

        /// <summary>
        /// 添加节点，知道父节点的编号
        /// </summary>
        /// <param name="model">节点实体</param>
        /// <param name="name">父节点编号</param>
        /// <returns>添加结果</returns>
        CustomResult Add(DAL.OT_Dic model, int id);

             /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="id">数据字典实体</param>
        /// <returns>影响行数</returns>
        CustomResult DeleteAndResult(int id);

          /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="listId">数据字典实体</param>
        /// <returns>影响行数</returns>
        CustomResult DeleteAndResult(List<int> listId);

        #endregion
    }
}

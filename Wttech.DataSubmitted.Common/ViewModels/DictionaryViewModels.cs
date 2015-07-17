/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个数据字典实体集合文件
* 创建标识：ta0395侯兴鼎20141201
*/
using System;

namespace Wttech.DataSubmitted.Common.ViewModels
{
    /// <summary>
    /// 节点实体类
    /// </summary>
    public class NodeViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 节点编号
        /// </summary>
        public int DictId { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name { get; set; } 

        #endregion
    }

    /// <summary>
    /// 数据字典实体
    /// </summary>
    public class DictionaryViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 编号
        /// </summary>
        public int DictId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属
        /// </summary>
        public Nullable<int> Belong { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        public Nullable<int> Rank { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Rek { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Nullable<byte> IsDelete { get; set; } 

        #endregion
    }
}

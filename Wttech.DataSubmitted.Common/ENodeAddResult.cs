/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个添加节点结果枚举类文件
* 创建标识：ta0395侯兴鼎20141106
*/

namespace Wttech.DataSubmitted.Common
{
    /// <summary>
    /// 添加节点结果
    /// </summary>
    public enum ENodeAddResult
    {
        /// <summary>
        /// 节点已存在
        /// </summary>
        Exist = 0,
        /// <summary>
        /// 添加失败
        /// </summary>
        Fail=1,
        /// <summary>
        /// 添加成功
        /// </summary>
        Succeed=2,
        /// <summary>
        /// 不存在该根节点
        /// </summary>
        Inexist=3,
    }
}

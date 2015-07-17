/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个添加操作接口文件
* 创建标识：ta0395侯兴鼎20141030
*/

namespace Wttech.DataSubmitted.IBLL
{
    /// <summary>
    /// 添加操作接口
    /// </summary>
    public interface ICreate<T>
    {
        #region Methods

        /// <summary>
        /// 添加数据信息
        /// </summary>
        /// <param name="args">数据信息</param>
        /// <returns>添加结果</returns>
        byte Create(T args);

        #endregion
    }
}

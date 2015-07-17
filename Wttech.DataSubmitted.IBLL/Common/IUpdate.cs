/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个修改操作接口文件
* 创建标识：ta0395侯兴鼎20141030
*/

using Wttech.DataSubmitted.Common;
namespace Wttech.DataSubmitted.IBLL
{
    /// <summary>
    /// 修改操作接口
    /// </summary>
    public interface IUpdate<T>
    {
        #region Methods

        /// <summary>
        /// 修改数据信息
        /// </summary>
        /// <typeparam name="T">数据表类型集合</typeparam>
        /// <param name="args">参数</param>
        /// <returns>影响行数</returns>
        CustomResult Update(T args);

        #endregion
    }
}

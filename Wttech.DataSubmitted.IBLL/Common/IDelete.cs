/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个删除操作接口文件
* 创建标识：ta0395侯兴鼎20141030
*/

namespace Wttech.DataSubmitted.IBLL
{
    /// <summary>
    /// 删除操作接口
    /// </summary>
    public interface IDelete<T>
    {
        #region Methods

        /// <summary>
        /// 删除数据信息
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="args">参数</param>
        /// <returns>结果</returns>
        byte Delete(T args); 

        #endregion
    }
}

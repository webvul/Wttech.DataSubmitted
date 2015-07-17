/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个查询操作接口文件
* 创建标识：ta0395侯兴鼎20141030
*/

namespace Wttech.DataSubmitted.IBLL
{
    /// <summary>
    /// 配置项查询接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQuery<T>
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T GetList();

        #endregion
    }
}

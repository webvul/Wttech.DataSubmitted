/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个登录结果枚举类文件
* 创建标识：ta0395侯兴鼎20141106
*/

namespace Wttech.DataSubmitted.Common
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public enum  ELoginResult
    {
        /// <summary>
        /// 用户名为空
        /// </summary>
        NameIsNull=0,
        /// <summary>
        /// 密码为空
        /// </summary>
        PasswordIsNull=1,
        /// <summary>
        /// 用户名不存在
        /// </summary>
        NameInexist=2,
        /// <summary>
        /// 用户名存在，但是密码不匹配
        /// </summary>
        PasswordError=3,
        /// <summary>
        /// 用户名和密码匹配，但是该记录为已删除状态
        /// </summary>
        IsDelete=4,
        /// <summary>
        /// 登录成功
        /// </summary>
        Succeed=5,
    }
}

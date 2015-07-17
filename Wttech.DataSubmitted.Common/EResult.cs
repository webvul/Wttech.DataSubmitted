/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个操作数据结果枚举类文件
* 创建标识：ta0395侯兴鼎20141107
*/

namespace Wttech.DataSubmitted.Common
{
    /// <summary>
    /// 操作结果
    /// </summary>
    public enum EResult
    {
        /// <summary>
        /// 操作失败
        /// </summary>
        Fail= 0,
        /// <summary>
        /// 操作成功
        /// </summary>
        Succeed=1 ,
        /// <summary>
        /// 参数1为空
        /// </summary>
        IsNull1=2,
        /// <summary>
        /// 参数2为空
        /// </summary>
        IsNull2 = 3,
        /// <summary>
        /// 参数3为空
        /// </summary>
        IsNull3 = 4,
        /// <summary>
        /// 参数4为空
        /// </summary>
        IsNull4 = 5,
        /// <summary>
        /// 参数5为空
        /// </summary>
        IsNull5 = 6,

        /// <summary>
        /// 重复且为正常状态
        /// </summary>
        IsRepeat=7,

        /// <summary>
        /// 重复且为删除状态
        /// </summary>
        IsRepeatDel=8,
    }
}

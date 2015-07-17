/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个假期配置接口文件
* 创建标识：ta0395侯兴鼎20141103
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.DAL;

namespace Wttech.DataSubmitted.IBLL
{
    /// <summary>
    /// 假期配置接口
    /// </summary>
    public interface IHolidayConfig : IUpdate<OT_HDayConfig>, IQuery<List<OT_HDayConfig>>, IUpdate<int>
    {
        #region Methods

        /// <summary>
        /// 根据报表配置ID读取（假期和非假期表都存在HolidayConfig表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OT_HDayConfig GetById(int id);

        /// <summary>
        /// 根据报表配置ID读取（假期和非假期表都存在HolidayConfig表）
        /// </summary>
        /// <param name="id">报表编号（配置ID）</param>
        /// <returns></returns>
        HolidayConfigViewModel GetHolidayConfigById(int id);

        /// <summary>
        /// 获取假期配置信息列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        List<HolidayConfigViewModel> GetList(int type);

        /// <summary>
        /// 通过假期名称获取假期时间
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        HolidayTimeViewModel GetHolidayTime(string name);

        #endregion
    }
}

/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个用户管理模块实体类集合类文件
* 创建标识：ta0395侯兴鼎20141111
*/
using System;
using System.Collections.Generic;

namespace Wttech.DataSubmitted.Common.ViewModels
{
    /// <summary>
    /// 用户信息实体
    /// </summary>
    public class UserInfoViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 用户编号
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户角色列表
        /// </summary>
        public List<RoleInfoViewModel> RoleList { get; set; }

        /// <summary>
        /// 操作-无须赋值
        /// </summary>
        public int Action { get; set; } 

        #endregion

    }

    /// <summary>
    /// 修改用户信息实体
    /// </summary>
    public class UpdateUserInfoViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 用户编号
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户角色列表
        /// </summary>
        public List<Guid> RoleIdList { get; set; } 

        #endregion

    }

    /// <summary>
    /// 添加用户信息实体
    /// </summary>
    public class AddUserInfoViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户角色列表
        /// </summary>
        public List<Guid> RoleIdList { get; set; } 

        #endregion
    }

    /// <summary>
    /// 角色分配实体,用户与角色是多对多的关系
    /// </summary>
    public class RoleAssignViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 用户编号
        /// </summary>
        public List<Guid> UserIdList { get; set; }

        /// <summary>
        /// 用户角色列表
        /// </summary>
        public List<Guid> RoleIdList { get; set; } 

        #endregion
    }

    /// <summary>
    /// 角色信息实体
    /// </summary>
    public class RoleInfoViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 角色编号
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; } 

        #endregion
    }

    /// <summary>
    /// 用户角色关系实体
    /// </summary>
    public class UserRoleViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 用户角色关系编号
        /// </summary>
        public Guid UserRoleId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public Guid RoleId { get; set; } 

        #endregion
    }

    /// <summary>
    /// 分页用户信息
    /// </summary>
    public class UserInfoPageViewModel
    {
        #region 4 Properties

        /// <summary>
        /// 用户信息集合
        /// </summary>
        public List<UserInfoViewModel> UserInfo { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public int Count { get; set; } 

        #endregion
    }
}

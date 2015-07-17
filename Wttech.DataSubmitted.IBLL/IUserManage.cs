/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个用户权限管理模块接口文件
* 创建标识：ta0395侯兴鼎20141030
*/
using System;
using System.Collections.Generic;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.Common.ViewModels;
using System.Web;
using Wttech.DataSubmitted.Common;

namespace Wttech.DataSubmitted.IBLL
{
    /// <summary>
    /// 用户角色管理模块接口
    /// </summary>
    public interface IUserManage : IQuery<List<UserInfoViewModel>>, IDelete<List<Guid>>, ICreate<AddUserInfoViewModel>, IUpdate<UpdateUserInfoViewModel>
    {
        #region Methods

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns>验证结果</returns>
        byte Login(string name, string pwd);

        /// <summary>
        /// 角色分配
        /// </summary>
        /// <param name="listInfo">用户角色关系实体集合</param>
        /// <returns>分配结果</returns>
        byte RoleAssign(RoleAssignViewModel listInfo);

        /// <summary>
        /// 获取角色信息列表
        /// </summary>
        /// <returns>角色信息列表</returns>
        List<RoleInfoViewModel> GetRoleList();

        /// <summary>
        /// 通过用户名获取所有的用户信息集合
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns>用户信息</returns>
        UserInfoPageViewModel GetListbyName(string name, int pageIndex, int pageSize);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="pwd">新密码</param>
        /// <param name="oldpwd">新密码</param>
        /// <returns>修改结果</returns>
        CustomResult UpdatePassword(Guid userId, string pwd, string oldpassword);

        /// <summary>
        /// 初始化单条密码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>修改结构</returns>
        CustomResult StartPassword(Guid userId);

        /// <summary>
        /// 初始化单条密码
        /// </summary>
        /// <param name="listUserRoleId">用户角色关系编号集合</param>
        /// <returns>修改结构</returns>
        CustomResult StartPassword(List<Guid> listUserId);

        /// <summary>
        /// 通过用户编号获取用户名称
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        string GetNamebyId(Guid id);

        /// <summary>
        /// 通过角色编号获取角色名称
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        string GetRoleNamebyId(Guid id);

        #endregion
    }
}

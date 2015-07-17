/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个用户权限管理类文件
* 创建标识：ta0395侯兴鼎20141030
*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL;

namespace Wttech.DataSubmitted.BLL
{
    /// <summary>
    /// 用户权限管理类
    /// </summary>
    public class UserManage : IUserManage
    {
        #region 9 Public Methods

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="Session">用户信息缓存</param>
        /// <returns><验证结果/returns>
        public byte Login(string name, string pwd)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //查找数据库中用户名匹配的记录
                List<OT_User> list = db.OT_User.Where(a => a.Name == name).ToList();

                //存在该用户名
                if (list != null && list.Count > 0)
                {
                    //对密码进行加密
                    MD5Encryptor md5 = new MD5Encryptor();
                    string password = md5.Encrypt(pwd);

                    //密码比对
                    if (list[0].Password == password)
                    {
                        //检查状态
                        if (list[0].IsDelete == (byte)EUserStatus.Normal)
                        {
                            SaveUserInfo(list[0].Id);
                            return (byte)ELoginResult.Succeed;
                        }
                        else
                        {
                            return (byte)ELoginResult.IsDelete;
                        }
                    }
                    else
                    {
                        return (byte)ELoginResult.PasswordError;
                    }
                }
                else
                {
                    return (byte)ELoginResult.NameInexist;
                }
            }
        }

        /// <summary>
        /// 保存用户信息到缓存中
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="Session">存储用户信息缓存</param>
        private void SaveUserInfo(Guid userId)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                List<UserInfoViewModel> list = db.OT_User.Where(a => a.Id == userId & a.IsDelete == (byte)EUserStatus.Normal).Select(a => new UserInfoViewModel
                 {
                     UserId = a.Id,
                     UserName = a.Name,
                 }).ToList();

                //给用户的角色赋值
                foreach (UserInfoViewModel model in list)
                {
                    model.RoleList = db.OT_UserRole.Where(a => a.UserId == userId && a.IsDelete == (byte)EDataStatus.Normal).Select(a => new RoleInfoViewModel
                    {
                        RoleId = a.RoleId,
                        RoleName = a.OT_Role.Name
                    }).ToList();
                }

                SessionManage.SetSession("UserInfo", list[0]);
            }
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="userInfo">添加用户信息实体</param>
        /// <returns>添加结果</returns>
        public byte Create(AddUserInfoViewModel userInfo)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //查找用户表中是否已经存在该用户
                var user = db.OT_User.Where(a => a.Name == userInfo.UserName).Select(a => new
                {
                    UserId = a.Id,
                    IsDelete=a.IsDelete
                }).ToList();

                //有记录，则返回用户已存在
                if (user != null && user.Count > 0)
                {
                    if (user[0].IsDelete == (byte)EUserStatus.Normal)
                        return (byte)EResult.IsRepeat;
                    else
                        return (byte)EResult.IsRepeatDel; ;
                }
                using (TransactionScope transaction = new TransactionScope())
                {
                    //构建并添加用户信息
                    OT_User userModel = new OT_User();
                    userModel.Id = Guid.NewGuid();
                    userModel.IsDelete = (byte)EUserStatus.Normal;
                    userModel.Name = userInfo.UserName;
                    userModel.Password = ConfigurationManager.AppSettings["StartPassword"];
                    db.OT_User.Add(userModel);

                    //构建添加用户角色关系
                    foreach (Guid roleId in userInfo.RoleIdList)
                    {
                        OT_UserRole model = new OT_UserRole();
                        model.Id = Guid.NewGuid();
                        model.RoleId = roleId;
                        model.UserId = userModel.Id;
                        model.IsDelete = (byte)EDataStatus.Normal;
                        db.OT_UserRole.Add(model);
                    }
                    return Result.SaveChangesResult(db, transaction).ResultKey;
                }
            }
        }

        /// <summary>
        /// 角色分配
        /// </summary>
        /// <param name="listInfo">用户角色关系实体集合</param>
        /// <returns>分配结果</returns>
        public byte RoleAssign(RoleAssignViewModel listInfo)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    //删除用户角色关系
                    var list = db.OT_UserRole.Where(a => listInfo.UserIdList.Contains(a.UserId)).ToList();
                    db.OT_UserRole.RemoveRange(list);
                    //构建并添加用户角色关系
                    foreach (Guid uesrId in listInfo.UserIdList)
                    {
                        foreach (Guid roleId in listInfo.RoleIdList)
                        {
                            if (roleId == null)
                            {
                                return (byte)EResult.IsNull3;
                            }
                            OT_UserRole userRoleInfo = new OT_UserRole();
                            userRoleInfo.Id = Guid.NewGuid();
                            userRoleInfo.IsDelete = (byte)EDataStatus.Normal;
                            userRoleInfo.RoleId = roleId;
                            userRoleInfo.UserId = uesrId;
                            db.OT_UserRole.Add(userRoleInfo);
                        }
                    }
                    return Result.SaveChangesResult(db, transaction).ResultKey;
                }
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="listUserRoleId">用户角色关系编号集合</param>
        /// <returns>删除结果</returns>
        public byte Delete(List<Guid> listUserId)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    foreach (Guid userId in listUserId)
                    {
                        //获取用户角色关系记录
                        var list = db.OT_UserRole.Where(a => a.UserId == userId & a.IsDelete == (byte)EDataStatus.Normal).ToList();

                        if (list != null && list.Count > 0)
                        {
                            //将用户角色关系记录标识为删除状态
                            foreach (OT_UserRole userRoleInfo in list)
                            {
                                userRoleInfo.IsDelete = (byte)EDataStatus.IsDelete;
                            }

                            //获取用户记录
                            var listUserInfo = db.OT_User.Where(a => a.Id == userId & a.IsDelete == (byte)EUserStatus.Normal).ToList();

                            //将用户状态标识为删除状态
                            foreach (OT_User userInfo in listUserInfo)
                            {
                                userInfo.IsDelete = (byte)EUserStatus.IsDelete;
                            }
                        }
                    }
                    return Result.SaveChangesResult(db, transaction).ResultKey;
                }
            }
        }

        /// <summary>
        /// 获取所有的用户信息集合
        /// </summary>
        /// <returns>用户信息集合</returns>
        public List<UserInfoViewModel> GetList()
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //获取用户信息列表
                List<UserInfoViewModel> list = db.OT_User.Where(a => a.IsDelete == (byte)EDataStatus.Normal & a.Name != "admin").Select(a => new UserInfoViewModel
                {
                    UserId = a.Id,
                    UserName = a.Name
                }).ToList();

                //给用户的角色赋值
                foreach (UserInfoViewModel model in list)
                {
                    model.RoleList = db.OT_UserRole.Where(a => a.UserId == model.UserId && a.IsDelete == (byte)EDataStatus.Normal).Select(a => new RoleInfoViewModel
                    {
                        RoleId = a.RoleId,
                        RoleName = a.OT_Role.Name
                    }).ToList();
                }

                return list;
            }
        }

        /// <summary>
        /// 获取角色信息列表
        /// </summary>
        /// <returns>角色信息列表</returns>
        public List<RoleInfoViewModel> GetRoleList()
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                List<RoleInfoViewModel> list = db.OT_Role.Where(a => a.IsDelete == (byte)EDataStatus.Normal).Select(a => new RoleInfoViewModel
                {
                    RoleId = a.Id,
                    RoleName = a.Name,
                }).ToList();
                return list;
            }
        }

        /// <summary>
        /// 通过用户名获取所有的用户信息集合
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns>用户信息</returns>
        public UserInfoPageViewModel GetListbyName(string name, int pageIndex, int pageSize)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //获取用户信息列表
                List<UserInfoViewModel> list;

                int pageCount, count;//总页数，总行数

                //获取数据总条数
                count = db.OT_User.Where(a => a.Name.Contains(name) & a.IsDelete == (byte)EDataStatus.Normal & a.Name != "admin").Count();

                //获取总页数
                pageCount = count % pageSize > 0 ? count / pageSize + 1 : count / pageSize;

                //如果页数大于总页数，或者小于1，则返回第一页
                if (pageIndex > pageCount || pageIndex < 1)
                    pageIndex = 1;

                //获取用户信息
                if (pageIndex == 1)
                {
                    list = db.OT_User.Where(a => a.Name.Contains(name) & a.IsDelete == (byte)EDataStatus.Normal & a.Name != "admin").OrderBy(a => a.Name).Take(pageSize).Select(a => new UserInfoViewModel
                      {
                          UserId = a.Id,
                          UserName = a.Name,
                      }).ToList();
                }
                else
                {
                    int excludedRows = (pageIndex - 1) * pageSize;//计算起始索引

                    list = db.OT_User.Where(a => a.Name.Contains(name) & a.IsDelete == (byte)EDataStatus.Normal & a.Name != "admin").OrderBy(a => a.Name).Skip(excludedRows).Take(pageSize).Select(a => new UserInfoViewModel
            {
                UserId = a.Id,
                UserName = a.Name,
            }).ToList();
                }

                //给用户的角色赋值
                foreach (UserInfoViewModel model in list)
                {
                    model.RoleList = db.OT_UserRole.Where(a => a.UserId == model.UserId && a.IsDelete == (byte)EDataStatus.Normal).Select(a => new RoleInfoViewModel
                    {
                        RoleId = a.RoleId,
                        RoleName = a.OT_Role.Name
                    }).ToList();
                }

                //构建分页用户信息并返回
                UserInfoPageViewModel upModel = new UserInfoPageViewModel();
                upModel.Count = count;
                upModel.PageCount = pageCount;
                upModel.PageIndex = pageIndex;
                upModel.UserInfo = list;

                return upModel;
            }
        }

        /// <summary>
        /// 通过用户编号获取用户名称
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public string GetNamebyId(Guid id)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                //获取用户信息列表
                var name = db.OT_User.Where(a => a.Id == id & a.IsDelete == (byte)EDataStatus.Normal).Select(a => a.Name).ToList();

                if (name != null && name.Count > 0)
                    return name[0].ToString();
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// 通过角色编号获取角色名称
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public string GetRoleNamebyId(Guid id)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                var name = db.OT_Role.Where(a => a.Id == id & a.IsDelete == (byte)EDataStatus.Normal).Select(a => a.Name).ToList();

                if (name != null && name.Count > 0)
                    return name[0].ToString();
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userInfo">修改用户信息实体</param>
        /// <returns>结果</returns>
        public CustomResult Update(UpdateUserInfoViewModel userInfo)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    CustomResult pReturnValue = new CustomResult();

                    //查找用户表中是否已经存在该用户
                    var user = db.OT_User.Where(a => a.Name == userInfo.UserName&a.Id!=userInfo.UserId).Select(a => new
                    {
                        UserId = a.Id,
                        IsDelete = a.IsDelete
                    }).ToList();

                    //有记录，则返回用户已存在
                    if (user != null && user.Count > 0)
                    {
                        if (user[0].IsDelete == (byte)EUserStatus.Normal)
                        {
                            pReturnValue.ResultKey = (byte)EResult.IsRepeat;
                            pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.AddFaileHasUser;
                        }
                        else
                        {
                            pReturnValue.ResultKey = (byte)EResult.IsRepeatDel;
                            pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.AddFaileHasDelUser;
                            
                        }
                        return pReturnValue;
                    }

                    //修改用户名
                    var listUserName = db.OT_User.Where(a => a.Id == userInfo.UserId).ToList();

                    //如果用户不存在，这返回，说明数据不是正常输入
                    if (!(listUserName != null && listUserName.Count > 0))
                    {
                        pReturnValue.ResultKey = (byte)EResult.IsNull4;
                    }

                    listUserName[0].Name = userInfo.UserName;

                    //获取用户角色关系
                    var list = db.OT_UserRole.Where(a => a.UserId == userInfo.UserId & a.IsDelete == (byte)EDataStatus.Normal).ToList();

                    //删除用户角色关系
                    foreach (var model in list)
                    {
                        model.IsDelete = (byte)EDataStatus.IsDelete;
                    }

                    //添加用户角色关系
                    foreach (Guid roleId in userInfo.RoleIdList)
                    {
                        OT_UserRole userRoleInfo = new OT_UserRole();
                        userRoleInfo.Id = Guid.NewGuid();
                        userRoleInfo.IsDelete = (byte)EDataStatus.Normal;
                        userRoleInfo.RoleId = roleId;
                        userRoleInfo.UserId = userInfo.UserId;
                        db.OT_UserRole.Add(userRoleInfo);
                    }
                    pReturnValue = Result.SaveChangesResult(db, transaction);
                    return pReturnValue;
                }
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="pwd">新密码</param>
        /// <param name="oldpwd">旧密码</param>
        /// <returns>修改结果</returns>
        public CustomResult  UpdatePassword(Guid userId, string pwd, string oldpwd)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    MD5Encryptor md5 = new MD5Encryptor();
                    string password = md5.Encrypt(oldpwd);

                    var list = db.OT_User.Where(a => a.Id == userId & a.Password == password & a.IsDelete == (byte)EUserStatus.Normal).ToList();

                    if (list != null && list.Count > 0)
                    {

                        password = md5.Encrypt(pwd);
                        foreach (var info in list)
                        {
                            info.Password = password;
                        }
                        SessionManage.SetSession("UserInfo", null);
                        return Result.SaveUpdateResult(db, transaction);
                    }
                    else
                    {
                        CustomResult pReturnValue = new CustomResult();
                        pReturnValue.ResultKey = (byte)EResult.Fail;//程序已经使用多处，所有不变动
                        pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.OldPasswordError;
                        return pReturnValue;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化单条密码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>修改结构</returns>
        public CustomResult StartPassword(Guid userId)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    var list = db.OT_User.Where(a => a.Id == userId & a.IsDelete == (byte)EUserStatus.Normal).ToList();

                    if (list != null && list.Count > 0)
                    {
                        string password = ConfigurationManager.AppSettings["StartPassword"];
                        foreach (var i in list)
                        {
                            i.Password = password;
                        }
                        CustomResult pReturnValue = new CustomResult();
                        if (db.SaveChanges() > 0)
                        {
                            transaction.Complete();
                            pReturnValue.ResultKey = (byte)EResult.Succeed;
                            pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.StartPassword;
                        }
                        else
                        {
                            pReturnValue.ResultKey = (byte)EResult.Fail;//程序已经使用多处，所有不变动
                            pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.StartPassword;
                        }
                        return pReturnValue;
                    }
                    else
                    {
                        CustomResult pReturnValue = new CustomResult();
                        pReturnValue.ResultKey = (byte)EResult.Fail;//程序已经使用多处，所有不变动
                        pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.Inexist;
                        return pReturnValue;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化单条密码
        /// </summary>
        /// <param name="listUserRoleId">用户角色关系编号集合</param>
        /// <returns>修改结构</returns>
        public CustomResult StartPassword(List<Guid> listUserId)
        {
            using (DataSubmittedEntities db = new DataSubmittedEntities())
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    int flag=0;
                    string password = ConfigurationManager.AppSettings["StartPassword"];
                    foreach (Guid userId in listUserId)
                    {
                        var list = db.OT_User.Where(a => a.Id == userId & a.IsDelete == (byte)EUserStatus.Normal).ToList();

                        if (list != null && list.Count > 0)
                        {
                            foreach (var i in list)
                            {
                                i.Password = password;
                            }
                            flag++;
                        }
                      
                    }
                    if(flag>0)
                    {
                        CustomResult pReturnValue = new CustomResult();
                        if (db.SaveChanges() > 0)
                        {
                            transaction.Complete();
                            pReturnValue.ResultKey = (byte)EResult.Succeed;
                            pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.StartPassword;
                        }
                        else
                        {
                            pReturnValue.ResultKey = (byte)EResult.Fail;//程序已经使用多处，所有不变动
                            pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.StartPassword;
                        }
                        return pReturnValue;
                    }
                    else
                    {
                        CustomResult pReturnValue = new CustomResult();
                        pReturnValue.ResultKey = (byte)EResult.Fail;//程序已经使用多处，所有不变动
                        pReturnValue.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.Inexist;
                        return pReturnValue;
                    }
                }
            }
        }

        #endregion
    }
}
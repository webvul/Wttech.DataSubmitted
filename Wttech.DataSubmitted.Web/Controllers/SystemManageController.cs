/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个用户权限管理模块接口控制器文件
* 创建标识：ta0395侯兴鼎20141105
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wttech.DataSubmitted.DAL;
using Wttech.DataSubmitted.IBLL;
using Wttech.DataSubmitted.BLL;
using Microsoft.Practices.Unity;
using Wttech.DataSubmitted.Common;
using Wttech.DataSubmitted.Common.ViewModels;
using System.Configuration;
using Wttech.DataSubmitted.Common.Resources;

namespace Wttech.DataSubmitted.Web.Controllers
{
    /// <summary>
    /// 用户权限管理模块控制器
    /// </summary>
    public class SystemManageController : Controller
    {
        #region 4 Properties

        #region 用户权限管理模块

        /// <summary>
        /// 用户权限管理模块接口
        /// </summary>
        [Dependency]
        public IUserManage userManage { get; set; }

        #endregion

        #endregion

        #region 9 Public Methods

        #region 假期报送配置计划模块

        #region 假期管理

        /// <summary>
        /// 编辑每日报送假期配置管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult editHolConfig()
        {
            return View();
        }

        /// <summary>
        /// 编辑北京段假期配置管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult editBJHol()
        {
            return View();
        }
        /// <summary>
        /// 编辑假期配置模板1（包含时间段）页面
        /// </summary>
        /// <returns></returns>
        public ActionResult editTemplate()
        {
            return View();
        }

        /// <summary>
        /// 加载假期配置管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult HolidayConfig()
        {
            return View();
        }

        /// <summary>
        /// 加载假期名称管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult HolidayName()
        {
            return View();
        }

        /// <summary>
        /// 加载假期基本信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetHoliday()
        {
            var result = ReportFactory.Instance.DicGetList((int)EDicParentId.Holiday);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 新增假期名称
        /// </summary>
        /// <param name="name">假期名称</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddHoliday(string name)
        {
            CustomResult result = new CustomResult();
            if (string.IsNullOrEmpty(name))
            {
                result.ResultKey = (byte)ENodeAddResult.Exist;
                result.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.InputHolidayName;
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            result = ReportFactory.Instance.DicAdd((int)EDicParentId.Holiday, name);
            if (result.ResultKey == (byte)ENodeAddResult.Succeed)
            {
                ReportFactory.Instance.log.WriteLog(Common.OperationType.Update, string.Format("新增了假期名称{0}", name));
            }

            //精确假期名称存在提示信息
            if (result.ResultKey == (byte)ENodeAddResult.Exist)
            {
                result.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.HolidayExist;
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 删除假期名称
        /// </summary>
        /// <param name="dictIdList">假期编号集合</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteHoliday(List<int> dictIdList)
        {
            var result = ReportFactory.Instance.DicDelete(dictIdList);
            if (result.ResultKey == (byte)EResult.Succeed)
            {
                string name = string.Empty;
                foreach (int id in dictIdList)
                {
                    foreach (DictionaryViewModel model in ReportFactory.Instance.dataDictionary.GetNode(id))
                    {
                        name = string.Format("{0},{1}", name, model.Name);
                    }
                }
                ReportFactory.Instance.log.WriteLog(Common.OperationType.Update, string.Format("删除了假期名称{0}", name));
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 编辑假期名称
        /// </summary>
        /// <param name="DictId">假期名称编号</param>
        /// <param name="Name">假期名称</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateHoliday(NodeViewModel model)
        {
            CustomResult result = new CustomResult();
            if (string.IsNullOrEmpty(model.Name))
            {
                result.ResultKey = (byte)ENodeAddResult.Exist;
                result.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.InputHolidayName;
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            result = ReportFactory.Instance.DicUpdate(model);

            //精确假期名称存在提示信息
            if (result.ResultKey == (byte)ENodeAddResult.Exist)
            {
                result.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.HolidayExist;
            }

            else if (result.ResultKey == (byte)EResult.Succeed)
            {
                ReportFactory.Instance.log.WriteLog(Common.OperationType.Update, string.Format("编辑了假期名称{0}", model.Name));
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 恢复假期名称
        /// </summary>
        /// <param name="dictIdList">假期名称集合</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecoverHoliday(List<int> dictIdList)
        {
            var result = ReportFactory.Instance.DicRecover(dictIdList);
            if (result.ResultKey == (byte)EResult.Succeed)
            {
                string name = string.Empty;
                foreach (int id in dictIdList)
                {
                    foreach (DictionaryViewModel model in ReportFactory.Instance.dataDictionary.GetNode(id))
                    {
                        name = string.Format("{0},{1}", name, model.Name);
                    }
                }
                ReportFactory.Instance.log.WriteLog(Common.OperationType.Update, string.Format("恢复了假期名称{0}", name));
            }

            //变更提示信息
            if (result.ResultKey == (byte)EResult.Succeed)
            {
                result.ResultValue = TipInfo.RecoverSuccess; ;
            }
            else if (result.ResultKey == (byte)EResult.Fail)
            {
                result.ResultValue = TipInfo.RecoverFaile; ;
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 获取假期名称下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetHolidayList()
        {
            var result = ReportFactory.Instance.DicGetListbyId((int)EDicParentId.Holiday);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 通过假期名称获取假期时间
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetHolidayTime(string name)
        {
            var result = ReportFactory.Instance.GetHolidayTime(name);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        #endregion

        /// <summary>
        /// 加载假期报送配置列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetConfig(int type)
        {
            var result = ReportFactory.Instance.GetConfig(type);

            //if (Session["UserInfo"] != null)
            //{
            //    string name = string.Empty;
            //    switch (type)
            //    {
            //        case 1: name = "每日报送";
            //            break;
            //        case 2: name = "北京段";
            //            break;
            //        case 3: name = "天津段";
            //            break;
            //    }
            //ReportFactory.Instance.log.WriteLog(Common.OperationType.Query, string.Format("查询了{0}假期配置", name));
            //}
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 修改假期配置表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateConfig(HolidayConfigViewModel model)
        {
            var result = ReportFactory.Instance.UpdateConfig(model);

            //修改成功并且用户登录缓存未过期，则记录日志
            if (result.ResultKey == (byte)EResult.Succeed)
            {
                model.ConfigName = ReportFactory.Instance.holidayConfig.GetById(model.HolidayConfigId).ConfigName;
                ReportFactory.Instance.log.WriteLog(Common.OperationType.Update, string.Format("修改了假期配置表中的{0}", model.ConfigName));
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 设置默认假期
        /// </summary>
        /// <param name="holidayId">假期编号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetHoliday(int holidayId = 0)
        {
            var result = ReportFactory.Instance.SetHoliday(holidayId);

            //设置成功并且用户登录缓存未过期，则记录日志
            if (result.ResultKey == (byte)EResult.Succeed)
            {
                ReportFactory.Instance.log.WriteLog(Common.OperationType.Update, string.Format("设置默认假期名称为{0}", ReportFactory.Instance.dataDictionary.GetNode(holidayId)[0].Name));
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        #endregion

        #region 用户权限管理模块

        /// <summary>
        /// 加载登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录结果</returns>
        [HttpPost]
        public JsonResult Login(string userName, string password)
        {
            //判断用户名是否为空
            if (string.IsNullOrEmpty(userName))
                return Json((byte)ELoginResult.NameIsNull, JsonRequestBehavior.DenyGet);

            //判断密码是否为空
            if (string.IsNullOrEmpty(password))
                return Json((byte)ELoginResult.PasswordIsNull, JsonRequestBehavior.DenyGet);

            //尝试登录并返回登录结果
            byte result = userManage.Login(userName, password);
            if (result == (byte)ELoginResult.Succeed)
            {
                ReportFactory.Instance.log.WriteLog(Common.OperationType.Login, "登录系统");
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 加载用户信息
        /// </summary>
        /// <param name="userInfo">用户信息实体</param>
        /// <returns></returns>
        public ActionResult UserManagement()
        {
            return View();
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示行数</param>
        /// <returns>用户信息列表</returns>
        [HttpPost]
        public JsonResult UserManagement(string userName = "", int pageIndex = 1, int pageSize = 10)
        {
            //int myPageIndex=1, myPageSize = 10;
            if (pageIndex <= 0)
                pageIndex = 1;

            if (pageSize <= 0)
                pageSize = 10;

            //返回查询结果结果
            var result = userManage.GetListbyName(userName, pageIndex, pageSize);

            //if (Session["UserInfo"] != null)
            //    ReportFactory.Instance.log.WriteLog(Common.OperationType.Query, "查询用户信息");
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userRoleId">用户编号</param>
        /// <param name="roleId">角色编号</param>
        /// <param name="userName">用户名</param>
        /// <returns>修改结果</returns>
        [HttpPost]
        public JsonResult Update(string userId, List<string> roleList, string userName)
        {
            CustomResult result = new CustomResult();
            #region 数据判定

            Guid id = Guid.NewGuid();
            if (!Guid.TryParse(userId, out id))
            {
                result.ResultKey=(byte)EResult.IsNull1;
                result.ResultValue=Wttech.DataSubmitted.Common.Resources.TipInfo.SelectUser;
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            bool flag = false;
            foreach (string roleId in roleList)
            {
                if (Guid.TryParse(roleId, out id))
                    flag = true;
            }
            if (!(roleList != null && roleList.Count > 0 && flag))
            {
                result.ResultKey = (byte)EResult.IsNull2;
                result.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.SelectRole;
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            if (string.IsNullOrEmpty(userName))
            {
                result.ResultKey = (byte)EResult.IsNull3;
                result.ResultValue = Wttech.DataSubmitted.Common.Resources.TipInfo.InputUserName;
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            #endregion

            //构造修改用户信息实体
            UpdateUserInfoViewModel userInfoModel = new UpdateUserInfoViewModel();
            userInfoModel.UserId = Guid.Parse(userId);
            userInfoModel.UserName = userName;

            List<Guid> list = new List<Guid>();
            foreach (string roleId in roleList)
            {
                if (Guid.TryParse(roleId, out id))
                {
                    list.Add(id);
                }
            }
            userInfoModel.RoleIdList = list;

            //交给业务逻辑层处理
             result = userManage.Update(userInfoModel);

            if (Session["UserInfo"] != null)
                ReportFactory.Instance.log.WriteLog(Common.OperationType.Update, string.Format("修改了{0}的信息", userName));
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        /// <summary>
        /// 初始化密码
        /// </summary>
        /// <param name="userRoleId">用户角色关系编号</param>
        /// <returns>初始化结果</returns>
        public JsonResult StartPassword(string userId)
        {
            Guid urId = Guid.NewGuid();
            if (!Guid.TryParse(userId, out urId))
                return Json((byte)EResult.IsNull1, JsonRequestBehavior.DenyGet);

            var result = userManage.StartPassword(urId);

            if (Session["UserInfo"] != null)
                ReportFactory.Instance.log.WriteLog(Common.OperationType.Update, string.Format("初始化了{0}的密码", userManage.GetNamebyId(urId)));
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="roleList">角色编号</param>
        /// <param name="userName">用户名</param>
        /// <returns>添加结果</returns>
        [HttpPost]
        public JsonResult Add(List<Guid> roleList, string userName)
        {
            if (!(roleList != null && roleList.Count > 0))
                return Json((byte)EResult.IsNull1, JsonRequestBehavior.DenyGet);

            if (string.IsNullOrEmpty(userName.Trim()))
                return Json((byte)EResult.IsNull2, JsonRequestBehavior.DenyGet);

            AddUserInfoViewModel userInfoModel = new AddUserInfoViewModel();
            userInfoModel.UserName = userName;

            userInfoModel.RoleIdList = roleList;

            byte result = userManage.Create(userInfoModel);

            ReportFactory.Instance.log.WriteLog(Common.OperationType.Update, string.Format("添加了新用户{0}", userName));
            return Json(result);
        }

        /// <summary>
        /// 加载角色分配页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UserRoleAssign()
        {
            return View();
        }

        [HttpPost]
        /// <summary>
        /// 角色分配
        /// </summary>
        /// <returns></returns>
        public JsonResult UserRoleAssign(RoleAssignViewModel roleAssignList)
        {
            if (roleAssignList == null || roleAssignList.UserIdList.Count < 1)
                return Json((byte)EResult.IsNull1, JsonRequestBehavior.DenyGet);

            if (roleAssignList != null && roleAssignList.RoleIdList.Count < 1)
                return Json((byte)EResult.IsNull2, JsonRequestBehavior.DenyGet);

            byte result = userManage.RoleAssign(roleAssignList);

            string user = string.Empty;
            foreach (Guid id in roleAssignList.UserIdList)
            {
                user = string.Format("{0}{1}，", user, userManage.GetNamebyId(id));
            }

            string role = string.Empty;
            foreach (Guid id in roleAssignList.RoleIdList)
            {
                role = string.Format("{0}{1}，", role, userManage.GetRoleNamebyId(id));
            }

            if (Session["UserInfo"] != null)
                ReportFactory.Instance.log.WriteLog(Common.OperationType.Update, string.Format("分配用户{0}角色为{1}", user, role));

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="userRoleId">用户角色关系编号集合</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(List<Guid> userIdList)
        {
            if (!(userIdList != null && userIdList.Count > 0))
                return Json((byte)EResult.IsNull1, JsonRequestBehavior.DenyGet);

            string user = string.Empty;
            foreach (Guid id in userIdList)
            {
                user = string.Format("{0}{1}，", user, userManage.GetNamebyId(id));
            }

            ReportFactory.Instance.log.WriteLog(Common.OperationType.Update, string.Format("删除了用户{0}", user));

            byte result = userManage.Delete(userIdList);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 加载修改密码页
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdatePassWord()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="password">新密码</param>
        /// <param name="password">旧密码</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdatePassWord(string userId, string password, string oldpassword)
        {
            Guid uId = Guid.NewGuid();
            if (!Guid.TryParse(userId, out uId))
                return Json((byte)EResult.IsNull1, JsonRequestBehavior.DenyGet);

            if (string.IsNullOrEmpty(password))
                return Json((byte)EResult.IsNull2, JsonRequestBehavior.DenyGet);

            var result = userManage.UpdatePassword(uId, password, oldpassword);

            ReportFactory.Instance.log.WriteLog(Common.OperationType.Update, "修改了密码");

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRoleList()
        {
            var result = userManage.GetRoleList();
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUserInfo()
        {
            //UserInfoViewModel result=null;
            //if (Session["UserInfo"] != null)
            //    result = (UserInfoViewModel)Session["UserInfo"];
            //SessionManage.GetLoginUser();
            return Json(SessionManage.GetLoginUser(), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
        public JsonResult Exit()
        {
            ReportFactory.Instance.log.WriteLog(Common.OperationType.LoginOut, "退出系统");
            Session["UserInfo"] = null;
            return Json(true, JsonRequestBehavior.DenyGet);
        }

        #endregion

        #region 日志查询

        /// <summary>
        /// 加载日志查询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult LogManage()
        {
            return View();
        }

        /// <summary>
        /// 查询日志信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LogManage(LogQueryViewModel model)
        {
            //ReportFactory.Instance.log.WriteLog(Common.OperationType.Query, "查询日志信息");

            var result = ReportFactory.Instance.log.GetList(model);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 加载日志类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LogType()
        {
            var result = OperationType.OperationTypeList;
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        #endregion

        #region 批量报表导出

        /// <summary>
        /// 加载批量报表导出页面
        /// </summary>
        /// <returns></returns>
        public ActionResult batchExport()
        {

            //假期ID和名称集合
            List<NodeViewModel> pList = ReportFactory.Instance.dataDictionary.GetListbyId(1);
            int[] SelectId = ReportFactory.Instance.holidayConfig.GetList().Select(s => s.HDayId.Value).ToArray();
            //当前报表配置的节假日id
            List<int> pConfigList = pList.Where(s => SelectId.Contains(s.DictId)).Select(s => s.DictId).ToList();
            ViewBag.SelectedIdList = pConfigList;
            ViewBag.HDayIdList = pList;
            ViewBag.HasYearList = Utility.GetExportHasYear();
            return View();
        }

        #endregion

        #endregion
    }
}
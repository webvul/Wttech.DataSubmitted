using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Wttech.DataSubmitted.BLL;
using Wttech.DataSubmitted.Web.Controllers;
using Wttech.DataSubmitted.Common.ViewModels;

namespace Wttech.DataSubmitted.Tests
{
    [TestClass]
    public class UserManageTest
    {   
        UserManage us= new UserManage();
        SystemManageController sm = new SystemManageController();


        /// <summary>
        /// OK——添加用户功能
        /// </summary>
        [TestMethod]
        public void TestAdd()
        {
            AddUserInfoViewModel model = new AddUserInfoViewModel();

            model.UserName = "sa";
            List<Guid > list = new List<Guid>();
            list.Add(Guid.Parse("7d03e715-c233-4047-9679-d3590d4d6f7c"));
            list.Add(Guid.Parse("557e6ac5-2b26-46d4-a821-fd86b2e2c6d1"));
            model.RoleIdList = list;
            
       

            us.Create(model);

            model.UserName = "aa";
            list.Clear();
            list.Add(Guid.Parse("7d03e715-c233-4047-9679-d3590d4d6f7c"));
            model.RoleIdList = list;
            us.Create(model);
        }

        /// <summary>
        /// OK——删除用户功能
        /// </summary>
        [TestMethod]
        public void TestDelete()
        {
            List<Guid> list = new List<Guid>();
            list.Add(Guid.Parse("736d8731-8b33-4326-b593-22f803ca6cd0"));
            list.Add(Guid.Parse("557e6ac5-2b26-46d4-a821-fd86b2e2c6d1"));
            us.Delete(list);
        }

        /// <summary>
        /// OK——加载用户信息
        /// </summary>
        [TestMethod]
        public void TestGetList()
        {
            us.GetList();
        }

        /// <summary>
        /// ——通过用户名查询用户信息
        /// </summary>
        [TestMethod]
        public void TestGetListbyName()
        {
            us.GetListbyName("",1,10);
            us.GetListbyName("", 0, 10);
            us.GetListbyName("", -11, 10);
            us.GetListbyName("", 2, 10);
            us.GetListbyName("", 1, 1);
            us.GetListbyName("", 2, 1);
            us.GetListbyName("", 3, 1);
            us.GetListbyName("a", 1, 10);
            us.GetListbyName("admin", 1, 10);
        }

        /// <summary>
        /// OK——获取角色列表信息
        /// </summary>
        [TestMethod]
        public void TestGetRoleList()
        {
            us.GetRoleList();
        }

        /// <summary>
        /// OK——登录功能测试
        /// </summary>
        [TestMethod]
        public void TestLogin()
        {
        }

        /// <summary>
        ///OK ——分配角色
        /// </summary>
        [TestMethod]
        public void TestRoleAssign()
        {
            //RoleAssignViewModel model = new RoleAssignViewModel();
            //model.UserId = Guid.Parse("1af8a65b-af1e-4686-ab69-8e800332f4db");
            //     List<Guid > list = new List<Guid>();
            //list.Add(Guid.Parse("7d03e715-c233-4047-9679-d3590d4d6f7c"));
            //list.Add(Guid.Parse("557e6ac5-2b26-46d4-a821-fd86b2e2c6d1"));
            //model.RoleList = list;
            //List<RoleAssignViewModel> listR=new List<RoleAssignViewModel>();
            //listR.Add(model);
            //us.RoleAssign(listR);
        
        }

        /// <summary>
        /// OK——初始化用户密码
        /// </summary>
        [TestMethod]
        public void TestStartPassword()
        {
            us.StartPassword(Guid.NewGuid());
            us.StartPassword(new List<Guid>());
            us.StartPassword(Guid.Parse("4379ef89-37e4-491e-ae69-7cc10c7239d1"));
            us.StartPassword(Guid.Parse("4ee62fc7-9986-4eb3-ab6d-440fa6cbc830"));
            List<Guid> list = new List<Guid>();
            list.Add(Guid.Parse("4ee62fc7-9986-4eb3-ab6d-440fa6cbc830"));
            list.Add(Guid.Parse("2dc1871e-4b19-4237-a680-ac159701d508"));
            us.StartPassword(list);
        }

        /// <summary>
        /// OK——修改用户信息
        /// </summary>
        [TestMethod]
        public void TestUpdate()
        {
            UpdateUserInfoViewModel model = new UpdateUserInfoViewModel();
            model.UserId = Guid.Parse("3b0dd8ec-1b2d-4863-93cd-7874266fa5a4");
            model.UserName = "佚玥";
            List<Guid> list = new List<Guid>();
            list.Add(Guid.Parse("7d03e715-c233-4047-9679-d3590d4d6f7c"));
            list.Add(Guid.Parse("557e6ac5-2b26-46d4-a821-fd86b2e2c6d1"));
            model.RoleIdList = list;
           us.Update(model);

           model.UserId = Guid.Parse("3b0dd8ec-1b2d-4863-93cd-7874266fa7a4");
           model.UserName = "玥";
           list.Clear();
           list.Add(Guid.Parse("7d03e715-c233-4047-9679-d3590d4d6f7c"));
           list.Add(Guid.Parse("557e6ac5-2b26-46d4-a821-fd86b2e2c6d1"));
           model.RoleIdList = list;
           us.Update(model);
        }

        /// <summary>
        /// OK——修改用户信息
        /// </summary>
        [TestMethod]
        public void TestUpdatePassword()
        {
            us.UpdatePassword(Guid.Parse("4379ef89-37e4-491e-ae69-7cc10c7239d1"), "222222", "111111");
            us.UpdatePassword(Guid.Parse("1af8a65b-af1e-4686-ab69-8e800332f4db"), "111111", "222222");
            us.UpdatePassword(Guid.Parse("1af8a65b-af1e-4686-ab69-8e800332f5db"), "111111", "222222");
        }

        [TestMethod]
        public void TestMethod2()
        {
           
         //   sm.Add();
          //  sm.Delete();
            sm.GetRoleList();
            sm.HolidayName();
            //sm.StartPassword();
            //sm.Update();
            //sm.UpdatePassWord();
            //sm.UserManagement();
            //sm.UserRoleAssign();
        }
    }
}
